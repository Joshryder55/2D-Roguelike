using Godot;
using System;
public partial class UltimateBar : TextureProgressBar {
	CharacterStats stats;
	public override void _Ready() {
		// Player may not be spawned yet — defer lookup to _Process
	}
	public override void _Process(double delta) {
		if (stats == null) {
			CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
			if (player == null) return;
			stats = player.GetNode<CharacterStats>("Stats");
			MaxValue = stats.GetUltimateChargeRequired();
		}
		IceWizardStats iceStats = stats as IceWizardStats;
		if (iceStats != null && iceStats.activeUltimate == IceWizardStats.UltimateAbility.None) {
			Visible = false;
			return;
		}
		Visible = true;
		Value = stats.ultimateCharge;
		MaxValue = stats.GetUltimateChargeRequired();
	}
}
