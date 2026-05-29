using Godot;

public partial class CorruptedBullHealth : EnemyHealth {
	public override int maxHealth { get; set; } = 500;
	public override int health { get; set; } = 500;
	public override int xpValue { get; set; } = 100;
	public override float coinDropChance { get; set; } = 1.0f;
	public override int scoreValue { get; set; } = 50;

	public override void Die(bool canShatter = true) {
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		saveData.hasCompletedLevel1 = true;

		Node level = GetTree().CurrentScene;
		Area2D gate = level.GetNodeOrNull<Area2D>("Level1ExitGate");
		if (gate != null) {
			gate.Visible = true;
			gate.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
		}

		base.Die(canShatter);
	}
}
