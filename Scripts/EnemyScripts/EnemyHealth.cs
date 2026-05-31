using Godot;
public partial class EnemyHealth : Node
{
	GameManager gameManager;
	public virtual int health { get; set; } = 20;
	public virtual int maxHealth { get; set; } = 20;
	public virtual int xpValue { get; set; } = 5;
	public virtual float coinDropChance { get; set; } = 0.1f;
	public virtual int scoreValue { get; set; } = 1;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");

		// Scale health based on time of run
		float multiplier = gameManager.GetEnemyHealthMultiplier();
		maxHealth = Mathf.RoundToInt(maxHealth * multiplier);
		health    = maxHealth;
	}

	// canShatter = false prevents chain explosions from shatter damage killing nearby enemies
	public virtual void TakeDamage(int amount, bool canShatter = true)
	{
		Enemy enemy = GetParent() as Enemy;
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		IceWizardStats iceStats = player?.GetNode<IceWizardStats>("Stats");

		if (enemy != null && enemy.currentStatus == Enemy.StatusEffect.Frozen && iceStats != null)
		{
			// Brittle — bonus damage to frozen enemies
			if (iceStats.hasBrittle)
				amount = Mathf.RoundToInt(amount * iceStats.brittleBonusDamageMultiplier);

			// Shatter bonus from Flash Freeze
			if (iceStats.flashFreezeShatterBonus > 0f)
				amount = Mathf.RoundToInt(amount * (1f + iceStats.flashFreezeShatterBonus));
		}

		health -= amount;
		GetParent().GetNode<ProgressBar>("ProgressBar").Visible = true;
		if (health <= 0)
			Die(canShatter);
	}

	public virtual void Die(bool canShatter = true)
	{
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		CharacterStats stats = player.GetNode<CharacterStats>("Stats");
		IceWizardStats iceStats = stats as IceWizardStats;

		if (!gameManager.ultimateIsActive)
			stats.ultimateCharge++;

		gameManager.AddScore(scoreValue);

		// Brittle Shatter — explode on death if frozen, only if canShatter is true
		if (canShatter && iceStats != null && iceStats.brittleShatter &&
			GetParent() is Enemy deadEnemy &&
			deadEnemy.currentStatus == Enemy.StatusEffect.Frozen)
		{
			float shatterRadius = 100f;
			Vector2 deathPos = GetParent<Node2D>().GlobalPosition;
			foreach (Node node in GetTree().GetNodesInGroup("enemies"))
			{
				if (node is CharacterBody2D body && IsInstanceValid(body))
				{
					float dist = deathPos.DistanceTo(body.GlobalPosition);
					if (dist <= shatterRadius && dist > 0)
					{
						EnemyHealth nearbyHealth = body.GetNodeOrNull<EnemyHealth>("EnemyHealth");
						// Pass false so shatter damage can't trigger another shatter
						nearbyHealth?.TakeDamage(Mathf.RoundToInt(iceStats.frostNovaDamage * 0.5f), false);
					}
				}
			}
		}

		// Coin drop
		if (GD.Randf() < coinDropChance * gameManager.GetCoinDropMultiplier())
		{
			PackedScene coinScene = GD.Load<PackedScene>("res://Scenes/Coin.tscn");
			CoinPickup coin = coinScene.Instantiate() as CoinPickup;
			coin.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			GetTree().CurrentScene.CallDeferred("add_child", coin);
		}

		// XP vacuum power-up (1.5% chance) — collect all xp orbs on the map
		if (GD.Randf() < 0.015f)
		{
			PackedScene vacuumScene = GD.Load<PackedScene>("res://Scenes/PowerUpXPVacuum.tscn");
			PowerUpXPVacuum vacuum = vacuumScene.Instantiate<PowerUpXPVacuum>();
			vacuum.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			GetTree().CurrentScene.CallDeferred("add_child", vacuum);
		}

		// XP orb
		PackedScene orbScene = GD.Load<PackedScene>("res://Scenes/XPOrb.tscn");
		XPOrb orb = orbScene.Instantiate<XPOrb>();
		orb.xpValue = xpValue;
		orb.GlobalPosition = GetParent<Node2D>().GlobalPosition;
		GetTree().CurrentScene.CallDeferred("add_child", orb);

		GetParent().QueueFree();
	}
}
