using Godot;
using System;

public partial class PlayerCombat : Node {
	GameManager gameManager;
	[Export] public PackedScene ProjectileScene;
	float fireRate = 1.0f;
	float range = 500.0f;
	Timer fireTimer;
	CharacterBody2D player;
	Random random = new Random();

	public override void _Ready() {
		gameManager = GetNode<GameManager>("/root/GameManager");
		gameManager.Reset();
		
		player = GetParent<CharacterBody2D>();
		
		fireTimer = new Timer();
		fireTimer.WaitTime = fireRate;
		fireTimer.Timeout += TryShoot;
		AddChild(fireTimer);
		fireTimer.Start();
	}

	private void TryShoot() {
		
		if(gameManager.isDead) return;
		
		CharacterBody2D nearestEnemy = FindNearestEnemy();
		if (nearestEnemy == null) return;
		
		Vector2 mainDirection = (nearestEnemy.GlobalPosition - player.GlobalPosition).Normalized();
		
		spawnProjectile(mainDirection);
		
		//Check if multishot procs
		if(gameManager.hasMultiShot && random.NextDouble() < 0.2){
			spawnProjectile(mainDirection.Rotated(0.26f));
			spawnProjectile(mainDirection.Rotated(-0.26f));
		}
	}
	
	private void spawnProjectile(Vector2 Direction) {
		Projectile projectile = ProjectileScene.Instantiate<Projectile>();
		projectile.Direction = Direction;
		GetTree().CurrentScene.AddChild(projectile);
		projectile.GlobalPosition = player.GlobalPosition;	 
	}
	

	private CharacterBody2D FindNearestEnemy() {
		CharacterBody2D nearest = null;
		float nearestDistance = range;

		foreach (Node node in GetTree().GetNodesInGroup("enemies")) {
			if (node is CharacterBody2D enemy) {
				float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
				if (distance < nearestDistance) {
					nearestDistance = distance;
					nearest = enemy;
				}
			}
		}
		return nearest;
	}
}
