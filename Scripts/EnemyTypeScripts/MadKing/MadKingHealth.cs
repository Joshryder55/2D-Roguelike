using Godot;

public partial class MadKingHealth : EnemyHealth {
	public override int maxHealth { get; set; } = 1000;
	public override int health { get; set; } = 1000;
	public override int xpValue { get; set; } = 200;
	public override float coinDropChance { get; set; } = 1.0f;
	public override int scoreValue { get; set; } = 100;

	public override void TakeDamage(int amount, bool canShatter = true) {
		// Block all direct damage — king only dies from minion deaths
	}

	public override void Die(bool canShatter = true) {
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		saveData.hasCompletedLevel3 = true;

		// Show victory screen or end game
		GD.Print("Mad King defeated! Game complete!");

		base.Die(canShatter);
	}
}
