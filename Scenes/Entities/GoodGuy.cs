using Godot;
using GWJ.Scenes.Emotes;
using GWJ.Autoloads;

namespace GWJ.Scenes.Entities;

public partial class GoodGuy : CharacterBody2D
{
    [Export] public float Speed = 150;

    private string id = System.Guid.NewGuid().ToString();
    private BadGuy badGuyRef;
    private bool isChasing = false;

    private BodyAnimation AnimBody;
    private Emote Emote;
    private Timer ChaseTimer;

    public override void _Ready()
    {
        AnimBody = GetNode<BodyAnimation>("BodyAnimation");
        Emote = GetNode<Emote>("Emote");
        ChaseTimer = GetNode<Timer>("ChaseTimer");

        var players = GetTree().GetNodesInGroup("player");
        if (players.Count > 0)
        {
            badGuyRef = players[0] as BadGuy;
        }
        else
        {
            GD.PrintErr("No player found in group 'player'. GoodGuy will not chase.");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (badGuyRef == null)
        {
            GD.Print("BadGuy reference is null or not alive. Stopping chase.");
            return;
        }

        if (isChasing)
        {
            Vector2 direction = (badGuyRef.GlobalPosition - GlobalPosition).Normalized();
            Velocity = direction * Speed;
            MoveAndSlide();
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is BadGuy badGuy && !isChasing)
        {
            Emote.PlayEmote(Emote.EmoteType.Exclaimation, Emote.EmoteTimer.Short);
            isChasing = true;
            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Walk);
            GD.Print($"GoodGuy {id} started chasing BadGuy.");
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is BadGuy badGuy && isChasing)
        {
            Emote.PlayEmote(Emote.EmoteType.QuestionMark, Emote.EmoteTimer.Short);
            isChasing = false;
            Velocity = Vector2.Zero;
            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Idle);
            GD.Print($"GoodGuy {id} stopped chasing BadGuy.");
        }
    }

    private void OnArrestAreaEntered(Node2D body)
    {
        if (body is BadGuy badGuy && isChasing)
        {
            isChasing = false;
            Velocity = Vector2.Zero;
            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Hit);
            GD.Print($"GoodGuy {id} arrested BadGuy.");
        }
    }

    public void BlowUp()
    {
        SignalManager.Instance.EmitSpawnGhost((Vector2I)GlobalPosition + new Vector2I(0, -16));
        SignalManager.Instance.EmitOnPoliceExploded();
        QueueFree();
    }
}
