using OpenAI_API;
using OpenAI_API.Chat;

namespace ChatBot.Services;

public class OpenAi : IOpenAi
{
    private readonly OpenAIAPI _api;
    private readonly Conversation _chat;
    public OpenAi(string apiKey)
    {
        _api = new OpenAIAPI(new APIAuthentication(apiKey));
        _chat = _api.Chat.CreateConversation();
    }

    public async Task<string> GetResponse(string input)
    {
        _chat.AppendUserInput(input);
        return await _chat.GetResponseFromChatbotAsync();
    }
    
}