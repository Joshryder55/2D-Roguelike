using Godot;
using System;

public partial class EnemyManager : Node {
	[Export] public CharacterBody2D Player;
	[Export] public PackedScene EnemyScene;

	public override void _Ready() {
		GD.Print("EnemyManager Ready");
		GD.Print("EnemyScene is: " + EnemyScene);
		SpawnEnemy(new Vector2(300, 300));
		
	}

	private void SpawnEnemy(Vector2 spawnPosition) {
	GD.Print("Spawning enemy");
	Enemy enemy = EnemyScene.Instantiate<Enemy>();
	enemy.Player = Player;
	enemy.GlobalPosition = spawnPosition;
	AddChild(enemy);
	GD.Print("Enemy added, Player assigned: " + enemy.Player);
}

}
