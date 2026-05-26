using Godot;

public partial class CharacterStats : Node
{
	GameManager gameManager;

	public virtual int maxHealth { get; set; } = 100;
	public virtual int health { get; set; } = 100;
	public virtual float fireRate { get; set; } = 1.0f;
	public virtual float range { get; set; } = 500.0f;
	public virtual float playerSpeed { get; set; } = 100;
	public virtual int damageBonus { get; set; } = 0;
	public virtual int ultimateCharge { get; set; } = 100;
	
	public virtual int GetUltimateChargeRequired() { return int.MaxValue; }

	public virtual float healthMultiplier { get; set; } = 1.1f;
	public virtual float speedMultiplier { get; set; } = 1.0f;
	public virtual float fireRateMultiplier { get; set; } = 0.95f;
	public virtual float rangeMultiplier { get; set; } = 1.0f;

	public virtual void ApplyLevelUp()
	{
		maxHealth   = Mathf.RoundToInt(maxHealth * healthMultiplier);
		health      = Mathf.Min(health + Mathf.RoundToInt(maxHealth * 0.1f), maxHealth);
		playerSpeed *= speedMultiplier;
		fireRate    *= fireRateMultiplier;
		range       *= rangeMultiplier;
	}

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
	}

	public virtual void TakeDamage(int amount)
	{
		// Ice Shield — chance to block incoming damage
		IceWizardStats iceStats = this as IceWizardStats;
		if (iceStats != null && iceStats.hasIceShield)
		{
			if (GD.Randf() < iceStats.iceShieldBlockChance)
			{
				GD.Print("Ice Shield blocked!");

				// Retaliate — deal damage back to attacker
				// Note: attacker reference not available here so we deal AOE to nearest enemy
				if (iceStats.iceShieldRetaliate)
				{
					CharacterBody2D player = GetParent() as CharacterBody2D;
					if (player != null)
					{
						foreach (Node node in GetTree().GetNodesInGroup("enemies"))
						{
							if (node is CharacterBody2D body && IsInstanceValid(body))
							{
								float dist = player.GlobalPosition.DistanceTo(body.GlobalPosition);
								if (dist <= 80f) // only hit very close enemies
								{
									EnemyHealth eh = body.GetNodeOrNull<EnemyHealth>("EnemyHealth");
									eh?.TakeDamage(iceStats.iceShieldRetaliationDamage);
								}
							}
						}
					}
				}
				return; // block the hit
			}
		}

		health -= amount;
		if (health <= 0)
			Die();
	}

	public virtual void Die()
	{
		gameManager.isDead = true;
		GD.Print("Coins Collected: " + gameManager.coins);
		PackedScene gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
		GetTree().CurrentScene.AddChild(gameOverScene.Instantiate());
	}
}
