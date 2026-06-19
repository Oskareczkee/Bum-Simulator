using Godot;
using System;

public partial class Credits : Control
{
	[Signal] public delegate void BackPressedEventHandler(string origin);

	public override void _Ready()
	{
		BackPressed += (GetNode("/root/GameManager") as GameManager).LoadMainMenu;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBackPressed()
	{
		EmitSignal(SignalName.BackPressed, "credits_screen");
	}
}
