using Godot;
using System;

public partial class IceWizardStats : CharacterStats
{
	
	//Overriding base stats for stats from CharacterStats
	public override int maxHealth { get; set;} = 10;
	public override int health { get; set; } = 10;
	public override float fireRate { get; set; } = 1.0f;
	public override float range { get; set; } = 500.0f;
	//public override float projectileSpeed { get; set; } = 300.0f;
	//public override int projectileDamage { get; set; } = 10;
	
	public override float playerSpeed { get; set; } = 100;
	
	//Used to select current ultimate when clicked in the skill tree
	public enum UltimateAbility { None, FrostNova, IceSpike }
	public UltimateAbility activeUltimate = UltimateAbility.None;
	
	//Passive and modifiers
	public bool hasMultiShot = false;
	public int multishotCount = 2; // 2 shots to start, can be upgraded, 5 max
	public float multiShotChance = .2f; //20% chance, can be upgraded
	
	//Passive and modifiers
	public bool hasFreezeOnHit = false;
	public float freezeChance = 0.15f; //15% chance, can be upgraded
	public float freezeDuration = 2.0f; //2 second freeze, can be upgraded
	
	//Passive and Modifiers
	public bool hasBrittle = false;
	
	//Passive and Modifiers
	public bool hasIceShield = false;
	
	
	
	//Passive and Modifiers
	public bool hasPermafrost = false;
	
	//Ultimate and modifiers
	public bool hasBlizzard = false;
	
	//Ultimate and Modifiers
	public bool hasFlashFreeze = false;
	
	//Ultimate and modifiers
	public bool hasFrostNova = false;
	public float frostNovaRadius = 300.0f;
	public int frostNovaDamage = 15;
	public float frostNovaFreezeDuration = 5.0f;
	
	//Ultimate and modifiers
	public bool hasIceSpike = false;
	public float iceSpikeSize = 0.5f;
	public int iceSpikeDamage = 15;
	public float iceSpikeFreezeDuration = 3.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
	
		// Apply stat bonuses
		maxHealth += saveData.healthBonus;
		health = maxHealth;
		playerSpeed += saveData.moveSpeedBonus;
		fireRate -= saveData.attackSpeedBonus;
		
		// Apply ability unlocks
		hasMultiShot = saveData.hasMultiShot;
		hasFreezeOnHit = saveData.hasFreezeOnHit;
		hasFrostNova = saveData.hasFrostNova;
		hasIceSpike = saveData.hasIceSpike;
		hasBlizzard = saveData.hasBlizzard;
		hasFlashFreeze = saveData.hasFlashFreeze;
		hasPermafrost = saveData.hasPermafrost;
		hasBrittle = saveData.hasBrittle;	
		hasIceShield = saveData.hasIceShield;
		
		activeUltimate = saveData.activeUltimate;

		// Frost Nova upgrades
		frostNovaDamage += saveData.frostNovaDamageLevel * 5;
		frostNovaRadius += saveData.frostNovaRadiusLevel * 50;
		frostNovaFreezeDuration += saveData.frostNovaFreezeDurationLevel * 1;

		// Ice Spike upgrades
		iceSpikeDamage += saveData.iceSpikeDamageLevel * 5;
		iceSpikeFreezeDuration += saveData.iceSpikeFreezeDurationLevel * 0.5f;
		iceSpikeSize += saveData.iceSpikeSizeLevel * 0.1f;

		// Multishot upgrades
		multishotCount += saveData.multishotCountLevel * 1;
		multiShotChance += saveData.multishotChanceLevel * 0.1f;

		// Chance to Freeze upgrades
		freezeChance += saveData.freezeChanceLevel * 0.05f;
		freezeDuration += saveData.freezeDurationLevel * 0.5f;
	}
	

	public override int GetUltimateChargeRequired() {
		return activeUltimate switch {
			UltimateAbility.FrostNova => 40,
			UltimateAbility.IceSpike => 25,
			_ => int.MaxValue
		};
	}

}
