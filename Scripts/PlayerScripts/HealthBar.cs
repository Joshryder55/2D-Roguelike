using Godot;
using System;

public partial class HealthBar : TextureProgressBar {
	CharacterStats stats;

	public override void _Ready() {
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		stats = player.GetNode<CharacterStats>("Stats");
	}

	public override void _Process(double delta) {
		MaxValue = stats.maxHealth;
		Value = stats.health;
	}
	
}
