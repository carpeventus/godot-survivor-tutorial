using Godot;
using System;

public partial class AxeAbility : Node2D
{

    [Export] public float RotationCircle = 2;
    [Export] public float MaxRadius = 200f;

    public override void _Ready() {
        Tween tween = CreateTween();
        tween.TweenMethod(Callable.From<float>(RotateAndIncreaseRadius), 0.0, 2.0, 3.0);
        tween.TweenCallback(Callable.From(QueueFree));
    }

    private void RotateAndIncreaseRadius(float rotation) {
        
    }
    
}
