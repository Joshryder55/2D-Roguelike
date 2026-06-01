using Godot;

public partial class CorruptedKnightHealth : EnemyHealth {
	public override int maxHealth { get; set; } = 2000;
	public override int health { get; set; } = 2000;
	public override int xpValue { get; set; } = 150;
	public override float coinDropChance { get; set; } = 1.0f;
	public override int scoreValue { get; set; } = 75;

	public override void Die(bool canShatter = true) {
	PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
	saveData.hasCompletedLevel2 = true;

	Node level = GetTree().CurrentScene;
	Area2D gate = level.GetNodeOrNull<Area2D>("Level2ExitGate");
	GD.Print("Gate found: " + (gate != null));
	if (gate != null) {
		gate.CallDeferred("set_visible", true);
		gate.GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred("set_disabled", false);
		GD.Print("Gate enabled!");
	}

	base.Die(canShatter);
}
}
