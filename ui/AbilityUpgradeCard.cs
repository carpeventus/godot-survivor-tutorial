using Godot;
using System;

public partial class AbilityUpgradeCard : PanelContainer {
    private Label _nameLabel;
    private Label _descLabel;

    [Signal]
    public delegate void UpgradeCardSelectedEventHandler(); 
    public override void _Ready() {
        _nameLabel = GetNode<Label>("%AbilityCardName");
        _descLabel = GetNode<Label>("%AbilityCardDesc");
        GuiInput += OnGuiInput;
    }

    public void SetAbilityCard(AbilityUpgrade abilityUpgrade) {
        _nameLabel.Text = abilityUpgrade.Name;
        _descLabel.Text = abilityUpgrade.Description;
    }

    private void OnGuiInput(InputEvent @event) {
        if (@event.IsActionPressed("left_click")) {
            EmitSignal(SignalName.UpgradeCardSelected);
        }
    }
}
