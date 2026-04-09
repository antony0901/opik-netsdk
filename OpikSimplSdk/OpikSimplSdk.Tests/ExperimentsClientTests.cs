using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class ExperimentsClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("FindExperiments", HttpMethod.Post, "/v1/experiments/find", async c => _ = await c.Experiments.FindExperimentsAsync(new FindExperimentsRequest()));
        yield return Case("CreateExperiment", HttpMethod.Post, "/v1/experiments", c => c.Experiments.CreateExperimentAsync(new CreateExperimentRequest()));
        yield return Case("GetExperimentById", HttpMethod.Get, "/v1/experiments/e1", async c => _ = await c.Experiments.GetExperimentByIdAsync("e1"));
        yield return Case("UpdateExperiment", HttpMethod.Patch, "/v1/experiments/e1", c => c.Experiments.UpdateExperimentAsync("e1", new UpdateExperimentRequest()));
        yield return Case("DeleteExperimentsById", HttpMethod.Post, "/v1/experiments/delete", c => c.Experiments.DeleteExperimentsByIdAsync(["e1"]));
        yield return Case("CreateExperimentItems", HttpMethod.Post, "/v1/experiment-items", c => c.Experiments.CreateExperimentItemsAsync([new ExperimentItem()]));
        yield return Case("GetExperimentItem", HttpMethod.Get, "/v1/experiment-items/i1", async c => _ = await c.Experiments.GetExperimentItemByIdAsync("i1"));
        yield return Case("DeleteExperimentItems", HttpMethod.Post, "/v1/experiment-items/delete", c => c.Experiments.DeleteExperimentItemsAsync(["i1"]));
        yield return Case("ExperimentItemsBulk", HttpMethod.Post, "/v1/experiment-items/bulk", c => c.Experiments.ExperimentItemsBulkAsync(new ExperimentItemsBulkRequest()));
        yield return Case("StreamExperimentItems", HttpMethod.Get, "/v1/experiment-items/stream?experimentName=exp&limit=10&lastRetrievedId=i1&truncate=True", async c => await Drain(c.Experiments.StreamExperimentItemsAsync("exp", 10, "i1", true)), ndjson: true);
        yield return Case("StreamExperiments", HttpMethod.Get, "/v1/experiments/stream?name=exp&limit=10&lastRetrievedId=e1", async c => await Drain(c.Experiments.StreamExperimentsAsync("exp", 10, "e1")), ndjson: true);
        yield return Case("FindExperimentNames", HttpMethod.Get, "/v1/experiments/feedback-scores/names?experimentIds=e1%2Ce2", async c => _ = await c.Experiments.FindFeedbackScoreNamesAsync("e1,e2"), list: true);
        yield return Case("FindGroups", HttpMethod.Post, "/v1/experiments/groups/find", async c => _ = await c.Experiments.FindExperimentGroupsAsync(new FindExperimentGroupsRequest()));
        yield return Case("FindGroupAggregations", HttpMethod.Post, "/v1/experiments/groups/aggregations", async c => _ = await c.Experiments.FindExperimentGroupsAggregationsAsync(new FindExperimentGroupsRequest()));
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

    [Fact]
    public async Task ExperimentItemsBulk_ShouldAllowSmallPayload()
    {
        var (client, _) = TestClientFactory.CreateOpikClient();
        await client.Experiments.ExperimentItemsBulkAsync(new ExperimentItemsBulkRequest());
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
