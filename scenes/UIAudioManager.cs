using Godot;
using System;

public partial class UIAudioManager : Node
{
    public override void _Ready()
    {
        InstallSounds(GetTree().Root);
    }

    public void InstallSounds(Node node)
    {
        foreach (Node n in node.GetChildren())
        {
            if (n is BaseButton button)
            {
                button.MouseEntered += PlayButtonHover;
                button.Pressed += PlayButtonClick;
            }
            //Add more if needed

            //recursion
            InstallSounds(n);
        }

    }


    public void PlayButtonClick() => GetNode<AudioStreamPlayer>("ButtonClick").Play();

    public void PlayButtonHover() => GetNode<AudioStreamPlayer>("ButtonHover").Play();
}
