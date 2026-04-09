# Opik .NET SDK — Port Requirements

Source: [Opik Python SDK REST API Clients Reference](https://www.comet.com/docs/opik/python-sdk-reference/rest_api/clients/index.html)

This document describes what the C# SDK must implement to match the Python `opik.rest_client` surface.

---

## 1. Architecture Overview

The Python SDK exposes all REST clients through a single entry point:

```python
client = opik.Opik()
client.rest_client.traces     # TracesClient
client.rest_client.spans      # SpansClient
client.rest_client.datasets   # DatasetsClient
# ... etc
```

The C# equivalent should follow the same pattern:

```csharp
var client = new OpikClient(config);
client.Traces     // ITracesClient
client.Spans      // ISpansClient
client.Datasets   // IDatasetsClient
// ... etc
```

`IOpikClient` in `OpikSimplSdk.Http` is the root entry point. All sub-clients should be accessible as properties on it.

---

## 2. Configuration

The client must accept:
- `BaseUrl` (string) — API host URL
- `ApiKey` (string) — authentication key
- `WorkspaceName` (string, optional) — defaults to user's default workspace

---

## 3. Common Types

All methods accept an optional `RequestOptions` equivalent. In C#, use a `RequestOptions` record/class:

```csharp
public record RequestOptions
{
    public TimeSpan? Timeout { get; init; }
    public int? ChunkSize { get; init; }   // for streaming responses
}
```

### Shared enums

| Python literal | C# enum name |
|---|---|
| `'ui' \| 'sdk' \| 'online_scoring'` | `FeedbackScoreSource` |
| `'general' \| 'tool' \| 'llm' \| 'guardrail'` | `SpanType` |
| `'numerical' \| 'categorical'` | `FeedbackDefinitionType` |
| `'private' \| 'public'` | `Visibility` |
| `'running' \| 'completed' \| 'cancelled'` | `ExperimentStatus` |
| `'regular' \| 'trial' \| 'mini-batch'` | `ExperimentType` |
| `'mustache' \| 'jinja2'` | `PromptType` |
| `'trace' \| 'span'` | `EntityType` |

### Flexible JSON fields

Python uses `JsonListStringWrite` / `JsonListString` for fields like `input`, `output`, `metadata` that accept `Dict | List[Dict] | str`. In C#, represent these as `JsonElement?` or `object?` (serialize as raw JSON).

### Streaming responses

Python methods returning `Iterator[bytes]` map to `IAsyncEnumerable<byte[]>` or `Stream` in C#. Use `IAsyncEnumerable<T>` where `T` is the deserialized entity when possible.

---

## 4. Client Interfaces

### 4.1 `ITracesClient`

```csharp
public interface ITracesClient
{
    // CRUD
    Task CreateTraceAsync(CreateTraceRequest request, RequestOptions? options = null);
    Task CreateTracesAsync(IEnumerable<TraceWrite> traces, RequestOptions? options = null);
    Task<TracePublic> GetTraceByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    Task UpdateTraceAsync(string id, UpdateTraceRequest request, RequestOptions? options = null);
    Task DeleteTraceByIdAsync(string id, RequestOptions? options = null);
    Task DeleteTracesAsync(IEnumerable<string> ids, RequestOptions? options = null);

    // Listing / search
    Task<TracePagePublic> GetTracesByProjectAsync(GetTracesRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> SearchTracesAsync(SearchTracesRequest request, RequestOptions? options = null);

    // Feedback scores
    Task AddTraceFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    Task DeleteTraceFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    Task ScoreBatchOfTracesAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, RequestOptions? options = null);

    // Stats
    Task<ProjectStatsPublic> GetTraceStatsAsync(string? projectId = null, string? projectName = null, string? filters = null, RequestOptions? options = null);

    // Comments
    Task AddTraceCommentAsync(string traceId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetTraceCommentAsync(string commentId, string traceId, RequestOptions? options = null);
    Task UpdateTraceCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteTraceCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);

    // Threads
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

    // Thread comments
    Task AddThreadCommentAsync(string threadId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetThreadCommentAsync(string commentId, string threadId, RequestOptions? options = null);
    Task UpdateThreadCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteThreadCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}
```

### 4.2 `ISpansClient`

```csharp
public interface ISpansClient
{
    // CRUD
    Task CreateSpanAsync(CreateSpanRequest request, RequestOptions? options = null);
    Task CreateSpansAsync(IEnumerable<SpanWrite> spans, RequestOptions? options = null);
    Task<SpanPublic> GetSpanByIdAsync(string id, bool? stripAttachments = null, RequestOptions? options = null);
    Task UpdateSpanAsync(string id, UpdateSpanRequest request, RequestOptions? options = null);
    Task DeleteSpanByIdAsync(string id, RequestOptions? options = null);

    // Listing / search
    Task<SpanPagePublic> GetSpansByProjectAsync(GetSpansRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> SearchSpansAsync(SearchSpansRequest request, RequestOptions? options = null);

    // Feedback scores
    Task AddSpanFeedbackScoreAsync(string id, FeedbackScoreRequest request, RequestOptions? options = null);
    Task DeleteSpanFeedbackScoreAsync(string id, string name, string? author = null, RequestOptions? options = null);
    Task ScoreBatchOfSpansAsync(IEnumerable<FeedbackScoreBatchItem> scores, RequestOptions? options = null);
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? projectId = null, SpanType? type = null, RequestOptions? options = null);

    // Stats
    Task<ProjectStatsPublic> GetSpanStatsAsync(string? projectId = null, string? projectName = null, string? traceId = null, SpanType? type = null, string? filters = null, RequestOptions? options = null);

    // Comments
    Task AddSpanCommentAsync(string spanId, CommentRequest request, RequestOptions? options = null);
    Task<Comment> GetSpanCommentAsync(string commentId, string spanId, RequestOptions? options = null);
    Task UpdateSpanCommentAsync(string commentId, CommentRequest request, RequestOptions? options = null);
    Task DeleteSpanCommentsAsync(IEnumerable<string> ids, RequestOptions? options = null);
}
```

### 4.3 `IDatasetsClient`

```csharp
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

    // Items
    Task CreateOrUpdateDatasetItemsAsync(CreateOrUpdateDatasetItemsRequest request, RequestOptions? options = null);
    Task<DatasetItemPublic> GetDatasetItemByIdAsync(string itemId, RequestOptions? options = null);
    Task<DatasetItemPagePublic> GetDatasetItemsAsync(string id, GetDatasetItemsRequest request, RequestOptions? options = null);
    Task DeleteDatasetItemsAsync(IEnumerable<string> itemIds, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamDatasetItemsAsync(string datasetName, string? lastRetrievedId = null, int? steamLimit = null, RequestOptions? options = null);

    // Experiment comparison
    Task<DatasetItemPageCompare> FindDatasetItemsWithExperimentItemsAsync(string id, FindDatasetItemsWithExperimentsRequest request, RequestOptions? options = null);
    Task<PageColumns> GetDatasetItemsOutputColumnsAsync(string id, string? experimentIds = null, RequestOptions? options = null);
    Task<ProjectStatsPublic> GetDatasetExperimentItemsStatsAsync(string id, string experimentIds, string? filters = null, RequestOptions? options = null);

    // AI expansion
    Task<DatasetExpansionResponse> ExpandDatasetAsync(string id, ExpandDatasetRequest request, RequestOptions? options = null);
}
```

### 4.4 `IExperimentsClient`

```csharp
public interface IExperimentsClient
{
    Task<ExperimentPagePublic> FindExperimentsAsync(FindExperimentsRequest request, RequestOptions? options = null);
    Task CreateExperimentAsync(CreateExperimentRequest request, RequestOptions? options = null);
    Task<ExperimentPublic> GetExperimentByIdAsync(string id, RequestOptions? options = null);
    Task UpdateExperimentAsync(string id, UpdateExperimentRequest request, RequestOptions? options = null);
    Task DeleteExperimentsByIdAsync(IEnumerable<string> ids, RequestOptions? options = null);

    // Items
    Task CreateExperimentItemsAsync(IEnumerable<ExperimentItem> items, RequestOptions? options = null);
    Task<ExperimentItemPublic> GetExperimentItemByIdAsync(string id, RequestOptions? options = null);
    Task DeleteExperimentItemsAsync(IEnumerable<string> ids, RequestOptions? options = null);
    Task ExperimentItemsBulkAsync(ExperimentItemsBulkRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamExperimentItemsAsync(string experimentName, int? limit = null, string? lastRetrievedId = null, bool? truncate = null, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> StreamExperimentsAsync(string name, int? limit = null, string? lastRetrievedId = null, RequestOptions? options = null);

    // Feedback / grouping
    Task<IList<string>> FindFeedbackScoreNamesAsync(string? experimentIds = null, RequestOptions? options = null);
    Task<ExperimentGroupResponse> FindExperimentGroupsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
    Task<ExperimentGroupAggregationsResponse> FindExperimentGroupsAggregationsAsync(FindExperimentGroupsRequest request, RequestOptions? options = null);
}
```

### 4.5 `IProjectsClient`

```csharp
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
```

### 4.6 `IFeedbackDefinitionsClient`

```csharp
public interface IFeedbackDefinitionsClient
{
    Task<FeedbackDefinitionPagePublic> FindFeedbackDefinitionsAsync(int? page = null, int? size = null, string? name = null, FeedbackDefinitionType? type = null, RequestOptions? options = null);
    Task CreateFeedbackDefinitionAsync(FeedbackCreate request, RequestOptions? options = null);
    Task<FeedbackPublic> GetFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    Task UpdateFeedbackDefinitionAsync(string id, FeedbackUpdate request, RequestOptions? options = null);
    Task DeleteFeedbackDefinitionByIdAsync(string id, RequestOptions? options = null);
    Task DeleteFeedbackDefinitionsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);
}
```

`FeedbackCreate` and `FeedbackUpdate` are discriminated unions (numerical vs categorical). Use a base class + subclasses or a tagged union pattern.

### 4.7 `IPromptsClient`

```csharp
public interface IPromptsClient
{
    Task<PromptPagePublic> GetPromptsAsync(GetPromptsRequest request, RequestOptions? options = null);
    Task CreatePromptAsync(CreatePromptRequest request, RequestOptions? options = null);
    Task<PromptDetail> GetPromptByIdAsync(string id, RequestOptions? options = null);
    Task UpdatePromptAsync(string id, UpdatePromptRequest request, RequestOptions? options = null);
    Task DeletePromptAsync(string id, RequestOptions? options = null);
    Task DeletePromptsBatchAsync(IEnumerable<string> ids, RequestOptions? options = null);

    // Versions
    Task<PromptVersionDetail> CreatePromptVersionAsync(string name, PromptVersionDetail version, RequestOptions? options = null);
    Task<PromptVersionDetail> GetPromptVersionByIdAsync(string versionId, RequestOptions? options = null);
    Task<PromptVersionPagePublic> GetPromptVersionsAsync(string id, int? page = null, int? size = null, RequestOptions? options = null);
    Task<PromptVersionDetail> RetrievePromptVersionAsync(string name, string? commit = null, RequestOptions? options = null);
    Task<PromptVersionDetail> RestorePromptVersionAsync(string promptId, string versionId, RequestOptions? options = null);
}
```

### 4.8 `IAttachmentsClient`

```csharp
public interface IAttachmentsClient
{
    Task<AttachmentPage> AttachmentListAsync(AttachmentListRequest request, RequestOptions? options = null);
    Task<StartMultipartUploadResponse> StartMultiPartUploadAsync(StartMultipartUploadRequest request, RequestOptions? options = null);
    Task CompleteMultiPartUploadAsync(CompleteMultipartUploadRequest request, RequestOptions? options = null);
    Task UploadAttachmentAsync(UploadAttachmentRequest request, RequestOptions? options = null);
    IAsyncEnumerable<byte[]> DownloadAttachmentAsync(DownloadAttachmentRequest request, RequestOptions? options = null);
    Task DeleteAttachmentsAsync(DeleteAttachmentsRequest request, RequestOptions? options = null);
}
```

### 4.9 `ICheckClient`

```csharp
public interface ICheckClient
{
    Task AccessAsync(Dictionary<string, object?> authDetails, RequestOptions? options = null);
    Task<WorkspaceNameHolder> GetWorkspaceNameAsync(RequestOptions? options = null);
}
```

### 4.10 `IWorkspacesClient`

```csharp
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
```

---

## 5. Out-of-Scope Clients (Lower Priority)

These exist in the Python SDK but are lower priority for the initial port. Implement after the core clients above are stable:

| Client | Description |
|---|---|
| `AutomationRuleEvaluatorsClient` | Automated evaluation rule management |
| `OptimizationsClient` | Optimization experiment management |
| `LlmProviderKeyClient` | LLM provider API key management |
| `ServiceTogglesClient` | Feature flag management |
| `SystemUsageClient` | System usage monitoring |
| `ChatCompletionsClient` | Chat completion API proxy |
| `OpenTelemetryIngestionClient` | OpenTelemetry data ingestion |
| `GuardrailsClient` | Content validation |
| `RedirectClient` | URL redirection |

---

## 6. Project Structure Recommendation

```
OpikSimplSdk/
  OpikSimplSdk.Core/
    Models/
      Traces/          # TracePublic, TraceWrite, TracePagePublic, etc.
      Spans/           # SpanPublic, SpanWrite, SpanPagePublic, etc.
      Datasets/        # DatasetPublic, DatasetItemWrite, etc.
      Experiments/     # ExperimentPublic, ExperimentItem, etc.
      Projects/        # ProjectPublic, ProjectMetricResponsePublic, etc.
      Prompts/         # PromptDetail, PromptVersionDetail, etc.
      FeedbackScores/  # FeedbackScoreBatchItem, FeedbackCreate, etc.
      Attachments/     # AttachmentPage, MultipartUploadPart, etc.
      Workspaces/      # WorkspaceConfiguration, WorkspaceMetricResponse, etc.
      Common/          # RequestOptions, Comment, ProjectStatsPublic, etc.
    Clients/
      ITracesClient.cs
      ISpansClient.cs
      IDatasetsClient.cs
      IExperimentsClient.cs
      IProjectsClient.cs
      IFeedbackDefinitionsClient.cs
      IPromptsClient.cs
      IAttachmentsClient.cs
      ICheckClient.cs
      IWorkspacesClient.cs
  OpikSimplSdk.Http/
    IOpikClient.cs           # Root entry point exposing all sub-clients
    OpikClient.cs            # Concrete HttpClient-based implementation
    Clients/
      TracesClient.cs
      SpansClient.cs
      # ... etc
```

- Interfaces live in `OpikSimplSdk.Core` (no HTTP dependency).
- Concrete implementations using `HttpClient` live in `OpikSimplSdk.Http`.

---

## 7. Key Implementation Notes

### Authentication
The Python SDK sends the API key as a header. The header name used by Opik is `authorization` with value `Bearer <api_key>` or as `comet-sdk-api-key`. Verify against the actual API when implementing.

### Pagination
Most list methods follow the pattern: `page` (0-indexed), `size` (items per page). Return types include a page wrapper with `content`, `total`, `page`, `size` fields.

### Streaming
Python methods returning `Iterator[bytes]` are NDJSON (newline-delimited JSON) streams. In C#, read line by line and deserialize each line as the relevant entity type.

### `OMIT` vs `null`
Python distinguishes `OMIT` (field not sent) from `null` (field sent as null). In C#, use `Optional<T>` wrapper or simply omit fields with `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]`.

### Discriminated unions
`FeedbackCreate`, `FeedbackUpdate`, and `FeedbackPublic` have numerical and categorical variants (distinguished by `type` field). Use a base class with a `Type` discriminator property and subclasses for each variant.

### Bulk endpoint size limit
`ExperimentItemsBulk` has a **4 MB maximum request size** — document and enforce this in the C# implementation.

---

## 8. Milestone Plan (Converted from Epics)

This section converts the implementation epics into a delivery roadmap with dependencies and acceptance criteria.

### M1 — Foundation (Weeks 1–2)

**Goal:** Establish compile-ready SDK skeleton and shared contracts.

Includes:
- **Epic 1:** SDK Foundation & Client Entry Point
- **Epic 2:** Core Contracts & Shared Types
- **Epic 3:** HTTP Infrastructure Layer
- **Epic 4:** Serialization Strategy (Optional/Union/Flexible JSON)

Deliverables:
- `IOpikClient` and `OpikClient` exposing all required sub-clients.
- Core model/common folders created; placeholder `Class1.cs` replaced.
- Shared enums and `RequestOptions` implemented.
- Reusable HTTP pipeline (auth, serialization, error handling, timeout support).
- Union + optional/null/omit behavior defined and documented.

Exit criteria:
- Solution builds successfully.
- All client interfaces compile against shared contracts.
- Auth + serialization behavior validated with at least smoke-level tests.

#### M1 Task Breakdown (Implemented)

- [x] **M1.1 Core scaffolding cleanup**
    - Replace placeholder core scaffold file with a meaningful assembly marker.
    - Create foundational folder structure: `Common`, `Models`, `Clients`.

- [x] **M1.2 Shared config and request primitives**
    - Add `OpikClientConfig` (`BaseUrl`, `ApiKey`, `WorkspaceName`).
    - Add `RequestOptions` (`Timeout`, `ChunkSize`).

- [x] **M1.3 Shared enums**
    - Add enums for API literal parity:
        `FeedbackScoreSource`, `SpanType`, `FeedbackDefinitionType`, `Visibility`,
        `ExperimentStatus`, `ExperimentType`, `PromptType`, `EntityType`.

- [x] **M1.4 Core client contracts**
    - Define interfaces in Core for:
        `ITracesClient`, `ISpansClient`, `IDatasetsClient`, `IExperimentsClient`,
        `IProjectsClient`, `IFeedbackDefinitionsClient`, `IPromptsClient`,
        `IAttachmentsClient`, `ICheckClient`, `IWorkspacesClient`.

- [x] **M1.5 Discriminated-union foundations**
    - Add base + variant models for feedback definitions:
        `FeedbackCreate`, `FeedbackUpdate`, `FeedbackPublic` with numerical/categorical variants.

- [x] **M1.6 Optional/omit serialization foundation**
    - Add `Optional<T>` + JSON converter factory as baseline support for omit/null semantics.
    - Configure JSON defaults with null-ignoring behavior.

- [x] **M1.7 HTTP transport foundation**
    - Add reusable transport abstraction `IOpikHttpTransport`.
    - Add `OpikHttpTransport` with:
        - Base URL handling
        - Auth header mode selection (`authorization: Bearer` / `comet-sdk-api-key`)
        - Generic send helpers + response deserialization
        - Request-level timeout support

- [x] **M1.8 Root SDK entry point**
    - Define root `IOpikClient` contract exposing all required sub-clients.
    - Implement `OpikClient` constructor wiring config + transport.
    - Add explicit placeholders for M2–M4 concrete client implementations.

- [x] **M1.9 Project wiring and build validation**
    - Add project reference from `OpikSimplSdk.Http` -> `OpikSimplSdk.Core`.
    - Validate with solution build (`dotnet build OpikSimplSdk.sln`).

---

### M2 — Core Data Plane Clients (Weeks 3–5)

**Goal:** Implement highest-value ingestion/query workflows first.

Includes:
- **Epic 5:** Traces & Threads Vertical Slice
- **Epic 6:** Spans Vertical Slice
- **Epic 7:** Datasets Vertical Slice

Deliverables:
- Full `ITracesClient` support (CRUD, search, feedback, comments, threads).
- Full `ISpansClient` support (CRUD, search, feedback, stats, comments).
- Full `IDatasetsClient` support (CRUD, items, comparison, expansion).
- NDJSON streaming utility in production use for search/stream endpoints.

Exit criteria:
- All methods in `ITracesClient`, `ISpansClient`, `IDatasetsClient` implemented.
- Streaming + pagination behavior verified for each implemented client.

Dependencies:
- Requires M1 complete.

#### M2 Task Breakdown (In Progress)

- [x] **M2.1 HTTP client base abstractions**
    - Add shared `ClientBase` with query-string helper.

- [x] **M2.2 Streaming transport support**
    - Add transport-level byte streaming method for NDJSON endpoints.
    - Implement async line-by-line streaming over response body.

- [x] **M2.3 Traces client implementation skeleton**
    - Implement all `ITracesClient` methods in `TracesClient` with endpoint wiring.
    - Include threads/comments/feedback methods and stream search methods.

- [x] **M2.4 Spans client implementation skeleton**
    - Implement all `ISpansClient` methods in `SpansClient` with endpoint wiring.

- [x] **M2.5 Datasets client implementation skeleton**
    - Implement all `IDatasetsClient` methods in `DatasetsClient` with endpoint wiring.
    - Include dataset-item streaming method.

- [x] **M2.6 Root client composition**
    - Wire `Traces`, `Spans`, and `Datasets` into `OpikClient` using lazy initialization.

- [ ] **M2.7 Endpoint verification pass against live Opik API**
    - Validate route paths and payload contracts against real API docs/traffic.
    - Adjust provisional route names if needed.

- [ ] **M2.8 Behavior validation tests**
    - Add tests for pagination, stream framing, and request serialization per client.

---

### M3 — Experimentation & Configuration Surface (Weeks 6–8)

**Goal:** Deliver experiment lifecycle + project/prompt/feedback configuration APIs.

Includes:
- **Epic 8:** Experiments Vertical Slice
- **Epic 9:** Projects, Prompts, Feedback Definitions

Deliverables:
- Full `IExperimentsClient` implementation, including bulk/item streams.
- 4 MB `ExperimentItemsBulk` limit enforcement.
- Full `IProjectsClient`, `IPromptsClient`, `IFeedbackDefinitionsClient` implementations.

Exit criteria:
- All methods in the four interfaces above implemented.
- Bulk payload limit behavior documented and tested.
- Prompt version lifecycle operations validated.

Dependencies:
- Requires M1 complete.
- Benefits from M2 complete (shared paging/streaming reuse), but can overlap partially.

#### M3 Task Breakdown (Implemented)

- [x] **M3.1 Experiments client implementation skeleton**
    - Implement all `IExperimentsClient` methods in `ExperimentsClient`.
    - Add stream endpoints for experiments and experiment items.

- [x] **M3.2 4 MB bulk request guard**
    - Enforce `ExperimentItemsBulk` maximum payload size using serialized request byte count.
    - Throw explicit argument exception when payload exceeds 4 MB.

- [x] **M3.3 Projects client implementation skeleton**
    - Implement all `IProjectsClient` methods in `ProjectsClient`.

- [x] **M3.4 Prompts client implementation skeleton**
    - Implement all `IPromptsClient` methods in `PromptsClient`.
    - Include prompt version lifecycle methods (create/get/list/retrieve/restore).

- [x] **M3.5 Feedback definitions client implementation skeleton**
    - Implement all `IFeedbackDefinitionsClient` methods in `FeedbackDefinitionsClient`.

- [x] **M3.6 Root client composition**
    - Wire `Experiments`, `Projects`, `FeedbackDefinitions`, and `Prompts` into `OpikClient` via lazy initialization.

- [ ] **M3.7 API route and payload contract verification**
    - Confirm provisional endpoint paths and payload names against live Opik API behavior.

- [ ] **M3.8 Tests for bulk limit and prompt version flows**
    - Add tests for 4 MB guard, stream behavior, and prompt version lifecycle.

---

### M4 — Platform Utilities, Hardening, and Release (Weeks 9–10)

**Goal:** Complete remaining clients and ship a production-ready first release.

Includes:
- **Epic 10:** Attachments, Check, Workspaces
- **Epic 11:** Streaming, Pagination, and Consistency Hardening
- **Epic 12:** Quality, Validation, and Release Readiness

Deliverables:
- Full `IAttachmentsClient`, `ICheckClient`, `IWorkspacesClient` implementations.
- Cross-client consistency pass (errors, cancellation, retries, pagination, stream parsing).
- Test suite coverage for critical workflows.
- Developer docs and usage examples.

Exit criteria:
- Entire required client surface implemented.
- Build + tests pass in CI.
- v0.1.0 release checklist complete.

Dependencies:
- Requires M2 and M3 functionally complete.

---

## 9. Dependency Order (At-a-Glance)

1. **M1** must land first (contracts + transport + serialization).
2. **M2** and **M3** can proceed in parallel after M1.
3. **M4** starts after M2 + M3 are stable.

Suggested critical path:

`M1 -> M2 -> M4` with `M3` running parallel after `M1`.
