using Godot;

public partial class PlayerSaveData : Node
{
	public IceWizardStats.UltimateAbility selectedUltimate = IceWizardStats.UltimateAbility.None;

	// Ability unlocks
	public bool hasCompletedLevel1 = false;
	public bool hasCompletedLevel2 = false;
	public bool hasCompletedLevel3 = false;

	// Ice Wizard unlocks
	public bool hasMultiShot   = false;
	public bool hasFreezeOnHit = false;
	public bool hasFrostNova   = false;
	public bool hasIceSpike    = false;
	public bool hasBlizzard    = false;
	public bool hasFlashFreeze = false;
	public bool hasPermafrost  = false;
	public bool hasBrittle     = false;
	public bool hasIceShield   = false;

	// Shared stat bonuses
	public float moveSpeedBonus   = 0;
	public int   healthBonus      = 0;
	public float attackSpeedBonus = 0;
	public int   damageBonus      = 0;

	// Frost Nova upgrades
	public int frostNovaDamageLevel         = 0;
	public int frostNovaRadiusLevel         = 0;
	public int frostNovaFreezeDurationLevel = 0;

	// Ice Spike upgrades
	public int iceSpikeDamageLevel         = 0;
	public int iceSpikeFreezeDurationLevel = 0;
	public int iceSpikeSizeLevel           = 0;

	// Multishot upgrades
	public int multishotCountLevel  = 0;
	public int multishotChanceLevel = 0;

	// Freeze on hit upgrades
	public int freezeChanceLevel   = 0;
	public int freezeDurationLevel = 0;

	// Blizzard upgrades
	public int blizzardDurationLevel = 0;
	public int blizzardSizeLevel     = 0;
	public int blizzardChillLevel    = 0;

	// Flash Freeze upgrades
	public int flashFreezeDurationLevel = 0;
	public int flashFreezeShatterLevel  = 0;
	public int flashFreezeGlacialLevel  = 0;

	// Brittle upgrades
	public int  brittleBonusDamageLevel = 0;
	public bool brittleShatterUnlocked  = false;

	// Ice Shield upgrades
	public int  iceShieldBlockChanceLevel  = 0;
	public bool iceShieldRetaliateUnlocked = false;

	// Permafrost upgrades
	public int permafrostRadiusLevel = 0;
	public int permafrostChillLevel  = 0;
}
