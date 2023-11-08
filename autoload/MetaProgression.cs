using Godot;
using Godot.Collections;

public partial class MetaProgression : Node
{
    public SavedData SavedData { get; private set; } = new();

    public override void _Ready()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnExperienceVialCollected;
        MetaUpgrade metaUpgrade = ResourceLoader.Load<MetaUpgrade>("res://resources/meta_upgrade/experience_gain.tres");
        AddMetaUpgrade(metaUpgrade);
        GD.Print(SavedData.SavedDict["ExperienceGain"].Quantity);
    }

    private void OnExperienceVialCollected(int exNum)
    {
        SavedData.MetaUpgradeCurrency += exNum;
    }

    public void AddMetaUpgrade(MetaUpgrade metaUpgrade)
    {
        var upgradeId = metaUpgrade.Id;
        if (SavedData.SavedDict.TryGetValue(upgradeId, out var dictValue))
        {
            dictValue.Quantity += 1;
        }
        else
        {
            SavedData.SavedDict.Add(upgradeId, new SavedDataDictValue(1));
        }
    }
}

public partial class SavedData : GodotObject
{
    public int MetaUpgradeCurrency { get;  set; }
    public Dictionary<string, SavedDataDictValue> SavedDict { get; private set; } = new();
}

public partial class SavedDataDictValue : GodotObject
{
    public int Quantity { get; set; }

    public SavedDataDictValue(int quantity)
    {
        Quantity = quantity;
    }
}