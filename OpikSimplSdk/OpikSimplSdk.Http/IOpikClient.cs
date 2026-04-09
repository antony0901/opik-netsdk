using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Core.Common;

namespace OpikSimplSdk.Http;

/// <summary>
/// Root SDK entry point exposing all Opik API client groups.
/// </summary>
public interface IOpikClient
{
	/// <summary>Gets the client configuration used to initialize the SDK.</summary>
	OpikClientConfig Config { get; }

	/// <summary>Gets the traces client.</summary>
	ITracesClient Traces { get; }
	/// <summary>Gets the spans client.</summary>
	ISpansClient Spans { get; }
	/// <summary>Gets the datasets client.</summary>
	IDatasetsClient Datasets { get; }
	/// <summary>Gets the experiments client.</summary>
	IExperimentsClient Experiments { get; }
	/// <summary>Gets the projects client.</summary>
	IProjectsClient Projects { get; }
	/// <summary>Gets the feedback definitions client.</summary>
	IFeedbackDefinitionsClient FeedbackDefinitions { get; }
	/// <summary>Gets the prompts client.</summary>
	IPromptsClient Prompts { get; }
	/// <summary>Gets the attachments client.</summary>
	IAttachmentsClient Attachments { get; }
	/// <summary>Gets the access check client.</summary>
	ICheckClient Check { get; }
	/// <summary>Gets the workspaces client.</summary>
	IWorkspacesClient Workspaces { get; }
}