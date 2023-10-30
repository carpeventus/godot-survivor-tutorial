using Godot;
using System;

public partial class HitFlashComponent : Node {
	[Export] public Sprite2D Sprite;
	[Export] public HealthComponent Health;
	[Export] public ShaderMaterial HitFlashShader;
	public Tween RunningTween { get; private set; }
	public override void _Ready() {
		Health.HealthChange += OnHealthChanged;
		Sprite.Material = HitFlashShader;
	}

	private void OnHealthChanged() {
		if (RunningTween is not null && RunningTween.IsValid()) {
			RunningTween.Kill();
		}
		HitFlashShader.SetShaderParameter("weight", 1.0);
		GD.Print(HitFlashShader.GetShaderParameter("weight"));
		RunningTween = CreateTween();
		RunningTween.TweenProperty(Sprite.Material, "shader_parameter/weight", 0.0, 0.2)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
	}
}
