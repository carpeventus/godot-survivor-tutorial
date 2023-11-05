using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class UpgradeManager : Node {
    
    [Export] public ExperienceManager ExperienceManager;
    [Export] public PackedScene UpgradeScreenScene;

    public WeightTable<AbilityUpgrade> AbilityUpgradePool { get; private set; } = new();

    #region MyRegion

    public AbilityUpgrade AxeAbilityUpgrade { get; private set; } 
    public AbilityUpgrade SwordRateUpgrade { get; private set; } 
    public AbilityUpgrade SwordDamageUpgrade { get; private set; } 
    public AbilityUpgrade AxeDamageUpgrade { get; private set; } 
    public AbilityUpgrade PlayerSpeedUpgrade { get; private set; } 

    #endregion


    public Godot.Collections.Dictionary<string, UpgradeDictValue> Upgrades { get; private set; } = new();

    public override void _Ready()
    {
        AxeAbilityUpgrade = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrade/axe.tres");
        SwordRateUpgrade = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrade/swordrate.tres");
        SwordDamageUpgrade = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrade/sword_damage.tres");
        AxeDamageUpgrade = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrade/axe_damage.tres");
        PlayerSpeedUpgrade = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrade/player_speed.tres");
        
        AbilityUpgradePool.AddItem(new ItemWeight<AbilityUpgrade>(AxeAbilityUpgrade.Id, AxeAbilityUpgrade, 10));
        AbilityUpgradePool.AddItem(new ItemWeight<AbilityUpgrade>(SwordRateUpgrade.Id, SwordRateUpgrade, 10));
        AbilityUpgradePool.AddItem(new ItemWeight<AbilityUpgrade>(SwordDamageUpgrade.Id, SwordDamageUpgrade, 10));
        AbilityUpgradePool.AddItem(new ItemWeight<AbilityUpgrade>(PlayerSpeedUpgrade.Id, PlayerSpeedUpgrade, 5));
        ExperienceManager.LevelUp += OnLevelUp;
    }

    private void OnLevelUp(float newLevel) {
        Array<AbilityUpgrade> upgrades = PickUpDifferentAbilityUpgrades();
        UpgradeScreen upgradeScreen = UpgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreen);
        upgradeScreen.UpgradeSelected += OnUpgradeSelected;
        upgradeScreen.ShowAbilityUpgrades(upgrades);
    }

    private Array<AbilityUpgrade> PickUpDifferentAbilityUpgrades()
    {
        Array<AbilityUpgrade> chosenResult = new Array<AbilityUpgrade>();
        for (int i = 0; i < 3; i++)
        {
            // 池子里的被选完了
            if (AbilityUpgradePool.Items.Count == chosenResult.Count)
            {
                return chosenResult;
            }

            var chosenIds = chosenResult.Select(a => a.Id).ToHashSet();
            // 在一次选择中，不重复出现相同的技能
            AbilityUpgrade chosen = AbilityUpgradePool.PickItem(chosenIds);
            if (chosen is not null)
            {
                chosenResult.Add(chosen);
            }
        }
        return chosenResult;
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
            AbilityUpgradePool.RemoveItem(abilityUpgrade.Id);
        }

        UpdateUpgradeAbility(abilityUpgrade);
        GetNode<GameEvents>("/root/GameEvents").EmitAbilityUpgradeAdded(abilityUpgrade, Upgrades);
    }

    private void UpdateUpgradeAbility(AbilityUpgrade chosen)
    {
        // 只有当拥有斧头后才能升级斧头相关的能力
        if (chosen.Id.Equals(AxeAbilityUpgrade.Id))
        {
            AbilityUpgradePool.AddItem(new ItemWeight<AbilityUpgrade>(AxeDamageUpgrade.Id, AxeDamageUpgrade, 10));
        }
    }
}

public partial class UpgradeDictValue: GodotObject {
    public string Id { get; set; }
    public int Quantity { get; set; }
    public AbilityUpgrade AbilityUpgradeResource { get; set; }

    public UpgradeDictValue(string id, int quantity, AbilityUpgrade abilityUpgradeResource) {
        Id = id;
        Quantity = quantity;
        AbilityUpgradeResource = abilityUpgradeResource;
    }
}