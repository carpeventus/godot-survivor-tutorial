using Godot;
using System;

public partial class AxeAbilityController : Node {
    [Export] public PackedScene AxeAbilityScene;


    private Timer _timer;
    
    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout() {
        if (GetTree().GetFirstNodeInGroup("player") is not Player player) {
            return;
        }
        
        AxeAbility axeAbility = AxeAbilityScene.Instantiate<AxeAbility>();
        var foreGround = GetTree().GetFirstNodeInGroup("ForegroundLayer");
        foreGround.AddChild(axeAbility);
        axeAbility.GlobalPosition = player.GlobalPosition;

    }
}
