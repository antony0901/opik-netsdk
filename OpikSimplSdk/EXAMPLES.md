# Opik .NET SDK — Usage Examples

## 1) Create the root client

```csharp
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Http;

var config = new OpikClientConfig
{
    BaseUrl = "https://api.comet.com/opik",
    ApiKey = "<YOUR_API_KEY>",
    WorkspaceName = "default"
};

var client = new OpikClient(config);
```

---

## 2) Traces

```csharp
using OpikSimplSdk.Core.Models;

await client.Traces.CreateTraceAsync(new CreateTraceRequest());

var trace = await client.Traces.GetTraceByIdAsync("trace-id");

await client.Traces.AddTraceFeedbackScoreAsync(
    "trace-id",
    new FeedbackScoreRequest());

await foreach (var line in client.Traces.SearchTracesAsync(new SearchTracesRequest()))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}
```

---

## 3) Spans

```csharp
using OpikSimplSdk.Core.Models;

await client.Spans.CreateSpanAsync(new CreateSpanRequest());

var span = await client.Spans.GetSpanByIdAsync("span-id");

await client.Spans.AddSpanCommentAsync(
    "span-id",
    new OpikSimplSdk.Core.Clients.CommentRequest { Text = "Looks good" });
```

---

## 4) Datasets

```csharp
using OpikSimplSdk.Core.Models;

await client.Datasets.CreateDatasetAsync(new CreateDatasetRequest());

var dataset = await client.Datasets.GetDatasetByNameAsync("my-dataset");

await foreach (var line in client.Datasets.StreamDatasetItemsAsync("my-dataset", steamLimit: 100))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}
```

---

## 5) Experiments

```csharp
using OpikSimplSdk.Core.Models;

await client.Experiments.CreateExperimentAsync(new CreateExperimentRequest());

await client.Experiments.ExperimentItemsBulkAsync(new ExperimentItemsBulkRequest());

await foreach (var line in client.Experiments.StreamExperimentsAsync("my-experiment", limit: 50))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}
```

---

## 6) Projects / Prompts / Feedback Definitions

```csharp
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;

var projects = await client.Projects.FindProjectsAsync(new FindProjectsRequest());

var prompts = await client.Prompts.GetPromptsAsync(new GetPromptsRequest());

await client.FeedbackDefinitions.CreateFeedbackDefinitionAsync(
    new NumericalFeedbackCreate
    {
        Name = "quality",
        Type = FeedbackDefinitionType.Numerical,
        Min = 0,
        Max = 1
    });
```

---

## 7) Using custom `HttpClient`

```csharp
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Http;
using OpikSimplSdk.Http.Infrastructure;

var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

var client = new OpikClient(
    new OpikClientConfig
    {
        BaseUrl = "https://api.comet.com/opik",
        ApiKey = "<YOUR_API_KEY>"
    },
    httpClient,
    AuthHeaderMode.AuthorizationBearer);
```

---

## Notes

- Current SDK shape is scaffold-first and method request/response models are placeholders.
- Streaming APIs return NDJSON lines as `IAsyncEnumerable<byte[]>`.
- `Attachments`, `Check`, and `Workspaces` are not implemented yet.
