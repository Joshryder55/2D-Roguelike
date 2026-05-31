using Godot;
using System;

public partial class IceBolt : Projectile
{
	
	public override float speed { get; set; } = 300.0f;
	public override int damage { get; set; } = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
	}

	
	protected override void OnBodyEntered(Node2D body) {
		base.OnBodyEntered(body);  // runs Projectile's damage logic first
		
		IceWizardStats iceStats = characterStats as IceWizardStats;
		if (iceStats == null) return;
		
		if (!iceStats.hasFreezeOnHit) return;
		
			// roll random chance
			Random random = new Random();
			
			if (random.NextDouble() < iceStats.freezeChance) {  // 15% chance
				
				Enemy enemy = body as Enemy;
					if (enemy != null && !enemy.immuneToAilments && !enemy.IsQueuedForDeletion()) {
						
						float originalSpeed = enemy.speed;
						
						enemy.speed = 0;
						enemy.currentStatus = Enemy.StatusEffect.Frozen;
						enemy.sprite.Play("Frozen");

						SoundManager sound = GetNode<SoundManager>("/root/SoundManager");
						SceneTreeTimer freezeSoundDelay = GetTree().CreateTimer(0.08f);
						freezeSoundDelay.Timeout += () => {
							if (IsInstanceValid(enemy) && !enemy.IsQueuedForDeletion())
								sound.PlaySfx("EnemyFreeze");
						};
						
						Timer freezeTimer = new Timer();
						freezeTimer.WaitTime = iceStats.freezeDuration;
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
