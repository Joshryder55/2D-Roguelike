using Godot;
using System;

public partial class IceBolt : Projectile
{
	
	public override float speed { get; set; } = 300.0f;
	public override int damage { get; set; } = 10;
	GameManager gameManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		gameManager = GetNode<GameManager>("/root/GameManager");
	}

	
	protected override void OnBodyEntered(Node2D body) {
		base.OnBodyEntered(body);  // runs Projectile's damage logic first
		
		if (!gameManager.hasFreezeOnHit) return;
		
			// roll random chance
			Random random = new Random();
			
			if (random.NextDouble() < 0.15) {  // 15% chance
				
				Enemy enemy = body as Enemy;
					if (enemy != null) {
						float originalSpeed = enemy.speed;
						enemy.speed = 0;
						
						Timer freezeTimer = new Timer();
						freezeTimer.WaitTime = 2.0f;
						freezeTimer.OneShot = true;
							freezeTimer.Timeout += () => {
								enemy.speed = originalSpeed;
								freezeTimer.QueueFree();
							};
						enemy.AddChild(freezeTimer);
						freezeTimer.Start();
					}	
				}
			}
	
}
