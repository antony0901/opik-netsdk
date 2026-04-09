using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class ProjectsClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("FindProjects", HttpMethod.Post, "/v1/projects/find", async c => _ = await c.Projects.FindProjectsAsync(new FindProjectsRequest()));
        yield return Case("CreateProject", HttpMethod.Post, "/v1/projects", c => c.Projects.CreateProjectAsync(new CreateProjectRequest()));
        yield return Case("GetProjectById", HttpMethod.Get, "/v1/projects/p1", async c => _ = await c.Projects.GetProjectByIdAsync("p1"));
        yield return Case("RetrieveProject", HttpMethod.Get, "/v1/projects/retrieve?name=my-project", async c => _ = await c.Projects.RetrieveProjectAsync("my-project"));
        yield return Case("UpdateProject", HttpMethod.Patch, "/v1/projects/p1", c => c.Projects.UpdateProjectAsync("p1", new UpdateProjectRequest()));
        yield return Case("DeleteProject", HttpMethod.Delete, "/v1/projects/p1", c => c.Projects.DeleteProjectByIdAsync("p1"));
        yield return Case("DeleteProjectsBatch", HttpMethod.Post, "/v1/projects/delete", c => c.Projects.DeleteProjectsBatchAsync(["p1"]));
        yield return Case("FindFeedbackScoreNames", HttpMethod.Get, "/v1/projects/feedback-scores/names?projectIds=p1%2Cp2", async c => _ = await c.Projects.FindFeedbackScoreNamesByProjectIdsAsync("p1,p2"));
        yield return Case("GetProjectMetrics", HttpMethod.Post, "/v1/projects/p1/metrics", async c => _ = await c.Projects.GetProjectMetricsAsync("p1", new GetProjectMetricsRequest()));
        yield return Case("GetProjectStats", HttpMethod.Post, "/v1/projects/stats", async c => _ = await c.Projects.GetProjectStatsAsync(new GetProjectStatsRequest()));
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task ShouldCallExpectedEndpoint(TracesClientTests.ClientCallCase testCase)
    {
        var (client, handler) = TestClientFactory.CreateOpikClient((_, _) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(testCase.ResponseBody, System.Text.Encoding.UTF8, "application/json")
        }));

        await testCase.Invoke(client);

        var request = Assert.Single(handler.Requests);
        Assert.Equal(testCase.Method, request.Method);
        Assert.Equal(testCase.PathAndQuery, request.PathAndQuery);
    }

    private static object[] Case(string name, HttpMethod method, string pathAndQuery, Func<OpikSimplSdk.Http.OpikClient, Task> invoke)
        => [new TracesClientTests.ClientCallCase(name, method, pathAndQuery, invoke, "{}", false)];
}
