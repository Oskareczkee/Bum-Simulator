using Godot;
using System;

public partial class Interactable : Area2D
{
	[Export] public string InteractName = string.Empty;
	[Export] public bool IsInteractable = true;

	public Action Interact { get; set; } = null;

	public override void _Ready(){}
	public override void _Process(double delta){}
}
