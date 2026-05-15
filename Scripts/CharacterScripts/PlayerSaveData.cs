using Godot;
using System;

public partial class PlayerSaveData : Node
{
	
	public IceWizardStats.UltimateAbility activeUltimate = IceWizardStats.UltimateAbility.None;
	
	
	// Ice Wizard unlocks
	public bool hasMultiShot = false;
	public bool hasFreezeOnHit = false;
	public bool hasFrostNova = false;
	public bool hasIceSpike = false;
	public bool hasBlizzard = false;
	public bool hasFlashFreeze = false;
	public bool hasPermafrost = false;
	public bool hasBrittle = false;
	public bool hasIceShield = false;

	// Ice Wizard upgrade levels
	public int multishotLevel = 0;
	public int freezeOnHitLevel = 0;
	public int frostNovaLevel = 0;
	// etc

	// Shared stat bonuses
	public float moveSpeedBonus = 0;
	public int healthBonus = 0;
	public float attackSpeedBonus = 0;
	public int damageBonus = 0;
	
	// Frost Nova upgrades
	public int frostNovaDamageLevel = 0;
	public int frostNovaRadiusLevel = 0;
	public int frostNovaFreezeDurationLevel = 0;

	// Ice Spike upgrades
	public int iceSpikeDamageLevel = 0;
	public int iceSpikeFreezeDurationLevel = 0;
	public int iceSpikeSizeLevel = 0;

	// Multishot upgrades
	public int multishotCountLevel = 0;
	public int multishotChanceLevel = 0;

	// Chance to Freeze upgrades
	public int freezeChanceLevel = 0;
	public int freezeDurationLevel = 0;
	
	
}
