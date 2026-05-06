using Godot;
using System;

public partial class GameManager : Node
{
	public int health = 10;
	
	public void TakeDamage(int amount){
		health -= amount;
		GD.Print("Health: " + health);
		
		if(health <= 0){
			Die();
		}
	}
	
	public void Die(){
		GD.Print("You Died!");
		//Add Scene Change later
	}
}
