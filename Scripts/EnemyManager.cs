using Godot;
using System;

public partial class EnemyManager : Node {
	[Export] public CharacterBody2D Player;
	[Export] public PackedScene EnemyScene;

	
	public override void _Ready() {
		
		Timer spawnTimer = new Timer();
		spawnTimer.WaitTime = 3.0f;
		spawnTimer.Timeout += SpawnEnemy;
		AddChild(spawnTimer);
		spawnTimer.Start();
		
		SpawnEnemy();
		SpawnEnemy();
		
		
	}

	private Vector2 GetRandomSpawnPosition(){
		Vector2 cameraPos = Player.GetNode<Camera2D>("Camera2D").GlobalPosition;
		float offset = 600.0f;
		
		Vector2 Spawn = new Vector2();
		
		Random random = new Random();
		int SpawnSide = random.Next(4);
		
		switch(SpawnSide){
			case 0: //Top 
				Spawn = new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y - offset);
				break;
			
			case 1: //Right
				Spawn = new Vector2(cameraPos.X + offset, cameraPos.Y + random.Next(-500,500));
				break;
			
			case 2: //Bottom
				Spawn = new Vector2(cameraPos.X + random.Next(-500, 500), cameraPos.Y + offset);
				break;
			
			default: //Left
				Spawn = new Vector2(cameraPos.X - offset, cameraPos.Y + random.Next(-500, 500));
				break;
		}
		
		return Spawn;
		
	}
	
	private void SpawnEnemy() {
	Enemy enemy = EnemyScene.Instantiate<Enemy>();
	enemy.Player = Player;
	enemy.GlobalPosition = GetRandomSpawnPosition();
	AddChild(enemy);
}
}
