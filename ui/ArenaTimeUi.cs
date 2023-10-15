
using Godot;
using System;
using System.Globalization;

public partial class ArenaTimeUi : CanvasLayer {
	[Export] public ArenaTimeManager ArenaTimeManager;
	private Label _label;
	public override void _Ready() {
		_label = GetNode<Label>("MarginContainer/Label");
	}

	public override void _Process(double delta) {
		ShowTime();
	}

	private void ShowTime() {
		_label.Text = FormatTime(ArenaTimeManager.GetTimeElapse());
	}

	private string FormatTime(double time) {
		var minute = Mathf.FloorToInt(time / 60);
		var second = Mathf.FloorToInt(time % 60);
		return $"{minute:00}" + " : " + $"{second:00}";
	}
}
