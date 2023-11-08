using Godot;
using System;

public partial class ScreenTransitionLayer : CanvasLayer
{
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public async void Transition(string sceneName)
	{
		_animationPlayer.Play("default");
		await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
		GetTree().ChangeSceneToFile(sceneName);
		_animationPlayer.PlayBackwards("default");
	}
	
	public async void ReloadTransition()
	{
		_animationPlayer.Play("default");
		await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
		GetTree().ReloadCurrentScene();
		_animationPlayer.PlayBackwards("default");
	}
	
}
