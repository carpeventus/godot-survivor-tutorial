using Godot;
using System;

public partial class VialDropComponent : Node
{
	// 这里hitString不要加空格
	[Export(PropertyHint.Range, "0.0,1.0,0.01")] public float DropPercent = 0.5f;
	[Export] public PackedScene ExperienceVialScene;
	[Export] public HealthComponent HealthComponent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HealthComponent.Died += OnDied;
	}

	private void OnDied()
	{
		if (GD.Randf() < DropPercent)
		{
			return;
		}
		if (Owner is not Node2D owner)
		{
			return;
		}

		var spawnPosition = owner.GlobalPosition;
		ExperienceVial experienceVial = ExperienceVialScene.Instantiate<ExperienceVial>();
		experienceVial.GlobalPosition = spawnPosition;
		var entities = GetTree().GetFirstNodeInGroup("EntitiesLayer");
		entities.AddChild(experienceVial);
	}
}
