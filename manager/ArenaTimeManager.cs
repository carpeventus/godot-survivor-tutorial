using Godot;
using System;

public partial class ArenaTimeManager : Node {
	[Export] public PackedScene EndScreenScene;
	[Export] public int DifficultyInterval = 5;

	[Signal]
	public delegate void ArenaDifficultyUpEventHandler(int difficulty);
	public int ArenaDifficulty { get; private set; } = 0;
	private Timer _timer;

	public override void _Process(double delta)
	{
		// 每隔5秒增加难度
		var nextTimeTarget = _timer.WaitTime - ((ArenaDifficulty + 1) * DifficultyInterval);
		if (_timer.TimeLeft <= nextTimeTarget)
		{
			ArenaDifficulty += 1;
			EmitSignal(SignalName.ArenaDifficultyUp, ArenaDifficulty);
		}
	}

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
