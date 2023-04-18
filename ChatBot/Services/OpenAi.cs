using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;

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

    public async Task<List<String>> GetImageGen(string input)
    {
        var img = await _api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest(input));
        var links = img.Data.Select(x => x.Url).ToList();
        return links;
    }
    
}