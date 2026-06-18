using Godot;
using System;

public partial class Hud : Control
{
    private TextureProgressBar progressBar;
    private Label metalCansCounter;
    private Label glassBottlesCounter;
    private Label moneyCounter;
    private Label beerCounter;

    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>("%IntoxicationBar");
        metalCansCounter = GetNode<Label>("%MetalCansCount");
        glassBottlesCounter = GetNode<Label>("%GlassBottlesCount");
        moneyCounter = GetNode<Label>("%MoneyCount");
        beerCounter = GetNode<Label>("%BeerCount");
    }

    public void UpdateIntoxicationBar(int newPercentValue)
    {
        progressBar.Value = newPercentValue;
    }

    public void UpdateMetalCans(int newValue)
    {
        metalCansCounter.Text = newValue.ToString();
    }

    public void UpdateGlassBottles(int newValue)
    {
        glassBottlesCounter.Text = newValue.ToString();
    }

    public void UpdateMoney(float newValue)
    {
        moneyCounter.Text = newValue.ToString("F2");
    }

    public void UpdateBeer(int newValue)
    {
        beerCounter.Text = newValue.ToString();
    }

}
