using Godot;

public partial class GameOver : Control
{
	[Signal] public delegate void MainMenuEventHandler(string origin);
	[Signal] public delegate void RetryEventHandler();
	[Signal] public delegate void QuitEventHandler();

	private void OnMainMenuPressed()
	{
		EmitSignal(SignalName.MainMenu, "end_game_screen");
	}

	private void OnQuitPressed()
	{
		EmitSignal(SignalName.Quit);
	}
}
