using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;

namespace OpikSimplSdk.Core.Clients;

public interface ITracesClient
{
    Task CreateTraceAsync(CreateTraceRequest request, RequestOptions? options = null);
    Task CreateTracesAsync(IEnumerable<TraceWrite> traces, RequestOptions? options = null);
    Task<TracePublic> GetTraceByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    Task UpdateTraceAsync(string id, UpdateTraceRequest request, RequestOptions? options = null);
    Task DeleteTraceByIdAsync(string id, RequestOptions? options = null);
    Task DeleteTracesAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task<TracePagePublic> GetTracesByProjectAsync(GetTracesRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> SearchTracesAsync(SearchTracesRequest request, RequestOptions? options = null);
    Task AddTraceFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    Task DeleteTraceFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    Task ScoreBatchOfTracesAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, RequestOptions? options = null);
    Task<ProjectStatsPublic> GetTraceStatsAsync(string? projectId = null, string? projectName = null, string? filters = null, RequestOptions? options = null);
    Task AddTraceCommentAsync(string traceId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetTraceCommentAsync(string commentId, string traceId, RequestOptions? options = null);
    Task UpdateTraceCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteTraceCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task<TraceThread> GetTraceThreadAsync(GetTraceThreadRequest request, RequestOptions? options = null);
    Task<TraceThreadPage> GetTraceThreadsAsync(GetTraceThreadsRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> SearchTraceThreadsAsync(SearchTraceThreadsRequest request, RequestOptions? options = null);
    Task OpenTraceThreadAsync(string threadId, string? projectName = null, string? projectId = null, RequestOptions? options = null);
    Task CloseTraceThreadAsync(CloseTraceThreadRequest request, RequestOptions? options = null);
    Task UpdateThreadAsync(string threadModelId, IEnumerable<string>? tags = null, RequestOptions? options = null);
    Task DeleteTraceThreadsAsync(IEnumerable<string> threadIds, string? projectName = null, string? projectId = null, RequestOptions? options = null);
    Task ScoreBatchOfThreadsAsync(IEnumerable<FeedbackScoreBatchItemThread> scores, RequestOptions? options = null);
    Task<IList<string>> FindTraceThreadsFeedbackScoreNamesAsync(string projectId, RequestOptions? options = null);
    Task DeleteThreadFeedbackScoresAsync(string projectName, string threadId, IEnumerable<string> names, string? author = null, RequestOptions? options = null);
    Task AddThreadCommentAsync(string threadId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetThreadCommentAsync(string commentId, string threadId, RequestOptions? options = null);
    Task UpdateThreadCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteThreadCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

public interface ISpansClient
{
    Task CreateSpanAsync(CreateSpanRequest request, RequestOptions? options = null);
    Task CreateSpansAsync(IEnumerable<SpanWrite> spans, RequestOptions? options = null);
    Task<SpanPublic> GetSpanByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    Task UpdateSpanAsync(string id, UpdateSpanRequest request, RequestOptions? options = null);
    Task DeleteSpanByIdAsync(string id, RequestOptions? options = null);
    Task<SpanPagePublic> GetSpansByProjectAsync(GetSpansRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> SearchSpansAsync(SearchSpansRequest request, RequestOptions? options = null);
    Task AddSpanFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    Task DeleteSpanFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    Task ScoreBatchOfSpansAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, SpanType? type = null, RequestOptions? options = null);
    Task<ProjectStatsPublic> GetSpanStatsAsync(string? projectId = null, string? projectName = null, string? traceId = null, SpanType? type = null, string? filters = null, RequestOptions? options = null);
    Task AddSpanCommentAsync(string spanId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetSpanCommentAsync(string commentId, string spanId, RequestOptions? options = null);
    Task UpdateSpanCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteSpanCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

public interface IDatasetsClient
{
    Task<DatasetPagePublic> FindDatasetsAsync(FindDatasetsRequest request, RequestOptions? options = null);
    Task CreateDatasetAsync(CreateDatasetRequest request, RequestOptions? options = null);
    Task<DatasetPublic> GetDatasetByIdAsync(string id, RequestOptions? options = null);
    Task<DatasetPublic> GetDatasetByNameAsync(string datasetName, RequestOptions? options = null);
    Task UpdateDatasetAsync(string id, UpdateDatasetRequest request, RequestOptions? options = null);
    Task DeleteDatasetAsync(string id, RequestOptions? options = null);
    Task DeleteDatasetByNameAsync(string datasetName, RequestOptions? options = null);
    Task DeleteDatasetsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task CreateOrUpdateDatasetItemsAsync(CreateOrUpdateDatasetItemsRequest request, RequestOptions? options = null);
    Task<DatasetItemPublic> GetDatasetItemByIdAsync(string itemId, RequestOptions? options = null);
    Task<DatasetItemPagePublic> GetDatasetItemsAsync(string id, GetDatasetItemsRequest request, RequestOptions? options = null);
    Task DeleteDatasetItemsAsync(IEnumerable<string> itemIds, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamDatasetItemsAsync(string datasetName, string? lastRetrievedId = null, int? steamLimit = null, RequestOptions? options = null);
    Task<DatasetItemPageCompare> FindDatasetItemsWithExperimentItemsAsync(string id, FindDatasetItemsWithExperimentsRequest request, RequestOptions? options = null);
    Task<PageColumns> GetDatasetItemsOutputColumnsAsync(string id, string? experimentIds = null, RequestOptions? options = null);
    Task<ProjectStatsPublic> GetDatasetExperimentItemsStatsAsync(string id, string experimentIds, string? filters = null, RequestOptions? options = null);
    Task<DatasetExpansionResponse> ExpandDatasetAsync(string id, ExpandDatasetRequest request, RequestOptions? options = null);
}

public interface IExperimentsClient
{
    Task<ExperimentPagePublic> FindExperimentsAsync(FindExperimentsRequest request, RequestOptions? options = null);
    Task CreateExperimentAsync(CreateExperimentRequest request, RequestOptions? options = null);
    Task<ExperimentPublic> GetExperimentByIdAsync(string id, RequestOptions? options = null);
    Task UpdateExperimentAsync(string id, UpdateExperimentRequest request, RequestOptions? options = null);
    Task DeleteExperimentsByIdAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task CreateExperimentItemsAsync(IEnumerable<ExperimentItem> items, RequestOptions? options = null);
    Task<ExperimentItemPublic> GetExperimentItemByIdAsync(string id, RequestOptions? options = null);
    Task DeleteExperimentItemsAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task ExperimentItemsBulkAsync(ExperimentItemsBulkRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamExperimentItemsAsync(string experimentName, int? limit = null, string? lastRetrievedId = null, bool? truncate = null, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamExperimentsAsync(string name, int? limit = null, string? lastRetrievedId = null, RequestOptions? options = null);
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? experimentIds = null, RequestOptions? options = null);
    Task<ExperimentGroupResponse> FindExperimentGroupsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
    Task<ExperimentGroupAggregationsResponse> FindExperimentGroupsAggregationsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
}

public interface IProjectsClient
{
    Task<ProjectPagePublic> FindProjectsAsync(FindProjectsRequest request, RequestOptions? options = null);
    Task CreateProjectAsync(CreateProjectRequest request, RequestOptions? options = null);
    Task<ProjectPublic> GetProjectByIdAsync(string id, RequestOptions? options = null);
    Task<ProjectDetailed> RetrieveProjectAsync(string name, RequestOptions? options = null);
    Task UpdateProjectAsync(string id, UpdateProjectRequest request, RequestOptions? options = null);
    Task DeleteProjectByIdAsync(string id, RequestOptions? options = null);
    Task DeleteProjectsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task<FeedbackScoreNames> FindFeedbackScoreNamesByProjectIdsAsync(string? projectIds = null, RequestOptions? options = null);
    Task<ProjectMetricResponsePublic> GetProjectMetricsAsync(string id, GetProjectMetricsRequest request, RequestOptions? options = null);
    Task<ProjectStatsSummary> GetProjectStatsAsync(GetProjectStatsRequest request, RequestOptions? options = null);
}

public interface IFeedbackDefinitionsClient
{
    Task<FeedbackDefinitionPagePublic> FindFeedbackDefinitionsAsync(int? page = null, int? size = null, string? name = null, FeedbackDefinitionType? type = null, RequestOptions? options = null);
    Task CreateFeedbackDefinitionAsync(FeedbackCreate request, RequestOptions? options = null);
    Task<FeedbackPublic> GetFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    Task UpdateFeedbackDefinitionAsync(string id, FeedbackUpdate request, RequestOptions? options = null);
    Task DeleteFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    Task DeleteFeedbackDefinitionsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

public interface IPromptsClient
{
    Task<PromptPagePublic> GetPromptsAsync(GetPromptsRequest request, RequestOptions? options = null);
    Task CreatePromptAsync(CreatePromptRequest request, RequestOptions? options = null);
    Task<PromptDetail> GetPromptByIdAsync(string id, RequestOptions? options = null);
    Task UpdatePromptAsync(string id, UpdatePromptRequest request, RequestOptions? options = null);
    Task DeletePromptAsync(string id, RequestOptions? options = null);
    Task DeletePromptsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task<PromptVersionDetail> CreatePromptVersionAsync(string name, PromptVersionDetail version, RequestOptions? options = null);
    Task<PromptVersionDetail> GetPromptVersionByIdAsync(string versionId, RequestOptions? options = null);
    Task<PromptVersionPagePublic> GetPromptVersionsAsync(string id, int? page = null, int? size = null, RequestOptions? options = null);
    Task<PromptVersionDetail> RetrievePromptVersionAsync(string name, string? commit = null, RequestOptions? options = null);
    Task<PromptVersionDetail> RestorePromptVersionAsync(string promptId, string versionId, RequestOptions? options = null);
}

public interface IAttachmentsClient
{
    Task<AttachmentPage> AttachmentListAsync(AttachmentListRequest request, RequestOptions? options = null);
    Task<StartMultipartUploadResponse> StartMultiPartUploadAsync(StartMultipartUploadRequest request, RequestOptions? options = null);
    Task CompleteMultiPartUploadAsync(CompleteMultipartUploadRequest request, RequestOptions? options = null);
    Task UploadAttachmentAsync(UploadAttachmentRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> DownloadAttachmentAsync(DownloadAttachmentRequest request, RequestOptions? options = null);
    Task DeleteAttachmentsAsync(DeleteAttachmentsRequest request, RequestOptions? options = null);
}

public interface ICheckClient
{
    Task AccessAsync(Dictionary<string, object?> authDetails, RequestOptions? options = null);
    Task<WorkspaceNameHolder> GetWorkspaceNameAsync(RequestOptions? options = null);
}

public interface IWorkspacesClient
{
    Task<WorkspaceConfiguration> GetWorkspaceConfigurationAsync(RequestOptions? options = null);
    Task<WorkspaceConfiguration> UpsertWorkspaceConfigurationAsync(string? timeoutToMarkThreadAsInactive = null, RequestOptions? options = null);
    Task DeleteWorkspaceConfigurationAsync(RequestOptions? options = null);
    Task<WorkspaceMetricResponse> GetCostAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    Task<WorkspaceMetricResponse> GetMetricAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    Task<Result> CostsSummaryAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    Task<WorkspaceMetricsSummaryResponse> MetricsSummaryAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
}

public sealed record CommentRequest
{
    public string? Text { get; init; }
}
