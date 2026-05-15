using Godot;
using System;

public partial class FrostNova : Area2D
{
	
	CircleShape2D circle;
	public IceWizardStats iceStats;
	public float growSpeed = 200.0f;
	AnimatedSprite2D sprite;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
		CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		circle = new CircleShape2D();
		circle.Radius = 10f;
		collisionShape.Shape = circle;
		BodyEntered += OnBodyEntered;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if(circle.Radius >= iceStats.frostNovaRadius) {
			QueueFree();
			return;
		}
		circle.Radius += growSpeed * (float)delta;
		
		// scale visual to match collision
		float scale = circle.Radius / 300; // 10 is your starting radius
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = new Vector2(scale, scale);
	}
	
	private void OnBodyEntered(Node2D body) {
		
		
		if (iceStats == null) return;
		
		if (body is CharacterBody2D) {
			EnemyHealth enemyHealth = body.GetNode<EnemyHealth>("EnemyHealth");
			enemyHealth?.TakeDamage(iceStats.frostNovaDamage);
			
			Enemy enemy = body as Enemy;
			if (enemy != null) {
				float originalSpeed = enemy.speed;
				enemy.speed = 0;
				enemy.currentStatus = Enemy.StatusEffect.Frozen;
				enemy.sprite.Play("Frozen");
				
				Timer freezeTimer = new Timer();
				freezeTimer.WaitTime = iceStats.frostNovaFreezeDuration;
				freezeTimer.OneShot = true;
				freezeTimer.Timeout += () => {
					enemy.speed = originalSpeed;
					enemy.sprite.Play("Walking");
					enemy.currentStatus = Enemy.StatusEffect.None;
					freezeTimer.QueueFree();
				};
				enemy.AddChild(freezeTimer);
				freezeTimer.Start();
			}
		}
	}
	
}
