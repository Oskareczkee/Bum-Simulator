using Godot;
using System;

public partial class PlayerData : Node
{
    public static PlayerData Instance { get; private set; }

    public int MetalCans { get; set; } = 5;
    public int GlassBottles { get; set; } = 5;
    public float Money { get; set; } = 0.0f;

    public override void _Ready()
    {
        Instance = this;
    }
}
