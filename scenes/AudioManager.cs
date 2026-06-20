using Godot;
using System;

public partial class AudioManager : Node
{
    public void PlaySellSound() => GetNode<AudioStreamPlayer>("SellSound").Play();
    public void PlayRejectSound() => GetNode<AudioStreamPlayer>("RejectSound").Play();
}
