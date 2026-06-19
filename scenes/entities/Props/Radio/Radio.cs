using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Radio : StaticBody2D
{
	private AudioStreamPlayer2D MusicPlayer;
    private Interactable Interactable;

    private AnimationPlayer NoteAnimation;
    private Sprite2D FlyingNote;

	private string MusicFolder = "/Music";
	private string[] MusicPaths = [];
    private int CurrentMusicIndex = 0;

    private bool IsOff = false;
	
	public override void _Ready()
	{
        MusicFolder = $"{SceneFilePath.GetBaseDir()}{MusicFolder}";

        MusicPlayer = GetNode<AudioStreamPlayer2D>("MusicPlayer");
        Interactable = GetNode<Interactable>("Interactable");
        NoteAnimation = GetNode<AnimationPlayer>("FlyingNote/FlyingNoteAnimation");
        FlyingNote = GetNode<Sprite2D>("FlyingNote");

        MusicPlayer.Finished += OnFinishedTrack;
        LoadMusicPaths();
        ChangeSong(); //stream is empty by default


        Interactable.Interact += OnInteract;
	}

    private void OnFinishedTrack()
    {
        CurrentMusicIndex++;
        if (CurrentMusicIndex >= MusicPaths.Length)
            CurrentMusicIndex = 0;

        ChangeSong();
    }

    private void OnInteract()
    {
        //if current index is 0, and radio is disabled, play first song
        if(CurrentMusicIndex == 0 && IsOff)
        {
            IsOff = false;
            ChangeSong();
            ShowNoteAnimation();
            return;
        }

        CurrentMusicIndex++;

        //player looped through all songs, and disabled radio
        if (CurrentMusicIndex >= MusicPaths.Length)
        {
            CurrentMusicIndex = 0;
            IsOff = true;
            MusicPlayer.Stop();
            HideNoteAnimation();

            return;
        }

        ChangeSong();
    }

    private void ChangeSong()
    {
        string path = MusicPaths[CurrentMusicIndex];
        var stream = GD.Load<AudioStream>(path);

        if (stream == null)
        {
            GD.PrintErr($"Failed to load music file: {path}");
            return;
        }

        MusicPlayer.Stream = stream;
        MusicPlayer.Play();

    }

    private void HideNoteAnimation()
    {
        FlyingNote.Visible = false;
        NoteAnimation.Stop();
    }

    private void ShowNoteAnimation()
    {
        FlyingNote.Visible = true;
        NoteAnimation.Play();
    }

    private void LoadMusicPaths()
    {
        var dir = DirAccess.Open(MusicFolder);

        if (dir == null)
        {
            GD.PrintErr($"Could not open directory: {MusicFolder}");
            return;
        }

        MusicPaths = dir.GetFiles()
            .Where(file => file.EndsWith(".mp3"))
            .Select(file => $"{MusicFolder}/{file}")
            .ToArray();
    }
}
