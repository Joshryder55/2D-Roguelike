using Godot;
using System;

public partial class BasicSkeletonHealth : EnemyHealth
{
	public override int maxHealth { get; set; } = 15;
	public override int health { get; set; } = 15;
	public override int xpValue { get; set; } = 8;
	public override float coinDropChance { get; set; } = 0.11f;
}
