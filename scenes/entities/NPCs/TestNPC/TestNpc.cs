using Godot;
using System;

public partial class TestNpc : StaticBody2D
{
    private Interactable Interactable;
    private AnimatedSprite2D Sprite2D;
	private AudioStreamPlayer2D DialogueStream;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Interactable = GetNode<Interactable>("Interactable");
		Sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		DialogueStream = GetNode<AudioStreamPlayer2D>("DialogueStream");  

		Interactable.Interact += OnInteract;
	}

    private void OnInteract()
    {
        if (!DialogueStream.Playing)
            DialogueStream.Play();
    }

    public override void _Process(double delta)
	{
	}
}
