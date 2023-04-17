using Discord;
using Discord.Interactions;

namespace ChatBot.Modals;

public class GptAsk : IModal
{
    public string Title => "📝 AI에게 질문하기";
    [InputLabel("질문 내용")]
    [ModalTextInput("input", placeholder: "무엇이든지 물어보셈", style: TextInputStyle.Paragraph)]
    public required string Input { get; set; }
}