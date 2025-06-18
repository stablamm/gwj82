using Godot;
using GWJ.Autoloads;
using GWJ.Scenes.Entities;

namespace GWJ.Scenes.UI;

public partial class FogOfWar : ColorRect
{
    private BadGuy badGuyRef;
    private ShaderMaterial Shader;

    public override void _Ready()
    {
        Shader = (ShaderMaterial)Material;
        var players = GetTree().GetNodesInGroup("player");
        if (players.Count > 0)
        {
            badGuyRef = players[0] as BadGuy;
        }
        else
        {
            GD.PrintErr("No player found in group 'player'. FogOfWar will not update.");
        }


        // Show by default.
        // This is turned off in the editor so I can see behind it.
        Show();
    }

    public override void _Process(double delta)
    {
        if (badGuyRef != null)
        {
            UpdateShader();
        }
    }

    private void UpdateShader()
    {
        if (GameManager.Instance.NerveLevel == GameManager.Instance.MaxNerveLevel)
        {
            Shader.SetShaderParameter("reveal_radius", 750f);
        }
        else if (GameManager.Instance.NerveLevel > 0)
        {
            var revealRadius = (GameManager.Instance.NerveLevel % GameManager.Instance.MaxNerveLevel) * 75;
            Shader.SetShaderParameter("reveal_radius", revealRadius);
        }

        Vector2 screenPos = badGuyRef.GetGlobalTransformWithCanvas().Origin;
        Shader.SetShaderParameter("player_screen_pos", screenPos);
    }
}
