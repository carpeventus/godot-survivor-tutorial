using Godot;
using System;

public partial class VelocityComponent : Node
{
    [Export] public float MaxSpeed = 40f;
    [Export] public float Acceleration = 20.0f;
    
    public Vector2 BodyVelocity = Vector2.Zero;

    public void AccelerateInDirection(Vector2 direction)
    {
        var targetVelocity = MaxSpeed * direction;
        BodyVelocity = BodyVelocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-Acceleration * GetProcessDeltaTime() * 20)));
    }
    
    public void Move(CharacterBody2D body)
    {
        body.Velocity = BodyVelocity;
        body.MoveAndSlide();
        BodyVelocity = body.Velocity;
    }

    public void Decelerate()
    {
        AccelerateInDirection(Vector2.Zero);
    }
    
    public void AccelerateToPlayer() {
        if (Owner is not Node2D owner)
        {
            return;
        }
        if (GetTree().GetFirstNodeInGroup("Player") is Player player) {
            var direction =  (player.GlobalPosition - owner.GlobalPosition).Normalized();
            AccelerateInDirection(direction);
        }
    }
}
