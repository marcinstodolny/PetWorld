namespace PetWorld.Infrastructure.Services.WriterCritic.Parsing;

public interface ICriticResponseParser
{
    (bool Approved, string Feedback) Parse(string response, int feedbackMaxLength, string defaultFeedback);
}