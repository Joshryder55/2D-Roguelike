using Godot;

public partial class GameManager : Node
{
	public bool isDead = false;
	public bool ultimateIsActive = false;
	public int coins = 0;
	public int score = 0;

	// XP and leveling
	public int xp = 0;
	public int level = 1;
	public int xpToNextLevel = 100;
	public float gameTime = 0f;
	
	public string currentLevel = "res://Scenes/Level1.tscn";

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Process(double delta)
	{
		if (!isDead && !GetTree().Paused)
			gameTime += (float)delta;
	}

	public float GetCoinDropMultiplier()
	{
		return Mathf.Max(1.0f, 2.5f - gameTime / 200f);
	}

	// 4 phase enemy scaling (time-based at 3, 6, and 11 minutes).
	// Each rate is a per-minute multiplier added on top of 1.0.
	static float enemyScalingRamp(float gameTimeMinutes,
								   float earlyRate,    // 0–3 min
								   float smallRate,    // 3–6 min
								   float bigRate,      // 6–11 min
								   float largestRate)  // 11+ min
	{
		float multiplier = 1.0f;
		multiplier += Mathf.Min(gameTimeMinutes,                         3f) * earlyRate;
		multiplier += Mathf.Min(Mathf.Max(gameTimeMinutes -  3f, 0f),   3f) * smallRate;
		multiplier += Mathf.Min(Mathf.Max(gameTimeMinutes -  6f, 0f),   5f) * bigRate;
		multiplier +=           Mathf.Max(gameTimeMinutes - 11f, 0f)         * largestRate;
		return multiplier;
	}

	// All enemy scaling multipliers cap at (map level + 1.0): L1 = 2.0×, L2 = 3.0×, L3 = 4.0×, etc.
	// HP: gentle warm-up → progressively steeper ramp.
	public float GetEnemyHealthMultiplier()
	{
		float levelCap = level + 1.0f;
		return Mathf.Min(levelCap, enemyScalingRamp(gameTime / 60f,
			earlyRate:   0.04f,
			smallRate:   0.10f,
			bigRate:     0.20f,
			largestRate: 0.35f));
	}

	// Speed: no change first 3 min, then very gradual increases.
	public float GetEnemySpeedMultiplier()
	{
		float levelCap = level + 1.0f;
		return Mathf.Min(levelCap, enemyScalingRamp(gameTime / 60f,
			earlyRate:   0f,
			smallRate:   0.03f,
			bigRate:     0.06f,
			largestRate: 0.10f));
	}

	// Damage: same as speed.
	public float GetEnemyDamageMultiplier()
	{
		float levelCap = level + 1.0f;
		return Mathf.Min(levelCap, enemyScalingRamp(gameTime / 60f,
			earlyRate:   0f,
			smallRate:   0.03f,
			bigRate:     0.06f,
			largestRate: 0.10f));
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
		xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.25f);

		GetNode<SoundManager>("/root/SoundManager").PlaySfx("LevelUp", true);

		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player != null)
		{
			CharacterStats stats = player.GetNode<CharacterStats>("Stats");
			stats.ApplyLevelUp();
		}

		var menu = new LevelUpMenu();
		GetTree().CurrentScene.AddChild(menu);
	}

	public void Reset()
	{
		isDead = false;
		// coins intentionally NOT reset — they persist across runs
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
