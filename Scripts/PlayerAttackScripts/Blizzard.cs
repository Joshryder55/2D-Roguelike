using Godot;

public partial class Blizzard : Area2D
{
	public IceWizardStats iceStats;

	// Tuning values — adjust for balancing
	private float _duration     = 6.0f;   // how long the blizzard lasts
	private float _tickInterval = 0.5f;   // damage every 0.5s
	private float _slowFactor   = 0.4f;   // enemies move at 40% speed inside

	private float _elapsed = 0f;
	private float _tickElapsed = 0f;

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
	}

	public override void _Process(double delta)
	{
		_elapsed     += (float)delta;
		_tickElapsed += (float)delta;

		// Damage tick
		if (_tickElapsed >= _tickInterval)
		{
			_tickElapsed = 0f;
			DamageEnemiesInside();
		}

		// Expire after duration
		if (_elapsed >= _duration)
		{
			RestoreAllSpeeds();
			GetNode<GameManager>("/root/GameManager").ultimateIsActive = false;
			QueueFree();
		}
	}

	private void DamageEnemiesInside()
	{
		foreach (Node2D body in GetOverlappingBodies())
		{
			if (body is not CharacterBody2D) continue;

			// Damage
			EnemyHealth eh = body.GetNodeOrNull<EnemyHealth>("EnemyHealth");
			eh?.TakeDamage(iceStats?.frostNovaDamage / 3 ?? 5); // reuse frost nova damage / 3

			// Slow — only apply if not already frozen
			Enemy enemy = body as Enemy;
			if (enemy != null && enemy.currentStatus == Enemy.StatusEffect.None)
			{
				enemy.speed = enemy.baseSpeed * _slowFactor;
			}
		}
	}

	// Restore speeds when blizzard ends for any still-slowed enemies
	private void RestoreAllSpeeds()
	{
		foreach (Node2D body in GetOverlappingBodies())
		{
			Enemy enemy = body as Enemy;
			if (enemy != null && enemy.currentStatus == Enemy.StatusEffect.None)
				enemy.speed = enemy.baseSpeed;
		}
	}

	// Called when an enemy walks OUT of the blizzard — restore their speed
	public override void _EnterTree()
	{
		BodyExited += OnBodyExited;
	}

	private void OnBodyExited(Node2D body)
	{
		Enemy enemy = body as Enemy;
		if (enemy != null && enemy.currentStatus == Enemy.StatusEffect.None)
			enemy.speed = enemy.baseSpeed;
	}
}
