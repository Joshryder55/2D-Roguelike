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
	public UltimateAbility activeUltimate = UltimateAbility.FrostNova;
	
	//Passive and modifiers
	public bool hasMultiShot = true;
	public int multishotCount = 4; // 2 shots to start, can be upgraded, 5 max
	public float multiShotChance = .99f; //20% chance, can be upgraded
	
	//Passive and modifiers
	public bool hasFreezeOnHit = false;
	public float freezeChance = 0.15f; //15% chance, can be upgraded
	public float freezeDuration = 2.0f; //2 second freeze, can be upgraded
	
	//Ultimate and modifiers
	public bool hasFrostNova = true;
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
	}
	

	public override int GetUltimateChargeRequired() {
		return activeUltimate switch {
			UltimateAbility.FrostNova => 40,
			UltimateAbility.IceSpike => 25,
			_ => int.MaxValue
		};
	}

}
