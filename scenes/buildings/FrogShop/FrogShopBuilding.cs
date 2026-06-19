using Godot;
using System;

public partial class FrogShopBuilding : Node2D
{
    private Interactable Interactable;
    private GameManager GameManager;
    [Export] public PackedScene GoToScene;

    public override void _Ready()
	{
        Interactable = GetNode<Interactable>("Interactable");
        GameManager = GetNode<GameManager>("/root/GameManager");

        Interactable.Interact += OnInteract;
    }

    private void OnInteract()
    {
        GameManager.ChangeCurrentScene(GoToScene);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
