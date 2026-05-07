using Godot;
using System;

public partial class Projectile : Area2D {
	AnimatedSprite2D sprite;
	public Vector2 Direction;
	float speed = 300;
	float maxDistance = 2000.0f;
	Vector2 startPosition;

	public override void _Ready() {
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		startPosition = GlobalPosition;
		BodyEntered += OnBodyEntered;
		Rotation = Direction.Angle() + Mathf.Pi / 2;;
	}

	public override void _PhysicsProcess(double delta) {
		GlobalPosition += Direction * speed * (float)delta;
		
		if (GlobalPosition.DistanceTo(startPosition) > maxDistance)
			QueueFree();
			
		sprite.Play("Default");
			
	}

	
	private void OnBodyEntered(Node2D body) {
		
		if (body is CharacterBody2D) {
			//Instantiating object of enemyHealth
			EnemyHealth enemyHealth = body.GetNode<EnemyHealth>("EnemyHealth");
			
			if (enemyHealth != null){
				enemyHealth.TakeDamage(10);
			}
			
			QueueFree();
		}
	}
	
}
