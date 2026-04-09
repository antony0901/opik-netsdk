using OpikSimplSdk.Core.Clients;
using OpikSimplSdk.Http;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class OpikClientTests
{
    [Fact]
    public void ShouldExposeConfiguredClientGroups()
    {
        var (client, _) = TestClientFactory.CreateOpikClient();

        Assert.NotNull(client.Traces);
        Assert.NotNull(client.Spans);
        Assert.NotNull(client.Datasets);
        Assert.NotNull(client.Experiments);
        Assert.NotNull(client.Projects);
        Assert.NotNull(client.FeedbackDefinitions);
        Assert.NotNull(client.Prompts);
    }

    [Fact]
    public void ShouldThrowForM4ClientGroups()
    {
        var (client, _) = TestClientFactory.CreateOpikClient();

        Assert.Throws<NotImplementedException>(() => _ = client.Attachments);
        Assert.Throws<NotImplementedException>(() => _ = client.Check);
        Assert.Throws<NotImplementedException>(() => _ = client.Workspaces);
    }

    [Fact]
    public void ShouldExposeCorrectInterfaceTypes()
    {
        var (client, _) = TestClientFactory.CreateOpikClient();

        Assert.IsAssignableFrom<ITracesClient>(client.Traces);
        Assert.IsAssignableFrom<ISpansClient>(client.Spans);
        Assert.IsAssignableFrom<IDatasetsClient>(client.Datasets);
        Assert.IsAssignableFrom<IExperimentsClient>(client.Experiments);
        Assert.IsAssignableFrom<IProjectsClient>(client.Projects);
        Assert.IsAssignableFrom<IFeedbackDefinitionsClient>(client.FeedbackDefinitions);
        Assert.IsAssignableFrom<IPromptsClient>(client.Prompts);
    }
}
