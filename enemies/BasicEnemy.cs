using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D {
	[Export] public float Speed = 40f;
	
	public HealthComponent HealthComponent { get; private set; }

	public override void _Ready() {
		HealthComponent = GetNode<HealthComponent>("HealthComponent");
	}
	
	public override void _Process(double delta) {
		var direction = GetDirectionToPlayer();
		Velocity = Speed * direction;
		MoveAndSlide();
	}

	private Vector2 GetDirectionToPlayer() {
		if (GetTree().GetFirstNodeInGroup("Player") is Player player) {
			return (player.GlobalPosition - GlobalPosition).Normalized();
		}
		return Vector2.Zero;
	}
}
