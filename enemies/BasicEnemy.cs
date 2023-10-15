using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D {
	[Export] public float Speed = 40f;

	public override void _Ready() {
		GetNode<HurtBox>("HurtBox").Hurt += OnHurt;
	}

	private void OnHurt(HitBox hitBox) {
		QueueFree();
	}
	
	public override void _Process(double delta) {
		var direction = GetDirectionToPlayer();
		Velocity = Speed * direction;
		MoveAndSlide();
	}

	private Vector2 GetDirectionToPlayer() {
		if (GetTree().GetFirstNodeInGroup("player") is Player player) {
			return (player.GlobalPosition - GlobalPosition).Normalized();
		}
		return Vector2.Zero;
	}
}
