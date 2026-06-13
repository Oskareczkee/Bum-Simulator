using Godot;
using System;

public partial class Hud : Control
{
    private TextureProgressBar progressBar;

    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>("%IntoxicationBar");
    }

    public void UpdateIntoxicationBar(int newPercentValue)
    {
        progressBar.Value = newPercentValue;
    }
}
