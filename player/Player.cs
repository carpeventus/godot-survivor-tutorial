using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float MoveSpeed = 60.0f;
	[Export] public float Acceleration = 20.0f;
	
	public Vector2 InputMovement { get; private set; }
	public Vector2 Direction { get; private set; }
	
	
	public override void _PhysicsProcess(double delta) {
		var targetVelocity = Direction * MoveSpeed;
		Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-delta * 20)));
		MoveAndSlide();
	}

	public override void _Process(double delta) {
		HandleInput();
	}

	private void HandleInput() {
		InputMovement = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Direction = InputMovement.Normalized();
	}
}
