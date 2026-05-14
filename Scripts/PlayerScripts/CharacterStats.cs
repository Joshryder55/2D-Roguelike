using Godot;
using System;

public partial class CharacterStats : Node
{
	GameManager gameManager;

	public virtual int maxHealth { get; set;} = 10;
	public virtual int health { get; set; } = 10;
	
	public virtual float fireRate { get; set; } = 1.0f;
	public virtual float range { get; set; } = 500.0f;
	//public virtual float projectileSpeed { get; set; } = 300.0f;
	//public virtual int projectileDamage { get; set; } = 10;
	
	public virtual float playerSpeed { get; set; } = 100;
	
	public virtual int ultimateCharge { get; set; } = 50;
	public virtual int GetUltimateChargeRequired() { return int.MaxValue; }

	// Stat multipliers for future leveling up. Will need to be updated for what we actually want - set to 1 for now, until ready to implement the scaling
	public virtual float healthMultiplier { get; set; } = 1.0f;
	public virtual float speedMultiplier { get; set; } = 1.0f;
	public virtual float fireRateMultiplier { get; set; } = 1.0f;
	public virtual float rangeMultiplier { get; set; } = 1.0f;

	public virtual void ApplyLevelUp() {
		maxHealth = Mathf.RoundToInt(maxHealth * healthMultiplier);
		playerSpeed *= speedMultiplier;
		fireRate *= fireRateMultiplier;
		range *= rangeMultiplier;
	}
	
	
	public override void _Ready() {
		gameManager = GetNode<GameManager>("/root/GameManager");
	}
	
	public virtual void TakeDamage(int amount){
		health -= amount;
		//GD.Print("Health: " + health);
		
		if(health <= 0){
			Die();
		}
	}
	
	public virtual void Die(){
		gameManager.isDead = true;
		//GD.Print("You Died!");
		PackedScene gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
		GetTree().CurrentScene.AddChild(gameOverScene.Instantiate());
	}
	
	
}
