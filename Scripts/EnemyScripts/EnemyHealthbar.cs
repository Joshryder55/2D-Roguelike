using Godot;
using System;

public partial class EnemyHealthbar : ProgressBar
{
	EnemyHealth enemyHealth;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyHealth = GetParent().GetNode<EnemyHealth>("EnemyHealth");
		MaxValue = enemyHealth.maxHealth;
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Value = enemyHealth.health;
	}
}
