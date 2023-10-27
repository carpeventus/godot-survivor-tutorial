using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class UpgradeManager : Node {
    [Export] public Array<AbilityUpgrade> AbilityUpgradePool;
    [Export] public ExperienceManager ExperienceManager;
    [Export] public PackedScene UpgradeScreenScene;

    public Godot.Collections.Dictionary<string, UpgradeDictValue> Upgrades { get; private set; } = new();
    public Node AbilitysNode { get; private set; }

    public override void _Ready()
    {
        ExperienceManager.LevelUp += OnLevelUp;
    }

    private void OnLevelUp(float newLevel) {
        Array<AbilityUpgrade> upgrades = PickUpThreeDifferentAbilityUpgrades();
        UpgradeScreen upgradeScreen = UpgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreen);
        upgradeScreen.UpgradeSelected += OnUpgradeSelected;
        upgradeScreen.ShowAbilityUpgrades(upgrades);
        // 暂停游戏
    }

    private Array<AbilityUpgrade> PickUpThreeDifferentAbilityUpgrades()
    {
        Array<AbilityUpgrade> result = new Array<AbilityUpgrade>();
        
        Array<AbilityUpgrade> pool = AbilityUpgradePool.Duplicate();

        for (int i = 0; i < 3; i++)
        {
            if (pool.Count == 0)
            {
                return result;
            }
            AbilityUpgrade chosen = pool.PickRandom();
            result.Add(chosen);
            List<AbilityUpgrade> filter = pool
                .Where(ability => !ability.Id.Equals(chosen.Id)).ToList();
            pool = new Array<AbilityUpgrade>(filter);
        }
        return result;
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

        UpgradeDictValue value = Upgrades[abilityUpgrade.Id];
        // 达到上限，从池子中移除
        if (value.Quantity == abilityUpgrade.MaxQuantity)
        {
            List<AbilityUpgrade> filter = AbilityUpgradePool
                .Where(ability => !ability.Id.Equals(value.Id)).ToList();
            AbilityUpgradePool = new Array<AbilityUpgrade>(filter);
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