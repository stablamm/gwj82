using Godot;

namespace GWJ.Scenes.Entities;

public partial class BeachUmbrella : StaticBody2D
{
    public enum UmbrellaState
    {
        Close,
        Open
    }

    private bool isPlayerInArea = false;

    public UmbrellaState State { get; private set; } = UmbrellaState.Close;

    private AnimationPlayer AnimPlayer;

    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");
    }

    public override void _Input(InputEvent @event)
    {
        if (!isPlayerInArea) return;

        if (@event.IsActionPressed("interact"))
        {
            ChangeUmbrellaState();
        }
    }

    private void OnInteractAreaEntered(Node2D body)
    {
        if (body is BadGuy b)
        {
            isPlayerInArea = true;
        }
    }

    private void OnInteractAreaExited(Node2D body)
    {
        if (body is BadGuy b)
        {
            isPlayerInArea = false;
        }
    }

    private void ChangeUmbrellaState()
    {
        if (State == UmbrellaState.Close)
        {
            State = UmbrellaState.Open;
        }
        else
        {
            State = UmbrellaState.Close;
        }

        AnimPlayer.Play(State.ToString());
    }
}
