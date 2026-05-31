using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	
public AnimatedSprite2D sprite;

public CharacterBody2D Player;	

CharacterStats characterStats;

protected GameManager gameManager;

NavigationAgent2D navAgent;

Area2D damageArea;
Timer damageTimer;

public virtual bool immuneToAilments { get; set; } = false;
public bool killedByUltimate = false;
public enum StatusEffect {None, Frozen, Burning, Poisoned}
public StatusEffect currentStatus = StatusEffect.None;

public virtual float speed { get; set; } = 50;
public float baseSpeed;
public virtual int contactDamage { get; set; } = 6;


public override void _Ready() {
	
	sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	gameManager = GetNode<GameManager>("/root/GameManager");
	
	var playerNode = GetTree().GetFirstNodeInGroup("player");
	if (playerNode != null)
		characterStats = playerNode.GetNode<CharacterStats>("Stats");

	damageArea = GetNode<Area2D>("HitDetection");
	damageArea.BodyEntered += OnBodyEntered;
	damageArea.BodyExited += OnBodyExited;

	if (sprite.SpriteFrames.HasAnimation("Walking"))
	sprite.Play("Walking");
	else
	sprite.Play("Still");
	
	damageTimer = new Timer();
	damageTimer.WaitTime = 0.5f;
	damageTimer.Timeout += DealDamage;
	AddChild(damageTimer);
	
	// Set speed and damage scaling at spawn
	speed         *= gameManager.GetEnemySpeedMultiplier();
	contactDamage  = Mathf.RoundToInt(contactDamage * gameManager.GetEnemyDamageMultiplier());
	baseSpeed      = speed;
}

private void OnBodyEntered(Node2D body) {
	if (body.IsInGroup("player")) {
		damageTimer.Start();
	}
}

private void OnBodyExited(Node2D body) {
	if (body.IsInGroup("player")) {
		damageTimer.Stop();
	}
}

private void DealDamage() {
	if (characterStats == null) {
		var playerNode = GetTree().GetFirstNodeInGroup("player");
		if (playerNode != null)
			characterStats = playerNode.GetNode<CharacterStats>("Stats");
		else return;
	}
	characterStats.TakeDamage(contactDamage);
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

	if (Player.GlobalPosition.X < GlobalPosition.X) {
		sprite.FlipH = true;
	} else {
		sprite.FlipH = false;
	}

	if (currentStatus == StatusEffect.None) {
		if (Velocity.Length() > 0) {
			sprite.Play("Walking");
		} else {
			sprite.Play("Still");
		}
	}
}


//My shitty attempt at better pathfinding, literally cannot figure it tf out
//public override void _PhysicsProcess(double delta) {
	//if (Player == null) return;
	//
	//if (gameManager.isDead) {
		//Velocity = Vector2.Zero;
		//sprite.Play("Still");
		//return;
	//}
//
	//navAgent.TargetPosition = Player.GlobalPosition;
//
//
	//Vector2 direction = Vector2.Zero;
	//if (!navAgent.IsNavigationFinished()) {
		//direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
	//}
	//
//
	//Velocity = direction * speed;
	//MoveAndSlide();
//
	//if (!gameManager.isDead) {
		//if (Player.GlobalPosition.X < GlobalPosition.X) {
			//sprite.FlipH = true;
		//} else {
			//sprite.FlipH = false;
		//}
	//}
//
	//if (currentStatus == StatusEffect.None) {
		//if (Velocity.Length() > 0) {
			//sprite.Play("Walking");
		//} else {
			//sprite.Play("Still");
		//}
	//}
//}


}
