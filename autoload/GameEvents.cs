using Godot;
using System;

public partial class GameEvents : Node {

    [Signal]
    public delegate void ExperienceVialCollectedEventHandler(float exNum);
    public void EmitExperienceVialCollected(float ex) {
        EmitSignal(SignalName.ExperienceVialCollected, ex);
    }
    
}
