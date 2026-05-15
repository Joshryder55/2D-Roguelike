using Godot;
using System;

public partial class EnemyHealth : Node
{
	public virtual int health { get; set; }= 20;
	public virtual int maxHealth { get; set; }= 20;
	public virtual float coinDropChance { get; set; } = 0.1f; // 10% chance
	
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
		
		// Coin drop chance
		if (GD.Randf() < coinDropChance) {
			PackedScene coinScene = GD.Load<PackedScene>("res://Scenes/Coin.tscn");
			CoinPickup coin = coinScene.Instantiate() as CoinPickup;
			coin.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			GetTree().CurrentScene.CallDeferred("add_child", coin);
		}
		

		// Spawn XP orb at the mob's position on death
		PackedScene orbScene = GD.Load<PackedScene>("res://Scenes/XPOrb.tscn");
		XPOrb orb = orbScene.Instantiate<XPOrb>();
		orb.GlobalPosition = GetParent<Node2D>().GlobalPosition;
		GetTree().CurrentScene.CallDeferred("add_child", orb);

		GetParent().QueueFree();
	}
	
}
