using Godot;
using Godot.Collections;
using GWJ.Autoloads;

public partial class NerveLevels : Node2D
{
    Dictionary<int, Sprite2D> NerveLevelSprites = new();
    
    public override void _Ready()
    {
        for (int i = 1; i <= GameManager.Instance.MaxNerveLevel; i++)
        {
            NerveLevelSprites[i] = GetNode<Sprite2D>($"Splice_{i}");
        }

        SignalManager.Instance.Connect(
            nameof(SignalManager.OnNerveLevelChangedEventHandler)
            , new Callable(this, nameof(OnNerveLevelChanged)));

        UpdateNerveLevelDisplay();
    }
    
    public void OnNerveLevelChanged()
    {
        UpdateNerveLevelDisplay();
    }

    public void UpdateNerveLevelDisplay()
    {
        foreach (var kvp in NerveLevelSprites)
        {
            kvp.Value.Visible = kvp.Key <= GameManager.Instance.NerveLevel;
        }
    }
}
