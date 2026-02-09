using PetWorld.Application.Models;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;

namespace PetWorld.Application.Abstraction.AI;

public interface IWriterCriticService
{
    Task<Result<WriterCriticResult>> GenerateResponseAsync(
        string question,
        IReadOnlyList<Product> products,
        CancellationToken cancellationToken = default);
}
