using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Http;

public interface IOpikClient
{
	OpikClientConfig Config { get; }

	ITracesClient Traces { get; }
	ISpansClient Spans { get; }
	IDatasetsClient Datasets { get; }
	IExperimentsClient Experiments { get; }
	IProjectsClient Projects { get; }
	IFeedbackDefinitionsClient FeedbackDefinitions { get; }
	IPromptsClient Prompts { get; }
	IAttachmentsClient Attachments { get; }
	ICheckClient Check { get; }
	IWorkspacesClient Workspaces { get; }
}