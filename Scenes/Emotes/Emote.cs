using Godot;
using System;

namespace GWJ.Scenes.Emotes;

public partial class Emote : Node2D
{
    public enum EmoteTimer
    {
        VeryShort = 1,
        Short = 3,
        Medium = 5,
        Long = 8
    }

    public enum EmoteType
    {
        Default,
        Exclaimation,
        QuestionMark,
    }

    private EmoteType currentEmote = EmoteType.Default;

    private Sprite2D Sprite;
    private AnimationPlayer AnimPlayer;
    private Timer DisplayTimer;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite");
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");
        DisplayTimer = GetNode<Timer>("DisplayTimer");

        ResetEmote();
    }

    private void OnDisplayTimerTimeout()
    {
        ResetEmote();
    }

    public void ResetEmote()
    {
        currentEmote = EmoteType.Default;
        AnimPlayer.Play(currentEmote.ToString());
    }

    public void PlayEmote(EmoteType e, EmoteTimer t)
    {
        currentEmote = e;
        AnimPlayer.Play(currentEmote.ToString());
        
        DisplayTimer.WaitTime = (float)t;
        DisplayTimer.OneShot = true;
        DisplayTimer.Start();
    }
}
