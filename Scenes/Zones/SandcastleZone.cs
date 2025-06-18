using Godot;
using GWJ.Scenes.Entities;
using System;

public partial class SandcastleZone : Node2D
{
    [Export] public int NumberOfSandcastles { get; private set; } = 5;

    private PackedScene PackedSandcastle = GD.Load<PackedScene>("res://Scenes/Entities/sandcastle.tscn");

    private Area2D SpawnArea;
    private CollisionShape2D CollisionArea;

    public override void _Ready()
    {
        SpawnArea = GetNode<Area2D>("SpawnArea");
        CollisionArea = SpawnArea.GetNode<CollisionShape2D>("CollisionArea");

        SpawnSandcastles();
    }

    private void SpawnSandcastles()
    {
        for (int i = 0; i < NumberOfSandcastles; i++)
        {
            Vector2 spawnPos = GetRandomPointInArea();
            var sandcastle = PackedSandcastle.Instantiate<Sandcastle>();
            AddChild(sandcastle);
            sandcastle.GlobalPosition = spawnPos;
        }
    }

    private Vector2 GetRandomPointInArea()
    {
        if (CollisionArea.Shape is RectangleShape2D rect)
        {
            Vector2 extents = rect.Size * 0.5f;
            Vector2 localPos = new Vector2(
                (float)GD.RandRange(-extents.X, extents.X),
                (float)GD.RandRange(-extents.Y, extents.Y)
            );

            return SpawnArea.GlobalPosition + localPos.Rotated(SpawnArea.GlobalRotation);
        }
        else if (CollisionArea.Shape is CircleShape2D circle)
        {
            float radius = circle.Radius;
            float angle = (float)GD.RandRange(0, Mathf.Tau);
            float r = (float)GD.RandRange(0, radius);
            
            Vector2 localPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
            
            return SpawnArea.GlobalPosition + localPos.Rotated(SpawnArea.GlobalRotation);
        }

        GD.PushWarning("Unsupported spawn shape in SandcastleZone.");
        return SpawnArea.GlobalPosition;
    }
}