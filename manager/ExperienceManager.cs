using Godot;
using System;

public partial class ExperienceManager : Node {
	public float CurrentEx { get; private set; } = 0f;
	public override void _Ready() {
		GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnCollectedExperience;
	}
	
	private void OnCollectedExperience(float exNum) {
		IncreasePlayerExperience(exNum);
	}

	private void IncreasePlayerExperience(float exNum) {
		GD.Print("ex collected in ExperienceManager");
		CurrentEx += exNum;
	}
}
