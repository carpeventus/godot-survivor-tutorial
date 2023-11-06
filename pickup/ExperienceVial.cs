using Godot;
using System;

public partial class ExperienceVial : Area2D
{

	private bool _beCollected;
	private Sprite2D _sprite;
	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		AreaEntered += OnPickupAreaEntered;
	}

	private void OnPickupAreaEntered(Node2D body)
	{
		GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred(CollisionShape2D.MethodName.SetDisabled, true);
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.TweenProperty(_sprite, "scale", Vector2.Zero, 0.05f).SetDelay(0.45);
		tween.TweenMethod(Callable.From<float>((percent => TweenCollected(percent, GlobalPosition))), 0f, 1f, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Expo);
		// 等前面的执行完了，执行Callback
		tween.Chain().TweenCallback(Callable.From(Collected));
		GetNode<RandomAudioStreamPlayer2D>("RandomAudioStreamPlayer2D").PlayerRandom();
	}

	private void Collected()
	{
		if (_beCollected)
		{
			return;
		}
		_beCollected = true;
		// GD 可以像静态方法一样调用 GameEvents.EmitExperienceVialCollected(1f);
		// C# 只能👇这样...
		GetNode<GameEvents>("/root/GameEvents").EmitExperienceVialCollected(1f);
		QueueFree();
	}
	
	private void TweenCollected(float percent, Vector2 startPosition)
	{
		if (GetTree().GetFirstNodeInGroup("Player") is not Player player)
		{
			return;	
		}

		if (GlobalPosition.DistanceTo(player.GlobalPosition) <= 8f)
		{
			Collected();
		}
		GlobalPosition = startPosition.Lerp(player.GlobalPosition, percent);
		Vector2 direction = (player.GlobalPosition - startPosition).Normalized();
		// 要正确的旋转方向，要将经验瓶的头部指向右边，但是现在是指向上边的，相当于一开始就往逆时针方向都旋转了90°。因此+90°顺时针旋转来修正这个
		var targetRotation = Mathf.RadToDeg(direction.Angle()) + Mathf.DegToRad(90);
		Rotation = Mathf.LerpAngle(RotationDegrees, targetRotation, (float)(1 - Mathf.Exp(-GetProcessDeltaTime())));
	}
}
