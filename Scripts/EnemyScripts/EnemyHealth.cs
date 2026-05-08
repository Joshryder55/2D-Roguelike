using Godot;
using System;

public partial class EnemyHealth : Node
{
	public virtual int health { get; set; }= 20;
	public virtual int maxHealth { get; set; }= 20;
	
	public virtual void TakeDamage(int amount){
		health -= amount;
		GetParent().GetNode<ProgressBar>("ProgressBar").Visible = true;
		GD.Print("Enemy Health: " + health);
		if (health <= 0){
			Die();
		}
	}
	
	public virtual void Die() {
		GD.Print("Enemy Killed");
		GetParent().QueueFree();
	}
	
}
