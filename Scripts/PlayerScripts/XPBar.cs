using Godot;
using System;

// Simple XP progress bar and level label if we want to use it. (Not sure how we want to organize the UI yet so added this for visual cue for xp pickup and leveling)
public partial class XPBar : ProgressBar
{
	GameManager gameManager;
	Label levelLabel;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		levelLabel = GetParent().GetNode<Label>("LevelLabel");

		MaxValue = gameManager.xpToNextLevel;
	}

	public override void _Process(double delta)
	{
		// Max value has to update each level since xpToNextLevel always increases on level up
		MaxValue = gameManager.xpToNextLevel;
		Value = gameManager.xp;
		levelLabel.Text = "Level " + gameManager.level;
	}
}
