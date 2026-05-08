using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	
AnimatedSprite2D sprite;

public CharacterBody2D Player;	

GameManager gameManager;

Area2D damageArea;
Timer damageTimer;

float speed = 50;


public override void _Ready() {
	
	sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	gameManager = GetNode<GameManager>("/root/GameManager");

	damageArea = GetNode<Area2D>("HitDetection");
	damageArea.BodyEntered += OnBodyEntered;
	damageArea.BodyExited += OnBodyExited;

	damageTimer = new Timer();
	damageTimer.WaitTime = 0.5f;
	damageTimer.Timeout += DealDamage;
	AddChild(damageTimer);
}

private void OnBodyEntered(Node2D body) {
	if (body is CharacterBody2D) {
		damageTimer.Start();
	}
}

private void OnBodyExited(Node2D body) {
	damageTimer.Stop();
}

private void DealDamage() {
	gameManager.TakeDamage(1);
}


public override void _PhysicsProcess(double delta) {
	if (Player == null) return;
	
	if (gameManager.isDead) {
		Velocity = Vector2.Zero;
		sprite.Play("Still");
		return;
	}

	Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
	Velocity = direction * speed;
	MoveAndSlide();

	if(!gameManager.isDead){
	if(Player.GlobalPosition.X < GlobalPosition.X){
		sprite.FlipH = true;
	}
	else {
		sprite.FlipH = false;
	}
	}
	

	if (Velocity.Length() > 0){
		sprite.Play("Walking");
	}
	else{
		sprite.Play("Still");
	}
	
	
	

}
}
