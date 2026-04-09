using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Http.Clients;
using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http;

public sealed class OpikClient : IOpikClient
{
    private readonly Lazy<ITracesClient> _traces;
    private readonly Lazy<ISpansClient> _spans;
    private readonly Lazy<IDatasetsClient> _datasets;

    public OpikClientConfig Config { get; }
    public IOpikHttpTransport Transport { get; }

    public ITracesClient Traces => _traces.Value;
    public ISpansClient Spans => _spans.Value;
    public IDatasetsClient Datasets => _datasets.Value;
    public IExperimentsClient Experiments => throw new NotImplementedException("M3: Experiments client implementation pending.");
    public IProjectsClient Projects => throw new NotImplementedException("M3: Projects client implementation pending.");
    public IFeedbackDefinitionsClient FeedbackDefinitions => throw new NotImplementedException("M3: Feedback definitions client implementation pending.");
    public IPromptsClient Prompts => throw new NotImplementedException("M3: Prompts client implementation pending.");
    public IAttachmentsClient Attachments => throw new NotImplementedException("M4: Attachments client implementation pending.");
    public ICheckClient Check => throw new NotImplementedException("M4: Check client implementation pending.");
    public IWorkspacesClient Workspaces => throw new NotImplementedException("M4: Workspaces client implementation pending.");

    public OpikClient(OpikClientConfig config, HttpClient? httpClient = null, AuthHeaderMode authHeaderMode = AuthHeaderMode.AuthorizationBearer)
    {
        Config = config;
        Transport = new OpikHttpTransport(httpClient ?? new HttpClient(), config, authHeaderMode);

        _traces = new Lazy<ITracesClient>(() => new TracesClient(Transport));
        _spans = new Lazy<ISpansClient>(() => new SpansClient(Transport));
        _datasets = new Lazy<IDatasetsClient>(() => new DatasetsClient(Transport));
    }
}
