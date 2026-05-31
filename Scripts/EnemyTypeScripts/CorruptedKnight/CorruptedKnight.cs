using Godot;
using System;

public partial class CorruptedKnight : Enemy {
	public override float speed { get; set; } = 120f;
	public override int contactDamage { get; set; } = 25;
	public override bool immuneToAilments { get; set; } = true;

	private enum KnightState { Walking, Windup, Sweeping, Recovering }
	private KnightState currentState = KnightState.Walking;

	private float sweepRange = 120f;
	private float windupDuration = 1.0f;
	private float windupTimer = 0f;
	private float recoveryDuration = 1f;
	private float recoveryTimer = 0f;

	private int normalDamage = 25;
	private int sweepDamage = 50;

	private Area2D sweepHitbox;

	public override void _Ready() {
		base._Ready();
		sweepHitbox = GetNode<Area2D>("SweepHitbox");
		sweepHitbox.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		sweepHitbox.BodyEntered += OnSweepHit;
				
	}

	public override void _PhysicsProcess(double delta) {
		if (Player == null) return;
		if (gameManager.isDead) {
			Velocity = Vector2.Zero;
			sprite.Play("Still");
			return;
		}

		switch (currentState) {
			case KnightState.Walking:
				HandleWalking(delta);
				break;
			case KnightState.Windup:
				HandleWindup(delta);
				break;
			case KnightState.Sweeping:
				HandleSweeping();
				break;
			case KnightState.Recovering:
				HandleRecovery(delta);
				break;
		}

		if (Velocity.X < 0)
			sprite.FlipH = true;
		else if (Velocity.X > 0)
			sprite.FlipH = false;
	}

	private void HandleWalking(double delta) {
		contactDamage = normalDamage;
		Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = direction * speed;
		MoveAndSlide();

		if (currentStatus == StatusEffect.None)
			sprite.Play("Walking");

		float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
		if (distanceToPlayer <= sweepRange) {
			currentState = KnightState.Windup;
			windupTimer = windupDuration;
			Velocity = Vector2.Zero;
		}
	}

	private void HandleWindup(double delta) {
		Velocity = Vector2.Zero;
		sprite.Play("Charge");

		windupTimer -= (float)delta;
		if (windupTimer <= 0f) {
			currentState = KnightState.Sweeping;
			contactDamage = sweepDamage;
			sweepHitbox.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
			sprite.Play("Swing");
			sprite.AnimationFinished += OnSwingFinished;
		}
	}

	private void HandleSweeping() {
		Velocity = Vector2.Zero;

		// Disable hitbox on recovery frames 15-16
		if (sprite.Frame >= 14) {
			sweepHitbox.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
			contactDamage = normalDamage;
		}
	}

	private void OnSwingFinished() {
		sprite.AnimationFinished -= OnSwingFinished;
		sweepHitbox.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		contactDamage = normalDamage;
		currentState = KnightState.Recovering;
		recoveryTimer = recoveryDuration;
	}

	private void HandleRecovery(double delta) {
		Velocity = Vector2.Zero;
		sprite.Play("Still");

		recoveryTimer -= (float)delta;
		if (recoveryTimer <= 0f) {
			currentState = KnightState.Walking;
		}
	}
	
	
	
	private void OnSweepHit(Node2D body) {
		if (body.IsInGroup("player")) {
			CharacterStats stats = body.GetNode<CharacterStats>("Stats");
			stats.TakeDamage(sweepDamage);
	   	 }
	}
	
	
	
}
