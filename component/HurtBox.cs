using Godot;

public partial class HurtBox : Area2D {
	[Export] public HealthComponent HealthComponent;
	public override void _Ready() {
		AreaEntered += OnHurt;
	}

	private void OnHurt(Area2D area) {
		if (area is not HitBox hitBox) {
			return;
		}

        HealthComponent?.TakeDamage(hitBox.Damage);
	}
}
