namespace PetWorld.Infrastructure.Services.WriterCritic.Prompts;

public interface ICriticBuilder
{
    string BuildPrompt(string question, string answer, string catalog);
    string BuildInstructions(int feedbackMaxLength);
}
