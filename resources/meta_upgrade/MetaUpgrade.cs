using Godot;
using System;

public partial class MetaUpgrade : Resource
{
    [Export] public string Id;
    [Export] public string Name;
    [Export] public int MaxQuantity;
    [Export] public int ExperienceCost = 10;
    [Export(PropertyHint.MultilineText)] public string Description;
}
