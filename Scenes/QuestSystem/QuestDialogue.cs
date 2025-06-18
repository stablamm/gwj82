using Godot;
using GWJ.Autoloads;
using System.Text;

namespace GWJ.Scenes.QuestSystem;

public partial class QuestDialogue : Node2D
{
    private int questId = -1; // Default value indicating no quest selected

    private RichTextLabel QuestText;

    public override void _Ready()
    {
        QuestText = GetNode<RichTextLabel>("%QuestText");
        
        Hide();

        SignalManager.Instance.Connect(
            nameof(SignalManager.QuestSystem_ShowQuestDialogue_EventHandler),
            new Callable(this, nameof(ShowQuestDialogue))
        );
    }

    public void OnQuestAccepted()
    {
        QuestManager.Instance.ActivateQuest(questId);
        questId = -1;

        Hide();
    }

    public void OnQuestDeclined()
    {
        questId = -1;
        Hide();
    }

    public void ShowQuestDialogue(int qId)
    {
        var quest = QuestManager.Instance.QuestList[qId];

        StringBuilder qtb = new(); // Quest Text Builder

        qtb.AppendLine($"[center]{quest.Title}[/center]\n");
        qtb.AppendLine(quest.Description);

        QuestText.Text = qtb.ToString();

        questId = qId; 
        Show();
    }
}
