using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	
public AnimatedSprite2D sprite;

public CharacterBody2D Player;	

CharacterStats characterStats;

GameManager gameManager;

NavigationAgent2D navAgent;

Area2D damageArea;
Timer damageTimer;

public enum StatusEffect {None, Frozen, Burning, Poisoned}
public StatusEffect currentStatus = StatusEffect.None;

public virtual float speed { get; set; } = 50;


public override void _Ready() {
	
	sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	gameManager = GetNode<GameManager>("/root/GameManager");
	
	characterStats = GetTree().GetFirstNodeInGroup("player").GetNode<CharacterStats>("Stats");

	damageArea = GetNode<Area2D>("HitDetection");
	damageArea.BodyEntered += OnBodyEntered;
	damageArea.BodyExited += OnBodyExited;

	sprite.Play("Walking");
	
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
	characterStats.TakeDamage(1);
}


public override void _PhysicsProcess(double delta) {
	if (Player == null) return;
	
	if (gameManager.isDead) {
		Velocity = Vector2.Zero;
		sprite.Play("Still");
		return;
	}

	navAgent.TargetPosition = Player.GlobalPosition;


	Vector2 direction = Vector2.Zero;
	if (!navAgent.IsNavigationFinished()) {
		direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
	}

	Velocity = direction * speed;
	MoveAndSlide();

	if (!gameManager.isDead) {
		if (Player.GlobalPosition.X < GlobalPosition.X) {
			sprite.FlipH = true;
		} else {
			sprite.FlipH = false;
		}
	}

	if (currentStatus == StatusEffect.None) {
		if (Velocity.Length() > 0) {
			sprite.Play("Walking");
		} else {
			sprite.Play("Still");
		}
	}
}


}
