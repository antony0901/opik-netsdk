using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;

namespace OpikSimplSdk.Core.Clients;

/// <summary>
/// Provides operations for trace lifecycle, querying, feedback, comments, and thread management.
/// </summary>
public interface ITracesClient
{
    /// <summary>Creates a single trace.</summary>
    Task CreateTraceAsync(CreateTraceRequest request, RequestOptions? options = null);
    /// <summary>Creates multiple traces in batch.</summary>
    Task CreateTracesAsync(IEnumerable<TraceWrite> traces, RequestOptions? options = null);
    /// <summary>Gets a trace by identifier.</summary>
    Task<TracePublic> GetTraceByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    /// <summary>Updates a trace by identifier.</summary>
    Task UpdateTraceAsync(string id, UpdateTraceRequest request, RequestOptions? options = null);
    /// <summary>Deletes a trace by identifier.</summary>
    Task DeleteTraceByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes traces in batch by identifiers.</summary>
    Task DeleteTracesAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Finds traces by project and filters.</summary>
    Task<TracePagePublic> GetTracesByProjectAsync(GetTracesRequest request, RequestOptions? options = null);
    /// <summary>Streams trace search results as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> SearchTracesAsync(SearchTracesRequest request, RequestOptions? options = null);
    /// <summary>Adds a feedback score to a trace.</summary>
    Task AddTraceFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    /// <summary>Deletes a trace feedback score.</summary>
    Task DeleteTraceFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    /// <summary>Scores multiple traces in a single request.</summary>
    Task ScoreBatchOfTracesAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    /// <summary>Finds feedback score names for traces.</summary>
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, RequestOptions? options = null);
    /// <summary>Gets trace statistics for a project scope.</summary>
    Task<ProjectStatsPublic> GetTraceStatsAsync(string? projectId = null, string? projectName = null, string? filters = null, RequestOptions? options = null);
    /// <summary>Adds a comment to a trace.</summary>
    Task AddTraceCommentAsync(string traceId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Gets a trace comment by identifiers.</summary>
    Task<Comment> GetTraceCommentAsync(string commentId, string traceId, RequestOptions? options = null);
    /// <summary>Updates a trace comment.</summary>
    Task UpdateTraceCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Deletes trace comments in batch.</summary>
    Task DeleteTraceCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Gets a trace thread.</summary>
    Task<TraceThread> GetTraceThreadAsync(GetTraceThreadRequest request, RequestOptions? options = null);
    /// <summary>Finds trace threads.</summary>
    Task<TraceThreadPage> GetTraceThreadsAsync(GetTraceThreadsRequest request, RequestOptions? options = null);
    /// <summary>Streams trace thread search results as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> SearchTraceThreadsAsync(SearchTraceThreadsRequest request, RequestOptions? options = null);
    /// <summary>Opens a trace thread.</summary>
    Task OpenTraceThreadAsync(string threadId, string? projectName = null, string? projectId = null, RequestOptions? options = null);
    /// <summary>Closes a trace thread.</summary>
    Task CloseTraceThreadAsync(CloseTraceThreadRequest request, RequestOptions? options = null);
    /// <summary>Updates thread metadata.</summary>
    Task UpdateThreadAsync(string threadModelId, IEnumerable<string>? tags = null, RequestOptions? options = null);
    /// <summary>Deletes trace threads in batch.</summary>
    Task DeleteTraceThreadsAsync(IEnumerable<string> threadIds, string? projectName = null, string? projectId = null, RequestOptions? options = null);
    /// <summary>Scores multiple threads in a single request.</summary>
    Task ScoreBatchOfThreadsAsync(IEnumerable<FeedbackScoreBatchItemThread> scores, RequestOptions? options = null);
    /// <summary>Finds feedback score names for trace threads.</summary>
    Task<IList<string>> FindTraceThreadsFeedbackScoreNamesAsync(string projectId, RequestOptions? options = null);
    /// <summary>Deletes thread feedback scores.</summary>
    Task DeleteThreadFeedbackScoresAsync(string projectName, string threadId, IEnumerable<string> names, string? author = null, RequestOptions? options = null);
    /// <summary>Adds a comment to a thread.</summary>
    Task AddThreadCommentAsync(string threadId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Gets a thread comment by identifiers.</summary>
    Task<Comment> GetThreadCommentAsync(string commentId, string threadId, RequestOptions? options = null);
    /// <summary>Updates a thread comment.</summary>
    Task UpdateThreadCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Deletes thread comments in batch.</summary>
    Task DeleteThreadCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for span lifecycle, querying, feedback, stats, and comments.
/// </summary>
public interface ISpansClient
{
    /// <summary>Creates a single span.</summary>
    Task CreateSpanAsync(CreateSpanRequest request, RequestOptions? options = null);
    /// <summary>Creates multiple spans in batch.</summary>
    Task CreateSpansAsync(IEnumerable<SpanWrite> spans, RequestOptions? options = null);
    /// <summary>Gets a span by identifier.</summary>
    Task<SpanPublic> GetSpanByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    /// <summary>Updates a span by identifier.</summary>
    Task UpdateSpanAsync(string id, UpdateSpanRequest request, RequestOptions? options = null);
    /// <summary>Deletes a span by identifier.</summary>
    Task DeleteSpanByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Finds spans by project and filters.</summary>
    Task<SpanPagePublic> GetSpansByProjectAsync(GetSpansRequest request, RequestOptions? options = null);
    /// <summary>Streams span search results as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> SearchSpansAsync(SearchSpansRequest request, RequestOptions? options = null);
    /// <summary>Adds a feedback score to a span.</summary>
    Task AddSpanFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    /// <summary>Deletes a span feedback score.</summary>
    Task DeleteSpanFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    /// <summary>Scores multiple spans in a single request.</summary>
    Task ScoreBatchOfSpansAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    /// <summary>Finds feedback score names for spans.</summary>
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, SpanType? type = null, RequestOptions? options = null);
    /// <summary>Gets span statistics for a project scope.</summary>
    Task<ProjectStatsPublic> GetSpanStatsAsync(string? projectId = null, string? projectName = null, string? traceId = null, SpanType? type = null, string? filters = null, RequestOptions? options = null);
    /// <summary>Adds a comment to a span.</summary>
    Task AddSpanCommentAsync(string spanId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Gets a span comment by identifiers.</summary>
    Task<Comment> GetSpanCommentAsync(string commentId, string spanId, RequestOptions? options = null);
    /// <summary>Updates a span comment.</summary>
    Task UpdateSpanCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    /// <summary>Deletes span comments in batch.</summary>
    Task DeleteSpanCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for dataset lifecycle, items, comparisons, and expansion workflows.
/// </summary>
public interface IDatasetsClient
{
    /// <summary>Finds datasets by filters.</summary>
    Task<DatasetPagePublic> FindDatasetsAsync(FindDatasetsRequest request, RequestOptions? options = null);
    /// <summary>Creates a dataset.</summary>
    Task CreateDatasetAsync(CreateDatasetRequest request, RequestOptions? options = null);
    /// <summary>Gets a dataset by identifier.</summary>
    Task<DatasetPublic> GetDatasetByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Gets a dataset by name.</summary>
    Task<DatasetPublic> GetDatasetByNameAsync(string datasetName, RequestOptions? options = null);
    /// <summary>Updates a dataset by identifier.</summary>
    Task UpdateDatasetAsync(string id, UpdateDatasetRequest request, RequestOptions? options = null);
    /// <summary>Deletes a dataset by identifier.</summary>
    Task DeleteDatasetAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes a dataset by name.</summary>
    Task DeleteDatasetByNameAsync(string datasetName, RequestOptions? options = null);
    /// <summary>Deletes datasets in batch.</summary>
    Task DeleteDatasetsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Creates or updates dataset items.</summary>
    Task CreateOrUpdateDatasetItemsAsync(CreateOrUpdateDatasetItemsRequest request, RequestOptions? options = null);
    /// <summary>Gets a dataset item by identifier.</summary>
    Task<DatasetItemPublic> GetDatasetItemByIdAsync(string itemId, RequestOptions? options = null);
    /// <summary>Gets paged dataset items for a dataset.</summary>
    Task<DatasetItemPagePublic> GetDatasetItemsAsync(string id, GetDatasetItemsRequest request, RequestOptions? options = null);
    /// <summary>Deletes dataset items in batch.</summary>
    Task DeleteDatasetItemsAsync(IEnumerable<string> itemIds, RequestOptions? options = null);
    /// <summary>Streams dataset items as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> StreamDatasetItemsAsync(string datasetName, string? lastRetrievedId = null, int? steamLimit = null, RequestOptions? options = null);
    /// <summary>Finds dataset items joined with experiment items.</summary>
    Task<DatasetItemPageCompare> FindDatasetItemsWithExperimentItemsAsync(string id, FindDatasetItemsWithExperimentsRequest request, RequestOptions? options = null);
    /// <summary>Gets available output columns for dataset item comparisons.</summary>
    Task<PageColumns> GetDatasetItemsOutputColumnsAsync(string id, string? experimentIds = null, RequestOptions? options = null);
    /// <summary>Gets statistics for dataset experiment items.</summary>
    Task<ProjectStatsPublic> GetDatasetExperimentItemsStatsAsync(string id, string experimentIds, string? filters = null, RequestOptions? options = null);
    /// <summary>Expands a dataset using AI-assisted generation.</summary>
    Task<DatasetExpansionResponse> ExpandDatasetAsync(string id, ExpandDatasetRequest request, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for experiment lifecycle, items, feedback naming, grouping, and streaming.
/// </summary>
public interface IExperimentsClient
{
    /// <summary>Finds experiments by filters.</summary>
    Task<ExperimentPagePublic> FindExperimentsAsync(FindExperimentsRequest request, RequestOptions? options = null);
    /// <summary>Creates an experiment.</summary>
    Task CreateExperimentAsync(CreateExperimentRequest request, RequestOptions? options = null);
    /// <summary>Gets an experiment by identifier.</summary>
    Task<ExperimentPublic> GetExperimentByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Updates an experiment by identifier.</summary>
    Task UpdateExperimentAsync(string id, UpdateExperimentRequest request, RequestOptions? options = null);
    /// <summary>Deletes experiments in batch.</summary>
    Task DeleteExperimentsByIdAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Creates experiment items in batch.</summary>
    Task CreateExperimentItemsAsync(IEnumerable<ExperimentItem> items, RequestOptions? options = null);
    /// <summary>Gets an experiment item by identifier.</summary>
    Task<ExperimentItemPublic> GetExperimentItemByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes experiment items in batch.</summary>
    Task DeleteExperimentItemsAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Performs bulk experiment item upsert/update operations.</summary>
    Task ExperimentItemsBulkAsync(ExperimentItemsBulkRequest request, RequestOptions? options = null);
    /// <summary>Streams experiment items as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> StreamExperimentItemsAsync(string experimentName, int? limit = null, string? lastRetrievedId = null, bool? truncate = null, RequestOptions? options = null);
    /// <summary>Streams experiments as raw bytes (NDJSON lines).</summary>
    IAsyncEnumerable<byte[]> StreamExperimentsAsync(string name, int? limit = null, string? lastRetrievedId = null, RequestOptions? options = null);
    /// <summary>Finds feedback score names for experiments.</summary>
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? experimentIds = null, RequestOptions? options = null);
    /// <summary>Finds experiment groups.</summary>
    Task<ExperimentGroupResponse> FindExperimentGroupsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
    /// <summary>Gets experiment group aggregations.</summary>
    Task<ExperimentGroupAggregationsResponse> FindExperimentGroupsAggregationsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for project lifecycle, metrics, stats, and feedback naming.
/// </summary>
public interface IProjectsClient
{
    /// <summary>Finds projects by filters.</summary>
    Task<ProjectPagePublic> FindProjectsAsync(FindProjectsRequest request, RequestOptions? options = null);
    /// <summary>Creates a project.</summary>
    Task CreateProjectAsync(CreateProjectRequest request, RequestOptions? options = null);
    /// <summary>Gets a project by identifier.</summary>
    Task<ProjectPublic> GetProjectByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Retrieves a project by name.</summary>
    Task<ProjectDetailed> RetrieveProjectAsync(string name, RequestOptions? options = null);
    /// <summary>Updates a project by identifier.</summary>
    Task UpdateProjectAsync(string id, UpdateProjectRequest request, RequestOptions? options = null);
    /// <summary>Deletes a project by identifier.</summary>
    Task DeleteProjectByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes projects in batch.</summary>
    Task DeleteProjectsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Finds feedback score names by project identifiers.</summary>
    Task<FeedbackScoreNames> FindFeedbackScoreNamesByProjectIdsAsync(string? projectIds = null, RequestOptions? options = null);
    /// <summary>Gets project metrics.</summary>
    Task<ProjectMetricResponsePublic> GetProjectMetricsAsync(string id, GetProjectMetricsRequest request, RequestOptions? options = null);
    /// <summary>Gets project statistics summary.</summary>
    Task<ProjectStatsSummary> GetProjectStatsAsync(GetProjectStatsRequest request, RequestOptions? options = null);
}

/// <summary>
/// Provides CRUD operations for feedback definitions.
/// </summary>
public interface IFeedbackDefinitionsClient
{
    /// <summary>Finds feedback definitions by filters.</summary>
    Task<FeedbackDefinitionPagePublic> FindFeedbackDefinitionsAsync(int? page = null, int? size = null, string? name = null, FeedbackDefinitionType? type = null, RequestOptions? options = null);
    /// <summary>Creates a feedback definition.</summary>
    Task CreateFeedbackDefinitionAsync(FeedbackCreate request, RequestOptions? options = null);
    /// <summary>Gets a feedback definition by identifier.</summary>
    Task<FeedbackPublic> GetFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Updates a feedback definition by identifier.</summary>
    Task UpdateFeedbackDefinitionAsync(string id, FeedbackUpdate request, RequestOptions? options = null);
    /// <summary>Deletes a feedback definition by identifier.</summary>
    Task DeleteFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes feedback definitions in batch.</summary>
    Task DeleteFeedbackDefinitionsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for prompt lifecycle and version management.
/// </summary>
public interface IPromptsClient
{
    /// <summary>Finds prompts by filters.</summary>
    Task<PromptPagePublic> GetPromptsAsync(GetPromptsRequest request, RequestOptions? options = null);
    /// <summary>Creates a prompt.</summary>
    Task CreatePromptAsync(CreatePromptRequest request, RequestOptions? options = null);
    /// <summary>Gets a prompt by identifier.</summary>
    Task<PromptDetail> GetPromptByIdAsync(string id, RequestOptions? options = null);
    /// <summary>Updates a prompt by identifier.</summary>
    Task UpdatePromptAsync(string id, UpdatePromptRequest request, RequestOptions? options = null);
    /// <summary>Deletes a prompt by identifier.</summary>
    Task DeletePromptAsync(string id, RequestOptions? options = null);
    /// <summary>Deletes prompts in batch.</summary>
    Task DeletePromptsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
    /// <summary>Creates a prompt version.</summary>
    Task<PromptVersionDetail> CreatePromptVersionAsync(string name, PromptVersionDetail version, RequestOptions? options = null);
    /// <summary>Gets a prompt version by identifier.</summary>
    Task<PromptVersionDetail> GetPromptVersionByIdAsync(string versionId, RequestOptions? options = null);
    /// <summary>Gets prompt versions with pagination.</summary>
    Task<PromptVersionPagePublic> GetPromptVersionsAsync(string id, int? page = null, int? size = null, RequestOptions? options = null);
    /// <summary>Retrieves a prompt version by name and optional commit.</summary>
    Task<PromptVersionDetail> RetrievePromptVersionAsync(string name, string? commit = null, RequestOptions? options = null);
    /// <summary>Restores a prompt to a specific version.</summary>
    Task<PromptVersionDetail> RestorePromptVersionAsync(string promptId, string versionId, RequestOptions? options = null);
}

/// <summary>
/// Provides operations for attachment listing, upload, download, and deletion.
/// </summary>
public interface IAttachmentsClient
{
    /// <summary>Lists attachments by filters.</summary>
    Task<AttachmentPage> AttachmentListAsync(AttachmentListRequest request, RequestOptions? options = null);
    /// <summary>Starts a multipart attachment upload.</summary>
    Task<StartMultipartUploadResponse> StartMultiPartUploadAsync(StartMultipartUploadRequest request, RequestOptions? options = null);
    /// <summary>Completes a multipart attachment upload.</summary>
    Task CompleteMultiPartUploadAsync(CompleteMultipartUploadRequest request, RequestOptions? options = null);
    /// <summary>Uploads an attachment.</summary>
    Task UploadAttachmentAsync(UploadAttachmentRequest request, RequestOptions? options = null);
    /// <summary>Downloads attachment data as a byte stream.</summary>
    IAsyncEnumerable<byte[]> DownloadAttachmentAsync(DownloadAttachmentRequest request, RequestOptions? options = null);
    /// <summary>Deletes attachments.</summary>
    Task DeleteAttachmentsAsync(DeleteAttachmentsRequest request, RequestOptions? options = null);
}

/// <summary>
/// Provides authorization checks and workspace discovery operations.
/// </summary>
public interface ICheckClient
{
    /// <summary>Validates access for the provided auth details.</summary>
    Task AccessAsync(Dictionary<string, object?> authDetails, RequestOptions? options = null);
    /// <summary>Gets the current workspace name.</summary>
    Task<WorkspaceNameHolder> GetWorkspaceNameAsync(RequestOptions? options = null);
}

/// <summary>
/// Provides operations for workspace configuration and metrics.
/// </summary>
public interface IWorkspacesClient
{
    /// <summary>Gets workspace configuration.</summary>
    Task<WorkspaceConfiguration> GetWorkspaceConfigurationAsync(RequestOptions? options = null);
    /// <summary>Creates or updates workspace configuration.</summary>
    Task<WorkspaceConfiguration> UpsertWorkspaceConfigurationAsync(string? timeoutToMarkThreadAsInactive = null, RequestOptions? options = null);
    /// <summary>Deletes workspace configuration.</summary>
    Task DeleteWorkspaceConfigurationAsync(RequestOptions? options = null);
    /// <summary>Gets workspace cost metrics.</summary>
    Task<WorkspaceMetricResponse> GetCostAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    /// <summary>Gets workspace metrics.</summary>
    Task<WorkspaceMetricResponse> GetMetricAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    /// <summary>Gets workspace cost summary.</summary>
    Task<Result> CostsSummaryAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
    /// <summary>Gets workspace metrics summary.</summary>
    Task<WorkspaceMetricsSummaryResponse> MetricsSummaryAsync(WorkspaceMetricsRequest request, RequestOptions? options = null);
}

/// <summary>
/// Represents a lightweight comment write request.
/// </summary>
public sealed record CommentRequest
{
    /// <summary>The comment text.</summary>
    public string? Text { get; init; }
}
