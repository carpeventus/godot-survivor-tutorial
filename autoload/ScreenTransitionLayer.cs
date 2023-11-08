using Godot;
using System;

public partial class ScreenTransitionLayer : CanvasLayer
{
	[Signal]
	public delegate void TransitionedEventHandler();

	private AnimationPlayer _animationPlayer;

	private bool _skipEmit;
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public async void Transition()
	{
		_animationPlayer.Play("default");
		await ToSignal(this, SignalName.Transitioned);
		_skipEmit = true;
		_animationPlayer.PlayBackwards("default");

	}

	public  void EmitScreenTransitioned()
	{
		if (_skipEmit)
		{
			_skipEmit = false;
			return;
		}

		EmitSignal(SignalName.Transitioned);
	}
	
}
