using Godot;
using System;
using Godot.Collections;

public partial class UpgradeManager : Node {
    [Export] public Array<AbilityUpgrade> AbilityUpgradePool;
    [Export] public ExperienceManager ExperienceManager;
    [Export] public PackedScene UpgradeScreenScene;

    public Dictionary<string, UpgradeDictValue> Upgrades { get; private set; } = new();

    public override void _Ready()
    {
        ExperienceManager.LevelUp += OnLevelUp;
    }

    private void OnLevelUp(float newLevel) {
        var chosen = AbilityUpgradePool.PickRandom();
        if (chosen is null) {
            return;
        }

        Array<AbilityUpgrade> upgrades = new Array<AbilityUpgrade> { chosen };
        UpgradeScreen upgradeScreen = UpgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreen);
        upgradeScreen.UpgradeSelected += OnUpgradeSelected;
        upgradeScreen.ShowAbilityUpgrades(upgrades);
        // 暂停游戏
    }

    private void OnUpgradeSelected(AbilityUpgrade abilityUpgrade) {
        ApplyUpgrade(abilityUpgrade);
    }

    private void ApplyUpgrade(AbilityUpgrade abilityUpgrade) {
        if (Upgrades.TryGetValue(abilityUpgrade.Id, out var upgrade)) {
            upgrade.Quantity += 1;
        }
        else {
            Upgrades[abilityUpgrade.Id] = new UpgradeDictValue(abilityUpgrade.Id, 1, abilityUpgrade);
        }
        GetNode<GameEvents>("/root/GameEvents").EmitAbilityUpgradeAdded(abilityUpgrade, Upgrades);
    }
    
}

public partial class UpgradeDictValue: GodotObject {
    public string Id { get; set; }
    public int Quantity { get; set; }
    public AbilityUpgrade Resource { get; set; }

    public UpgradeDictValue(string id, int quantity, AbilityUpgrade resource) {
        Id = id;
        Quantity = quantity;
        Resource = resource;
    }
}