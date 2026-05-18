using Godot;
using System;

public partial class Arrow : Projectile {
	public Vector2 direction;

	public override float speed { get; set; } = 200.0f;
	public override int damage { get; set; } = 5;

	public override void _Ready() {
		Direction = direction;
		startPosition = GlobalPosition;
		BodyEntered += OnBodyEntered;
		Rotation = Direction.Angle();
	}
	
	public override void _PhysicsProcess(double delta) {
		GlobalPosition += Direction * speed * (float)delta;
		
		if (player != null && GlobalPosition.DistanceTo(player.GlobalPosition) > maxDistance)
			QueueFree();
	}

	protected override void OnBodyEntered(Node2D body) {
		if (body is CharacterBody2D) {
			// Check if it hit the player not an enemy
			if (body.IsInGroup("player")) {
				CharacterStats stats = body.GetNode<CharacterStats>("Stats");
				stats.TakeDamage(damage);
				QueueFree();
			}
		}
	}
}
