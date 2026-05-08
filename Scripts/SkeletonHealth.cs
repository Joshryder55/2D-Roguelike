using Godot;
using System;

public partial class SkeletonHealth : EnemyHealth
{
	public override int maxHealth { get; set; } = 10;
	public override int health { get; set; } = 10;
}
