using Godot;
using System;

public partial class ExperienceManager : Node {
	public float CurrentEx { get; private set; } = 0f;
	public int CurrentLevel { get; private set; } = 1;
	public float TargetEx { get; private set; } = 1f;
	public float TargetExGrowth { get; private set; } = 5f;

    [Signal] public delegate void ExperienceUpdatedEventHandler(float current, float target);
    [Signal] public delegate void LevelUpEventHandler(float newLevel);
    
	public override void _Ready() {
		GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnCollectedExperience;
	}
	
	private void OnCollectedExperience(float exNum) {
		IncreasePlayerExperience(exNum);
	}

	private void IncreasePlayerExperience(float exNum) {
        CurrentEx = Mathf.Min(CurrentEx + exNum, TargetEx);
        EmitSignal(SignalName.ExperienceUpdated, CurrentEx, TargetEx);
        if (Mathf.IsZeroApprox(TargetEx - CurrentEx)) {
            CurrentLevel += 1;
            TargetEx += TargetExGrowth;
            CurrentEx = 0f;
            EmitSignal(SignalName.ExperienceUpdated, CurrentEx, TargetEx);
            EmitSignal(SignalName.LevelUp, CurrentLevel);

        }
    }
}
