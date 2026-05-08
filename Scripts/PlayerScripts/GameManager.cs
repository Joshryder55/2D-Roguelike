using Godot;
using System;

public partial class GameManager : Node
{
	
	
	public int maxHealth = 10;
	public int health = 10;
	public bool isDead = false;
	
	public void Reset() {
	isDead = false;
	health = maxHealth;
	
	}
	
	public void TakeDamage(int amount){
		health -= amount;
		GD.Print("Health: " + health);
		
		if(health <= 0){
			Die();
		}
	}
	
	public void Die(){
		isDead = true;
		GD.Print("You Died!");
		PackedScene gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
		GetTree().CurrentScene.AddChild(gameOverScene.Instantiate());
	}
}
