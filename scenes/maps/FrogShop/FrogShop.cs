using Godot;
using System;

public partial class FrogShop : Node2D
{

	private Interactable ExitInteraction;
	private GameManager GameManager;


	public override void _Ready()
	{
		ExitInteraction = GetNode<Interactable>("ExitInteraction");
		GameManager = GetNode<GameManager>("/root/GameManager");

		ExitInteraction.Interact += OnExit;
	}

	private void OnExit()
	{
		//if (LeaveToScene == null)
		//	return;

		//GameManager.ChangeCurrentSceneAndTeleportPlayerToLastKnownPosition(LeaveToScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){}
}
