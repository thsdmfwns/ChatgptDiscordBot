namespace ChatBot.Services;

public interface IOpenAi
{
    Task<string> GetResponse(string input);
}