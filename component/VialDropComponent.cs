using Godot;

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

	private void OnDied() {
		var adjustedDropPercent = DropPercent;
		var extraDropPercent = GetNode<MetaProgression>("/root/MetaProgression").GetMetaUpgradeQuantity("ExperienceGain");
		adjustedDropPercent += extraDropPercent * 0.1f;
		var threshold = GD.Randf();
		if (adjustedDropPercent < threshold)
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
