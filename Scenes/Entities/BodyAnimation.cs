using Godot;

namespace GWJ.Scenes.Entities;

public partial class BodyAnimation : Node2D
{
    [Export] public Texture2D SpriteTexture;

    public enum AnimationState
    {
        Idle,
        Walk,
        Hit
    }

    private Sprite2D AnimSprite;
    private AnimationTree AnimTree;
    private AnimationNodeStateMachinePlayback AnimStateMachine;

    public override void _Ready()
    {
        AnimSprite = GetNode<Sprite2D>("AnimSprite");
        AnimTree = GetNode<AnimationTree>("AnimTree");
        AnimTree.Active = true;

        AnimStateMachine = (AnimationNodeStateMachinePlayback)AnimTree.Get("parameters/playback");
        AnimStateMachine.Travel("idle");

        if (SpriteTexture != null)
        {
            AnimSprite.Texture = SpriteTexture;
        }
    }

    public void UpdateTexture(Texture2D newTexture)
    {
        if (newTexture != null)
        {
            SpriteTexture = newTexture;
            AnimSprite.Texture = SpriteTexture;
        }
        else
        {
            GD.PrintErr("Attempted to update BodyAnimation texture with null value.");
        }
    }

    public void UpdateBlendPosition(Vector2 blendPosition)
    {
        AnimTree.Set("parameters/idle/blend_position", blendPosition);
        AnimTree.Set("parameters/walk/blend_position", blendPosition);
        AnimTree.Set("parameters/hit/blend_position", blendPosition);
    }

    public void UpdateAnimationState(AnimationState stateName)
    {
        if (AnimStateMachine.GetCurrentNode() != stateName.ToString().ToLower())
        {
            AnimStateMachine.Travel(stateName.ToString().ToLower());
        }
    }
}
