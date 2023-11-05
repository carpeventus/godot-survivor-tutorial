using Godot;
using System;

public partial class VignetteUI : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GetNode<GameEvents>("/root/GameEvents").PlayerTakeDamage += OnPlayerHurt;
	}
	public override void _ExitTree() {
		base._ExitTree();
		GetNode<GameEvents>("/root/GameEvents").PlayerTakeDamage -= OnPlayerHurt;
	}

	private void OnPlayerHurt() {
		GetNode<AnimationPlayer>("AnimationPlayer").Play("hurt");
	}


}
