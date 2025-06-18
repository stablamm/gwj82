using Godot;
using GWJ.Autoloads;

namespace GWJ.Scenes.Dialogues;

public partial class DialogueBox : Node2D
{
    public enum AnimationType
    {
        Default,
        FadeIn,
        FadeOut
    }

    private RichTextLabel OutputLabel;
    private AnimationPlayer AnimPlayer;

    public override void _Ready()
    {
        OutputLabel = GetNode<RichTextLabel>("%OutputLabel");
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");

        SignalManager.Instance.Connect(
            nameof(SignalManager.DialogueBox_Show_EventHandler),
            new Callable(this, nameof(ShowDialogueBox))
        );

        SignalManager.Instance.Connect(
            nameof(SignalManager.DialogueBox_Hide_EventHandler),
            new Callable(this, nameof(HideDialogueBox))
        );

        SignalManager.Instance.Connect(
            nameof(SignalManager.DialogueBox_SetText_EventHandler),
            new Callable(this, nameof(SetText))
        );

        SignalManager.Instance.Connect(
            nameof(SignalManager.DialogueBox_ClearText_EventHandler),
            new Callable(this, nameof(ClearText))
        );

        AnimPlayer.Play(AnimationType.Default.ToString());
    }

    public void ShowDialogueBox()
    {
        AnimPlayer.Play(AnimationType.FadeIn.ToString());
    }

    public void HideDialogueBox()
    {
        AnimPlayer.Play(AnimationType.FadeOut.ToString());
    }
    
    public void SetText(string text)
    {
        OutputLabel.Text = text;
    }

    public void ClearText()
    {
        OutputLabel.Clear();
    }
}
