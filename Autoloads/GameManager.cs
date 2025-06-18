using Godot;

namespace GWJ.Autoloads;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }

    #region Nerves
    public int NerveLevel { get; private set; } = 4;
    public int MaxNerveLevel { get; private set; } = 8;

    public void IncreaseNerveLevel()
    {
        if (NerveLevel < MaxNerveLevel)
        {
            NerveLevel++;
            SignalManager.Instance.EmitOnNerveLevelChanged();
        }
        else if (NerveLevel == MaxNerveLevel)
        {
            SignalManager.Instance.EmitOnMaximumNerveLevelReached();
        }
    }

    public void DecreaseNerveLevel()
    {
        if (NerveLevel > 0)
        {
            NerveLevel--;
            SignalManager.Instance.EmitOnNerveLevelChanged();
        }
    } 
    #endregion

    private int _landmineSpawnChance = 60;
    public int LandmineSpawnChance
    {
        get => _landmineSpawnChance;
        set
        {
            if (value < 0)
                _landmineSpawnChance = 0;
            else if (value > 100)
                _landmineSpawnChance = 100;
            else
                _landmineSpawnChance = value;
        }
    }
}
