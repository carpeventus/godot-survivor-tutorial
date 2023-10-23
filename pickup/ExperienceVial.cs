using Godot;
using System;

public partial class ExperienceVial : Area2D
{
	public override void _Ready() {
		AreaEntered += OnPickupAreaEntered;
	}

	private void OnPickupAreaEntered(Node2D body) {
		// GD 可以像静态方法一样调用 GameEvents.EmitExperienceVialCollected(1f);
		// C# 只能👇这样...
		GetNode<GameEvents>("/root/GameEvents").EmitExperienceVialCollected(1f);
		QueueFree();
	}
}
