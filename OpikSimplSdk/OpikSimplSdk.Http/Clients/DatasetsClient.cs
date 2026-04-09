using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal sealed class DatasetsClient : ClientBase, IDatasetsClient
{
    public DatasetsClient(IOpikHttpTransport transport) : base(transport)
    {
    }

    public Task<DatasetPagePublic> FindDatasetsAsync(FindDatasetsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<DatasetPagePublic>(HttpMethod.Post, "/v1/datasets/find", request, options);

    public Task CreateDatasetAsync(CreateDatasetRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/datasets", request, options);

    public Task<DatasetPublic> GetDatasetByIdAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync<DatasetPublic>(HttpMethod.Get, $"/v1/datasets/{id}", options: options);

    public Task<DatasetPublic> GetDatasetByNameAsync(string datasetName, RequestOptions? options = null)
        => Transport.SendAsync<DatasetPublic>(HttpMethod.Get, WithQuery("/v1/datasets/by-name", ("datasetName", datasetName)), options: options);

    public Task UpdateDatasetAsync(string id, UpdateDatasetRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Patch, $"/v1/datasets/{id}", request, options);

    public Task DeleteDatasetAsync(string id, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, $"/v1/datasets/{id}", options: options);

    public Task DeleteDatasetByNameAsync(string datasetName, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Delete, WithQuery("/v1/datasets/by-name", ("datasetName", datasetName)), options: options);

    public Task DeleteDatasetsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/datasets/delete", new { ids }, options);

    public Task CreateOrUpdateDatasetItemsAsync(CreateOrUpdateDatasetItemsRequest request, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/dataset-items/upsert", request, options);

    public Task<DatasetItemPublic> GetDatasetItemByIdAsync(string itemId, RequestOptions? options = null)
        => Transport.SendAsync<DatasetItemPublic>(HttpMethod.Get, $"/v1/dataset-items/{itemId}", options: options);

    public Task<DatasetItemPagePublic> GetDatasetItemsAsync(string id, GetDatasetItemsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<DatasetItemPagePublic>(HttpMethod.Post, $"/v1/datasets/{id}/items/find", request, options);

    public Task DeleteDatasetItemsAsync(IEnumerable<string> itemIds, RequestOptions? options = null)
        => Transport.SendAsync(HttpMethod.Post, "/v1/dataset-items/delete", new { itemIds }, options);

    public IAsyncEnumerable<byte[]> StreamDatasetItemsAsync(string datasetName, string? lastRetrievedId = null, int? steamLimit = null, RequestOptions? options = null)
        => Transport.StreamBytesAsync(HttpMethod.Get, WithQuery("/v1/dataset-items/stream", ("datasetName", datasetName), ("lastRetrievedId", lastRetrievedId), ("limit", steamLimit)), options: options);

    public Task<DatasetItemPageCompare> FindDatasetItemsWithExperimentItemsAsync(string id, FindDatasetItemsWithExperimentsRequest request, RequestOptions? options = null)
        => Transport.SendAsync<DatasetItemPageCompare>(HttpMethod.Post, $"/v1/datasets/{id}/items/compare", request, options);

    public Task<PageColumns> GetDatasetItemsOutputColumnsAsync(string id, string? experimentIds = null, RequestOptions? options = null)
        => Transport.SendAsync<PageColumns>(HttpMethod.Get, WithQuery($"/v1/datasets/{id}/items/output-columns", ("experimentIds", experimentIds)), options: options);

    public Task<ProjectStatsPublic> GetDatasetExperimentItemsStatsAsync(string id, string experimentIds, string? filters = null, RequestOptions? options = null)
        => Transport.SendAsync<ProjectStatsPublic>(HttpMethod.Get, WithQuery($"/v1/datasets/{id}/experiment-items/stats", ("experimentIds", experimentIds), ("filters", filters)), options: options);

    public Task<DatasetExpansionResponse> ExpandDatasetAsync(string id, ExpandDatasetRequest request, RequestOptions? options = null)
        => Transport.SendAsync<DatasetExpansionResponse>(HttpMethod.Post, $"/v1/datasets/{id}/expand", request, options);
}
