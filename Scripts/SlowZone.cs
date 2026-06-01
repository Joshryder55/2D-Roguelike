using Godot;
using System;

public partial class SlowZone : Area2D
{
	public float speedMultiplier = 0.5f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body){
		Enemy enemy = body as Enemy;
		if (enemy != null){
		enemy.speed = enemy.baseSpeed * speedMultiplier;
		}
	}
	
	private void OnBodyExited(Node2D body){
		Enemy enemy = body as Enemy;
		if (enemy!= null) {
			enemy.speed = enemy.baseSpeed;
		}
	}
}
