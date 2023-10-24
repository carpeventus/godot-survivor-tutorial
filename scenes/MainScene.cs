using Godot;
using System;

public partial class MainScene : Node2D {
	[Export] public PackedScene EndScreenScene; 
	public Player Player { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Player = GetNode<Player>("Entities/Player");
		Player.Health.Died += OnPlayerDied;
		GD.Print("Owner" + Owner);
	}

	private void OnPlayerDied() {
		EndScreen endScreen = EndScreenScene.Instantiate<EndScreen>();
		GD.Print("Init EndScreen");
		AddChild(endScreen);
		endScreen.SetDefeatScreen();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
