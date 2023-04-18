using ChatBot.Modals;
using ChatBot.Services;
using Discord;
using Discord.Interactions;
using ModalBuilder = Discord.Interactions.Builders.ModalBuilder;

namespace ChatBot.Modules;

public class SlashModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ai", "AI에게 질문해보세요!")]
    public async Task Ask()
    {
        await RespondWithModalAsync<GptAsk>("gpt_ask");
    }
    
    [SlashCommand("draw", "AI에게 그림을 그리게 해보세요!")]
    public async Task Draw()
    {
        await RespondWithModalAsync<GptDraw>("gpt_draw");
    }
}