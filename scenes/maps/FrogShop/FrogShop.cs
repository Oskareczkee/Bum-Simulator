using Godot;
using System;

public partial class FrogShop : Node2D
{
	private GameManager GameManager;
	private Interactable ShopExit;

	public override void _Ready()
	{
		GameManager = GetNode<GameManager>("/root/GameManager");
		ShopExit = GetNode<Interactable>("ShopExit");

		ShopExit.Interact += OnExit;
	}

	private void OnExit()
	{
		GD.Print("Shop exit interacted");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
