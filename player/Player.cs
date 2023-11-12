using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D {
	[Export] public ArenaTimeManager ArenaTimeManager;
	public Vector2 InputMovement { get; private set; }
	public Vector2 Direction { get; private set; }

	public Area2D PlayerHurtArea { get; private set; }

	public int CollisionCount { get; private set; }
	public Timer DamageIntervalTimer { get; private set; }
	public HealthComponent Health { get; private set; }
	public ProgressBar HealthBar { get; private set; }

	public Node AbilitiesNode { get; private set; }
	public AnimationPlayer AnimationPlayer { get; private set; }
	public Node2D SpriteWrap { get; private set; }
	public VelocityComponent VelocityComponent { get; private set; }
	public float BaseSpeed { get; private set; }

	#region MyRegion

	private readonly string AbilityPlayerSpeedId = "PlayerSpeed";
	

	#endregion
	
	public override void _Ready() {
		PlayerHurtArea = GetNode<Area2D>("PlayerHurtArea");
		DamageIntervalTimer = GetNode<Timer>("DamageIntervalTimer");
		Health = GetNode<HealthComponent>("HealthComponent");
		HealthBar = GetNode<ProgressBar>("HealthBar");
		AbilitiesNode = GetNode<Node>("Abilities");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		SpriteWrap = GetNode<Node2D>("SpriteWrap");
		VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		// 敌人是Body
		PlayerHurtArea.BodyEntered += OnEnemyBodyEntered;
		PlayerHurtArea.BodyExited += OnEnemyBodyExited;
		DamageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;
		Health.HealthDecreased += OnHealthDecreased;
		Health.HealthChange += OnHealthChanged;
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
		ArenaTimeManager.ArenaDifficultyUp += OnDifficultyUp;
		
		BaseSpeed = VelocityComponent.MaxSpeed;
		
		UpdateHealthBarDisplay();
	}

	private void OnDifficultyUp(int difficulty) {
		// 5、10、15、20等级会恢复hp，即每25s恢复1点
		var shouldHeal = difficulty % 5 == 0;
		if (!shouldHeal) {
			return;
		}
		var savedData = GetNode<MetaProgression>("/root/MetaProgression").SavedData;
		if (savedData.SavedDict.TryGetValue("HealthRegeneration", out var dictValue)) {
			if (dictValue.Quantity > 0) {
				Health.Heal(dictValue.Quantity);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded -= OnAbilityUpgradeAdded;

	}

	private void OnAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Dictionary<string, UpgradeDictValue> currentUpgrades)
	{
		// 是能力就添加到玩家能力组件中
		if (abilityUpgrade is AbilityResource ability)
		{
			AbilitiesNode.AddChild(ability.AbilityControllerScene.Instantiate());
		}
		// 是玩家属性升级，如移动速度提升等，执行下面逻辑
		else {
			if (abilityUpgrade.Id.Equals(AbilityPlayerSpeedId)) {
				 var dictValue = currentUpgrades[AbilityPlayerSpeedId];
				 VelocityComponent.MaxSpeed = BaseSpeed + (BaseSpeed * dictValue.Quantity * 0.1f);
			}
		}
		
	}
	
	private void OnHealthDecreased()
	{
		GetNode<GameEvents>("/root/GameEvents").EmitPlayerHurt();
		GetNode<RandomAudioStreamPlayer2D>("RandomAudioStreamPlayer2D").PlayerRandom();
	}
	
	

	private void OnHealthChanged() {
		UpdateHealthBarDisplay();
	}

	private void UpdateHealthBarDisplay()
	{
		HealthBar.Value = Health.GetPercent();
	}

	private void OnDamageIntervalTimerTimeout()
	{
		CheckDamage();
	}

	private void OnEnemyBodyEntered(Node2D enemy)
	{
		CollisionCount += 1;
		CheckDamage();
	}
	
	private void OnEnemyBodyExited(Node2D enemy)
	{
		CollisionCount -= 1;
	}

	private void CheckDamage()
	{
		if (!DamageIntervalTimer.IsStopped() || CollisionCount == 0)
		{
			return;	
		}
		// TODO damage
		Health.TakeDamage(1);
		DamageIntervalTimer.Start();
	}

	public override void _PhysicsProcess(double delta) {
		VelocityComponent.AccelerateInDirection(Direction);
		VelocityComponent.Move(this);
	}

	public override void _Process(double delta) {
		HandleInput();
		if (Mathf.IsZeroApprox(Velocity.Length()))
		{
			AnimationPlayer.Play("RESET");
		}
		else
		{
			AnimationPlayer.Play("walk");
		}

		int sign = Mathf.Sign(Velocity.X);
		if (sign != 0)
		{
			SpriteWrap.Scale = new Vector2(sign, 1);
		}
	}

	private void HandleInput() {
		InputMovement = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Direction = InputMovement.Normalized();
	}
}
