using Godot;
using System;

public partial class MetaUpgrade : Resource
{
    [Export] public string Id;
    [Export] public int MaxQuantity;
    [Export] public string Title;
    [Export(PropertyHint.MultilineText)] public string Description;
}
