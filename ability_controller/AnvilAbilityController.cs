using Godot;
using System;
using Godot.Collections;

public partial class AnvilAbilityController : Node {
	[Export] public float BaseRange = 64f;
	[Export] public PackedScene AnvilAbilityScene;
	private Timer _reloadTimer;

	public string AnvilCountAbilityId { get; private set; }= "AnvilCount";

	public int AdditionalAnvilCount = 0;
	
	public override void _Ready()
	{
		_reloadTimer = GetNode<Timer>("Timer");
		_reloadTimer.Timeout += OnTimerOut;
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;

	}
	
	
	public override void _ExitTree()
	{
		base._ExitTree();
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded -= OnAbilityUpgradeAdded;

	}

	private void OnAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Godot.Collections.Dictionary<string, UpgradeDictValue> currentUpgrades)
	{
		// 升级技能很多地方都在监听，这里需要过滤和自己有关的
		if (abilityUpgrade.Id.Equals(AnvilCountAbilityId))
		{
			UpgradeDictValue dictValue = currentUpgrades[AnvilCountAbilityId];
			AdditionalAnvilCount = dictValue.Quantity;
			// 需要重新启动下
			// _reloadTimer.Start();
		} 
	}

	private void OnTimerOut() {
		if (GetTree().GetFirstNodeInGroup("Player") is not Player player) {
			return;
		}
		var range = (float) GD.RandRange(16, BaseRange);
		var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
		// 两个就是180度，三个是120度
		var additionalRotation = 360.0 / (AdditionalAnvilCount + 1);
		for (int i = 0; i < AdditionalAnvilCount + 1; i++) {
			var adjustDirection = randomDirection.Rotated((float)Mathf.DegToRad(additionalRotation * i));
			GenerateOne(player, adjustDirection, range);
		}
		
	}

	private void GenerateOne(Player player, Vector2 randomDirection, float range) {
		var spawnPosition = player.GlobalPosition + range * randomDirection;
		var rayQueryParameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawnPosition, 1<<0);
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
