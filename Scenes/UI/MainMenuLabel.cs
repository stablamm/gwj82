using Godot;
using GWJ.Autoloads;
using GWJ.Scenes.Entities;

namespace GWJ.Scenes.UI;

[Tool]
public partial class MainMenuLabel : Node2D
{
    private string _labelText = string.Empty;
    [Export] public string LabelText
    {
        get => _labelText;
        set
        {
            _labelText = value;
            
            Label = GetNode<Label>("Label");
            if (Label != null)
            {
                Label.Text = _labelText;
            }
        }
    }

    [Export] public bool IsInteractive = false;

    private bool isPlayerInArea = false;

    private Label Label;

    public override void _Ready()
    {
        Label = GetNode<Label>("Label");
        Label.Text = LabelText;
    }

    public override void _Input(InputEvent @event)
    {
        if (!isPlayerInArea || !IsInteractive) return;

        if (@event.IsActionPressed("attack"))
        {
            GD.Print($"Label clicked: {LabelText}");

            if (LabelText.ToLower() == "start game")
            {
                SceneManager.Instance.LoadScene(SceneManager.SceneType.TestWorld);
            }
            else if (LabelText.ToLower() == "settings")
            {
                SceneManager.Instance.LoadScene(SceneManager.SceneType.SettingsMenu);
            }
            else if (LabelText.ToLower() == "credits")
            {
                GD.Print("Credits clicked - functionality not implemented yet.");
            }
            else if (LabelText.ToLower() == "exit")
            {
                GetTree().Quit();
            }
            else if (LabelText.ToLower() == "back")
            {
                SceneManager.Instance.LoadPreviousScene();
            }
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

    public void OnPlayerInteract()
    {
        GD.Print("Player interacted with label: " + LabelText);
    }
}