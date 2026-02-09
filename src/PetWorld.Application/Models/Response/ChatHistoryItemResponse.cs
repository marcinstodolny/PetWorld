namespace PetWorld.Application.Models.Response;

public sealed record ChatHistoryItemResponse(
    DateTime CreatedAt,
    string Question,
    string Answer,
    int Iterations);
