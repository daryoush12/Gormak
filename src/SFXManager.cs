using Godot;
using System;

public partial class SFXManager : AudioStreamPlayer2D
{
	public static SFXManager _sfxmanager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if (_sfxmanager == null)
        {
            _sfxmanager = this;
        }
        else
        {
            this.QueueFree();
        }
    }

	public void PlaySFX(AudioStream sfx)
    {
        this.Stream = sfx;
        this.Play();
    }
}
