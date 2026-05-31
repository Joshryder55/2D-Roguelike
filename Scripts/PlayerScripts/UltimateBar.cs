using Godot;
using System;

public partial class UltimateBar : TextureProgressBar {
	CharacterStats stats;

	public override void _Ready() {
		CharacterBody2D player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		stats = player.GetNode<CharacterStats>("Stats");
		MaxValue = stats.GetUltimateChargeRequired();
	}

	public override void _Process(double delta) {
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
