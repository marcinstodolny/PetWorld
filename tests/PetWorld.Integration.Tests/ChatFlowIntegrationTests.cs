using FluentAssertions;
using PetWorld.Application.Models.Request;
using PetWorld.Application.Models.Response;
using PetWorld.Integration.Tests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace PetWorld.Integration.Tests;

[Collection(IntegrationTestsCollection.Name)]
public sealed class ChatFlowIntegrationTests(IntegrationTestFixture fixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        if (!fixture.IsDockerAvailable)
        {
            return;
        }

        await fixture.Respawner!.ResetAsync();
        fixture.Factory!.FakeWriterCriticService.SetBehavior(FakeWriterCriticBehavior.Success("Testowa odpowiedź AI", 2));
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [SkippableFact]
    public async Task HappyPath_ShouldSaveChatMessage_AndReturnItInHistory()
    {
        Skip.IfNot(fixture.IsDockerAvailable, fixture.DockerUnavailableReason);
        var client = fixture.CreateClient();

        var response = await client.PostAsJsonAsync("/test-api/chat", new AskChatRequest("Jaką karmę polecasz dla kota?", "gpt-4o-mini"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var chatResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();
        chatResponse.Should().NotBeNull();
        chatResponse!.Answer.Should().Be("Testowa odpowiedź AI");

        var historyResponse = await client.GetAsync("/test-api/chat/history");

        historyResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var history = await historyResponse.Content.ReadFromJsonAsync<IReadOnlyList<ChatHistoryItemResponse>>();
        history.Should().NotBeNull();
        history.Should().ContainSingle();
        history![0].Question.Should().Be("Jaką karmę polecasz dla kota?");
        history[0].Answer.Should().Be("Testowa odpowiedź AI");
    }

    [SkippableFact]
    public async Task OpenAiFailure_ShouldReturnBadRequestWithError()
    {
        Skip.IfNot(fixture.IsDockerAvailable, fixture.DockerUnavailableReason);
        var client = fixture.CreateClient();
        fixture.Factory!.FakeWriterCriticService.SetBehavior(FakeWriterCriticBehavior.Failure("OpenAI temporary failure"));

        var response = await client.PostAsJsonAsync("/test-api/chat", new AskChatRequest("Czy macie karmę dla psa?", "gpt-4o-mini"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var payload = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        payload.Should().NotBeNull();
        payload!.Errors.Should().Contain("OpenAI temporary failure");
    }

    [SkippableFact]
    public async Task ValidationFailure_ShouldReturnBadRequest_WhenQuestionIsEmpty()
    {
        Skip.IfNot(fixture.IsDockerAvailable, fixture.DockerUnavailableReason);
        var client = fixture.CreateClient();

        var response = await client.PostAsJsonAsync("/test-api/chat", new AskChatRequest("   ", "gpt-4o-mini"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var payload = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        payload.Should().NotBeNull();
        payload!.Errors.Should().Contain("Question cannot be empty.");
    }

    private sealed record ErrorResponse(string[] Errors);
}
