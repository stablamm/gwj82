using Godot;
using Godot.Collections;
using System;

namespace GWJ.Scenes.Portraits;

public partial class Portrait : Node2D
{
    public enum PortraitAction
    {
        Blink,
        Yes,
        No
    }

    public enum PortraitType
    {
        BadGuy
    }

    public PortraitAction Action { get; private set; } = PortraitAction.Blink;
    public PortraitType Type { get; private set; } = PortraitType.BadGuy;

    private Dictionary<PortraitType, Texture2D> portraitTextures = new Dictionary<PortraitType, Texture2D>
    {
        { PortraitType.BadGuy, GD.Load<Texture2D>("res://Assets/bad_guy_portrait.png") },
    };

    private AnimationPlayer AnimPlayer;
    private Sprite2D HeadSprite;

    public override void _Ready()
    {
        HeadSprite = GetNode<Sprite2D>("HeadSprite");
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");
        
        AnimPlayer.Play(Action.ToString());
        HeadSprite.Texture = portraitTextures[Type];
    }

    public void SetAction(PortraitAction action)
    {
        Action = action;
        AnimPlayer.Play(Action.ToString());
    }

    public void SetType(PortraitType type)
    {
        Type = type;
    }
}
