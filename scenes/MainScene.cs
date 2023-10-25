using Godot;
using System;

public partial class MainScene : Node2D {
	[Export] public PackedScene EndScreenScene; 
	public Player Player { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Player = GetNode<Player>("Entities/Player");
		Player.Health.Died += OnPlayerDied;
	}

	private void OnPlayerDied() {
		EndScreen endScreen = EndScreenScene.Instantiate<EndScreen>();
		AddChild(endScreen);
		endScreen.SetDefeatScreen();
	}
}
