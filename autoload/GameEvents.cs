using Godot;
using System;
using Godot.Collections;

public partial class GameEvents : Node {

    [Signal]
    public delegate void ExperienceVialCollectedEventHandler(int exNum);
    
    [Signal]
    public delegate void AbilityUpgradeAddedEventHandler(AbilityUpgrade abilityUpgrade, Dictionary<string, UpgradeDictValue> currentUpgrades);

    [Signal]
    public delegate void PlayerTakeDamageEventHandler();
    
    public void EmitExperienceVialCollected(int ex) {
        EmitSignal(SignalName.ExperienceVialCollected, ex);
    }
    
    public void EmitPlayerHurt() {
        EmitSignal(SignalName.PlayerTakeDamage);
    }
    
    public void EmitAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Dictionary<string, UpgradeDictValue> currentUpgrades) {
        EmitSignal(SignalName.AbilityUpgradeAdded, abilityUpgrade, currentUpgrades);
    }
    
}
