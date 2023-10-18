using Godot;
using System;

public partial class ArenaTimeManager : Node {
	private Timer _timer;
	public override void _Ready() {
		_timer = GetNode<Timer>("Timer");
	}

	public double GetTimeElapse() {
		return _timer.WaitTime - _timer.TimeLeft;
	}
}
