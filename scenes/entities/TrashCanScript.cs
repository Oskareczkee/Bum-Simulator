using Godot;
using System;

public partial class TrashCanScript : StaticBody2D
{
    private Interactable Interactable;
    private Sprite2D Sprite2D;

    public override void _Ready()
    {
        Interactable = GetNode<Interactable>("Interactable");
        Sprite2D = GetNode<Sprite2D>("Sprite2D");

        Interactable.Interact += OnInteract;
    }

    private void OnInteract()
    {
        GD.Print("AAAEEEE");
        //Check if trashcan is opened (spirtes are flipped) 1-> closed 0->opened
        if (Sprite2D.Frame == 1)
        {
            Sprite2D.Frame = 0; //set frame to chest opened
            Interactable.IsInteractable = false;
            GD.Print("Player interacted with trash can");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
