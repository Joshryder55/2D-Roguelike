using Godot;
using System;

public partial class GameManager : Node
{
	
	//Checking to see if the player has died
	public bool isDead = false;

	public int coins = 1000;

	// XP and leveling
	public int xp = 0;
	public int level = 1;
	public int xpToNextLevel = 100;

	//These are the booleans showing if the player has the abilities or passives unlocked
	//Set to true for testing purposes WHEN COINS ARE IMPLEMENTED NEEDS TO BE SET TO FALSE

	public void AddXP(int amount) {
		xp += amount;
		if (xp >= xpToNextLevel) LevelUp();
	}

	void LevelUp() {
		level++;
		xp -= xpToNextLevel;
		// Each level requires 20% more XP than the previous - Will need to be adjusted and balanced for what we wnat
		xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

		// Scaling applied to player
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player != null) {
			CharacterStats stats = player.GetNode<CharacterStats>("Stats");
			stats.ApplyLevelUp();
		}
	}

	public void Reset() {
	isDead = false;
	xp = 0;
	level = 1;
	xpToNextLevel = 100;
	CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
	if (player != null) {
		CharacterStats stats = player.GetNode<CharacterStats>("Stats");
		stats.health = stats.maxHealth;
	}

	}
	
	
	
	////Ability Costs
	//public void UnlockAbility(string ability, int cost){
		//if(coins < cost) return;
		//coins -= cost;
		//
		//switch(ability) {
			//case "multishot": hasMultiShot = true; break;
			//case "freezeOnHit": hasFreezeOnHit = true; break;
			//case "frostNova": hasFrostNova = true; break;
			//case "iceSpike": hasIceSpike = true; break;
		//}
	//}
	//
	//public void Respec() {
		//// refund all coins spent - wire up costs later
		//hasMultiShot = false;
		//hasFreezeOnHit = false;
		//hasFrostNova = false;
		//hasIceSpike = false;
	//}
	
}
