using Godot;
using System.Threading.Tasks;

public partial class GameManager : Node
{
	[Export] public PackedScene MainMenuScene;
	[Export] public PackedScene GameOverScreen;
    [Export] public PackedScene GameScene;

	private Node CurrentScene;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		CurrentScene = GetNode<Node>("CurrentScene");

		LoadMainMenu("game_start");
	}

	public async void LoadMainMenu(string origin)
	{
		HideHUD();

        if (origin == "end_game_screen")
			await ClearEverything();

        MainMenu mainMenu = MainMenuScene.Instantiate() as MainMenu;

		mainMenu.Connect(MainMenu.SignalName.NewGamePressed, Callable.From<string>(NewGame));
        mainMenu.Connect(MainMenu.SignalName.SettingsPressed, Callable.From<string>(OpenSettings));
        mainMenu.Connect(MainMenu.SignalName.CreditsPressed, Callable.From<string>(OpenCredits));
        mainMenu.Connect(MainMenu.SignalName.QuitPressed, Callable.From(QuitGame));

        CurrentScene.AddChild(mainMenu);
	}

	public async void NewGame(string origin)
	{
		if (origin.Equals("main_menu"))
			await ClearEverything();

		if (origin.Equals("end_game_screen"))
            await ClearEverything();

        var gameScene = GameScene.Instantiate();
		CurrentScene.AddChild(gameScene);
		ShowHUD();
	}

	public void OpenSettings(string origin)
	{

	}

	public void OpenCredits(string origin)
	{

	}

	public void QuitGame()
	{
		GetTree().Quit();
	}

	public void GameOver()
	{
		HideHUD();
        GameOver gameOverScene = GameOverScreen.Instantiate() as GameOver;

        gameOverScene.MainMenu += LoadMainMenu;
        gameOverScene.Quit += QuitGame;
		GetNode("UIExternal").AddChild(gameOverScene);
    }

	private async Task ClearUIExternal()
	{
		foreach (var obj in GetNode("UIExternal").GetChildren())
			obj.QueueFree();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
    }

	public async void ChangeCurrentScene(PackedScene scene, bool loadDefaultStartingScript=true)
	{
		await ClearCurrentScene();
		var sceneInstance = scene.Instantiate();
		CurrentScene.AddChild(sceneInstance);
	}

	private async Task ClearCurrentScene()
	{
		foreach (var obj in CurrentScene.GetChildren())
			obj.QueueFree();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
    }

	private async Task ClearEverything()
	{
		await ClearUIExternal();
		await ClearCurrentScene();
	}

	private void ShowHUD() => GetNode<Control>("UI/HUD").Visible = true;
	private void HideHUD() => GetNode<Control>("UI/HUD").Visible = false;
}
