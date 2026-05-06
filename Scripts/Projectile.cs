using Godot;
using System;

public partial class Projectile : Area2D {
	public Vector2 Direction;
	float speed = 300;
	float maxDistance = 500;
	Vector2 startPosition;

	public override void _Ready() {
		startPosition = GlobalPosition;
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta) {
		GlobalPosition += Direction * speed * (float)delta;
		
		if (GlobalPosition.DistanceTo(startPosition) > maxDistance)
			QueueFree();
	}

	
	private void OnBodyEntered(Node2D body) {
		
		if (body is CharacterBody2D) {
			body.QueueFree();
			QueueFree();
		}
	}
	
}
