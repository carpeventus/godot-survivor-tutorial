using Godot;
using System;

public partial class PauseMenu : CanvasLayer {

	private PanelContainer _panelContainer;
	private AnimationPlayer _animationPlayer;
	public override void _Ready() {
		GetTree().Paused = true;
		_panelContainer = GetNode<PanelContainer>("%PanelContainer");
		GetNode<Button>("%ResumeButton").Pressed += OnResumeButtonPressed;
		GetNode<Button>("%OptionsButton").Pressed += OnOptionsButtonButtonPressed;
		GetNode<Button>("%QuitToMenuButton").Pressed += OnQuitToMenuButtonPressed;
		
		_animationPlayer.Play("default");
		
		var tween = CreateTween();
		tween.TweenProperty(_panelContainer, "scale", Vector2.Zero, 0f);
		tween.TweenProperty(_panelContainer, "scale", Vector2.One, 0.2f)
			.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
		
	}

	private void OnQuitToMenuButtonPressed() {
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void OnOptionsButtonButtonPressed() {
	}

	
	private void OnResumeButtonPressed() {
		_animationPlayer.PlayBackwards("default");
		
		var tween = CreateTween();
		tween.TweenProperty(_panelContainer, "scale", Vector2.One, 0f);
		tween.TweenProperty(_panelContainer, "scale", Vector2.Zero, 0.2f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);
		tween.TweenCallback(Callable.From(Resume));
	}


	private void Resume() {
		GetTree().Paused = false;
		QueueFree();
	}
}
