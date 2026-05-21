using Godot;
using System;

public partial class GameManager : Node
{
	
	public bool isDead = false;
	public bool ultimateIsActive = false;
	public int coins = 0;

	// XP and leveling
	public int xp = 0;
	public int level = 1;
	public int xpToNextLevel = 100;

	public float gameTime = 0f;

	public override void _Process(double delta) {
		if (!isDead) gameTime += (float)delta;
	}

	// Coin drop 2.5x at game start, back to 1x at 5 minutes.
	public float GetCoinDropMultiplier() {
		return Mathf.Max(1.0f, 2.5f - gameTime / 200f);
	}

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
		gameTime = 0f;
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player != null) {
			CharacterStats stats = player.GetNode<CharacterStats>("Stats");
			stats.health = stats.maxHealth;
		}

	}
	
}
