using OpikSimplSdk.Core.Common;
using OpikSimplSdk.Core.Models;
using OpikSimplSdk.Tests.TestInfrastructure;

namespace OpikSimplSdk.Tests;

public sealed class FeedbackDefinitionsClientTests
{
    public static IEnumerable<object[]> Cases()
    {
        yield return Case("FindDefinitions", HttpMethod.Get, "/v1/feedback-definitions?page=0&size=10&name=score&type=Numerical", async c => _ = await c.FeedbackDefinitions.FindFeedbackDefinitionsAsync(0, 10, "score", FeedbackDefinitionType.Numerical));
        yield return Case("CreateDefinition", HttpMethod.Post, "/v1/feedback-definitions", c => c.FeedbackDefinitions.CreateFeedbackDefinitionAsync(new NumericalFeedbackCreate { Name = "n", Type = FeedbackDefinitionType.Numerical }));
        yield return Case("GetDefinition", HttpMethod.Get, "/v1/feedback-definitions/f1", async c =>
        {
            await Assert.ThrowsAnyAsync<NotSupportedException>(() => c.FeedbackDefinitions.GetFeedbackDefinitionByIdAsync("f1"));
        });
        yield return Case("UpdateDefinition", HttpMethod.Patch, "/v1/feedback-definitions/f1", c => c.FeedbackDefinitions.UpdateFeedbackDefinitionAsync("f1", new NumericalFeedbackUpdate { Type = FeedbackDefinitionType.Numerical }));
        yield return Case("DeleteDefinition", HttpMethod.Delete, "/v1/feedback-definitions/f1", c => c.FeedbackDefinitions.DeleteFeedbackDefinitionByIdAsync("f1"));
        yield return Case("DeleteDefinitionsBatch", HttpMethod.Post, "/v1/feedback-definitions/delete", c => c.FeedbackDefinitions.DeleteFeedbackDefinitionsBatchAsync(["f1"]));
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
