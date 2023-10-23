using Godot;
using System;
using Godot.Collections;

public partial class UpgradeScreen : CanvasLayer {
    [Export] public PackedScene UpgradeCardScene;
    private HBoxContainer _cardContainer;
    [Signal]
    public delegate void UpgradeSelectedEventHandler(AbilityUpgrade abilityUpgrade); 
    public override void _Ready() {
        _cardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
        GetTree().Paused = true;
    }

    public void ShowAbilityUpgrades(Array<AbilityUpgrade> upgrades) {
        foreach (var upgrade in upgrades) {
            AbilityUpgradeCard abilityUpgradeCard = UpgradeCardScene.Instantiate<AbilityUpgradeCard>();
            _cardContainer.AddChild(abilityUpgradeCard);
            abilityUpgradeCard.SetAbilityCard(upgrade);
            // 鼠标点击后
            abilityUpgradeCard.UpgradeCardSelected += () => {
                OnUpgradeCardSelected(upgrade);
            };
        }
    }


    private void OnUpgradeCardSelected(AbilityUpgrade abilityUpgrade) {
        EmitSignal(SignalName.UpgradeSelected, abilityUpgrade);
        GetTree().Paused = false;
        QueueFree();
    }
}
