
using Godot;

[GlobalClass]
public partial class SoundButton : Button
{
	public override void _Ready() {
		Pressed += OnPressed;
	}

	private void OnPressed() {
		GetNode<RandomAudioStreamPlayer>("RandomAudioStreamPlayer").PlayerRandom();
	}
}
