using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D {
	[Export] public float Speed = 40f;
	
	public HealthComponent HealthComponent { get; private set; }
	public Node2D SpriteWrap { get; private set; }

	public override void _Ready() {
		SpriteWrap = GetNode<Node2D>("SpriteWrap");
		HealthComponent = GetNode<HealthComponent>("HealthComponent");
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = GetDirectionToPlayer();
		Velocity = Speed * direction;
		MoveAndSlide();
	}

	public override void _Process(double delta) {

		
		int sign = Mathf.Sign(Velocity.X);
		if (sign != 0)
		{
			SpriteWrap.Scale = new Vector2(sign, 1);
		}
	}

	private Vector2 GetDirectionToPlayer() {
		if (GetTree().GetFirstNodeInGroup("Player") is Player player) {
			return (player.GlobalPosition - GlobalPosition).Normalized();
		}
		return Vector2.Zero;
	}
}
