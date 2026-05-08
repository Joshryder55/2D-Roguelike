using Godot;
using System;

public partial class BasicZombieHealth : EnemyHealth
{
	public override int maxHealth { get; set; } = 20;
	public override int health { get; set; } = 20;
}
