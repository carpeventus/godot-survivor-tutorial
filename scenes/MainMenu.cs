using Godot;
using System;

public partial class MainMenu : CanvasLayer
{

	private PackedScene _optionMenuScene;
	public override void _Ready()
	{
		GetNode<Button>("%PlayButton").Pressed += OnPressedPlay;
		GetNode<Button>("%QuitButton").Pressed += OnPressedQuit;
		GetNode<Button>("%UpgradesButton").Pressed += OnUpgradesPressed;
		GetNode<Button>("%OptionsButton").Pressed += OnPressedOptions;
		_optionMenuScene = ResourceLoader.Load<PackedScene>("res://ui/options_menu.tscn");
	}

	private void OnUpgradesPressed() {
		var screenTransitionLayer = GetNode<ScreenTransitionLayer>("/root/ScreenTransitionLayer");
		screenTransitionLayer.Transition("res://ui/meta_menu.tscn");
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
		GetNode<MetaProgression>("/root/MetaProgression").SaveData();
		GetTree().Quit();

	}

	private void OnPressedPlay()
	{
		var screenTransitionLayer = GetNode<ScreenTransitionLayer>("/root/ScreenTransitionLayer");
		screenTransitionLayer.Transition("res://scenes/main_scene.tscn");
	}
}
