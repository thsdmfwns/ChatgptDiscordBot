using ChatBot.Modals;
using ChatBot.Services;
using Discord;
using Discord.Interactions;

namespace ChatBot.Modules;

public class ModalModule : InteractionModuleBase
{
    private readonly IOpenAi _openAi;

    public ModalModule(IOpenAi openAi)
    {
        _openAi = openAi;
    }

    [ModalInteraction("gpt_ask")]
    public async Task ModalResponse(GptAsk modal)
    {
        try
        {
            var askEb = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithDescription(modal.Input)
                .WithCurrentTimestamp()
                .Build();
            await RespondAsync("시간이 좀 걸리니 좀만 기달리쇼!", ephemeral:true);
            Context.Channel.TriggerTypingAsync(new RequestOptions());
            var res = await _openAi.GetResponse(modal.Input);
            var resEb = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithDescription(res)
                .WithCurrentTimestamp()
                .Build();
            await Context.Channel.SendMessageAsync(embeds: new []{askEb, resEb});
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType().ToString());
            Console.WriteLine(ex.Message);
        }
    }
    
    [ModalInteraction("gpt_draw")]
    public async Task DrawModalResponse(GptDraw modal)
    {
        try
        {
            var askEb = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithDescription(modal.Input)
                .WithCurrentTimestamp()
                .Build();
            await RespondAsync("시간이 좀 걸리니 좀만 기달리쇼!", ephemeral:true);
            Context.Channel.TriggerTypingAsync(new RequestOptions());
            var res = await _openAi.GetImageGen(modal.Input);
            var resEb = res.Select(x => new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithImageUrl(x)
                .WithCurrentTimestamp()
                .Build()).ToList();
            var list = new List<Embed>();
            list.Add(askEb);
            list.AddRange(resEb);
            await Context.Channel.SendMessageAsync(embeds: list.ToArray());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType().ToString());
            Console.WriteLine(ex.Message);
        }
    }
}