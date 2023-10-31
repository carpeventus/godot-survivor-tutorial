using Godot;

public partial class SwordAbility : Node2D
{
    
    public HitBox HitBox { get; private set; }
    public override void _Ready()
    {
        HitBox = GetNode<HitBox>("HitBox");
    }
}
