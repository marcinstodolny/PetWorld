using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Abstraction.Repository;
using PetWorld.Application.Models.Request;
using PetWorld.Application.Models.Response;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Application.UseCases.Chat;

public sealed class AskChatUseCase(
    IChatMessageRepository chatMessageRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IWriterCriticService writerCriticService)
{
    public async Task<Result<ChatResponse>> ExecuteAsync(
        AskChatRequest request,
        CancellationToken cancellationToken = default)
    {
        var questionResult = Question.Create(request.Question);
        if (questionResult.IsFailed)
        {
            return Result.Fail<ChatResponse>(questionResult.Errors);
        }

        var products = await productRepository.GetAllAsync(cancellationToken);

        var responseResult = await writerCriticService.GenerateResponseAsync(
            questionResult.Value.Value,
            products,
            request.Model,
            cancellationToken);

        if (responseResult.IsFailed)
        {
            return Result.Fail<ChatResponse>(responseResult.Errors);
        }

        var answerResult = Answer.Create(responseResult.Value.Answer);
        if (answerResult.IsFailed)
        {
            return Result.Fail<ChatResponse>(answerResult.Errors);
        }

        var iterationResult = IterationCount.Create(responseResult.Value.Iterations);
        if (iterationResult.IsFailed)
        {
            return Result.Fail<ChatResponse>(iterationResult.Errors);
        }

        var message = ChatMessage.Create(questionResult.Value, answerResult.Value, iterationResult.Value);
        await chatMessageRepository.AddAsync(message, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new ChatResponse(answerResult.Value.Value, iterationResult.Value.Value));
    }
}
