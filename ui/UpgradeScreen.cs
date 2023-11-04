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
        float delay = 0f;
        foreach (var upgrade in upgrades) {
            AbilityUpgradeCard abilityUpgradeCard = UpgradeCardScene.Instantiate<AbilityUpgradeCard>();
            _cardContainer.AddChild(abilityUpgradeCard);
            abilityUpgradeCard.SetAbilityCard(upgrade);
            abilityUpgradeCard.PlayIn(delay);
            // 鼠标点击后
            abilityUpgradeCard.UpgradeCardSelected += () => {
                OnUpgradeCardSelected(upgrade);
            };
            delay += 0.05f;
        }
    }


    private async void OnUpgradeCardSelected(AbilityUpgrade abilityUpgrade) {
        EmitSignal(SignalName.UpgradeSelected, abilityUpgrade);
        GetTree().Paused = false;
        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("out");
        await ToSignal(animationPlayer, AnimationMixer.SignalName.AnimationFinished);
        QueueFree();
    }
}
