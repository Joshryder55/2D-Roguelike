using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
public CharacterBody2D Player;	
Area2D damageArea;
Timer damageTimer;
PlayerHealth playerHealth;
float speed = 50;

public override void _Ready() {
	damageArea = GetNode<Area2D>("HitDetection");
	damageArea.BodyEntered += OnBodyEntered;
	damageArea.BodyExited += OnBodyExited;

	damageTimer = new Timer();
	damageTimer.WaitTime = 1.0f;
	damageTimer.Timeout += DealDamage;
	AddChild(damageTimer);
}

private void OnBodyEntered(Node2D body) {
	if (body is CharacterBody2D) {
		playerHealth = body.GetNode<PlayerHealth>("HealthComponent");
		if (playerHealth != null)
			damageTimer.Start();
	}
}

private void OnBodyExited(Node2D body) {
	damageTimer.Stop();
	playerHealth = null;
}

private void DealDamage() {
	playerHealth?.TakeDamage(1);
}


	public override void _PhysicsProcess(double delta) {
		 GD.Print("Player is: " + Player);
	if (Player == null) return;
	
	Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
	Velocity = direction * speed;
	MoveAndSlide();
}
}
