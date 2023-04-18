using Discord;
using Discord.Interactions;

namespace ChatBot.Modals;

public class GptDraw : IModal
{
    public string Title => "✏️ 그림 그리기";
    
    [InputLabel("그림 내용")]
    [ModalTextInput("input", placeholder: "무엇이든지 말해보셈", style: TextInputStyle.Paragraph)]
    public required string Input { get; set; }
}