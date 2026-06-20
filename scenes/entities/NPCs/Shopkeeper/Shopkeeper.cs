using Godot;
using System;

public partial class Shopkeeper : StaticBody2D
{
    [Signal] public delegate void UpdateBeerCounterEventHandler(int newValue);
    [Signal] public delegate void UpdateMoneyCounterEventHandler(double newValue);

    private const double BeerPrice = 5.0f;

    private Interactable Interactable;
    private AnimatedSprite2D Sprite2D;
    private AudioStreamPlayer2D DialogueStream;
    private AudioManager AudioManager;
    private Hud HUD;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        Sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        DialogueStream = GetNode<AudioStreamPlayer2D>("DialogueStream");
        AudioManager = GetNode<AudioManager>("/root/GameManager/AudioManager");
        HUD = GetNode<Hud>("/root/GameManager/UI/HUD");

        Interactable.Interact += OnInteract;
        UpdateBeerCounter += HUD.UpdateBeer;
        UpdateMoneyCounter += HUD.UpdateMoney;
    }

    private void OnInteract()
    {
        if (!DialogueStream.Playing)
            DialogueStream.Play();

        if (PlayerData.Instance.Money < BeerPrice)
        {
            AudioManager.PlayRejectSound();
            return;
        }

        PlayerData.Instance.Money -= BeerPrice;
        PlayerData.Instance.Beer++;
        AudioManager.PlaySellSound();

        EmitSignal(SignalName.UpdateBeerCounter, PlayerData.Instance.Beer);
        EmitSignal(SignalName.UpdateMoneyCounter, PlayerData.Instance.Money);
    }

    public override void _Process(double delta) { }
}
