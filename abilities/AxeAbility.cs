using Godot;
using System;

public partial class AxeAbility : Node2D
{

    [Export] public float RotationCircle = 2.0f;
    [Export] public float MaxRadius = 100f;

    private Vector2 _baseRotateInitDirection = Vector2.Right;
    
    public HitBox HitBox { get; private set; }
    
    public override void _Ready()
    {
        HitBox = GetNode<HitBox>("HitBox");
        _baseRotateInitDirection = Vector2.Right.Rotated(GD.Randf() * Mathf.Tau);
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(RotateAndIncreaseRadius), 0.0f, RotationCircle, 3f);
        tween.TweenCallback(Callable.From(QueueFree));
    }

    private void RotateAndIncreaseRadius(float rotations)
    {
        var percent = rotations / RotationCircle;
        var currentRadius = percent * MaxRadius;
        // 0、1、2圈都是2pi
        var currentDirection =_baseRotateInitDirection.Rotated(rotations * Mathf.Tau);
        Node2D player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        if (player is null)
        {
            return;
        }
        
        var rootPosition = player.GlobalPosition;
        GlobalPosition = rootPosition + currentDirection * currentRadius;
    }
    
}
