using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class SpansClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("CreateSpan", HttpMethod.Post, "/v1/spans", c => c.Spans.CreateSpanAsync(new CreateSpanRequest()));
        yield return Case("CreateSpans", HttpMethod.Post, "/v1/spans/batch", c => c.Spans.CreateSpansAsync([new SpanWrite()]));
        yield return Case("GetSpanById", HttpMethod.Get, "/v1/spans/s1?stripAttachments=True", async c => _ = await c.Spans.GetSpanByIdAsync("s1", true));
        yield return Case("UpdateSpan", HttpMethod.Patch, "/v1/spans/s1", c => c.Spans.UpdateSpanAsync("s1", new UpdateSpanRequest()));
        yield return Case("DeleteSpan", HttpMethod.Delete, "/v1/spans/s1", c => c.Spans.DeleteSpanByIdAsync("s1"));
        yield return Case("GetSpansByProject", HttpMethod.Post, "/v1/spans/find", async c => _ = await c.Spans.GetSpansByProjectAsync(new GetSpansRequest()));
        yield return Case("SearchSpans", HttpMethod.Post, "/v1/spans/search", async c => await Drain(c.Spans.SearchSpansAsync(new SearchSpansRequest())), ndjson: true);
        yield return Case("AddSpanFeedback", HttpMethod.Post, "/v1/spans/s1/feedback-scores", c => c.Spans.AddSpanFeedbackScoreAsync("s1", new FeedbackScoreRequest()));
        yield return Case("DeleteSpanFeedback", HttpMethod.Delete, "/v1/spans/s1/feedback-scores/acc?author=bob", c => c.Spans.DeleteSpanFeedbackScoreAsync("s1", "acc", "bob"));
        yield return Case("ScoreBatch", HttpMethod.Post, "/v1/spans/feedback-scores/batch", c => c.Spans.ScoreBatchOfSpansAsync([new FeedbackScoreBatchItem()]));
        yield return Case("FindNames", HttpMethod.Get, "/v1/spans/feedback-scores/names?projectId=p1&type=Llm", async c => _ = await c.Spans.FindFeedbackScoreNamesAsync("p1", OpikSimplSdk.Core.Common.SpanType.Llm), list: true);
        yield return Case("GetStats", HttpMethod.Get, "/v1/spans/stats?projectId=p1&projectName=proj&traceId=t1&type=Tool&filters=f", async c => _ = await c.Spans.GetSpanStatsAsync("p1", "proj", "t1", OpikSimplSdk.Core.Common.SpanType.Tool, "f"));
        yield return Case("AddComment", HttpMethod.Post, "/v1/spans/s1/comments", c => c.Spans.AddSpanCommentAsync("s1", new CommentRequest { Text = "x" }));
        yield return Case("GetComment", HttpMethod.Get, "/v1/spans/s1/comments/c1", async c => _ = await c.Spans.GetSpanCommentAsync("c1", "s1"));
        yield return Case("UpdateComment", HttpMethod.Patch, "/v1/spans/comments/c1", c => c.Spans.UpdateSpanCommentAsync("c1", new CommentRequest { Text = "x" }));
        yield return Case("DeleteComments", HttpMethod.Post, "/v1/spans/comments/delete", c => c.Spans.DeleteSpanCommentsAsync(["c1"]));
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task ShouldCallExpectedEndpoint(TracesClientTests.ClientCallCase testCase)
    {
        var responseBody = testCase.Ndjson ? "{\"x\":1}\n{\"y\":2}\n" : testCase.ResponseBody;
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
        => [new TracesClientTests.ClientCallCase(name, method, pathAndQuery, invoke, list ? "[]" : "{}", ndjson)];

    private static async Task Drain(IAsyncEnumerable<byte[]> stream)
    {
        await foreach (var _ in stream)
        {
        }
    }
}
