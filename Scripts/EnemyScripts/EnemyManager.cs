using Godot;
using System;

public partial class EnemyManager : Node {
	[Export] public CharacterBody2D Player;
	[Export] public PackedScene[] EnemyScenes;

	GameManager gameManager;
	Timer spawnTimer;
	Random random = new Random();

	float spawnInterval = 2.25f;
	const float minSpawnInterval = 0.5f;
	const int maxEnemies = 60;

	float elapsedTime = 0f;
	// Spawn Rampup interval: every 18 seconds shave 0.25s off the spawn interval - ADJUST FOR BALANCING
	const float rampInterval = 18f;
	const float rampAmount = 0.25f;
	float nextRampAt = 18f;

	public override void _Ready() {
		gameManager = GetNode<GameManager>("/root/GameManager");

		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnInterval;
		spawnTimer.Timeout += SpawnEnemy;
		AddChild(spawnTimer);
		spawnTimer.Start();

		//SpawnEnemy();
	}

	public override void _Process(double delta) {
		if (gameManager.isDead) return;

		elapsedTime += (float)delta;

		if (elapsedTime >= nextRampAt) {
			spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - rampAmount);
			spawnTimer.WaitTime = spawnInterval;
			nextRampAt += rampInterval;
		}
	}

	private Vector2 GetRandomSpawnPosition() {
		if (Player == null) return Vector2.Zero;
		Vector2 cameraPos = Player.GetNode<Camera2D>("Camera2D").GlobalPosition;
		float offset = 600.0f;

		int side = random.Next(4);
		return side switch {
			0 => new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y - offset),
			1 => new Vector2(cameraPos.X + offset,                 cameraPos.Y + random.Next(-500, 500)),
			2 => new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y + offset),
			_ => new Vector2(cameraPos.X - offset,                 cameraPos.Y + random.Next(-500, 500)),
		};
	}

	public void SpawnEnemy() {
		if (gameManager.isDead) return;
		if (Player == null) return;
		if (GetTree().GetNodesInGroup("enemies").Count >= maxEnemies) return;

		int index = random.Next(EnemyScenes.Length);
		Enemy enemy = EnemyScenes[index].Instantiate<Enemy>();
		enemy.Player = Player;
		enemy.GlobalPosition = GetRandomSpawnPosition();
		AddChild(enemy);
	}
}
