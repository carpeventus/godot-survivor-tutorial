using Godot;
using System;

public partial class EnemyManager : Node {
	[Export] public PackedScene BasicEnemyScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GetNode<Timer>("Timer").Timeout += OnTimeout;
	}

	private void OnTimeout() {
		if (GetTree().GetFirstNodeInGroup("player") is not Player player) {
			return;
		}

		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		Vector2 randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
		// 视口尺寸的3/2 * 随机方向 + 玩家位置 
		Vector2 spawnPosition = player.GlobalPosition + (2f / 3f) * viewportSize  * randomDirection;
		Node2D enemy = BasicEnemyScene.Instantiate<Node2D>();
		enemy.Position = spawnPosition;
		var entities = GetTree().GetFirstNodeInGroup("EntitiesLayer");
		entities.AddChild(enemy);
	}
}
