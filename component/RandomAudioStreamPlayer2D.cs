using Godot;
using System;
using Godot.Collections;

public partial class RandomAudioStreamPlayer2D : AudioStreamPlayer2D {
	[Export] public Array<AudioStream> Sounds;
	[Export] public bool RandomPitch = true;
	[Export] public float MinPitch = 0.9f;
	[Export] public float MaxPitch = 1.1f;
	public void PlayerRandom() {
		var sound = Sounds.PickRandom();
		Stream = sound;
		if (RandomPitch) {
			PitchScale = (float) GD.RandRange(MinPitch, MaxPitch);
		}
		Play();
	}
	
}
