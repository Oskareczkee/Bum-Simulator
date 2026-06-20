using Godot;
using System;

public partial class BottleMachine : StaticBody2D
{
    [Signal] public delegate void UpdateMoneyCounterEventHandler(double newValue);
    [Signal] public delegate void UpdateGlassBottlesCounterEventHandler(int newValue);

    private Interactable Interactable;
    private AnimatedSprite2D Sprite2D;
    private Hud HUD;
    private AudioManager AudioManager;

    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        Sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HUD = GetNode<Control>("/root/GameManager/UI/HUD") as Hud;
        AudioManager = GetNode<AudioManager>("/root/GameManager/AudioManager");

        Interactable.Interact += OnInteract;

        UpdateMoneyCounter += HUD.UpdateMoney;
        UpdateGlassBottlesCounter += HUD.UpdateGlassBottles;
    }

    private void OnInteract()
    {

        if (PlayerData.Instance.GlassBottles == 0)
        {
            AudioManager.PlayRejectSound();
            return;
        }

        PlayerData.Instance.GlassBottles--;
        PlayerData.Instance.Money += 1.0f;

        EmitSignal(SignalName.UpdateGlassBottlesCounter, PlayerData.Instance.GlassBottles);
        EmitSignal(SignalName.UpdateMoneyCounter, PlayerData.Instance.Money);

        AudioManager.PlaySellSound();
    }

    public override void _Process(double delta) { }
}
