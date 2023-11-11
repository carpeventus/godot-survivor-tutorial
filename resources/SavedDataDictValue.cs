using Godot;

public partial class SavedDataDictValue : Resource
{
    [Export] public int Quantity { get; set; }

    public SavedDataDictValue(int quantity)
    {
        Quantity = quantity;
    }

    public SavedDataDictValue() {
    }
}