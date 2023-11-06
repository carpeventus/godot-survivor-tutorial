using Godot;

public partial class HurtBox : Area2D {
	[Export] public HealthComponent HealthComponent;

	public PackedScene FloatingTextScene;

	[Signal]
	public delegate void HurtEventHandler();
	
	public override void _Ready()
	{
		FloatingTextScene = ResourceLoader.Load<PackedScene>("res://ui/floating_text.tscn");
		AreaEntered += OnHurt;
	}

	private void OnHurt(Area2D area) {
		if (area is not HitBox hitBox) {
			return;
		}
        HealthComponent?.TakeDamage(hitBox.Damage);
        EmitSignal(SignalName.Hurt);
        var floatingText = FloatingTextScene.Instantiate<FloatingText>();
        GetTree().GetFirstNodeInGroup("EntitiesLayer").AddChild(floatingText);
        floatingText.GlobalPosition = hitBox.GlobalPosition + Vector2.Up * 8;
        floatingText.Start(hitBox.Damage.ToString());
	}
}
