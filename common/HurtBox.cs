using Godot;

public partial class HurtBox : Area2D {
	[Signal]
	public delegate void HurtEventHandler(HitBox hitBox);
	public override void _Ready() {
		AreaEntered += OnHurt;
	}

	private void OnHurt(Area2D area) {
		if (area is HitBox hitBox) {
			EmitSignal(SignalName.Hurt, hitBox);
		}
	}
}
