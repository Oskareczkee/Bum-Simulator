using Godot;
using System;

public partial class FrogShop : Node2D
{

	private GameManager GameManager;
	private Interactable ShopExit;

	//for some reason adding export will not work and will always result in null value
	private const string ExitScenePath = "res://scenes/maps/Start/Test.tscn";

    public override void _Ready()
	{
		GameManager = GetNode<GameManager>("/root/GameManager");
		ShopExit = GetNode<Interactable>("ShopExit");

		ShopExit.Interact += OnExit;
	}

	private void OnExit()
	{
		GD.Print("Shop exit interacted");
        var ExitToScene = GD.Load<PackedScene>(ExitScenePath);
        GameManager.ChangeCurrentSceneAndTeleportPlayerToLastKnownPosition(ExitToScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
