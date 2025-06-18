using Godot;
using GWJ.Autoloads;
using System;

namespace GWJ.Scenes.Entities;

public partial class BeachGoer : CharacterBody2D
{
    [Export] public Texture2D[] BeachGoerTextures { get; set; } = Array.Empty<Texture2D>();

    public float WanderSpeed { get; set; } = 40f;
    public float ObstacleCheckDistance { get; set; } = 32f;
    public int MaxDirectionAttempts { get; set; } = 5;
    public float BuildChance { get; set; } = 0.1f; 
    
    private double TimeSinceLastBuild = 0.0;
    private double BuildCooldownSeconds = 5.0;
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

        TimeSinceLastBuild += delta;
    }

    private void OnWanderTimerTimeout()
    {
        if (TimeSinceLastBuild >= BuildCooldownSeconds && GD.Randf() < BuildChance)
        {
            BuildSandcastle();
            return;
        }

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

    private async void BuildSandcastle()
    {
        GD.Print("Preparing to build sandcastle...");

        // Stop moving
        WanderDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
        Body.UpdateAnimationState(BodyAnimation.AnimationState.Idle);

        // Wait 1 second to simulate "building"
        await ToSignal(GetTree().CreateTimer(1.0), "timeout");

        // Spawn the sandcastle
        SignalManager.Instance.EmitSpawnSandcastle(GlobalPosition);

        // Reset cooldown
        TimeSinceLastBuild = 0.0;

        // Resume movement
        OnWanderTimerTimeout();
        Body.UpdateAnimationState(BodyAnimation.AnimationState.Walk);
    }
}
