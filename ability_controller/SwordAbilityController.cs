using System;
using System.Collections.Generic;
using Godot;
using System.Linq;
using Godot.Collections;

public partial class SwordAbilityController : Node
{
	[Export] public string SwordRateId = "SwordRate";
	[Export] public string SwordDamageId = "SwordDamage";
	
	[Export(PropertyHint.Range, "0.0,1.0,0.01")] public float IncreaseRate = 0.1f;
	
	[Export(PropertyHint.Range, "0.0,1.0,0.01")] public float MinAttackRate = 0.2f;

	[Export] public PackedScene SwordScene;
	[Export] public float MaxRange = 150f;

	private float _damagePercent = 1f;
	private Timer _reloadTimer;
	private double _defaultReloadTime;
	
	public override void _Ready() {
		_reloadTimer = GetNode<Timer>("Timer");
		_reloadTimer.Timeout += OnTimerOut;
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
		_defaultReloadTime = _reloadTimer.WaitTime;
	}

	private void OnAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Godot.Collections.Dictionary<string, UpgradeDictValue> currentUpgrades)
	{
		// 升级技能很多地方都在监听，这里需要过滤和自己有关的
		if (abilityUpgrade.Id.Equals(SwordRateId))
		{
			UpgradeDictValue dictValue = currentUpgrades[SwordRateId];
			float increaseRate = dictValue.Quantity * IncreaseRate;
			_reloadTimer.WaitTime = Mathf.Max(MinAttackRate, _defaultReloadTime * (1 - increaseRate));
			// 需要重新启动下
			_reloadTimer.Start();
		} else if (abilityUpgrade.Id.Equals(SwordDamageId))
		{
			if (currentUpgrades.TryGetValue(SwordDamageId, out UpgradeDictValue upgradeDictValue))
			{
				_damagePercent = 1 + upgradeDictValue.Quantity * 0.15f;
			}
		}
	}


	private void OnTimerOut() {
		if (GetTree().GetFirstNodeInGroup("Player") is Player player) {
			Array<Node> enemies = GetTree().GetNodesInGroup("Enemy");
			// 在攻击范围内按照距离排序
			List<Node2D> nearEnemy = enemies
				.Where(e => e is Node2D node2D && InAttackRange(node2D, player))
				.Select(node => node as Node2D)
				.OrderBy(node => DistanceToPlayer(node, player)).ToList();
			if (nearEnemy.Count == 0) {
				return;
			}
			var foreGround = GetTree().GetFirstNodeInGroup("ForegroundLayer");
			SwordAbility sword = SwordScene.Instantiate<SwordAbility>();
			foreGround.AddChild(sword);
			sword.HitBox.Damage = Mathf.RoundToInt(sword.HitBox.Damage * _damagePercent);
			
			// sword的位置为距离player最近的敌人的位置 + 一点点随机位置
			sword.GlobalPosition = nearEnemy[0].GlobalPosition + Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
			// 将剑旋转为指向敌人的方向     
			sword.Rotation = (nearEnemy[0].GlobalPosition - sword.GlobalPosition).Angle();
			
		}
	}

	private bool InAttackRange(Node2D enemy, Player player) {
		return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) <= Mathf.Pow(MaxRange, 2);
	}

	private float DistanceToPlayer(Node2D enemy, Player player) {
		return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
	}
}
