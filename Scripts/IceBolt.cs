using Godot;
using System;

public partial class IceBolt : Projectile
{
	
	public override float speed { get; set; } = 300.0f;
	public override int damage { get; set; } = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	
}
