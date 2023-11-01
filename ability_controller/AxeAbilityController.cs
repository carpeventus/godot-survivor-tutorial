using Godot;
using System;

public partial class AxeAbilityController : Node
{

    [Export] public PackedScene AxeAbilityScene;
    private Timer _timer;
    private float _damagePercent = 1f;

    [Export] public string AxeDamageUpgradeId = "AxeDamage";
    
    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;

    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded -= OnAbilityUpgradeAdded;

    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Godot.Collections.Dictionary<string, UpgradeDictValue> currentUpgrades)
    {
        // 升级技能很多地方都在监听，这里需要过滤和自己有关的
        if (abilityUpgrade.Id.Equals(AxeDamageUpgradeId))
        {
            if (currentUpgrades.TryGetValue(AxeDamageUpgradeId, out UpgradeDictValue upgradeDictValue))
            {
                _damagePercent = 1 + upgradeDictValue.Quantity * 0.1f;
            }
        }
    }

    private void OnTimerTimeout() {
        if (GetTree().GetFirstNodeInGroup("Player") is not Player player) {
            return;
        }
        
        AxeAbility axeAbility = AxeAbilityScene.Instantiate<AxeAbility>();
        var foreGround = GetTree().GetFirstNodeInGroup("ForegroundLayer");
        foreGround.AddChild(axeAbility);
        axeAbility.HitBox.Damage = Mathf.RoundToInt(axeAbility.HitBox.Damage * _damagePercent);
        axeAbility.GlobalPosition = player.GlobalPosition;

    }
}
