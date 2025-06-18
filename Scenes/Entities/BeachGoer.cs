using Godot;
using System;

namespace GWJ.Scenes.Entities;

public partial class BeachGoer : CharacterBody2D
{
    [Export] public Texture2D[] BeachGoerTextures { get; set; } = Array.Empty<Texture2D>();

    public float WanderSpeed { get; set; } = 40f;
    public float ObstacleCheckDistance { get; set; } = 32f;
    public int MaxDirectionAttempts { get; set; } = 5;

    private Vector2 WanderDirection = Vector2.Zero;

    private BodyAnimation Body;

    public override void _Ready()
    {
        Body = GetNode<BodyAnimation>("BodyAnimation");
        if (BeachGoerTextures.Length > 0)
        {
            Body.UpdateTexture(BeachGoerTextures[GD.Randi() % BeachGoerTextures.Length]);
        }
        else
        {
            GD.PrintErr("No textures provided for BeachGoer.");
        }

        OnWanderTimerTimeout();
        Body.UpdateAnimationState(BodyAnimation.AnimationState.Walk);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = WanderDirection * WanderSpeed;
        Body.UpdateBlendPosition(Velocity.Normalized());
        MoveAndSlide();
    }

    private void OnWanderTimerTimeout()
    {
        for (int i = 0; i < MaxDirectionAttempts; i++)
        {
            float angle = (float)GD.RandRange(0, Math.Tau);
            Vector2 testDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized();

            if (!IsSandcastleAhead(testDir))
            {
                WanderDirection = testDir;
                return;
            }
        }

        // Fallback if all directions are blocked
        WanderDirection = Vector2.Zero;
        GD.Print("All directions blocked by sandcastles, stopping wander.");
    }

    private bool IsSandcastleAhead(Vector2 direction)
    {
        var spaceState = GetWorld2D().DirectSpaceState;

        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + direction * ObstacleCheckDistance);
        query.CollisionMask = 1;
        query.CollideWithAreas = true;

        var result = spaceState.IntersectRay(query);
        if (result.Count > 0 && result["collider"].Obj is Node collider)
        {
            return collider.IsInGroup("sandcastles");
        }

        return false;
    }
}
