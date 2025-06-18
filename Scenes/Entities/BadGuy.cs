using Godot;
using GWJ.Scenes.Emotes;
using GWJ.Autoloads;

namespace GWJ.Scenes.Entities;

public partial class BadGuy : CharacterBody2D
{
    [Export] public float Speed = 200f;
    [Export] public float BullyPowerDuration = 5f;

    private string id = System.Guid.NewGuid().ToString();
    private bool isAttacking = false;
    private bool bullyPowerActive = false;

    private BodyAnimation AnimBody;
    private Emote Emote;
    private RayCast2D LeftRay;
    private RayCast2D RightRay;
    private RayCast2D UpRay;
    private RayCast2D DownRay;
    private Timer BullyPowerTimer;

    public override void _Ready()
    {
        AnimBody = GetNode<BodyAnimation>("BodyAnimation");
        Emote = GetNode<Emote>("Emote");
        LeftRay = GetNode<RayCast2D>("LeftRay");
        RightRay = GetNode<RayCast2D>("RightRay");
        UpRay = GetNode<RayCast2D>("UpRay");
        DownRay = GetNode<RayCast2D>("DownRay");
        BullyPowerTimer = GetNode<Timer>("BullyPowerTimer");

        SignalManager.Instance.Connect(
            nameof(SignalManager.OnPoliceExplodedEventHandler)
            , new Callable(this, nameof(OnPoliceExploded)));

        BullyPowerTimer.WaitTime = BullyPowerDuration;
        BullyPowerTimer.OneShot = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isAttacking) return;

        var input = new Vector2( 
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
        );

        if (input.Length() > 0)
        {
            AnimBody.UpdateBlendPosition(input);

            Velocity = input.Normalized() * Speed;
            MoveAndSlide();

            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Walk);
        }
        else
        {
            Velocity = Vector2.Zero;
            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Idle);
        }

        if (Input.IsActionJustPressed("attack"))
        {
            AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Hit);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("bully_power"))
        {
            if (bullyPowerActive || GameManager.Instance.NerveLevel < GameManager.Instance.MaxNerveLevel)
                return;

            bullyPowerActive = true;
            BullyPowerTimer.Start();

            SignalManager.Instance.EmitEnableGlint();
            SignalManager.Instance.EmitDialogueBoxHide();
        }
    }

    private void OnPoliceExploded()
    {
        Emote.PlayEmote(Emote.EmoteType.Exclaimation, Emote.EmoteTimer.VeryShort);

        GameManager.Instance.IncreaseNerveLevel();
        GameManager.Instance.IncreaseNerveLevel();
    }

    private void OnBullyPowerTimeout()
    {
        bullyPowerActive = false;
        
        GameManager.Instance.DecreaseNerveLevel();
        GameManager.Instance.DecreaseNerveLevel();
        GameManager.Instance.DecreaseNerveLevel();
        GameManager.Instance.DecreaseNerveLevel();

        SignalManager.Instance.EmitDisableGlint();

        AnimBody.UpdateAnimationState(BodyAnimation.AnimationState.Idle);
    }

    public void UpdateAttackState(bool attacking) => isAttacking = attacking;

    public void Punch(string dir)
    {
        switch (dir.ToLower())
        {
            case "left":
                CheckForCollisions(LeftRay);
                break;
            case "right":
                CheckForCollisions(RightRay);
                break;
            case "up":
                CheckForCollisions(UpRay);
                break;
            case "down":
                CheckForCollisions(DownRay);
                break;
            default:
                GD.PrintErr("Invalid direction for punch: " + dir);
                return;
        }
    }

    private void CheckForCollisions(RayCast2D ray)
    {
        ray.ForceRaycastUpdate();
        if (ray.IsColliding())
        {
            var collider = ray.GetCollider();
            if (collider is Sandcastle sandcastle)
            {
                sandcastle.OnHit();
            }
        }
    }
}
