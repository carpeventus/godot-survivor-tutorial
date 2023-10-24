using Godot;
using System;

public partial class ArenaTimeManager : Node {
	[Export] public PackedScene EndScreenScene;
	
	private Timer _timer;
	
	public override void _Ready() {
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout() {
		EndScreen victory = EndScreenScene.Instantiate<EndScreen>();
		Owner.AddChild(victory);
		victory.SetVictoryScreen();
	}
	
	public double GetTimeElapse() {
		return _timer.WaitTime - _timer.TimeLeft;
	}
}
