using Godot;
using System;

public partial class HitBox : Area2D
{
    [Export] public int Damage { get; private set; }
}
