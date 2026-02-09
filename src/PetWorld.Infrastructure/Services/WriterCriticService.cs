using System.Text;
using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Models;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;

namespace PetWorld.Infrastructure.Services;

public sealed class WriterCriticService : IWriterCriticService //TODO : Implement real AI logic here, this is just a placeholder for test purposes.
{
    public Task<Result<WriterCriticResult>> GenerateResponseAsync(
        string question,
        IReadOnlyList<Product> products,
        CancellationToken cancellationToken = default)
    {
        var response = BuildResponse(question, products);
        var result = new WriterCriticResult(response, Iterations: 1, Approved: true, Feedback: null);

        return Task.FromResult(Result.Success(result));
    }

    private static string BuildResponse(string question, IReadOnlyList<Product> products)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Dziękujemy za pytanie!");
        builder.AppendLine($"Pytanie: {question}");

        if (products.Count == 0)
        {
            builder.AppendLine("Aktualnie nie mamy produktów w katalogu.");
            return builder.ToString().Trim();
        }

        builder.AppendLine("Polecane produkty z naszego katalogu:");

        foreach (var product in products.Take(3))
        {
            builder.AppendLine($"- {product.Name.Value} ({product.Category}) — {product.Price.Amount} {product.Price.Currency}: {product.Description.Value}");
        }

        return builder.ToString().Trim();
    }
}
