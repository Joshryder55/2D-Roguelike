using Godot;
using System;

public partial class BasicSkeleton : Enemy
{
	
	public override float speed { get; set; } = 100;
	public override int contactDamage { get; set; } = 6;

}
