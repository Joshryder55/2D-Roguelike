using Godot;
using System;

public partial class BasicSkeletonArcherHealth : EnemyHealth
{
	public override int maxHealth { get; set; } = 20;
	public override int health { get; set; } = 20;
	public override int xpValue { get; set; } = 12;
	public override float coinDropChance { get; set; } = 0.13f;
}
