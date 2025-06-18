using Godot;
using GWJ.Autoloads;

namespace GWJ.Scenes.Entities;

public partial class Sandcastle : StaticBody2D
{
    private int maxFrames = 3;
    private bool isMouseHovering = false;
    private bool isGlintEnabled = false;
    private bool spawnLandmineOnDeath = false;

    private Sprite2D Sprite;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite");

        var rand = GD.RandRange(0, 100);
        if (rand < GameManager.Instance.LandmineSpawnChance)
        {
            spawnLandmineOnDeath = true;
        }

        SignalManager.Instance.Connect(
            nameof(SignalManager.EnableGlintEventHandler)
            , new Callable(this, nameof(TurnGlintOn)));

        SignalManager.Instance.Connect(
            nameof(SignalManager.DisableGlintEventHandler)
            , new Callable(this, nameof(TurnGlintOff)));
    }

    public override void _Input(InputEvent @event)
    {
        if (!isMouseHovering) return;

        if (@event is InputEventMouseButton mouseButtonEvent 
            && mouseButtonEvent.IsPressed() 
            && mouseButtonEvent.ButtonIndex == MouseButton.Right)
        {
            PlantFlag();
        }
    }

    private void OnMouseEntered()
    {
        isMouseHovering = true;
    }

    private void OnMouseExited()
    {
        isMouseHovering = false;
    }

    private void TurnGlintOn()
    {
        if (!spawnLandmineOnDeath) return;

        if (!isGlintEnabled)
        {
            EnableGlint();
            isGlintEnabled = true;
        }
    }

    private void TurnGlintOff()
    {
        if (!spawnLandmineOnDeath) return;

        if (isGlintEnabled)
        {
            DisableGlint();
            isGlintEnabled = false;
        }
    }

    public void OnHit()
    {
        if (Sprite.Frame == maxFrames)
        {
            GD.Print("Sandcastle destroyed!");
            
            if (spawnLandmineOnDeath)
                SignalManager.Instance.EmitSpawnLandmine((Vector2I)GlobalPosition + new Vector2I(0, -16));

            GameManager.Instance.IncreaseNerveLevel();
            QuestManager.Instance.UpdateQuestProgress(QuestTargetType.Sandcastle);   
            
            QueueFree();
        }
        else
        {
            Sprite.Frame++;
            GD.Print($"Sandcastle hit! Frame: {Sprite.Frame}");
        }
    }

    public void PlantFlag()
    {
        GD.Print("Flag planted on the sandcastle!");
    }

    public void EnableGlint()
    {
        var mat = Sprite.Material as ShaderMaterial;
        if (mat != null)
            mat.SetShaderParameter("glint_enabled", true);
    }

    public void DisableGlint()
    {
        var mat = Sprite.Material as ShaderMaterial;
        if (mat != null)
            mat.SetShaderParameter("glint_enabled", false);
    }
}
