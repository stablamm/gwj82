using Godot;
using System.Collections.Generic;

namespace GWJ.Autoloads;

public partial class SceneManager : Node
{
    public static SceneManager Instance;

    public enum SceneType
    {
        MainMenu,
        TestWorld,
        SettingsMenu
    }

    public SceneType CurrentScene { get; private set; } = SceneType.MainMenu;
    private SceneType previousScene = SceneType.MainMenu;

    private Dictionary<SceneType, PackedScene> scenePaths = new Dictionary<SceneType, PackedScene>
    {
        { SceneType.MainMenu, GD.Load<PackedScene>("res://Scenes/UI/main_menu.tscn") },
        { SceneType.TestWorld, GD.Load<PackedScene>("res://Scenes/Worlds/test_world.tscn") },
        { SceneType.SettingsMenu, GD.Load<PackedScene>("res://Scenes/UI/settings_menu.tscn") }
    };

    public override void _Ready()
    {
        Instance = this;
    }

    public void LoadScene(SceneType sceneType)
    {
        if (CurrentScene == sceneType) return;

        previousScene = CurrentScene;
        CurrentScene = sceneType;
        GetTree().ChangeSceneToPacked(scenePaths[sceneType]);
    }

    public void LoadPreviousScene()
    {
        LoadScene(previousScene);
    }
}
