using Godot;
using Godot.Collections;

public partial class MetaProgression : Node
{

    public readonly string UserDataFilePath = "user://game.tres";
    public SavedData SavedData { get; private set; } = new();

    public override void _Ready()
    {
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnExperienceVialCollected;
        LoadSavedData();
    }
    
    private void LoadSavedData()
    {
        if (!ResourceLoader.Exists(UserDataFilePath))
        {
            return;
        }

        SavedData = ResourceLoader.Load<SavedData>(UserDataFilePath);
    }

    public void SaveData()
    {
         ResourceSaver.Save(SavedData, UserDataFilePath);
    }
    
    private void OnExperienceVialCollected(int exNum)
    {
        
        SavedData.MetaUpgradeCurrency += exNum;
        SaveData();
    }

    public int GetMetaUpgradeQuantity(string upgradeId) {
        if (SavedData.SavedDict.TryGetValue(upgradeId, out var dictValue)) {
            return dictValue.Quantity;
        }

        return 0;
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
        SaveData();
    }
}

