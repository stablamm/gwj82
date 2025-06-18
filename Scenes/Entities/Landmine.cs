using Godot;
using GWJ.Autoloads;

namespace GWJ.Scenes.Entities;

public partial class Landmine : Node2D
{
    private bool canDecreaseNerveLevel = true;
    private bool isArmed = false;

    private AnimationPlayer AnimPlayer;
    
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimPlayer");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!isArmed) return;

        if (body.GetType() == typeof(BadGuy))
        {
            AnimPlayer.Play("explode");
        }
        else if (body.GetType() == typeof(GoodGuy))
        {
            AnimPlayer.Play("explode");

            GoodGuy goodGuy = (GoodGuy)body;
            goodGuy.BlowUp();
            canDecreaseNerveLevel = false;
        }
    }

    private void OnArmedTimerTimeout() => isArmed = true;

    public void SpawnCrater()
    {
        SignalManager.Instance.EmitLandmineExplode((Vector2I)GlobalPosition + new Vector2I(0, 8));
        QuestManager.Instance.UpdateQuestProgress(QuestTargetType.Landmine);

        if (canDecreaseNerveLevel)
            GameManager.Instance.DecreaseNerveLevel();
    }
}
