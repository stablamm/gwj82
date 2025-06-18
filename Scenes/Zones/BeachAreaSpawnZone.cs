using Godot;
using GWJ.Autoloads;

namespace GWJ.Scenes.Zones;

public partial class BeachAreaSpawnZone : Node2D
{
    private Marker2D GoodGuySpawnMarker;
    private Marker2D BeachGoer_1;
    private Marker2D BeachGoer_2;
    private Marker2D BeachGoer_3;
    private Marker2D BeachGoer_4;
    private Marker2D UmbrellaSpawn_1;
    private Marker2D UmbrellaSpawn_2;

    public override void _Ready()
    {
        GoodGuySpawnMarker = GetNode<Marker2D>("GoodGuySpawnMarker");
        BeachGoer_1 = GetNode<Marker2D>("BeachGoer_1");
        BeachGoer_2 = GetNode<Marker2D>("BeachGoer_2");
        BeachGoer_3 = GetNode<Marker2D>("BeachGoer_3");
        BeachGoer_4 = GetNode<Marker2D>("BeachGoer_4");
        UmbrellaSpawn_1 = GetNode<Marker2D>("UmbrellaSpawn_1");
        UmbrellaSpawn_2 = GetNode<Marker2D>("UmbrellaSpawn_2");

        CallDeferred(nameof(RunSpawn));
    }

    public void RunSpawn()
    {
        SpawnGoodGuy();
        SpawnBeachGoers();
        SpawnUmbrellas();
    }

    public void SpawnGoodGuy()
        => SignalManager.Instance.EmitBeachAreaSpawnGoodGuy(GoodGuySpawnMarker.GlobalPosition);

    public void SpawnBeachGoers()
    {
        SignalManager.Instance.EmitBeachAreaSpawnBeachGoer(BeachGoer_1.GlobalPosition);
        SignalManager.Instance.EmitBeachAreaSpawnBeachGoer(BeachGoer_2.GlobalPosition);
        SignalManager.Instance.EmitBeachAreaSpawnBeachGoer(BeachGoer_3.GlobalPosition);
        SignalManager.Instance.EmitBeachAreaSpawnBeachGoer(BeachGoer_4.GlobalPosition);
    }

    public void SpawnUmbrellas()
    {
        SignalManager.Instance.EmitBeachAreaSpawnUmbrella(UmbrellaSpawn_1.GlobalPosition);
        SignalManager.Instance.EmitBeachAreaSpawnUmbrella(UmbrellaSpawn_2.GlobalPosition);
    }
}