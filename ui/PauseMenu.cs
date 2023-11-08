using Godot;
using System;

public partial class PauseMenu : CanvasLayer {

	private PanelContainer _panelContainer;
	private AnimationPlayer _animationPlayer;
	private PackedScene _optionMenuScene;

	private bool _isClosing;
	public override void _Ready()
	{
		_optionMenuScene = ResourceLoader.Load<PackedScene>("res://ui/options_menu.tscn");

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_panelContainer = GetNode<PanelContainer>("%PanelContainer");
		_panelContainer.PivotOffset = _panelContainer.Size / 2;
		GetNode<Button>("%ResumeButton").Pressed += OnResumeButtonPressed;
		GetNode<Button>("%OptionsButton").Pressed += OnOptionsButtonButtonPressed;
		GetNode<Button>("%QuitToMenuButton").Pressed += OnQuitToMenuButtonPressed;
		
		GetTree().Paused = true;
		_animationPlayer.Play("default");
		var tween = CreateTween();
		tween.TweenProperty(_panelContainer, "scale", Vector2.Zero, 0f);
		tween.TweenProperty(_panelContainer, "scale", Vector2.One, 0.2f)
			.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
		
	}
	
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			Close();
			GetTree().Root.SetInputAsHandled();
		}
	}
	
	private void OnQuitToMenuButtonPressed() {
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void OnOptionsButtonButtonPressed() {
		var optionsMenu = _optionMenuScene.Instantiate<OptionsMenu>();
		AddChild(optionsMenu);
		optionsMenu.BackButtonPressed += () => { OnOptionBackButtonPressed(optionsMenu); };
	}

	
	private void OnOptionBackButtonPressed(OptionsMenu optionsMenu)
	{
		optionsMenu.QueueFree();
	}


	private void Close()
	{
		if (_isClosing)
		{
			return;
		}

		_isClosing = true;
		_animationPlayer.PlayBackwards("default");
		
		var tween = CreateTween();
		tween.TweenProperty(_panelContainer, "scale", Vector2.One, 0f);
		tween.TweenProperty(_panelContainer, "scale", Vector2.Zero, 0.2f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);
		tween.TweenCallback(Callable.From(Resume));
	}
	
	private void OnResumeButtonPressed() {
		Close();
	}


	private void Resume() {
		GetTree().Paused = false;
		QueueFree();
	}
}
