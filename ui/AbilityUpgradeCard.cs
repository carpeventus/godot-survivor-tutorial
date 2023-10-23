using Godot;
using System;

public partial class AbilityUpgradeCard : PanelContainer {
    private Label _nameLabel;
    private Label _descLabel;

    [Signal]
    public delegate void UpgradeCardSelectedEventHandler(); 
    public override void _Ready() {
        _nameLabel = GetNode<Label>("VBoxContainer/Name");
        _descLabel = GetNode<Label>("VBoxContainer/Desc");
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
