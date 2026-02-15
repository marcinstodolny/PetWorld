namespace PetWorld.Infrastructure.Services.WriterCritic.Prompts;

public interface IWriterBuilder
{
    string BuildPrompt(string question, string catalog, string? feedback);
    string BuildInstructions();
}
