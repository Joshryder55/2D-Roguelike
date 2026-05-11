using Godot;
using System;

public partial class IceSpike : Projectile
{
	
	IceWizardStats iceStats;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		iceStats = characterStats as IceWizardStats;
		if (iceStats != null) {
			damage = iceStats.iceSpikeDamage;
			speed = 200f; // slower than ice bolt
		}
	}

protected override void OnBodyEntered(Node2D body) {
		if (body is CharacterBody2D) {
			//Instantiating object of enemyHealth
			EnemyHealth enemyHealth = body.GetNode<EnemyHealth>("EnemyHealth");
			
			if (enemyHealth != null){
				enemyHealth.TakeDamage(damage);
			}
				
				Enemy enemy = body as Enemy;
					if (enemy != null) {
						
						float originalSpeed = enemy.speed;
						
						enemy.speed = 0;
						enemy.currentStatus = Enemy.StatusEffect.Frozen;
						enemy.sprite.Play("Frozen");
						
						Timer freezeTimer = new Timer();
						freezeTimer.WaitTime = iceStats.iceSpikeFreezeDuration;
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
