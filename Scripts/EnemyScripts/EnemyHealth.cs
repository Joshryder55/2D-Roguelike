using Godot;
using System;

public partial class EnemyHealth : Node
{
	GameManager gameManager;
	public virtual int health { get; set; } = 20;
	public virtual int maxHealth { get; set; } = 20;
	public virtual int xpValue { get; set; } = 5;
	public virtual float coinDropChance { get; set; } = 0.1f;
	public virtual int scoreValue { get; set; } = 1; // override in subclasses for harder enemies

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
	}

	public virtual void TakeDamage(int amount)
	{
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

		if (gameManager.ultimateIsActive == false)
			stats.ultimateCharge++;

		// Add score on kill
		gameManager.AddScore(scoreValue);

		// Coin drop chance
		if (GD.Randf() < coinDropChance * gameManager.GetCoinDropMultiplier())
		{
			PackedScene coinScene = GD.Load<PackedScene>("res://Scenes/Coin.tscn");
			CoinPickup coin = coinScene.Instantiate() as CoinPickup;
			coin.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			GetTree().CurrentScene.CallDeferred("add_child", coin);
		}

		// Spawn XP orb
		PackedScene orbScene = GD.Load<PackedScene>("res://Scenes/XPOrb.tscn");
		XPOrb orb = orbScene.Instantiate<XPOrb>();
		orb.xpValue = xpValue;
		orb.GlobalPosition = GetParent<Node2D>().GlobalPosition;
		GetTree().CurrentScene.CallDeferred("add_child", orb);

		GetParent().QueueFree();
	}
}
