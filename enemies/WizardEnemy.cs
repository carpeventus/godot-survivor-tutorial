using Godot;
using System;

public partial class WizardEnemy : CharacterBody2D
{
	public Node2D SpriteWrap { get; private set; }
	public VelocityComponent VelocityComponent { get; private set; }
	public bool IsMoving { get; private set; } = false;

	public override void _Ready() {
		SpriteWrap = GetNode<Node2D>("SpriteWrap");
		VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsMoving)
		{
			VelocityComponent.AccelerateToPlayer();
		}
		else
		{
			VelocityComponent.Decelerate();
		}
		VelocityComponent.Move(this);
	}

	public void SetIsMoving(bool moving)
	{
		IsMoving = moving;
	}

	public override void _Process(double delta) {
		int sign = Mathf.Sign(Velocity.X);
		if (sign != 0)
		{
			SpriteWrap.Scale = new Vector2(sign, 1);
		}
	}
}
