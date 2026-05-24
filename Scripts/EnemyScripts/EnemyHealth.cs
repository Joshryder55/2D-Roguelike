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
	}

	public virtual void TakeDamage(int amount)
	{
		Enemy enemy = GetParent() as Enemy;
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		IceWizardStats iceStats = player?.GetNode<IceWizardStats>("Stats");

		if (enemy != null && enemy.currentStatus == Enemy.StatusEffect.Frozen && iceStats != null)
		{
			// Brittle — bonus damage to frozen enemies always
			if (iceStats.hasBrittle)
				amount = Mathf.RoundToInt(amount * 1.5f);

			// Shatter — extra bonus during flash freeze
			if (iceStats.flashFreezeShatterBonus > 0f)
				amount = Mathf.RoundToInt(amount * (1f + iceStats.flashFreezeShatterBonus));
		}

		health -= amount;
		GetParent().GetNode<ProgressBar>("ProgressBar").Visible = true;
		if (health <= 0)
			Die();
	}

	public virtual void Die()
	{
		GD.Print("ultimateIsActive: " + gameManager.ultimateIsActive);
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		CharacterStats stats = player.GetNode<CharacterStats>("Stats");

		if (!gameManager.ultimateIsActive)
			stats.ultimateCharge++;

		gameManager.AddScore(scoreValue);

		if (GD.Randf() < coinDropChance * gameManager.GetCoinDropMultiplier())
		{
			PackedScene coinScene = GD.Load<PackedScene>("res://Scenes/Coin.tscn");
			CoinPickup coin = coinScene.Instantiate() as CoinPickup;
			coin.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			GetTree().CurrentScene.CallDeferred("add_child", coin);
		}

		PackedScene orbScene = GD.Load<PackedScene>("res://Scenes/XPOrb.tscn");
		XPOrb orb = orbScene.Instantiate<XPOrb>();
		orb.xpValue = xpValue;
		orb.GlobalPosition = GetParent<Node2D>().GlobalPosition;
		GetTree().CurrentScene.CallDeferred("add_child", orb);

		GetParent().QueueFree();
	}
}
