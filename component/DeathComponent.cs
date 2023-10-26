using Godot;
using System;

public partial class DeathComponent : Node2D
{
	[Export] public HealthComponent HealthComponent;
	[Export] public Sprite2D Sprite;

	private GpuParticles2D _gpuParticles;
	public AnimationPlayer AnimationPlayer { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_gpuParticles = GetNode<GpuParticles2D>("GPUParticles2D");
		_gpuParticles.Texture = Sprite.Texture;
		HealthComponent.Died += OnDied;
	}

	private void OnDied()
	{
		MoveToEntityLayer();
		Die();
	}

	private void MoveToEntityLayer()
	{
		if (Owner is not Node2D owner)
		{
			return;
		}
		var entities = GetTree().GetFirstNodeInGroup("EntitiesLayer");
		GetParent().RemoveChild(this);
		entities.AddChild(this);
		GlobalPosition =  owner.GlobalPosition;
	}
	private void Die()
	{
		AnimationPlayer.Play("death");
	}
}
