using Godot;
using Godot.Collections;

public partial class EnemyManager : Node {
	#region EnemyScene
	[Export] public PackedScene BasicEnemyScene;
	[Export] public PackedScene WizardEnemyScene;
	[Export] public PackedScene BatEnemyScene;

	#endregion

	[Export] public ArenaTimeManager ArenaTimeManager;

	private Timer _timer;

	private double _baseSpawnTime;

	public WeightTable<PackedScene> WeightTable { get; private set; } = new ();
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimeout;
		ArenaTimeManager.ArenaDifficultyUp += OnArenaDifficultyUp;
		_baseSpawnTime = _timer.WaitTime;
		// Add First Enemy
		WeightTable.AddItem(new ItemWeight<PackedScene>("BasicEnemyScene", BasicEnemyScene, 10));
	}

	private void OnArenaDifficultyUp(int difficulty)
	{
		var timeOffset = (0.1 / 12) * difficulty;
		_timer.WaitTime = Mathf.Max(_timer.WaitTime - timeOffset, 0.5);
		// 6级产生巫师
		if (difficulty == 6)
		{
			WeightTable.AddItem(new ItemWeight<PackedScene>("WizardEnemyScene", WizardEnemyScene, 20));
		} else if (difficulty == 12) {
			WeightTable.AddItem(new ItemWeight<PackedScene>("BatEnemyScene", BatEnemyScene, 10));
		}
		
	}

	private void OnTimeout() {
		_timer.Start();
		
		var entities = GetTree().GetFirstNodeInGroup("EntitiesLayer");
		// 视口尺寸的3/2 * 随机方向 + 玩家位置 
		PackedScene enemyScene = WeightTable.PickItem();
		Node2D enemy = enemyScene?.Instantiate<Node2D>();
		if (enemy is not null)
		{
			entities.AddChild(enemy);
			enemy.Position = GetSpawnPosition();
		}
	}

	private Vector2 GetSpawnPosition()
	{
		if (GetTree().GetFirstNodeInGroup("Player") is not Player player)
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
			// 防止敌人卡在墙里，如果末端刚好在墙边缘，就会认为没有障碍，但是碰撞体有一定半径，会出现刚产生就卡墙
			// 增长射线，如果射线末端刚好在墙边缘，也能保证是在墙里生存的（返回的还是spawnPosition，未增加32p像素）
			var additionalOffset = randomDirection * 32f;
			// player 到生成的地方是否有碰撞
			PhysicsRayQueryParameters2D rayQueryParameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawnPosition + additionalOffset, 1<<0);
			Dictionary dictionary = GetTree().Root.World2D.DirectSpaceState.IntersectRay(rayQueryParameters);
			if (dictionary.Count == 0)
			{
				break;
			}
			randomDirection = randomDirection.Rotated(Mathf.DegToRad(90));
		}
		return spawnPosition;
	}
}
