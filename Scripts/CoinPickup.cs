using Godot;
using System;

public partial class CoinPickup : Area2D
{
	GameManager gameManager;
	
	
	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body) {
		if (body.IsInGroup("player")) {
			gameManager.coins++;
			QueueFree();
		}
	}
}
