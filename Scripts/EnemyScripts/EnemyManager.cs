using Godot;
using System;

public partial class EnemyManager : Node {
	[Export] public CharacterBody2D Player;
	[Export] public PackedScene[] EnemyScenes;
	[Export] public PackedScene BossScene;

	GameManager gameManager;
	Timer spawnTimer;
	Random random = new Random();

	float spawnInterval = 2.25f;
	const float minSpawnInterval = 0.5f;
	const int maxEnemies = 60;
	float elapsedTime = 0f;

	const float rampInterval = 18f;
	const float rampAmount = 0.25f;
	float nextRampAt = 18f;

	const float bossSpawnTime = 600f; // 10 minutes
	bool bossSpawned = false;
	bool bossDefeated = false;

	public override void _Ready() {
		gameManager = GetNode<GameManager>("/root/GameManager");
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnInterval;
		spawnTimer.Timeout += SpawnEnemy;
		AddChild(spawnTimer);
		spawnTimer.Start();
		SpawnEnemy();
	}

	public override void _Process(double delta) {
		if (gameManager.isDead) return;

		elapsedTime += (float)delta;

		// Spawn rampup
		if (elapsedTime >= nextRampAt && !bossSpawned) {
			spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - rampAmount);
			spawnTimer.WaitTime = spawnInterval;
			nextRampAt += rampInterval;
		}

		// Boss spawn at 10 minutes
		if (!bossSpawned && elapsedTime >= bossSpawnTime) {
			SpawnBoss();
		}

		// Check if boss is defeated to resume spawning
		if (bossSpawned && !bossDefeated) {
			if (GetTree().GetNodesInGroup("boss").Count == 0) {
				bossDefeated = true;
				ResumNormalSpawning();
			}
		}
	}

	private void SpawnBoss() {
	if (BossScene == null) return;
	bossSpawned = true;

	spawnInterval = 5f;
	spawnTimer.WaitTime = spawnInterval;

	CorruptedBull boss = BossScene.Instantiate<CorruptedBull>();
	boss.Player = Player;
	boss.GlobalPosition = GetRandomSpawnPosition();
	AddChild(boss);

	GD.Print("Boss spawned!");
}

	private void ResumNormalSpawning() {
		// Reset spawn interval to where it was before boss
		spawnInterval = minSpawnInterval;
		spawnTimer.WaitTime = spawnInterval;
		GD.Print("Boss defeated — spawning resumed!");
	}

	private Vector2 GetRandomSpawnPosition() {
		Vector2 cameraPos = Player.GetNode<Camera2D>("Camera2D").GlobalPosition;
		float offset = 600.0f;
		int side = random.Next(4);
		return side switch {
			0 => new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y - offset),
			1 => new Vector2(cameraPos.X + offset, cameraPos.Y + random.Next(-500, 500)),
			2 => new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y + offset),
			_ => new Vector2(cameraPos.X - offset, cameraPos.Y + random.Next(-500, 500)),
		};
	}

	private void SpawnEnemy() {
		if (gameManager.isDead) return;
		if (GetTree().GetNodesInGroup("enemies").Count >= maxEnemies) return;

		int index = random.Next(EnemyScenes.Length);
		Enemy enemy = EnemyScenes[index].Instantiate<Enemy>();
		enemy.Player = Player;
		enemy.GlobalPosition = GetRandomSpawnPosition();
		AddChild(enemy);
	}
}
