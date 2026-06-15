using Godot;
using System;

public partial class TrashCanScript : StaticBody2D
{
    [Signal] public delegate void UpdateMetalCansCounterEventHandler(int newValue);
    [Signal] public delegate void UpdateGlassBottlesCounterEventHandler(int newValue);

    private readonly float METAL_CAN_CHANCE = 0.4f;
    private readonly float BOTTLE_CHANCE = 0.1f;

    private Interactable Interactable;
    private Sprite2D Sprite2D;
    private AudioStreamPlayer2D Audio;
    private Hud HUD;

    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        Audio = GetNode<AudioStreamPlayer2D>("OpenSound");
        HUD = GetNode<Control>("/root/GameManager/UI/HUD") as Hud;

        Interactable.Interact += OnInteract;

        UpdateMetalCansCounter += HUD.UpdateMetalCans;
        UpdateGlassBottlesCounter += HUD.UpdateGlassBottles;
    }

    private void OnInteract()
    {
        //Check if trashcan is opened (spirtes are flipped) 1-> closed 0->opened
        if (Sprite2D.Frame != 1)
            return;

        Sprite2D.Frame = 0; //set frame to chest opened
        Interactable.IsInteractable = false;

        double roll = Random.Shared.NextDouble();

        //0-0.4 -> metal can, 0.4-0.5 -> glass bottle
        if (roll < METAL_CAN_CHANCE)
        {
            PlayerData.Instance.MetalCans++;
            EmitSignal(SignalName.UpdateMetalCansCounter, PlayerData.Instance.MetalCans);
        }
        else if (roll < METAL_CAN_CHANCE + BOTTLE_CHANCE)
        {
            PlayerData.Instance.GlassBottles++;
            EmitSignal(SignalName.UpdateGlassBottlesCounter, PlayerData.Instance.GlassBottles);
        }

        Audio.Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
