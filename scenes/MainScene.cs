using Godot;
using System;

public partial class MainScene : Node2D {
	[Export] public PackedScene EndScreenScene; 
	
	public PackedScene PauseMenuScene { get; private set; } 
	
	public Player Player { get; private set; }
	
	public override void _Ready()
	{
		PauseMenuScene = ResourceLoader.Load<PackedScene>("res://ui/pause_menu.tscn");
		Player = GetNode<Player>("Entities/Player");
		Player.Health.Died += OnPlayerDied;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			var pauseMenu = PauseMenuScene.Instantiate<PauseMenu>();
            AddChild(pauseMenu);
            GetTree().Root.SetInputAsHandled();
		}
	}

	private void OnPlayerDied() {
		EndScreen endScreen = EndScreenScene.Instantiate<EndScreen>();
		AddChild(endScreen);
		endScreen.SetDefeatScreen();
		GetNode<MetaProgression>("/root/MetaProgression").SaveData();
	}
}
