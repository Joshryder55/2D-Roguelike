using Godot;
using System;

public partial class BasicGoblinHealth : EnemyHealth {
	public override int maxHealth { get; set; } = 8;
	public override int health { get; set; } = 8;
	public override int xpValue { get; set; } = 5;
	public override float coinDropChance { get; set; } = 0.06f;
}
