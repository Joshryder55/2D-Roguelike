using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	
	GameManager gameManager;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		MaxValue = gameManager.maxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Value = gameManager.health;
	}
}
