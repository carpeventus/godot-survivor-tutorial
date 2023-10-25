using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class EnemyManager : Node {
	[Export] public PackedScene BasicEnemyScene;
	[Export] public ArenaTimeManager ArenaTimeManager;

	private Timer _timer;

	private double _baseSpawnTime;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimeout;
		ArenaTimeManager.ArenaDifficultyUp += OnArenaDifficultyUp;
		_baseSpawnTime = _timer.WaitTime;
	}

	private void OnArenaDifficultyUp(int difficulty)
	{
		var timeOffset = (0.1 / 12) * difficulty;
		_timer.WaitTime = Mathf.Max(_timer.WaitTime - timeOffset, 0.5);
	}

	private void OnTimeout() {
		_timer.Start();
		
		var entities = GetTree().GetFirstNodeInGroup("EntitiesLayer");
		// 视口尺寸的3/2 * 随机方向 + 玩家位置 
		Node2D enemy = BasicEnemyScene.Instantiate<Node2D>();
		entities.AddChild(enemy);
		enemy.Position = GetSpawnPosition();
	}

	private Vector2 GetSpawnPosition()
	{
		if (GetTree().GetFirstNodeInGroup("player") is not Player player)
		{
			return Vector2.Zero;
		}
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		
		Vector2 randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
		Vector2 spawnPosition = Vector2.Zero;
		// 最多旋转4次90度
		for (int i = 0; i < 4; i++)
		{
			spawnPosition = player.GlobalPosition + (2f / 3f) * viewportSize * randomDirection;
			// player 到生成的地方是否有碰撞
			PhysicsRayQueryParameters2D rayQueryParameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawnPosition, 1<<0);
			Dictionary dictionary = GetTree().Root.World2D.DirectSpaceState.IntersectRay(rayQueryParameters);
			if (dictionary.Count == 0)
			{
				break;
			}
			GD.Print("Rotate to find next");
			randomDirection = randomDirection.Rotated(Mathf.DegToRad(90));
		}
		return spawnPosition;
	}
}
