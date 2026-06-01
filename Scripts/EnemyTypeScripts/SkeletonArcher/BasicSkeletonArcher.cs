using Godot;
using System;

public partial class BasicSkeletonArcher : Enemy {
	
	[Export] public PackedScene ArrowScene;
	public float stopDistance = 300f;
	public float fireRate = 2.0f;
	private float fireCooldown = 0f;
	private bool isDrawing = false;

	public override float speed { get; set; } = 40f;
	public override int contactDamage { get; set; } = 8;

	public override void _PhysicsProcess(double delta) {
		if (Player == null) return;

		if (gameManager.isDead) {
			Velocity = Vector2.Zero;
			sprite.Play("Still");
			return;
		}

		float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);

		if (distanceToPlayer > stopDistance) {
			// Move toward player
			Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * speed;
			MoveAndSlide();

			if (currentStatus == StatusEffect.None) {
				sprite.Play("Walking");
			}
		} else {
			// In range — stop and shoot
			Velocity = Vector2.Zero;

		   	if (currentStatus == StatusEffect.None && !isDrawing) {
				sprite.Play("Still");
			}

			fireCooldown -= (float)delta;
			if (fireCooldown <= 0f) {
				Shoot();
				fireCooldown = fireRate;
			}
		}
		
	


		// Flip sprite
		if (Player.GlobalPosition.X < GlobalPosition.X) {
			sprite.FlipH = true;
		} else {
			sprite.FlipH = false;
		}
	}

   private void Shoot() {
		if (ArrowScene == null) return;
		if (isDrawing) return; // already drawing, don't interrupt

		isDrawing = true;
		sprite.Play("Draw");
		sprite.AnimationFinished += OnDrawFinished;
}

	private void OnDrawFinished() {
		sprite.AnimationFinished -= OnDrawFinished; // disconnect so it doesn't fire again
		isDrawing = false;

		// Spawn arrow
		Arrow arrow = ArrowScene.Instantiate() as Arrow;
		arrow.GlobalPosition = GlobalPosition;
		arrow.direction = (Player.GlobalPosition - GlobalPosition).Normalized();
		GetTree().CurrentScene.CallDeferred("add_child", arrow);
	}
}
