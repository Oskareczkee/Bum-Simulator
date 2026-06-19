using Godot;
using System;

public partial class Settings : Control
{
    [Signal] public delegate void BackPressedEventHandler(string origin);

    private Label MasterVolumeValue;
    private Label MusicVolumeValue;
    private Label SFXVolumeValue;
	private Label DialogueVolumeValue;

    private HSlider MasterVolumeSlider;
    private HSlider MusicVolumeSlider;
    private HSlider SFXVolumeSlider;
	private HSlider DialogueVolumeSlider;

    private const double SlidersMaxDb = 10;
	private const double SlidersMinDb = -40;

    public bool OnBackLoadPauseMenu = false;

    public override void _Ready()
    {
		MasterVolumeValue = GetNode<Label>("%MasterVolumeValue");
        MusicVolumeValue = GetNode<Label>("%MusicVolumeValue");
        SFXVolumeValue = GetNode<Label>("%SFXVolumeValue");
		DialogueVolumeValue = GetNode<Label>("%DialogueVolumeValue");

		MasterVolumeSlider = GetNode<HSlider>("%MasterVolumeSlider");
        MusicVolumeSlider = GetNode<HSlider>("%MusicVolumeSlider");
        SFXVolumeSlider = GetNode<HSlider>("%SFXVolumeSlider");
        DialogueVolumeSlider = GetNode<HSlider>("%DialogueVolumeSlider");

        InitSlidersAndValues();

        if(!OnBackLoadPauseMenu)
            BackPressed += (GetNode("/root/GameManager") as GameManager).LoadMainMenu;
        else
            BackPressed += (GetNode("/root/GameManager") as GameManager).LoadPauseMenu;
    }

    private void OnMasterVolumeChanged(float value)
    {
        int bus = AudioServer.GetBusIndex("Master");

        AudioServer.SetBusVolumeDb(bus, value);
        AudioServer.SetBusMute(bus, value <= SlidersMinDb);

        MasterVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(value, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";
    }

    private void OnMusicVolumeChanged(float value)
    {
        int bus = AudioServer.GetBusIndex("Music");

        AudioServer.SetBusVolumeDb(bus, value);
        AudioServer.SetBusMute(bus, value <= SlidersMinDb);

        MusicVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(value, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";
    }

    private void OnSFXVolumeChanged(float value)
    {
        int bus = AudioServer.GetBusIndex("SFX");

        AudioServer.SetBusVolumeDb(bus, value);
        AudioServer.SetBusMute(bus, value <= SlidersMinDb);

        SFXVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(value, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";
    }

    private void OnDialogueVolumeChanged(float value)
    {
        int bus = AudioServer.GetBusIndex("Dialogue");

        AudioServer.SetBusVolumeDb(bus, value);
        AudioServer.SetBusMute(bus, value <= SlidersMinDb);

        DialogueVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(value, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";
    }

    private void OnBackPressed()
    {
        EmitSignal(SignalName.BackPressed, "settings_screen");
    }

    private void InitSlidersAndValues()
    {
        int masterBus = AudioServer.GetBusIndex("Master");
        int musicBus = AudioServer.GetBusIndex("Music");
        int sfxBus = AudioServer.GetBusIndex("SFX");
        int dialogueBus = AudioServer.GetBusIndex("Dialogue");

        float masterDb = AudioServer.GetBusVolumeDb(masterBus);
        float musicDb = AudioServer.GetBusVolumeDb(musicBus);
        float sfxDb = AudioServer.GetBusVolumeDb(sfxBus);
        float dialogueDb = AudioServer.GetBusVolumeDb(dialogueBus);

        MasterVolumeSlider.MinValue = SlidersMinDb;
        MasterVolumeSlider.MaxValue = SlidersMaxDb;
        MasterVolumeSlider.Value = masterDb;

        MusicVolumeSlider.MinValue = SlidersMinDb;
        MusicVolumeSlider.MaxValue = SlidersMaxDb;
        MusicVolumeSlider.Value = musicDb;

        SFXVolumeSlider.MinValue = SlidersMinDb;
        SFXVolumeSlider.MaxValue = SlidersMaxDb;
        SFXVolumeSlider.Value = sfxDb;

        DialogueVolumeSlider.MinValue = SlidersMinDb;
        DialogueVolumeSlider.MaxValue = SlidersMaxDb;
        DialogueVolumeSlider.Value = dialogueDb;

        MasterVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(masterDb, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";

        MusicVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(musicDb, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";

        SFXVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(sfxDb, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";

        DialogueVolumeValue.Text =
            $"{Math.Round(Mathf.Remap(dialogueDb, (float)SlidersMinDb, (float)SlidersMaxDb, 0, 100))}%";
    }
}
