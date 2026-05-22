using Godot;

public partial class GameManager : Node
{
	public bool isDead = false;
	public bool ultimateIsActive = false;
	public int coins = 0;
	public int score = 0;  // incremented on enemy kill

	// XP and leveling
	public int xp = 0;
	public int level = 1;
	public int xpToNextLevel = 100;

	public float gameTime = 0f;

	public override void _Process(double delta)
	{
		if (!isDead && !GetTree().Paused)
			gameTime += (float)delta;
	}

	public float GetCoinDropMultiplier()
	{
		return Mathf.Max(1.0f, 2.5f - gameTime / 200f);
	}

	public void AddScore(int amount)
	{
		score += amount;
	}

	public void AddXP(int amount)
	{
		xp += amount;
		if (xp >= xpToNextLevel) LevelUp();
	}

	void LevelUp()
	{
		level++;
		xp -= xpToNextLevel;
		xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player != null)
		{
			CharacterStats stats = player.GetNode<CharacterStats>("Stats");
			stats.ApplyLevelUp();
		}
	}

	public void Reset()
	{
		isDead = false;
		coins = 0;
		score = 0;
		xp = 0;
		level = 1;
		xpToNextLevel = 100;
		gameTime = 0f;

		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player != null)
		{
			CharacterStats stats = player.GetNode<CharacterStats>("Stats");
			stats.health = stats.maxHealth;
		}
	}
}
