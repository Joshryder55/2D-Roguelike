using Godot;
using System;

public partial class GameManager : Node
{
	
	
	public int maxHealth = 10;
	public int health = 10;
	
	//Checking to see if the player has died
	public bool isDead = false;
	
	public int coins = 0;
	
	//These are the booleans showing if the player has the abilities or passives unlocked
	//Set to true for testing purposes WHEN COINS ARE IMPLEMENTED NEEDS TO BE SET TO FALSE
	public bool hasMultiShot = true;
	public bool hasFreezeOnHit = true;
	public bool hasFrostNova = true;
	public bool hasIceSpike = true;
	
	
	public void Reset() {
	isDead = false;
	health = maxHealth;
	
	}
	
	public void TakeDamage(int amount){
		health -= amount;
		GD.Print("Health: " + health);
		
		if(health <= 0){
			Die();
		}
	}
	
	public void Die(){
		isDead = true;
		GD.Print("You Died!");
		PackedScene gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
		GetTree().CurrentScene.AddChild(gameOverScene.Instantiate());
	}
	
	//Ability Costs
	public void UnlockAbility(string ability, int cost){
		if(coins < cost) return;
		coins -= cost;
		
		switch(ability) {
			case "multishot": hasMultiShot = true; break;
			case "freezeOnHit": hasFreezeOnHit = true; break;
			case "frostNova": hasFrostNova = true; break;
			case "iceSpike": hasIceSpike = true; break;
		}
	}
	
	public void Respec() {
		// refund all coins spent - wire up costs later
		hasMultiShot = false;
		hasFreezeOnHit = false;
		hasFrostNova = false;
		hasIceSpike = false;
	}
	
}
