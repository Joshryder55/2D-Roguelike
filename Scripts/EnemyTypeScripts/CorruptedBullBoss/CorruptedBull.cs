using Godot;
using System;

public partial class CorruptedBull : Enemy {
	public override float speed { get; set; } = 80f;
	public override int contactDamage { get; set; } = 20;
	public override bool immuneToAilments { get; set; } = true;

	private enum BullState { Walking, ChargeTelegraph, Charging, Recovering }
	private BullState currentState = BullState.Walking;

	private float chargeSpeed = 400f;
	private float chargeDuration = 2.5f;
	private float chargeTimer = 0f;
	private float telegraphDuration = 2.0f;
	private float telegraphTimer = 0f;
	private float cooldownDuration = 7f;
	private float cooldownTimer = 0f;
	private Vector2 chargeDirection = Vector2.Zero;

	private int normalDamage = 20;
	private int chargeDamage = 35;

	public override void _Ready() {
		base._Ready();
		cooldownTimer = cooldownDuration;
	}

	public override void _PhysicsProcess(double delta) {
		if (Player == null) return;
		if (gameManager.isDead) {
			Velocity = Vector2.Zero;
			sprite.Play("Still");
			return;
		}

		switch (currentState) {
			case BullState.Walking:
				HandleWalking(delta);
				break;
			case BullState.ChargeTelegraph:
				HandleTelegraph(delta);
				break;
			case BullState.Charging:
				HandleCharging(delta);
				break;
			case BullState.Recovering:
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

		cooldownTimer -= (float)delta;
		if (cooldownTimer <= 0f) {
			currentState = BullState.ChargeTelegraph;
			telegraphTimer = telegraphDuration;
			chargeDirection = (Player.GlobalPosition - GlobalPosition).Normalized();
		}
	}

	private void HandleTelegraph(double delta) {
		Velocity = Vector2.Zero;
		sprite.Play("Stomp");

		telegraphTimer -= (float)delta;
		if (telegraphTimer <= 0f) {
			currentState = BullState.Charging;
			chargeTimer = chargeDuration;
			contactDamage = chargeDamage;
		}
	}

	private void HandleCharging(double delta) {
		Velocity = chargeDirection * chargeSpeed;
		MoveAndSlide();
		sprite.Play("Charge");

		chargeTimer -= (float)delta;
		if (chargeTimer <= 0f) {
			currentState = BullState.Recovering;
			cooldownTimer = 2f;
			contactDamage = normalDamage;
			Velocity = Vector2.Zero;
		}
	}

	private void HandleRecovery(double delta) {
		Velocity = Vector2.Zero;
		sprite.Play("Still");

		cooldownTimer -= (float)delta;
		if (cooldownTimer <= 0f) {
			currentState = BullState.Walking;
			cooldownTimer = cooldownDuration;
		}
	}
}
