using Godot;
using System;

public partial class MainMenu : CanvasLayer
{

	private PackedScene _optionMenuScene;
	public override void _Ready()
	{
		GetNode<Button>("%PlayButton").Pressed += OnPressedPlay;
		GetNode<Button>("%QuitButton").Pressed += OnPressedQuit;
		GetNode<Button>("%OptionsButton").Pressed += OnPressedOptions;
		_optionMenuScene = ResourceLoader.Load<PackedScene>("res://ui/options_menu.tscn");
	}

	private void OnPressedOptions()
	{
		var optionsMenu = _optionMenuScene.Instantiate<OptionsMenu>();
		AddChild(optionsMenu);
		optionsMenu.BackButtonPressed += () => { OnBackButtonPressed(optionsMenu); };
	}

	private void OnBackButtonPressed(OptionsMenu optionsMenu)
	{
		optionsMenu.QueueFree();
	}
	
	private void OnPressedQuit()
	{
		GetTree().Quit();
	}

	private async void OnPressedPlay()
	{
		var screenTransitionLayer = GetNode<ScreenTransitionLayer>("/root/ScreenTransitionLayer");
		await ToSignal(screenTransitionLayer, ScreenTransitionLayer.SignalName.Transitioned);
		GetTree().ChangeSceneToFile("res://scenes/main_scene.tscn");
	}
}
