using Godot;
using GWJ.Autoloads;
using GWJ.Scenes.Entities;
using System;

namespace GWJ.Scenes.Shops;

public partial class Shop : StaticBody2D
{
    private bool isPlayerInArea = false;

    public override void _Input(InputEvent @event)
    {
        if (isPlayerInArea && @event.IsActionPressed("interact"))
        {
            if (!QuestManager.Instance.IsQuestCompleted(1) && !QuestManager.Instance.IsQuestActive(1))
                QuestManager.Instance.OfferQuest(1);
            else if (!QuestManager.Instance.IsQuestCompleted(2) && !QuestManager.Instance.IsQuestActive(2))
                QuestManager.Instance.OfferQuest(2);
            else
                GD.Print("No quests available at this time.");
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is BadGuy player)
        {
            isPlayerInArea = true;
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is BadGuy player)
        {
            isPlayerInArea = false;
        }
    }
}
