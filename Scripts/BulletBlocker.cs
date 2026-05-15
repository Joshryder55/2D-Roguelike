using Godot;
using System;

public partial class BulletBlocker : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area) {
		area.QueueFree();
	}
}
