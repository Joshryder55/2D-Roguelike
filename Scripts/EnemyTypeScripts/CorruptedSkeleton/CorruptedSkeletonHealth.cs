using Godot;

public partial class CorruptedSkeletonHealth : BasicSkeletonHealth {
	public override int maxHealth { get; set; } = 100;
	public override int health { get; set; } = 100;
	
	public override void Die(bool canShatter = true) {
		MadKing king = GetTree().GetFirstNodeInGroup("boss") as MadKing;
		if (king != null)
			king.TakeMinionDamage(100);
		base.Die(canShatter);
	}
}
