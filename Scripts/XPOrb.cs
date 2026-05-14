using Godot;
using System;

public partial class XPOrb : Area2D
{
	GameManager gameManager;
	CharacterBody2D player;

	public int xpValue = 1;

	// (How close the player needs to be before the orb moves to them)
	float attractRadius = 80.0f;
	float moveSpeed = 150.0f;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		if (player == null || gameManager.isDead) return;

		// Move toward the player once they are close enough
		if (GlobalPosition.DistanceTo(player.GlobalPosition) < attractRadius)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			GlobalPosition += direction * moveSpeed * (float)delta;
		}
	}

	void OnBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("player")) return;
		gameManager.AddXP(xpValue);
		QueueFree();
	}

}
