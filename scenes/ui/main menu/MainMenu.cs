using Godot;
using System;

public partial class MainMenu : Control
{
	[Signal] public delegate void NewGamePressedEventHandler(string source);
    [Signal] public delegate void SettingsPressedEventHandler(string source);
    [Signal] public delegate void CreditsPressedEventHandler(string source);
    [Signal] public delegate void QuitPressedEventHandler();

    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void OnNewGamePressed()
	{
		EmitSignal(SignalName.NewGamePressed, "main_menu");
	}

	private void OnSettingsPressed()
	{
        EmitSignal(SignalName.SettingsPressed, "main_menu");
    }

	private void OnCreditsPressed()
	{
        EmitSignal(SignalName.CreditsPressed, "main_menu");
    }

	private void OnQuitPressed()
	{
        EmitSignal(SignalName.QuitPressed);
    }
}
