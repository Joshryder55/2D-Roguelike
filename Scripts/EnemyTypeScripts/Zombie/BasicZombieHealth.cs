using Godot;
using System;

public partial class BasicZombieHealth : EnemyHealth
{
	public override int maxHealth { get; set; } = 30;
	public override int health { get; set; } = 30;
	public override int xpValue { get; set; } = 15;
	public override float coinDropChance { get; set; } = 0.15f;
}
