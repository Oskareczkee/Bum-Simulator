using Godot;
using System;

public partial class BottleMachine : StaticBody2D
{
    [Signal] public delegate void UpdateMoneyCounterEventHandler(float newValue);
    [Signal] public delegate void UpdateGlassBottlesCounterEventHandler(int newValue);

    private Interactable Interactable;
    private AnimatedSprite2D Sprite2D;
    private Hud HUD;

    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        Sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HUD = GetNode<Control>("/root/GameManager/UI/HUD") as Hud;

        Interactable.Interact += OnInteract;

        UpdateMoneyCounter += HUD.UpdateMoney;
        UpdateGlassBottlesCounter += HUD.UpdateGlassBottles;
    }

    private void OnInteract()
    {
        GD.Print("Player interacted with glass bottles machine");

        if (PlayerData.Instance.GlassBottles == 0)
            return; //Add reject sound

        PlayerData.Instance.GlassBottles--;
        PlayerData.Instance.Money += 1.0f;

        EmitSignal(SignalName.UpdateGlassBottlesCounter, PlayerData.Instance.GlassBottles);
        EmitSignal(SignalName.UpdateMoneyCounter, PlayerData.Instance.Money);

        //Add sound here
    }

    public override void _Process(double delta) { }
}
