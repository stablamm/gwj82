using Godot;
using System.Collections.Generic;

namespace GWJ.Autoloads;

public enum QuestTargetType
{
    Sandcastle = 0,
    Crater = 1,
    Police = 2,
    Landmine = 3,
}

public class Quest
{
    public string ID { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; } = false;
    public bool IsCompleted { get; private set; } = false;

    public QuestTargetType TargetType { get; private set; }
    public int TargetCount { get; private set; } = 0;
    public int CurrentCount { get; private set; } = 0;

    public Quest(string id, string title, string description, QuestTargetType type, int count)
    {
        ID = id;
        Title = title;
        Description = description;
        TargetType = type;
        TargetCount = count;
    }

    public void Activate()
    {
        IsActive = true;
        IsCompleted = false;
        CurrentCount = 0;
    }

    public void Reject()
    {
        IsActive = false;
        IsCompleted = false;
        CurrentCount = 0;
    }

    public void Complete()
    {
        if (CurrentCount < TargetCount) 
        {             
            GD.PrintErr($"Cannot complete quest {ID}. Current count ({CurrentCount}) is less than target count ({TargetCount}).");
            return;
        }

        IsActive = false;
        IsCompleted = true;

        GD.Print($"Quest ID {ID} completed.");
    }

    public void UpdateProgress(QuestTargetType targetDestroyed)
    {
        if (targetDestroyed == TargetType)
        {
            CurrentCount++;
            GD.Print($"Quest ID {ID}: Current {TargetType}, ({CurrentCount}/{TargetCount})");
            
            if (CurrentCount >= TargetCount)
            {
                Complete();
            }
        }
        else
        {
            GD.PrintErr($"Quest {ID} progress update failed. Target type mismatch: expected {TargetType}, received {targetDestroyed}.");
        }
    }
}

public partial class QuestManager : Node
{
    public static QuestManager Instance { get; private set; }

    public Dictionary<int, Quest> QuestList { get; private set; } = new Dictionary<int, Quest>();
    public int ActiveQuestId { get; private set; } = -1;

    public override void _Ready()
    {
        Instance = this;
        loadQuests();
    }

    private void loadQuests()
    {
        QuestList[1] = new Quest(
            "quest_1", 
            "Getting Started...", 
            "Let's see if you can cut it. Knock over 3 sandcastles for me.\n" +
            "Come see me once you've done that.",
            QuestTargetType.Sandcastle,
            3);

        QuestList[2] = new Quest(
            "quest_2",
            "Well That Esclated Quickly...",
            "You seem to be getting the hang of this. Now, I need you to blow up 5 landmines.",
            QuestTargetType.Landmine,
            5);
    }

    public void OfferQuest(int questId)
    {
        SignalManager.Instance.EmitQuestSystemShowQuestDialogue(questId);
    }

    public void ActivateQuest(int questId)
    {
        if (QuestList.ContainsKey(questId))
        {
            QuestList[questId].Activate();
            ActiveQuestId = questId;
            SignalManager.Instance.EmitQuestSystemShowActiveQuestDisplay();
        }
        else
        {
            GD.PrintErr($"Quest with ID {questId} does not exist.");
        }
    }

    public void RejectQuest(int questId)
    {
        if (QuestList.ContainsKey(questId))
        {
            QuestList[questId].Reject();
            ActiveQuestId = -1;
        }
    }

    public void CompleteQuest(int questId)
    {
        if (QuestList.ContainsKey(questId))
        {
            QuestList[questId].Complete();
            ActiveQuestId = -1;
        }
    }

    public bool IsQuestActive(int questId)
        => QuestList.TryGetValue(questId, out Quest quest) && quest.IsActive;

    public bool IsQuestCompleted(int questId)
        => QuestList.TryGetValue(questId, out Quest quest) && quest.IsCompleted;

    public void UpdateQuestProgress(QuestTargetType targetType)
    {
        if (ActiveQuestId == -1 || !QuestList.ContainsKey(ActiveQuestId))
        {
            GD.PrintErr("No active quest to update progress for.");
            return;
        }
        
        QuestList[ActiveQuestId].UpdateProgress(targetType);
        if (QuestList[ActiveQuestId].IsCompleted)
        {
            ActiveQuestId = -1;
        }
    }
}
