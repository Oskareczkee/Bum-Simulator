using Godot;
using System;

public partial class PlayerData : Node
{
    public static PlayerData Instance { get; private set; }

    public double Intoxication = 400.0f;
    public int MetalCans { get; set; } = 5;
    public int GlassBottles { get; set; } = 5;
    public double Money { get; set; } = 0.0f;
    public int Beer { get; set; } = 5;

    public Vector2 PositionBeforeSceneChange = Vector2.Zero;

    public override void _Ready()
    {
        Instance = this;
    }
}
