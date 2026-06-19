using Godot;
using System.Linq;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    [Export] public PackedScene MainMenuScene;
    [Export] public PackedScene GameOverScreen;
    [Export] public PackedScene GameScene;
    [Export] public PackedScene CreditsScene;
    [Export] public PackedScene SettingsScene;
    [Export] public PackedScene PauseScene;

    private Node CurrentScene;
    private UIAudioManager UIAudio;
    private AudioManager Audio;

    //flags
    private bool IsInGame = false;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CurrentScene = GetNode<Node>("CurrentScene");
        UIAudio = GetNode<UIAudioManager>("UIAudioManager");
        Audio = GetNode<AudioManager>("AudioManager");

        LoadMainMenu("game_start");
    }

    public override void _Input(InputEvent @event)
    {
        //pause menu action, should work everywhere
        if(@event.IsActionPressed("pause") && IsInGame)
            LoadPauseMenu("pause_event");
    }

    public async void LoadMainMenu(string origin)
    {
        IsInGame = false;
        HideHUD();

        //just ensure time scale is not lost
        if (origin == "pause_menu")
            Engine.TimeScale = 1.0f;

        if (origin == "credits_screen" || origin == "settings_screen")
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

        var container = GetNode<CanvasLayer>("CurrentScene/MainMenu/%MainMenuContainer");
        await RemoveChildrenFromContainer(container);

        var parent = mainMenu.GetNode("%MainMenuContainer");
        //copy from parent to container
        foreach (var child in mainMenu.GetNode("%MainMenuContainer").GetChildren())
        {
            parent.RemoveChild(child);
            container.AddChild(child);
        }
    }

    public async void LoadPauseMenu(string origin)
    {
        HideHUD();

        Node MainMenuNode = CurrentScene.GetNodeOrNull("MainMenu");
        if (MainMenuNode != null)
        {
            MainMenuNode.QueueFree();
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
        }

        MainMenu mainMenu = MainMenuScene.Instantiate<MainMenu>();
        var mainMenuContainer = mainMenu.GetNode<CanvasLayer>("%MainMenuContainer");

        await RemoveChildrenFromContainer(mainMenuContainer);
        await RemoveEverythingExcept(mainMenu, [mainMenuContainer]);

        PauseMenu pauseMenu = PauseScene.Instantiate() as PauseMenu;
        UIAudio.InstallSounds(pauseMenu);
        pauseMenu.SettingsPressed += OpenSettings;
        pauseMenu.MainMenuPressed += LoadMainMenu;
        pauseMenu.ContinuePressed += ClosePauseMenu;

        mainMenuContainer.AddChild(pauseMenu);

        CurrentScene.AddChild(mainMenu);

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed

        //set time scale, to stop time in the background
        Engine.TimeScale = 0;
        IsInGame = false;
    }

    public async void ClosePauseMenu(string origin)
    {
        ShowHUD();
        MainMenu mainMenuNode = CurrentScene.GetNode<MainMenu>("MainMenu");
        mainMenuNode.QueueFree();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
        Engine.TimeScale = 1.0f;

        IsInGame = true;
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
        IsInGame = true;
    }

    public async void OpenSettings(string origin)
    {
        Settings settingsMenu = SettingsScene.Instantiate() as Settings;

        if (origin == "pause_menu")
            settingsMenu.OnBackLoadPauseMenu = true;

        UIAudio.InstallSounds(settingsMenu);

        var container = GetNode<CanvasLayer>("CurrentScene/MainMenu/%MainMenuContainer");
        await RemoveChildrenFromContainer(container);

        container.AddChild(settingsMenu);
    }

    public async void OpenCredits(string origin)
    {
        Credits creditsMenu = CreditsScene.Instantiate() as Credits;
        UIAudio.InstallSounds(creditsMenu);

        var container = GetNode<CanvasLayer>("CurrentScene/MainMenu/%MainMenuContainer");
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
        foreach (var child in container.GetChildren())
            child.QueueFree();

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); //wait 1 idle frame to ensure game manager has been removed
    }

    private async Task RemoveEverythingExcept(Node container, Node[] except)
    {
        foreach(var child in container.GetChildren())
        {
            if (except.Contains(child))
                continue;

            child.QueueFree();
        }

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
