using Godot;
using System;

public partial class PauseMenu : Control
{
    [Signal] public delegate void ContinuePressedEventHandler(string source);
    [Signal] public delegate void SettingsPressedEventHandler(string source);
    [Signal] public delegate void MainMenuPressedEventHandler(string source);

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("pause"))
			EmitSignal(SignalName.ContinuePressed, "pause_menu");
    }

	private void OnContinuePressed()
	{
		EmitSignal(SignalName.ContinuePressed, "pause_menu");
	}

	private void OnSettingsPressed()
	{
		EmitSignal(SignalName.SettingsPressed, "pause_menu");
	}

	private void OnMainMenuPressed()
	{
		EmitSignal(SignalName.MainMenuPressed, "pause_menu");
    }
}
