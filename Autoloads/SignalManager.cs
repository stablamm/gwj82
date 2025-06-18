using Godot;

namespace GWJ.Autoloads;

public partial class SignalManager : Node
{
    public static SignalManager Instance;

    [Signal] public delegate void Spawn_Sandcastle_EventHandler(Vector2 position);
    [Signal] public delegate void SpawnLandmineEventHandler(Vector2I position);
    [Signal] public delegate void LandmineExplodeEventHandler(Vector2I position);
    [Signal] public delegate void SpawnGhostEventHandler(Vector2I position);
    [Signal] public delegate void OnPoliceExplodedEventHandler();
    [Signal] public delegate void OnNerveLevelChangedEventHandler();
    [Signal] public delegate void OnMaximumNerveLevelReachedEventHandler();

    [Signal] public delegate void EnableGlintEventHandler();
    [Signal] public delegate void DisableGlintEventHandler();

    [Signal] public delegate void DialogueBox_Show_EventHandler();
    [Signal] public delegate void DialogueBox_Hide_EventHandler();
    [Signal] public delegate void DialogueBox_SetText_EventHandler(string text);
    [Signal] public delegate void DialogueBox_ClearText_EventHandler();

    [Signal] public delegate void QuestSystem_ShowQuestDialogue_EventHandler(int questId);
    [Signal] public delegate void QuestSystem_ShowActiveQuestDisplay_EventHandler();

    [Signal] public delegate void BeachAreaSpawn_GoodGuySpawn_EventHandler(Vector2 position);
    [Signal] public delegate void BeachAreaSpawn_BeachGoerSpawn_EventHandler(Vector2 position);
    [Signal] public delegate void BeachAreaSpawn_UmbrellaSpawn_EventHandler(Vector2 position);

    public override void _Ready()
    {
        Instance = this;

        AddUserSignal(nameof(Spawn_Sandcastle_EventHandler));
        AddUserSignal(nameof(SpawnLandmineEventHandler));
        AddUserSignal(nameof(LandmineExplodeEventHandler));
        AddUserSignal(nameof(SpawnGhostEventHandler));
        AddUserSignal(nameof(OnPoliceExplodedEventHandler));
        AddUserSignal(nameof(OnNerveLevelChangedEventHandler));
        AddUserSignal(nameof(OnMaximumNerveLevelReachedEventHandler));

        AddUserSignal(nameof(EnableGlintEventHandler));
        AddUserSignal(nameof(DisableGlintEventHandler));

        AddUserSignal(nameof(DialogueBox_Show_EventHandler));
        AddUserSignal(nameof(DialogueBox_Hide_EventHandler));
        AddUserSignal(nameof(DialogueBox_SetText_EventHandler));
        AddUserSignal(nameof(DialogueBox_ClearText_EventHandler));

        AddUserSignal(nameof(QuestSystem_ShowQuestDialogue_EventHandler));
        AddUserSignal(nameof(QuestSystem_ShowActiveQuestDisplay_EventHandler));

        AddUserSignal(nameof(BeachAreaSpawn_GoodGuySpawn_EventHandler));
        AddUserSignal(nameof(BeachAreaSpawn_BeachGoerSpawn_EventHandler));
        AddUserSignal(nameof(BeachAreaSpawn_UmbrellaSpawn_EventHandler));
    }

    public void EmitSpawnSandcastle(Vector2 position) => EmitSignal(nameof(Spawn_Sandcastle_EventHandler), position);
    public void EmitSpawnLandmine(Vector2I position) => EmitSignal(nameof(SpawnLandmineEventHandler), position);
    public void EmitLandmineExplode(Vector2I position) => EmitSignal(nameof(LandmineExplodeEventHandler), position);
    public void EmitSpawnGhost(Vector2I position) => EmitSignal(nameof(SpawnGhostEventHandler), position);
    public void EmitOnPoliceExploded() => EmitSignal(nameof(OnPoliceExplodedEventHandler));
    public void EmitOnNerveLevelChanged() => EmitSignal(nameof(OnNerveLevelChangedEventHandler));
    public void EmitOnMaximumNerveLevelReached() => EmitSignal(nameof(OnMaximumNerveLevelReachedEventHandler));

    public void EmitEnableGlint() => EmitSignal(nameof(EnableGlintEventHandler));
    public void EmitDisableGlint() => EmitSignal(nameof(DisableGlintEventHandler));

    public void EmitDialogueBoxShow() => EmitSignal(nameof(DialogueBox_Show_EventHandler));
    public void EmitDialogueBoxHide() => EmitSignal(nameof(DialogueBox_Hide_EventHandler));
    public void EmitDialogueBoxSetText(string text) => EmitSignal(nameof(DialogueBox_SetText_EventHandler), text);
    public void EmitDialogueBoxClearText() => EmitSignal(nameof(DialogueBox_ClearText_EventHandler));

    public void EmitQuestSystemShowQuestDialogue(int questId) => EmitSignal(nameof(QuestSystem_ShowQuestDialogue_EventHandler), questId);
    public void EmitQuestSystemShowActiveQuestDisplay() => EmitSignal(nameof(QuestSystem_ShowActiveQuestDisplay_EventHandler));

    public Error EmitBeachAreaSpawnGoodGuy(Vector2 position) => EmitSignal(nameof(BeachAreaSpawn_GoodGuySpawn_EventHandler), position);
    public Error EmitBeachAreaSpawnBeachGoer(Vector2 position) => EmitSignal(nameof(BeachAreaSpawn_BeachGoerSpawn_EventHandler), position);
    public Error EmitBeachAreaSpawnUmbrella(Vector2 position) => EmitSignal(nameof(BeachAreaSpawn_UmbrellaSpawn_EventHandler), position);
}
