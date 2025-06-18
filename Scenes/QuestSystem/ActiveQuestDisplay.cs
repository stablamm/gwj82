using Godot;
using GWJ.Autoloads;

namespace GWJ.Scenes.QuestSystem;

public partial class ActiveQuestDisplay : Node2D
{
    public bool IsDisplayed { get; private set; } = false;
    public bool IsDebugEnabled { get; private set; } = true;

    private Label DebugQuestId;
    private Label QuestTitle;
    private Label QuestProgress;
    
    public override void _Ready()
    {
        DebugQuestId = GetNode<Label>("%DebugQuestId");
        QuestTitle = GetNode<Label>("%QuestTitle");
        QuestProgress = GetNode<Label>("%QuestProgress");

        UpdateQuestDisplay();

        SignalManager.Instance.Connect(
            nameof(SignalManager.QuestSystem_ShowActiveQuestDisplay_EventHandler)
            , new Callable(this, nameof(OnShowDisplay)));
    }

    public override void _Process(double delta)
    {
        if (!IsDisplayed) return;

        UpdateQuestDisplay();
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("toggle_quest_display"))
        {
            OnToggleDisplay();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsDisplayed) return;

        UpdateQuestDisplay();
    }

    private void OnShowDisplay()
    {
        IsDisplayed = true;
        Visible = true;
        
        UpdateQuestDisplay();
    }

    private void OnToggleDisplay()
    {
        IsDisplayed = !IsDisplayed;
        Visible = IsDisplayed;
        
        if (IsDisplayed)
        {
            UpdateQuestDisplay();
        }
    }

    public void UpdateQuestDisplay()
    {
        if (QuestManager.Instance.ActiveQuestId != -1)
        {
            var activeQuest = QuestManager.Instance.QuestList[QuestManager.Instance.ActiveQuestId];

            if (IsDebugEnabled)
                DebugQuestId.Text = $"Quest ID: {activeQuest.ID}";
            else
                DebugQuestId.Text = string.Empty;

            QuestTitle.Text = activeQuest.Title;
            QuestProgress.Text = $"{activeQuest.TargetType}: {activeQuest.CurrentCount}/{activeQuest.TargetCount}";
        }
        else
        {
            DebugQuestId.Text = string.Empty;
            QuestTitle.Text = "No active quest.";
            QuestProgress.Text = string.Empty;
        }
    }
}
