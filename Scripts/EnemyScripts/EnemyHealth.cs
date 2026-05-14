using Godot;
using System;

public partial class EnemyHealth : Node
{
	public virtual int health { get; set; }= 20;
	public virtual int maxHealth { get; set; }= 20;
	
	public virtual void TakeDamage(int amount){
		health -= amount;
		GetParent().GetNode<ProgressBar>("ProgressBar").Visible = true;
		if (health <= 0){
			Die();
		}
	}
	
	public virtual void Die() {

		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		CharacterStats stats = player.GetNode<CharacterStats>("Stats");
		stats.ultimateCharge++;

		// Spawn XP orb at the mob's position on death
		PackedScene orbScene = GD.Load<PackedScene>("res://Scenes/XPOrb.tscn");
		XPOrb orb = orbScene.Instantiate<XPOrb>();
		orb.GlobalPosition = GetParent<Node2D>().GlobalPosition;
		GetTree().CurrentScene.AddChild(orb);

		GetParent().QueueFree();
	}
	
}
