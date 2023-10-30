
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class WeightTable<T> : GodotObject where T : GodotObject
{
    private int _totalWeight;
    public List<ItemWeight<T>> Items { get; private set; } = new();

    public void AddItem(ItemWeight<T> itemWeight)
    {
        Items.Add(itemWeight);
        _totalWeight += itemWeight.Weight;
    }

    public T PickItem(HashSet<string> chosenIds = null)
    {
        var adjustList = new List<ItemWeight<T>>(Items);
        var adjustWeight = _totalWeight;
        
        if (chosenIds is not null && chosenIds.Count > 0)
        {
            adjustList = adjustList.Where(a => !chosenIds.Contains(a.Id)).ToList();
            adjustWeight = adjustList.Sum(a => a.Weight);
        }
        
        var chosenWeight = GD.RandRange(1, adjustWeight);
        
        adjustList.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        var itemsWeight = 0;
        foreach (var item in adjustList)
        {
            itemsWeight += item.Weight;
            if (itemsWeight >= chosenWeight)
            {
                return item.Item;
            }
        }
        return null;
    }

    public void RemoveItem(string id)
    {
        Items = Items.Where(item => !item.Id.Equals(id)).ToList();
        _totalWeight = Items.Sum(a => a.Weight);
    }
}

public partial class ItemWeight<T> :GodotObject where T : GodotObject
{
    public String Id { get; private set; }
    public T Item { get; private set; }
    public int Weight { get; private set; }

    public ItemWeight(String id, T item, int weight)
    {
        Id = id;
        Item = item;
        Weight = weight;
    }
}
