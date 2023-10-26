
using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class EnemyWeightTable : GodotObject
{
    private int _totalWeight;
    public Array<EnemyWeight> Items { get; private set; } = new();

    public void AddItem(EnemyWeight enemyWeight)
    {
        Items.Add(enemyWeight);
        _totalWeight += enemyWeight.Weight;
    }

    public PackedScene PickItem()
    {
        var chosenWeight = GD.RandRange(1, _totalWeight);
        var list = new List<EnemyWeight>(Items);
        GD.Print(list.Count);
        list.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        var itemsWeight = 0;
        foreach (var item in list)
        {
            itemsWeight += item.Weight;
            if (itemsWeight >= chosenWeight)
            {
                GD.Print(chosenWeight + " " + itemsWeight);
                return item.Item;
            }
        }
        return null;
    }
}

public partial class EnemyWeight : GodotObject
{
    public PackedScene Item { get; private set; }
    public int  Weight { get; private set; }

    public EnemyWeight(PackedScene item, int weight)
    {
        Item = item;
        Weight = weight;
    }
}
