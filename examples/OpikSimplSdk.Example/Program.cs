using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Http;
using OpikSimplSdk.Http.Infrastructure;
using OpikSimplSdk.Core.Clients;

// ---------------------------------------------------------------------------
// 1) Create the root client
// ---------------------------------------------------------------------------
var config = new OpikClientConfig
{
    BaseUrl = "http://localhost:5173/api",
    ApiKey = "<YOUR_API_KEY>",
    WorkspaceName = "default"
};

var client = new OpikClient(config);
Console.WriteLine("Client created.");

// ---------------------------------------------------------------------------
// 2) Traces
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Traces ---");

await client.Traces.CreateTraceAsync(new CreateTraceRequest());
var traceId = Guid.NewGuid().ToString();

var trace = await client.Traces.GetTraceByIdAsync(traceId);

await client.Traces.AddTraceFeedbackScoreAsync(
    traceId,
    new FeedbackScoreRequest());

await foreach (var line in client.Traces.SearchTracesAsync(new SearchTracesRequest()))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}

// ---------------------------------------------------------------------------
// 3) Spans
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Spans ---");

await client.Spans.CreateSpanAsync(new CreateSpanRequest());

var span = await client.Spans.GetSpanByIdAsync("span-id");

await client.Spans.AddSpanCommentAsync(
    "span-id",
    new CommentRequest { Text = "Looks good" });

// ---------------------------------------------------------------------------
// 4) Datasets
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Datasets ---");

await client.Datasets.CreateDatasetAsync(new CreateDatasetRequest());

var dataset = await client.Datasets.GetDatasetByNameAsync("my-dataset");

await foreach (var line in client.Datasets.StreamDatasetItemsAsync("my-dataset", steamLimit: 100))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}

// ---------------------------------------------------------------------------
// 5) Experiments
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Experiments ---");

await client.Experiments.CreateExperimentAsync(new CreateExperimentRequest());

await client.Experiments.ExperimentItemsBulkAsync(new ExperimentItemsBulkRequest());

await foreach (var line in client.Experiments.StreamExperimentsAsync("my-experiment", limit: 50))
{
    var ndjsonLine = System.Text.Encoding.UTF8.GetString(line);
    Console.WriteLine(ndjsonLine);
}

// ---------------------------------------------------------------------------
// 6) Projects / Prompts / Feedback Definitions
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Projects / Prompts / Feedback Definitions ---");

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

// ---------------------------------------------------------------------------
// 7) Using a custom HttpClient
// ---------------------------------------------------------------------------
Console.WriteLine("\n--- Custom HttpClient ---");

var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

var clientWithCustomHttp = new OpikClient(
    new OpikClientConfig
    {
        BaseUrl = "https://api.comet.com/opik",
        ApiKey = "<YOUR_API_KEY>"
    },
    httpClient,
    AuthHeaderMode.AuthorizationBearer);

Console.WriteLine("Client with custom HttpClient created.");
