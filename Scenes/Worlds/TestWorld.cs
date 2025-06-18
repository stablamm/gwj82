using Godot;
using GWJ.Scenes.Entities;
using GWJ.Autoloads;

namespace GWJ.Scenes.Worlds;

public partial class TestWorld : Node2D
{
    private PackedScene packedCrater = GD.Load<PackedScene>("res://Scenes/Entities/crater.tscn");
    private PackedScene packedGhost = GD.Load<PackedScene>("res://Scenes/Entities/ghost.tscn");
    private PackedScene packedLandmine = GD.Load<PackedScene>("res://Scenes/Entities/landmine.tscn");
    private PackedScene packedGoodGuy = GD.Load<PackedScene>("res://Scenes/Entities/good_guy.tscn");
    private PackedScene packedBeachGoer = GD.Load<PackedScene>("res://Scenes/Entities/beach_goer.tscn");
    private PackedScene packedUmbrella = GD.Load<PackedScene>("res://Scenes/Entities/beach_umbrella.tscn");
    private PackedScene packedSandcastle = GD.Load<PackedScene>("res://Scenes/Entities/sandcastle.tscn");

    private Node2D Entities;

    public override void _Ready()
    {
        Entities = GetNode<Node2D>("%Entities");

        SignalManager.Instance.Connect(
            nameof(SignalManager.LandmineExplodeEventHandler)
            , new Callable(this, nameof(OnLandmineExplode)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.SpawnGhostEventHandler)
            , new Callable(this, nameof(OnGhostSpawn)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.SpawnLandmineEventHandler)
            , new Callable(this, nameof(OnSpawnLandmine)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.BeachAreaSpawn_GoodGuySpawn_EventHandler)
            , new Callable(this, nameof(OnGoodGuySpawn)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.BeachAreaSpawn_BeachGoerSpawn_EventHandler)
            , new Callable(this, nameof(OnBeachGoerSpawn)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.BeachAreaSpawn_UmbrellaSpawn_EventHandler)
            , new Callable(this, nameof(OnUmbrellaSpawn)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.Spawn_Sandcastle_EventHandler)
            , new Callable(this, nameof(OnSpawnSandcastle)));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_increase_nerve_level"))
        {
            GameManager.Instance.IncreaseNerveLevel();
        }
        else if (@event.IsActionPressed("debug_decrease_nerve_level"))
        {
            GameManager.Instance.DecreaseNerveLevel();
        }
    }

    private void OnLandmineExplode(Vector2I position)
    {
        GD.Print($"Landmine exploded at position: {position}");
        
        var unpackedCrater = packedCrater.Instantiate<Crater>();
        unpackedCrater.GlobalPosition = position;
        
        AddChild(unpackedCrater);
    }

    private void OnGhostSpawn(Vector2I position)
    {
        GD.Print($"Ghost spawned at position: {position}");
        
        var unpackedGhost = packedGhost.Instantiate<Ghost>();
        unpackedGhost.GlobalPosition = position;
        
        AddChild(unpackedGhost);
    }

    private void OnSpawnLandmine(Vector2I position)
    {
        GD.Print($"Landmine spawned at position: {position}");
       
        var unpackedLandmine = packedLandmine.Instantiate<Landmine>();
        unpackedLandmine.GlobalPosition = position;
        
        AddChild(unpackedLandmine);
    }

    private void OnGoodGuySpawn(Vector2 position)
    {
        GD.Print($"GoodGuy spawned at position: {position}");

        var unpackedGoodGuy = packedGoodGuy.Instantiate<GoodGuy>();
        unpackedGoodGuy.GlobalPosition = position;
        
        Entities.AddChild(unpackedGoodGuy);
    }

    private void OnBeachGoerSpawn(Vector2 position)
    {
        GD.Print($"BeachGoer spawned at position: {position}");

        var unpackedBeachGoer = packedBeachGoer.Instantiate<BeachGoer>();
        unpackedBeachGoer.GlobalPosition = position;

        Entities.AddChild(unpackedBeachGoer);
    }

    private void OnUmbrellaSpawn(Vector2 position)
    {
        GD.Print($"Umbrella spawned at position: {position}");
        
        var unpackedUmbrella = packedUmbrella.Instantiate<BeachUmbrella>();
        unpackedUmbrella.GlobalPosition = position;
        
        Entities.AddChild(unpackedUmbrella);
    }

    private void OnSpawnSandcastle(Vector2 position)
    {
        GD.Print($"Sandcastle spawned at position: {position}");

        var unpackedSandcastle = packedSandcastle.Instantiate<Sandcastle>();
        unpackedSandcastle.GlobalPosition = position;

        Entities.AddChild(unpackedSandcastle);
    }
}
