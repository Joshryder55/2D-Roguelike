using Godot;

public partial class IceWizardStats : CharacterStats
{
	public override int maxHealth { get; set; } = 60;
	public override int health { get; set; } = 60;
	public override float fireRate { get; set; } = 1.0f;
	public override float range { get; set; } = 500.0f;
	public override float playerSpeed { get; set; } = 100;

	public enum UltimateAbility { None, FrostNova, IceSpike, Blizzard, FlashFreeze }
	public UltimateAbility activeUltimate = UltimateAbility.None;

	// Multishot
	public bool hasMultiShot = false;
	public int multishotCount = 2;
	public float multiShotChance = .2f;

	// Freeze on hit
	public bool hasFreezeOnHit = false;
	public float freezeChance = 0.15f;
	public float freezeDuration = 2.0f;

	// Passives
	public bool hasBrittle = false;
	public float brittleBonusDamageMultiplier = 1.5f;
	public bool brittleShatter = false;

	public bool hasIceShield = false;
	public float iceShieldBlockChance = 0.25f;
	public bool iceShieldRetaliate = false;
	public int iceShieldRetaliationDamage = 10;

	public bool hasPermafrost = false;
	public float permafrostRadius = 150.0f;
	public float permafrostSlowFactor = 0.75f;

	// Blizzard
	public bool hasBlizzard = false;
	public float blizzardDuration = 2.0f;
	public float blizzardRadius = 150.0f;
	public float blizzardSlowFactor = 0.4f;

	// Flash Freeze
	public bool hasFlashFreeze = false;
	public float flashFreezeDuration = 3.0f;
	public float flashFreezeShatterBonus = 0.0f;
	public float flashFreezeGlacialDuration = 0.0f;

	// Frost Nova
	public bool hasFrostNova = false;
	public float frostNovaRadius = 300.0f;
	public int frostNovaDamage = 15;
	public float frostNovaFreezeDuration = 5.0f;

	// Ice Spike
	public bool hasIceSpike = false;
	public float iceSpikeSize = 0.5f;
	public int iceSpikeDamage = 15;
	public float iceSpikeFreezeDuration = 3.0f;

	public override void _Ready()
	{
		base._Ready();

		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		maxHealth += saveData.healthBonus;
		health = maxHealth;
		playerSpeed += saveData.moveSpeedBonus;
		fireRate -= saveData.attackSpeedBonus;
		damageBonus = saveData.damageBonus;

		hasMultiShot   = saveData.hasMultiShot;
		hasFreezeOnHit = saveData.hasFreezeOnHit;
		hasFrostNova   = saveData.hasFrostNova;
		hasIceSpike    = saveData.hasIceSpike;
		hasBlizzard    = saveData.hasBlizzard;
		hasFlashFreeze = saveData.hasFlashFreeze;
		hasPermafrost  = saveData.hasPermafrost;
		hasBrittle     = saveData.hasBrittle;
		hasIceShield   = saveData.hasIceShield;

		// Load the player's selected ultimate
		activeUltimate = saveData.selectedUltimate;

		// Frost Nova upgrades
		frostNovaDamage         += saveData.frostNovaDamageLevel * 5;
		frostNovaRadius         += saveData.frostNovaRadiusLevel * 50;
		frostNovaFreezeDuration += saveData.frostNovaFreezeDurationLevel * 1;

		// Ice Spike upgrades
		iceSpikeDamage         += saveData.iceSpikeDamageLevel * 5;
		iceSpikeFreezeDuration += saveData.iceSpikeFreezeDurationLevel * 0.5f;
		iceSpikeSize           += saveData.iceSpikeSizeLevel * 0.1f;

		// Multishot upgrades
		multishotCount  += saveData.multishotCountLevel * 1;
		multiShotChance += saveData.multishotChanceLevel * 0.1f;

		// Freeze on hit upgrades
		freezeChance   += saveData.freezeChanceLevel * 0.05f;
		freezeDuration += saveData.freezeDurationLevel * 0.5f;

		// Blizzard upgrades
		blizzardDuration   += saveData.blizzardDurationLevel * 0.6f;
		blizzardRadius     += saveData.blizzardSizeLevel * 50f;
		blizzardSlowFactor  = Mathf.Max(0.15f, blizzardSlowFactor - saveData.blizzardChillLevel * 0.05f);

		// Flash Freeze upgrades
		flashFreezeDuration        += saveData.flashFreezeDurationLevel * 1f;
		flashFreezeShatterBonus    += saveData.flashFreezeShatterLevel * 0.1f;
		flashFreezeGlacialDuration += saveData.flashFreezeGlacialLevel * 1f;

		// Brittle upgrades
		brittleBonusDamageMultiplier += saveData.brittleBonusDamageLevel * 0.15f;
		brittleShatter                = saveData.brittleShatterUnlocked;

		// Ice Shield upgrades
		iceShieldBlockChance += saveData.iceShieldBlockChanceLevel * 0.1f;
		iceShieldRetaliate    = saveData.iceShieldRetaliateUnlocked;

		// Permafrost upgrades
		permafrostRadius     += saveData.permafrostRadiusLevel * 25f;
		permafrostSlowFactor  = Mathf.Max(0.4f, permafrostSlowFactor - saveData.permafrostChillLevel * 0.05f);
	}

	public override int GetUltimateChargeRequired()
	{
		return activeUltimate switch {
			UltimateAbility.FrostNova   => 40,
			UltimateAbility.IceSpike    => 25,
			UltimateAbility.Blizzard    => 50,
			UltimateAbility.FlashFreeze => 60,
			_ => int.MaxValue
		};
	}
}
