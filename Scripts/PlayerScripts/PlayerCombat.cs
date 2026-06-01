using Godot;
using System;
public partial class PlayerCombat : Node {
	GameManager gameManager;
	CharacterStats characterStats;
	[Export] public PackedScene ProjectileScene;
	Timer fireTimer;
	CharacterBody2D player;
	Random random = new Random();

	public override void _Ready() {
		gameManager = GetNode<GameManager>("/root/GameManager");
		gameManager.Reset();
		characterStats = GetParent().GetNode<CharacterStats>("Stats");
		player = GetParent<CharacterBody2D>();
		fireTimer = new Timer();
		fireTimer.WaitTime = characterStats.fireRate;
		fireTimer.Timeout += TryShoot;
		AddChild(fireTimer);
		fireTimer.Start();
	}

	public override void _Process(double delta) {
		if (fireTimer.WaitTime != characterStats.fireRate)
			fireTimer.WaitTime = characterStats.fireRate;
	}

	private void TryShoot() {
		if (gameManager.isDead) return;
		CharacterBody2D nearestEnemy = FindNearestEnemy();
		if (nearestEnemy == null) return;
		GetNode<SoundManager>("/root/SoundManager").PlaySfx("IceBolt");
		Vector2 mainDirection = (nearestEnemy.GlobalPosition - player.GlobalPosition).Normalized();

		IceWizardStats iceStats = characterStats as IceWizardStats;
		if (iceStats != null && iceStats.hasMultiShot && random.NextDouble() < iceStats.multiShotChance) {
			float totalSpread = 0.52f;
			float damageMultiplier = 1.0f / Mathf.Sqrt(iceStats.multishotCount);
			for (int i = 0; i < iceStats.multishotCount; i++) {
				float angle = (i / (float)(iceStats.multishotCount - 1)) * totalSpread - totalSpread / 2;
				spawnProjectile(mainDirection.Rotated(angle), damageMultiplier);
			}
		} else {
			spawnProjectile(mainDirection, 1.0f);
		}
	}

	private void spawnProjectile(Vector2 Direction, float damageMultiplier = 1.0f) {
		Projectile projectile = ProjectileScene.Instantiate<Projectile>();
		projectile.Direction = Direction;
		projectile.characterStats = characterStats;
		projectile.player = player;
		projectile.damage = Mathf.RoundToInt(projectile.damage * damageMultiplier);
		GetTree().CurrentScene.AddChild(projectile);
		projectile.GlobalPosition = player.GlobalPosition;
	}

	private CharacterBody2D FindNearestEnemy() {
		CharacterBody2D nearest = null;
		float nearestDistance = characterStats.range;
		foreach (Node node in GetTree().GetNodesInGroup("enemies")) {
			if (node is CharacterBody2D enemy && IsInstanceValid(enemy) && enemy.IsInsideTree()) {
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
