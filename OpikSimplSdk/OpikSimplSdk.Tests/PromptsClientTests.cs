using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class PromptsClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("GetPrompts", HttpMethod.Post, "/v1/prompts/find", async c => _ = await c.Prompts.GetPromptsAsync(new GetPromptsRequest()));
        yield return Case("CreatePrompt", HttpMethod.Post, "/v1/prompts", c => c.Prompts.CreatePromptAsync(new CreatePromptRequest()));
        yield return Case("GetPromptById", HttpMethod.Get, "/v1/prompts/p1", async c => _ = await c.Prompts.GetPromptByIdAsync("p1"));
        yield return Case("UpdatePrompt", HttpMethod.Patch, "/v1/prompts/p1", c => c.Prompts.UpdatePromptAsync("p1", new UpdatePromptRequest()));
        yield return Case("DeletePrompt", HttpMethod.Delete, "/v1/prompts/p1", c => c.Prompts.DeletePromptAsync("p1"));
        yield return Case("DeletePromptsBatch", HttpMethod.Post, "/v1/prompts/delete", c => c.Prompts.DeletePromptsBatchAsync(["p1"]));
        yield return Case("CreatePromptVersion", HttpMethod.Post, "/v1/prompt-versions?name=my-prompt", async c => _ = await c.Prompts.CreatePromptVersionAsync("my-prompt", new PromptVersionDetail()));
        yield return Case("GetPromptVersionById", HttpMethod.Get, "/v1/prompt-versions/v1", async c => _ = await c.Prompts.GetPromptVersionByIdAsync("v1"));
        yield return Case("GetPromptVersions", HttpMethod.Get, "/v1/prompts/p1/versions?page=0&size=20", async c => _ = await c.Prompts.GetPromptVersionsAsync("p1", 0, 20));
        yield return Case("RetrievePromptVersion", HttpMethod.Get, "/v1/prompt-versions/retrieve?name=my-prompt&commit=abc", async c => _ = await c.Prompts.RetrievePromptVersionAsync("my-prompt", "abc"));
        yield return Case("RestorePromptVersion", HttpMethod.Post, "/v1/prompts/p1/versions/v1/restore", async c => _ = await c.Prompts.RestorePromptVersionAsync("p1", "v1"));
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
