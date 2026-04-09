using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class TracesClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("CreateTrace", HttpMethod.Post, "/v1/traces", c => c.Traces.CreateTraceAsync(new CreateTraceRequest()));
        yield return Case("CreateTraces", HttpMethod.Post, "/v1/traces/batch", c => c.Traces.CreateTracesAsync([new TraceWrite()]));
        yield return Case("GetTraceById", HttpMethod.Get, "/v1/traces/t1?stripAttachments=True", async c => _ = await c.Traces.GetTraceByIdAsync("t1", true));
        yield return Case("UpdateTrace", HttpMethod.Patch, "/v1/traces/t1", c => c.Traces.UpdateTraceAsync("t1", new UpdateTraceRequest()));
        yield return Case("DeleteTraceById", HttpMethod.Delete, "/v1/traces/t1", c => c.Traces.DeleteTraceByIdAsync("t1"));
        yield return Case("DeleteTraces", HttpMethod.Post, "/v1/traces/delete", c => c.Traces.DeleteTracesAsync(["t1", "t2"]));
        yield return Case("GetTracesByProject", HttpMethod.Post, "/v1/traces/find", async c => _ = await c.Traces.GetTracesByProjectAsync(new GetTracesRequest()));
        yield return Case("SearchTraces", HttpMethod.Post, "/v1/traces/search", async c => await Drain(c.Traces.SearchTracesAsync(new SearchTracesRequest())), ndjson: true);
        yield return Case("AddTraceFeedbackScore", HttpMethod.Post, "/v1/traces/t1/feedback-scores", c => c.Traces.AddTraceFeedbackScoreAsync("t1", new FeedbackScoreRequest()));
        yield return Case("DeleteTraceFeedbackScore", HttpMethod.Delete, "/v1/traces/t1/feedback-scores/accuracy?author=alice", c => c.Traces.DeleteTraceFeedbackScoreAsync("t1", "accuracy", "alice"));
        yield return Case("ScoreBatchOfTraces", HttpMethod.Post, "/v1/traces/feedback-scores/batch", c => c.Traces.ScoreBatchOfTracesAsync([new FeedbackScoreBatchItem()]));
        yield return Case("FindFeedbackScoreNames", HttpMethod.Get, "/v1/traces/feedback-scores/names?projectId=p1", async c => _ = await c.Traces.FindFeedbackScoreNamesAsync("p1"), list: true);
        yield return Case("GetTraceStats", HttpMethod.Get, "/v1/traces/stats?projectId=p1&projectName=proj&filters=f", async c => _ = await c.Traces.GetTraceStatsAsync("p1", "proj", "f"));
        yield return Case("AddTraceComment", HttpMethod.Post, "/v1/traces/t1/comments", c => c.Traces.AddTraceCommentAsync("t1", new CommentRequest { Text = "x" }));
        yield return Case("GetTraceComment", HttpMethod.Get, "/v1/traces/t1/comments/c1", async c => _ = await c.Traces.GetTraceCommentAsync("c1", "t1"));
        yield return Case("UpdateTraceComment", HttpMethod.Patch, "/v1/traces/comments/c1", c => c.Traces.UpdateTraceCommentAsync("c1", new CommentRequest { Text = "x" }));
        yield return Case("DeleteTraceComments", HttpMethod.Post, "/v1/traces/comments/delete", c => c.Traces.DeleteTraceCommentsAsync(["c1"]));
        yield return Case("GetTraceThread", HttpMethod.Post, "/v1/trace-threads/get", async c => _ = await c.Traces.GetTraceThreadAsync(new GetTraceThreadRequest()));
        yield return Case("GetTraceThreads", HttpMethod.Post, "/v1/trace-threads/find", async c => _ = await c.Traces.GetTraceThreadsAsync(new GetTraceThreadsRequest()));
        yield return Case("SearchTraceThreads", HttpMethod.Post, "/v1/trace-threads/search", async c => await Drain(c.Traces.SearchTraceThreadsAsync(new SearchTraceThreadsRequest())), ndjson: true);
        yield return Case("OpenTraceThread", HttpMethod.Post, "/v1/trace-threads/th1/open", c => c.Traces.OpenTraceThreadAsync("th1", "proj", "p1"));
        yield return Case("CloseTraceThread", HttpMethod.Post, "/v1/trace-threads/close", c => c.Traces.CloseTraceThreadAsync(new CloseTraceThreadRequest()));
        yield return Case("UpdateThread", HttpMethod.Patch, "/v1/trace-threads/th1", c => c.Traces.UpdateThreadAsync("th1", ["a", "b"]));
        yield return Case("DeleteTraceThreads", HttpMethod.Post, "/v1/trace-threads/delete", c => c.Traces.DeleteTraceThreadsAsync(["th1"], "proj", "p1"));
        yield return Case("ScoreBatchOfThreads", HttpMethod.Post, "/v1/trace-threads/feedback-scores/batch", c => c.Traces.ScoreBatchOfThreadsAsync([new FeedbackScoreBatchItemThread()]));
        yield return Case("FindTraceThreadsFeedbackScoreNames", HttpMethod.Get, "/v1/trace-threads/feedback-scores/names?projectId=p1", async c => _ = await c.Traces.FindTraceThreadsFeedbackScoreNamesAsync("p1"), list: true);
        yield return Case("DeleteThreadFeedbackScores", HttpMethod.Post, "/v1/trace-threads/feedback-scores/delete", c => c.Traces.DeleteThreadFeedbackScoresAsync("proj", "th1", ["score"], "alice"));
        yield return Case("AddThreadComment", HttpMethod.Post, "/v1/trace-threads/th1/comments", c => c.Traces.AddThreadCommentAsync("th1", new CommentRequest { Text = "x" }));
        yield return Case("GetThreadComment", HttpMethod.Get, "/v1/trace-threads/th1/comments/c1", async c => _ = await c.Traces.GetThreadCommentAsync("c1", "th1"));
        yield return Case("UpdateThreadComment", HttpMethod.Patch, "/v1/trace-threads/comments/c1", c => c.Traces.UpdateThreadCommentAsync("c1", new CommentRequest { Text = "x" }));
        yield return Case("DeleteThreadComments", HttpMethod.Post, "/v1/trace-threads/comments/delete", c => c.Traces.DeleteThreadCommentsAsync(["c1"]));
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task ShouldCallExpectedEndpoint(ClientCallCase testCase)
    {
        var responseBody = testCase.Ndjson ? "{\"x\":1}\n\n{\"y\":2}\n" : testCase.ResponseBody;
        var (client, handler) = TestClientFactory.CreateOpikClient((_, _) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(responseBody, System.Text.Encoding.UTF8, testCase.Ndjson ? "application/x-ndjson" : "application/json")
        }));

        await testCase.Invoke(client);

        var request = Assert.Single(handler.Requests);
        Assert.Equal(testCase.Method, request.Method);
        Assert.Equal(testCase.PathAndQuery, request.PathAndQuery);
    }

    private static object[] Case(string name, HttpMethod method, string pathAndQuery, Func<OpikSimplSdk.Http.OpikClient, Task> invoke, bool list = false, bool ndjson = false)
        => [new ClientCallCase(name, method, pathAndQuery, invoke, list ? "[]" : "{}", ndjson)];

    private static async Task Drain(IAsyncEnumerable<byte[]> stream)
    {
        await foreach (var _ in stream)
        {
        }
    }

    public sealed record ClientCallCase(string Name, HttpMethod Method, string PathAndQuery, Func<OpikSimplSdk.Http.OpikClient, Task> Invoke, string ResponseBody, bool Ndjson)
    {
        public override string ToString() => Name;
    }
}
