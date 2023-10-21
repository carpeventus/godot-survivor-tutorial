using Godot;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Export] public int MaxHeath;

	[Signal] public delegate void DiedEventHandler();
	public int CurrentHeath { get; private set; }
	public override void _Ready()
	{
		CurrentHeath = MaxHeath;
	}

	public void TakeDamage(int damage)
	{
		CurrentHeath = Mathf.Max(MaxHeath - damage, 0);
		if (CurrentHeath <= 0)
		{ 
			// 这里使用CallDeferred是因为不能在碰撞时改变状态，具体来说就是不能EmitSignal
			Callable.From(EmitDied).CallDeferred();
		}
	}

	private void EmitDied()
	{
		EmitSignal(SignalName.Died);
		Owner.QueueFree();
	}
}
