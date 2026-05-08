using Godot;
using System;

public partial class Projectile : Area2D {
	AnimatedSprite2D sprite;
	public Vector2 Direction;
	float maxDistance = 2000.0f;
	Vector2 startPosition;
	
	public virtual float speed { get; set; } = 300.0f;
	public virtual int damage { get; set; } = 20;

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
			
		sprite.Play("default");
			
	}

	
	protected void OnBodyEntered(Node2D body) {
		
		if (body is CharacterBody2D) {
			//Instantiating object of enemyHealth
			EnemyHealth enemyHealth = body.GetNode<EnemyHealth>("EnemyHealth");
			
			if (enemyHealth != null){
				enemyHealth.TakeDamage(damage);
			}
			
			QueueFree();
		}
	}
	
}
