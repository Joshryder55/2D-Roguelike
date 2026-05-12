using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	CharacterStats characterStats;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		characterStats = player.GetNode<CharacterStats>("Stats");
		
		MaxValue = characterStats.maxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Value = characterStats.health;
	}
}
