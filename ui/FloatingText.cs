using Godot;

public partial class FloatingText : Node2D
{

	public void Start(string text)
	{
		GetNode<Label>("Label").Text = text;
		var tween = CreateTween();
		tween.SetParallel();
		
		// 下面两个TweenProperty虽然是串行执行的，但是GlobalPosition是相等的，第二次TweenProperty并不是以执行后的结果作为起始值
		// 但是在Callback调用的方法中，GlobalPosition是所有TweenProperty执行完后的结果
		tween.TweenProperty(this, "global_position", GlobalPosition + Vector2.Up * 8, 0.3f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", Vector2.One, 0.3f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "modulate", new Color(1, 1, 1), 0.3f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Cubic);

		tween.Chain();
		tween.TweenProperty(this, "global_position", GlobalPosition + Vector2.Up * 32, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", Vector2.Zero, 0.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 0.5f)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		
		tween.Chain().TweenCallback(Callable.From(QueueFree));
	}
	
}
