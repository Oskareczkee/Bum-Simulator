using Godot;
using System;

public partial class FrogShopBuilding : Node2D
{
    private Interactable Interactable;
    private GameManager GameManager;
    private PlayerScript Player;

    [Export] public PackedScene GoToScene;

    public override void _Ready()
	{
        Interactable = GetNode<Interactable>("Interactable");
        GameManager = GetNode<GameManager>("/root/GameManager");
        Player = GetNode<PlayerScript>("%Player");

        Interactable.Interact += OnInteract;
    }

    private void OnInteract()
    {
        PlayerData.Instance.PositionBeforeSceneChange = Player.Position;
        GameManager.ChangeCurrentScene(GoToScene);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
