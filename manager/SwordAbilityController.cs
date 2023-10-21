using System;
using System.Collections.Generic;
using Godot;
using System.Linq;
using Godot.Collections;

public partial class SwordAbilityController : Node {
	[Export] public PackedScene SwordScene;
	[Export] public float MaxRange = 150f;
	
	public override void _Ready() {
		GetNode<Timer>("Timer").Timeout += OnTimerOut;
	}

	private void OnTimerOut() {
		if (GetTree().GetFirstNodeInGroup("player") is Player player) {
			Array<Node> enemies = GetTree().GetNodesInGroup("basic_enemy");
			// 在攻击范围内按照距离排序
			List<Node2D> nearEnemy = enemies
				.Where(e => e is Node2D node2D && InAttackRange(node2D, player))
				.Select(node => node as Node2D)
				.OrderBy(node => DistanceToPlayer(node, player)).ToList();
			if (nearEnemy.Count == 0) {
				return;
			}
			SwordAbility sword = SwordScene.Instantiate<SwordAbility>();
			
			// sword的位置为距离player最近的敌人的位置 + 一点点随机位置
			sword.GlobalPosition = nearEnemy[0].GlobalPosition + Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
			// 将剑旋转为指向敌人的方向     
			sword.Rotation = (nearEnemy[0].GlobalPosition - sword.GlobalPosition).Angle();
			player.Owner.AddChild(sword);
		}
	}

	private bool InAttackRange(Node2D enemy, Player player) {
		return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) <= Mathf.Pow(MaxRange, 2);
	}

	private float DistanceToPlayer(Node2D enemy, Player player) {
		return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
	}
}
