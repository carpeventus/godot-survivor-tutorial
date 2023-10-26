using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	[Export] public float MoveSpeed = 60.0f;
	[Export] public float Acceleration = 20.0f;
	
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
	
	public override void _Ready()
	{
		PlayerHurtArea = GetNode<Area2D>("PlayerHurtArea");
		DamageIntervalTimer = GetNode<Timer>("DamageIntervalTimer");
		Health = GetNode<HealthComponent>("HealthComponent");
		HealthBar = GetNode<ProgressBar>("HealthBar");
		AbilitiesNode = GetNode<Node>("Abilities");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		SpriteWrap = GetNode<Node2D>("SpriteWrap");
		// 敌人是Body
		PlayerHurtArea.BodyEntered += OnEnemyBodyEntered;
		PlayerHurtArea.BodyExited += OnEnemyBodyExited;
		DamageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;
		Health.HealthChange += OnHealthChanged;
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
		UpdateHealthBarDisplay();
	}

	private void OnAbilityUpgradeAdded(AbilityUpgrade abilityUpgrade, Dictionary<string, UpgradeDictValue> currentUpgrades)
	{
		
		if (abilityUpgrade is not AbilityResource ability)
		{
			return;
		}
		
		AbilitiesNode.AddChild(ability.AbilityControllerScene.Instantiate());
	}
	
	private void OnHealthChanged()
	{
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
		var targetVelocity = Direction * MoveSpeed;
		Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-Acceleration * delta * 20)));
		MoveAndSlide();
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
