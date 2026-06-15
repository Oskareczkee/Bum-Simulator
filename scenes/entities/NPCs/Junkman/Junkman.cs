using Godot;
using System;

public partial class Junkman : StaticBody2D
{
    [Signal] public delegate void UpdateMetalCansCounterEventHandler(int newValue);
    [Signal] public delegate void UpdateMoneyCounterEventHandler(float newValue);

    private Interactable Interactable;
    private AudioStreamPlayer2D DialogueStream;
    private Hud HUD;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        DialogueStream = GetNode<AudioStreamPlayer2D>("DialogueStream");
        HUD = GetNode<Control>("/root/GameManager/UI/HUD") as Hud;

        Interactable.Interact += OnInteract;
        UpdateMetalCansCounter += HUD.UpdateMetalCans;
        UpdateMoneyCounter += HUD.UpdateMoney;
    }

    private void OnInteract()
    {
        GD.Print("Player interacted with junkman");

        if (!DialogueStream.Playing)
            DialogueStream.Play();

        if (PlayerData.Instance.MetalCans == 0)
            return;

        PlayerData.Instance.MetalCans--;
        PlayerData.Instance.Money += 0.50f;

        EmitSignal(SignalName.UpdateMetalCansCounter, PlayerData.Instance.MetalCans);
        EmitSignal(SignalName.UpdateMoneyCounter, PlayerData.Instance.Money);
    }

    public override void _Process(double delta){}
}
