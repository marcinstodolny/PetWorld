namespace PetWorld.Application.Models;

public sealed record WriterCriticResult(
    string Answer,
    int Iterations,
    bool Approved,
    string? Feedback);
