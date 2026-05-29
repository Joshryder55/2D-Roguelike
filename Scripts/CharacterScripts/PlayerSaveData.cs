using Godot;
public partial class PlayerSaveData : Node
{
	// ── Character Selection ───────────────────────────────────────────
	public enum Character { None, IceWizard, FireWizard }
	public Character selectedCharacter = Character.None;
	// Which wizards have been permanently unlocked
	public bool hasUnlockedIceWizard  = false;
	public bool hasUnlockedFireWizard = false;
	// ── Ice Wizard ────────────────────────────────────────────────────
	public IceWizardStats.UltimateAbility activeUltimate = IceWizardStats.UltimateAbility.None;
	public bool hasCompletedLevel1 = false;
	public bool hasMultiShot = false;
	public bool hasFreezeOnHit = false;
	public bool hasFrostNova = false;
	public bool hasIceSpike = false;
	public bool hasBlizzard = false;
	public bool hasFlashFreeze = false;
	public bool hasPermafrost = false;
	public bool hasBrittle = false;
	public bool hasIceShield = false;
	public float moveSpeedBonus = 0;
	public int healthBonus = 0;
	public float attackSpeedBonus = 0;
	public int damageBonus = 0;
	public int frostNovaDamageLevel = 0;
	public int frostNovaRadiusLevel = 0;
	public int frostNovaFreezeDurationLevel = 0;
	public int iceSpikeDamageLevel = 0;
	public int iceSpikeFreezeDurationLevel = 0;
	public int iceSpikeSizeLevel = 0;
	public int multishotCountLevel = 0;
	public int multishotChanceLevel = 0;
	public int freezeChanceLevel = 0;
	public int freezeDurationLevel = 0;
	public int blizzardDurationLevel = 0;
	public int blizzardSizeLevel = 0;
	public int blizzardChillLevel = 0;
	public int flashFreezeDurationLevel = 0;
	public int flashFreezeShatterLevel = 0;
	public int flashFreezeGlacialLevel = 0;
	public int brittleBonusDamageLevel = 0;
	public bool brittleShatterUnlocked = false;
	public int iceShieldBlockChanceLevel = 0;
	public bool iceShieldRetaliateUnlocked = false;
	public int permafrostRadiusLevel = 0;
	public int permafrostChillLevel = 0;
	// ── Fire Wizard ───────────────────────────────────────────────────
	public enum FireUltimateAbility { None, Inferno, Meteor, FireNova, FlameDash }
	public FireUltimateAbility activeFireUltimate = FireUltimateAbility.None;
	public bool hasFireMultiShot = false;
	public bool hasIgnite = false;
	public bool hasCombustion = false;
	public bool hasPyromaniac = false;
	public bool hasCauterize = false;
	public bool hasInferno = false;
	public bool hasMeteor = false;
	public bool hasFireNova = false;
	public bool hasFlameDash = false;
	// Inferno upgrades
	public int infernoDurationLevel = 0;
	public int infernoSizeLevel = 0;
	public int infernoIntensityLevel = 0;
	// Meteor upgrades
	public int meteorDamageLevel = 0;
	public int meteorBlastRadiusLevel = 0;
	public int meteorCraterLevel = 0;
	// Fire Nova upgrades
	public int fireNovaDamageLevel = 0;
	public int fireNovaBurnDurationLevel = 0;
	public int fireNovaSpeedLevel = 0;
	// Flame Dash upgrades
	public int flameDashRangeLevel = 0;
	public int flameDashTrailDurationLevel = 0;
	public int flameDashTrailDamageLevel = 0;
	// Ignite upgrades
	public int igniteChanceLevel = 0;
	public int igniteDurationLevel = 0;
	// Fire Multishot upgrades
	public int fireMultishotCountLevel = 0;
	public int fireMultishotChanceLevel = 0;
	// Combustion upgrades
	public int combustionChanceLevel = 0;
	public int combustionRadiusLevel = 0;
	// Pyromaniac upgrades
	public int pyromaniacStackCountLevel = 0;
	public int pyromaniacDamagePerStackLevel = 0;
	// Cauterize upgrades
	public int cauterizeChanceLevel = 0;
	public int cauterizeHealAmountLevel = 0;
	
}
