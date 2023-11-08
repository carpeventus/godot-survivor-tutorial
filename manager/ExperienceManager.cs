using Godot;
using System;

public partial class ExperienceManager : Node {
	public int CurrentEx { get; private set; } = 0;
	public int CurrentLevel { get; private set; } = 1;
	public int TargetEx { get; private set; } = 1;
	public int TargetExGrowth { get; private set; } = 5;

    [Signal] public delegate void ExperienceUpdatedEventHandler(float current, float target);
    [Signal] public delegate void LevelUpEventHandler(float newLevel);
    
	public override void _Ready() {
		GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnCollectedExperience;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected -= OnCollectedExperience;
	}
	

	private void OnCollectedExperience(int exNum) {
		IncreasePlayerExperience(exNum);
	}

	private void IncreasePlayerExperience(int exNum) {
        CurrentEx = Mathf.Min(CurrentEx + exNum, TargetEx);
        EmitSignal(SignalName.ExperienceUpdated, CurrentEx, TargetEx);
        if (Mathf.IsZeroApprox(TargetEx - CurrentEx)) {
            CurrentLevel += 1;
            TargetEx += TargetExGrowth;
            CurrentEx = 0;
            EmitSignal(SignalName.ExperienceUpdated, CurrentEx, TargetEx);
            EmitSignal(SignalName.LevelUp, CurrentLevel);

        }
    }
}
