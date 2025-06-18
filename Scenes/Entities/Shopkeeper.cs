using Godot;
using System;

namespace GWJ.Scenes.Entities;

public partial class Shopkeeper : Node2D
{
    private BodyAnimation Body;

    public override void _Ready()
    {
        Body = GetNode<BodyAnimation>("BodyAnimation");
    }
}
