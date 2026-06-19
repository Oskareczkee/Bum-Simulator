using Godot;
using System.Threading.Tasks;

public partial class GameManager : Node
{
	[Export] public PackedScene MainMenuScene;
	[Export] public PackedScene GameOverScreen;
    [Export] public PackedScene GameScene;
	[Export] public PackedScene CreditsScene;

	private Node CurrentScene;
	private UIAudioManager UIAudio;
	private AudioManager Audio;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		CurrentScene = GetNode<Node>("CurrentScene");
		UIAudio = GetNode<UIAudioManager>("UIAudioManager");
		Audio = GetNode<AudioManager>("AudioManager");

		LoadMainMenu("game_start");
	}

	public async void LoadMainMenu(string origin)
	{
		HideHUD();

        //await ClearEverything();

        //if (origin == "end_game_screen" ||)


		if(origin == "credits_screen")
		{
			LoadMainMenuIntoContainer(); //loads main menu container into previously created main menu container without disturbing main menu background music
			return;
        }

		await ClearEverything();

        MainMenu mainMenu = MainMenuScene.Instantiate() as MainMenu;
		UIAudio.InstallSounds(mainMenu);

		mainMenu.Connect(MainMenu.SignalName.NewGamePressed, Callable.From<string>(NewGame));
        mainMenu.Connect(MainMenu.SignalName.SettingsPressed, Callable.From<string>(OpenSettings));
        mainMenu.Connect(MainMenu.SignalName.CreditsPressed, Callable.From<string>(OpenCredits));
        mainMenu.Connect(MainMenu.SignalName.QuitPressed, Callable.From(QuitGame));

        CurrentScene.AddChild(mainMenu);
	}

	private async void LoadMainMenuIntoContainer()
	{
        MainMenu mainMenu = MainMenuScene.Instantiate() as MainMenu;
        UIAudio.InstallSounds(mainMenu);

        mainMenu.Connect(MainMenu.SignalName.NewGamePressed, Callable.From<string>(NewGame));
        mainMenu.Connect(MainMenu.SignalName.SettingsPressed, Callable.From<string>(OpenSettings));
        mainMenu.Connect(MainMenu.SignalName.CreditsPressed, Callable.From<string>(OpenCredits));
        mainMenu.Connect(MainMenu.SignalName.QuitPressed, Callable.From(QuitGame));

        var container = GetNode<Node>("CurrentScene/MainMenu/%MainMenuContainer");
        await RemoveChildrenFromContainer(container);

		var parent = mainMenu.GetNode("%MainMenuContainer");

		//copy from parent to container
        foreach (var child in mainMenu.GetNode("%MainMenuContainer").GetChildren())
		{
			parent.RemoveChild(child);
			container.AddChild(child);
		}
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

	public async void OpenSettings(string origin)
	{
	}

	public async void OpenCredits(string origin)
	{
        Credits creditsMenu = CreditsScene.Instantiate() as Credits;
        UIAudio.InstallSounds(creditsMenu);

        var container = GetNode<Node>("CurrentScene/MainMenu/%MainMenuContainer");
        await RemoveChildrenFromContainer(container);

        container.AddChild(creditsMenu);
    }

	public void QuitGame()
	{
		GetTree().Quit();
	}

	public void GameOver()
	{
		HideHUD();
        GameOver gameOverScene = GameOverScreen.Instantiate() as GameOver;
		UIAudio.InstallSounds(gameOverScene);

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

	public async void ChangeCurrentScene(PackedScene scene)
	{
		await PlayCloseTransition();
		await ClearCurrentScene();

		var sceneInstance = scene.Instantiate();
		CurrentScene.AddChild(sceneInstance);

		await PlayOpenTransition();
	}

	private async Task PlayOpenTransition()
	{
		ColorRect rectangle = GetNode<ColorRect>("UI/Transitions/CircleTransition/ColorRect");
		rectangle.Visible = true; //make it visible

       AnimationPlayer transitionPlayer = GetNode<AnimationPlayer>("UI/Transitions/CircleTransition/ColorRect/AnimationPlayer");
		transitionPlayer.Play("open");

		await ToSignal(transitionPlayer, AnimationPlayer.SignalName.AnimationFinished);
        rectangle.Visible = false; //make it invisible
    }

	private async Task PlayCloseTransition()
	{
        ColorRect rectangle = GetNode<ColorRect>("UI/Transitions/CircleTransition/ColorRect");
        rectangle.Visible = true; //make it visible

        AnimationPlayer transitionPlayer = GetNode<AnimationPlayer>("UI/Transitions/CircleTransition/ColorRect/AnimationPlayer");
        transitionPlayer.Play("close");

        await ToSignal(transitionPlayer, AnimationPlayer.SignalName.AnimationFinished);
        rectangle.Visible = false; //make it invisible
    }

	private async Task ClearCurrentScene()
	{
		foreach (var obj in CurrentScene.GetChildren())
			obj.QueueFree();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
    }

	private async Task RemoveChildrenFromContainer(Node container)
	{
		foreach(var child in container.GetChildren())
			child.QueueFree();

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
