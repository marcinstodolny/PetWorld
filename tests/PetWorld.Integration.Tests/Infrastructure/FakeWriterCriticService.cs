using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Models;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;

namespace PetWorld.Integration.Tests.Infrastructure;

public sealed class FakeWriterCriticService : IWriterCriticService
{
    private readonly Lock _sync = new();
    private FakeWriterCriticBehavior _behavior = FakeWriterCriticBehavior.Success("Domyślna odpowiedź testowa", 2);

    public void SetBehavior(FakeWriterCriticBehavior behavior)
    {
        lock (_sync)
        {
            _behavior = behavior;
        }
    }

    public Task<Result<WriterCriticResult>> GenerateResponseAsync(
        string question,
        IReadOnlyList<Product> products,
        string? modelOverride = null,
        CancellationToken cancellationToken = default)
    {
        FakeWriterCriticBehavior behavior;
        lock (_sync)
        {
            behavior = _behavior;
        }

        return behavior.Mode switch
        {
            FakeWriterCriticMode.Success => Task.FromResult(Result.Success(new WriterCriticResult(behavior.Answer!, behavior.Iterations))),
            FakeWriterCriticMode.InvalidResponse => Task.FromResult(Result.Success(new WriterCriticResult(string.Empty, behavior.Iterations))),
            FakeWriterCriticMode.Failure => Task.FromResult(Result.Fail<WriterCriticResult>(behavior.ErrorMessage!)),
            _ => throw new InvalidOperationException($"Unknown behavior mode: {behavior.Mode}")
        };
    }
}

public enum FakeWriterCriticMode
{
    Success,
    Failure,
    InvalidResponse
}

public sealed record FakeWriterCriticBehavior(FakeWriterCriticMode Mode, string? Answer, int Iterations, string? ErrorMessage)
{
    public static FakeWriterCriticBehavior Success(string answer, int iterations) => new(FakeWriterCriticMode.Success, answer, iterations, null);

    public static FakeWriterCriticBehavior Failure(string errorMessage) => new(FakeWriterCriticMode.Failure, null, 0, errorMessage);

    public static FakeWriterCriticBehavior InvalidResponse(int iterations = 2) => new(FakeWriterCriticMode.InvalidResponse, null, iterations, null);
}
