namespace OpikSimplSdk.Core.Models;

public sealed record CreateTraceRequest;
public sealed record TraceWrite;
public sealed record TracePublic;
public sealed record UpdateTraceRequest;
public sealed record TracePagePublic;
public sealed record GetTracesRequest;
public sealed record SearchTracesRequest;
public sealed record FeedbackScoreRequest;
public sealed record FeedbackScoreBatchItem;
public sealed record ProjectStatsPublic;
public sealed record Comment;
public sealed record TraceThread;
public sealed record GetTraceThreadRequest;
public sealed record GetTraceThreadsRequest;
public sealed record TraceThreadPage;
public sealed record SearchTraceThreadsRequest;
public sealed record CloseTraceThreadRequest;
public sealed record FeedbackScoreBatchItemThread;

public sealed record CreateSpanRequest;
public sealed record SpanWrite;
public sealed record SpanPublic;
public sealed record UpdateSpanRequest;
public sealed record SpanPagePublic;
public sealed record GetSpansRequest;
public sealed record SearchSpansRequest;

public sealed record DatasetPagePublic;
public sealed record FindDatasetsRequest;
public sealed record CreateDatasetRequest;
public sealed record DatasetPublic;
public sealed record UpdateDatasetRequest;
public sealed record CreateOrUpdateDatasetItemsRequest;
public sealed record DatasetItemPublic;
public sealed record GetDatasetItemsRequest;
public sealed record DatasetItemPagePublic;
public sealed record DatasetItemPageCompare;
public sealed record FindDatasetItemsWithExperimentsRequest;
public sealed record PageColumns;
public sealed record DatasetExpansionResponse;
public sealed record ExpandDatasetRequest;

public sealed record ExperimentPagePublic;
public sealed record FindExperimentsRequest;
public sealed record CreateExperimentRequest;
public sealed record ExperimentPublic;
public sealed record UpdateExperimentRequest;
public sealed record ExperimentItem;
public sealed record ExperimentItemPublic;
public sealed record ExperimentItemsBulkRequest;
public sealed record ExperimentGroupResponse;
public sealed record FindExperimentGroupsRequest;
public sealed record ExperimentGroupAggregationsResponse;

public sealed record ProjectPagePublic;
public sealed record FindProjectsRequest;
public sealed record CreateProjectRequest;
public sealed record ProjectPublic;
public sealed record ProjectDetailed;
public sealed record UpdateProjectRequest;
public sealed record FeedbackScoreNames;
public sealed record ProjectMetricResponsePublic;
public sealed record GetProjectMetricsRequest;
public sealed record ProjectStatsSummary;
public sealed record GetProjectStatsRequest;

public sealed record FeedbackDefinitionPagePublic;

public sealed record PromptPagePublic;
public sealed record GetPromptsRequest;
public sealed record CreatePromptRequest;
public sealed record PromptDetail;
public sealed record UpdatePromptRequest;
public sealed record PromptVersionDetail;
public sealed record PromptVersionPagePublic;

public sealed record AttachmentPage;
public sealed record AttachmentListRequest;
public sealed record StartMultipartUploadResponse;
public sealed record StartMultipartUploadRequest;
public sealed record CompleteMultipartUploadRequest;
public sealed record UploadAttachmentRequest;
public sealed record DownloadAttachmentRequest;
public sealed record DeleteAttachmentsRequest;

public sealed record WorkspaceNameHolder;
public sealed record WorkspaceConfiguration;
public sealed record WorkspaceMetricsRequest;
public sealed record WorkspaceMetricResponse;
public sealed record Result;
public sealed record WorkspaceMetricsSummaryResponse;
