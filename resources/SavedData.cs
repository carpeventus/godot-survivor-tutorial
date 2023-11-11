using Godot;
using Godot.Collections;

public partial class SavedData : Resource
{
    [Export] public int MetaUpgradeCurrency { get; set; }
    [Export] public Dictionary<string, SavedDataDictValue> SavedDict { get; private set; } = new();
    
}


