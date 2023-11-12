using Godot;
using System;
using Godot.Collections;

public partial class AnvilAbilityController : Node {
	[Export] public float BaseRange = 100f;
	[Export] public PackedScene AnvilAbilityScene;
	private Timer _reloadTimer;
	
	
	public override void _Ready()
	{
		_reloadTimer = GetNode<Timer>("Timer");
		_reloadTimer.Timeout += OnTimerOut;
	}
	private void OnTimerOut() {
		if (GetTree().GetFirstNodeInGroup("Player") is not Player player) {
			return;
		}
		GD.Print("Anvil ");
		var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
		var range = (float) GD.RandRange(0, BaseRange);
		var spawnPosition = player.GlobalPosition + range * randomDirection;
		PhysicsRayQueryParameters2D rayQueryParameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawnPosition, 1<<0);
		Dictionary dictionary = GetTree().Root.World2D.DirectSpaceState.IntersectRay(rayQueryParameters);
		if (dictionary.Count > 0) {
			spawnPosition = dictionary["position"].AsVector2();
		}
		
		var anvilAbility = AnvilAbilityScene.Instantiate<Node2D>();
		var foreGround = GetTree().GetFirstNodeInGroup("ForegroundLayer");
		foreGround.AddChild(anvilAbility);
		anvilAbility.GlobalPosition = spawnPosition;
	}
	
}
