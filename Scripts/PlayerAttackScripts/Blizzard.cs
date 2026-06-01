using Godot;

public partial class Blizzard : Area2D
{
	public IceWizardStats iceStats;

	private float _duration;
	private float _tickInterval = 0.5f;
	private float _slowFactor;
	private float _elapsed = 0f;
	private float _tickElapsed = 0f;
	private AudioStreamPlayer _blizzardSound;

	public override void _Ready()
	{
		_blizzardSound = GetNode<SoundManager>("/root/SoundManager").PlaySfxLooping("Blizzard");

		// Read tuning values from iceStats so upgrades apply
		_duration   = iceStats?.blizzardDuration   ?? 6.0f;
		_slowFactor = iceStats?.blizzardSlowFactor ?? 0.4f;

		// Apply upgraded radius to the collision shape
		float radius = iceStats?.blizzardRadius ?? 150.0f;
		var shape = GetNode<CollisionShape2D>("CollisionShape2D");
		if (shape.Shape is CircleShape2D circle)
			circle.Radius = radius;

		// Scale sprite to match radius (base radius is 150)
		float scale = radius / 150f;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = new Vector2(scale, scale);

		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");

		BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		_elapsed     += (float)delta;
		_tickElapsed += (float)delta;

		if (_tickElapsed >= _tickInterval)
		{
			_tickElapsed = 0f;
			DamageEnemiesInside();
		}

		if (_elapsed >= _duration)
		{
			RestoreAllSpeeds();
			GetNode<GameManager>("/root/GameManager").ultimateIsActive = false;
			QueueFree();
		}
	}

	public override void _ExitTree()
	{
		if (IsInstanceValid(_blizzardSound)) _blizzardSound.QueueFree();
	}

	private void DamageEnemiesInside()
	{
		foreach (Node2D body in GetOverlappingBodies())
		{
			if (body is not CharacterBody2D) continue;

			EnemyHealth eh = body.GetNodeOrNull<EnemyHealth>("EnemyHealth");
			eh?.TakeDamage(iceStats?.frostNovaDamage / 3 ?? 5);

			Enemy enemy = body as Enemy;
			if (enemy != null && !enemy.immuneToAilments && enemy.currentStatus == Enemy.StatusEffect.None)
				enemy.speed = enemy.baseSpeed * _slowFactor;
		}
	}

	private void RestoreAllSpeeds()
	{
		foreach (Node2D body in GetOverlappingBodies())
		{
			Enemy enemy = body as Enemy;
			if (enemy != null && !enemy.immuneToAilments && enemy.currentStatus == Enemy.StatusEffect.None)
				enemy.speed = enemy.baseSpeed;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		Enemy enemy = body as Enemy;
		if (enemy != null && !enemy.immuneToAilments && enemy.currentStatus == Enemy.StatusEffect.None)
			enemy.speed = enemy.baseSpeed;
	}
}
