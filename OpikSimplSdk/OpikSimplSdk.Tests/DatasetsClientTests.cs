using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class DatasetsClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("FindDatasets", HttpMethod.Post, "/v1/datasets/find", async c => _ = await c.Datasets.FindDatasetsAsync(new FindDatasetsRequest()));
        yield return Case("CreateDataset", HttpMethod.Post, "/v1/datasets", c => c.Datasets.CreateDatasetAsync(new CreateDatasetRequest()));
        yield return Case("GetDatasetById", HttpMethod.Get, "/v1/datasets/d1", async c => _ = await c.Datasets.GetDatasetByIdAsync("d1"));
        yield return Case("GetDatasetByName", HttpMethod.Get, "/v1/datasets/by-name?datasetName=my-dataset", async c => _ = await c.Datasets.GetDatasetByNameAsync("my-dataset"));
        yield return Case("UpdateDataset", HttpMethod.Patch, "/v1/datasets/d1", c => c.Datasets.UpdateDatasetAsync("d1", new UpdateDatasetRequest()));
        yield return Case("DeleteDataset", HttpMethod.Delete, "/v1/datasets/d1", c => c.Datasets.DeleteDatasetAsync("d1"));
        yield return Case("DeleteDatasetByName", HttpMethod.Delete, "/v1/datasets/by-name?datasetName=my-dataset", c => c.Datasets.DeleteDatasetByNameAsync("my-dataset"));
        yield return Case("DeleteDatasetsBatch", HttpMethod.Post, "/v1/datasets/delete", c => c.Datasets.DeleteDatasetsBatchAsync(["d1"]));
        yield return Case("CreateOrUpdateItems", HttpMethod.Post, "/v1/dataset-items/upsert", c => c.Datasets.CreateOrUpdateDatasetItemsAsync(new CreateOrUpdateDatasetItemsRequest()));
        yield return Case("GetDatasetItemById", HttpMethod.Get, "/v1/dataset-items/i1", async c => _ = await c.Datasets.GetDatasetItemByIdAsync("i1"));
        yield return Case("GetDatasetItems", HttpMethod.Post, "/v1/datasets/d1/items/find", async c => _ = await c.Datasets.GetDatasetItemsAsync("d1", new GetDatasetItemsRequest()));
        yield return Case("DeleteDatasetItems", HttpMethod.Post, "/v1/dataset-items/delete", c => c.Datasets.DeleteDatasetItemsAsync(["i1"]));
        yield return Case("StreamDatasetItems", HttpMethod.Get, "/v1/dataset-items/stream?datasetName=my-dataset&lastRetrievedId=i1&limit=10", async c => await Drain(c.Datasets.StreamDatasetItemsAsync("my-dataset", "i1", 10)), ndjson: true);
        yield return Case("CompareItems", HttpMethod.Post, "/v1/datasets/d1/items/compare", async c => _ = await c.Datasets.FindDatasetItemsWithExperimentItemsAsync("d1", new FindDatasetItemsWithExperimentsRequest()));
        yield return Case("GetOutputColumns", HttpMethod.Get, "/v1/datasets/d1/items/output-columns?experimentIds=e1%2Ce2", async c => _ = await c.Datasets.GetDatasetItemsOutputColumnsAsync("d1", "e1,e2"));
        yield return Case("GetExperimentItemStats", HttpMethod.Get, "/v1/datasets/d1/experiment-items/stats?experimentIds=e1&filters=f", async c => _ = await c.Datasets.GetDatasetExperimentItemsStatsAsync("d1", "e1", "f"));
        yield return Case("ExpandDataset", HttpMethod.Post, "/v1/datasets/d1/expand", async c => _ = await c.Datasets.ExpandDatasetAsync("d1", new ExpandDatasetRequest()));
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
