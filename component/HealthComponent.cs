using Godot;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Export] public int MaxHeath;

	[Signal] public delegate void DiedEventHandler();
	[Signal] public delegate void HealthChangeEventHandler();
	public int CurrentHeath { get; private set; }
	public override void _Ready()
	{
		CurrentHeath = MaxHeath;
	}

	public void TakeDamage(int damage)
	{
		CurrentHeath = Mathf.Max(CurrentHeath - damage, 0);
		
		EmitSignal(SignalName.HealthChange);
		if (CurrentHeath <= 0)
		{ 
			// 这里使用CallDeferred是因为不能在碰撞时改变状态，具体来说就是不能AddChild
			Callable.From(EmitDied).CallDeferred();
		}
	}

	public float GetPercent()
	{
		if (MaxHeath <= 0)
		{
			return 0;
		}

		float percent =  CurrentHeath / (float)MaxHeath;
		return Mathf.Max(0, percent);
	}
	private void EmitDied()
	{
		EmitSignal(SignalName.Died);
		Owner.QueueFree();
	}
}
