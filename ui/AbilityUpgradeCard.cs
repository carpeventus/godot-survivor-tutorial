using Godot;
using System;
using Godot.Collections;

public partial class AbilityUpgradeCard : PanelContainer {
    private Label _nameLabel;
    private Label _descLabel;
    private AnimationPlayer _animationPlayer;
    private AnimationPlayer _hoverAnimationPlayer;

    private bool _selected;

    [Signal]
    public delegate void UpgradeCardSelectedEventHandler(); 
    
    public override void _Ready() {
        _nameLabel = GetNode<Label>("%AbilityCardName");
        _descLabel = GetNode<Label>("%AbilityCardDesc");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _hoverAnimationPlayer = GetNode<AnimationPlayer>("HoverAnimationPlayer");
        GuiInput += OnGuiInput;
        MouseEntered += OnMouseEntered;
    }


    public void SetAbilityCard(AbilityUpgrade abilityUpgrade) {
        _nameLabel.Text = abilityUpgrade.Name;
        _descLabel.Text = abilityUpgrade.Description;
    }

    public async void PlayIn(float delay = 0f) {
        Modulate = Colors.Transparent;
        await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
        // 动画控制Modulate还原
        _animationPlayer.Play("in");
    }

    private void OnGuiInput(InputEvent @event) {
        if (_selected) {
            return;
        }
        if (@event.IsActionPressed("left_click") ) {
            EmitSelectedWhenAnimationFished();
        }
    }

    private void PlayDiscard() {
        _animationPlayer.Play("discard");
    }

    private async void EmitSelectedWhenAnimationFished() {
        _selected = true;
        _animationPlayer.Play("selected");
        Array<Node> cards =  GetTree().GetNodesInGroup("UpgradeCard");
        foreach (var card in cards) {
            if (card is AbilityUpgradeCard upgradeCard) {
                if (upgradeCard == this) {
                    continue;
                }
                upgradeCard.PlayDiscard();
            }

        }
        
        await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
        EmitSignal(SignalName.UpgradeCardSelected);
    }
    
    private void OnMouseEntered() {
        _hoverAnimationPlayer.Play("hover");
    }

}
