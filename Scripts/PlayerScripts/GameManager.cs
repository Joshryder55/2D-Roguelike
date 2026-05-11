using Godot;
using System;

public partial class GameManager : Node
{
	
	//Checking to see if the player has died
	public bool isDead = false;
	
	public int coins = 0;
	
	//These are the booleans showing if the player has the abilities or passives unlocked
	//Set to true for testing purposes WHEN COINS ARE IMPLEMENTED NEEDS TO BE SET TO FALSE
	
	
	
	public void Reset() {
	isDead = false;
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
