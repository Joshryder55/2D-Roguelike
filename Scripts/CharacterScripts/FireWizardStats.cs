//using Godot;
//
//
//public partial class FireWizardStats : CharacterStats
//{
	//// ── Base Stats (same as Ice Wizard) ───────────────────────────────
	//public override int maxHealth      { get; set; } = 5;
	//public override int health         { get; set; } = 5;
	//public override float moveSpeed    { get; set; } = 175f;
	//public override float fireRate     { get; set; } = 0.5f;
	//public override int damage         { get; set; } = 10;
	//public override float range        { get; set; } = 500f;
	//public override int ultimateCharge { get; set; } = 0;
//
	//// ── Active Ultimate ───────────────────────────────────────────────
	//public PlayerSaveData.FireUltimateAbility activeUltimate =
		//PlayerSaveData.FireUltimateAbility.None;
//
	//// ── Passive Flags ─────────────────────────────────────────────────
	//public bool hasIgnite        = false;
	//public bool hasFireMultiShot = false;
	//public bool hasCombustion    = false;
	//public bool hasPyromaniac    = false;
	//public bool hasCauterize     = false;
//
	//// ── Ignite ────────────────────────────────────────────────────────
	//public float igniteChance   = 0.15f; // 15% base chance
	//public float igniteDuration = 2f;    // seconds
	//public float igniteDamagePerTick = 2f;
//
	//// ── Multishot ─────────────────────────────────────────────────────
	//public float multiShotChance  = 0.3f;
	//public int   multishotCount   = 2;
//
	//// ── Combustion ────────────────────────────────────────────────────
	//public float combustionChance = 0.15f;
	//public float combustionRadius = 100f;
//
	//// ── Pyromaniac ────────────────────────────────────────────────────
	//public int   pyromaniacMaxStacks        = 5;
	//public float pyromaniacDamagePerStack   = 0.05f; // 5% per stack
//
	//// ── Cauterize ─────────────────────────────────────────────────────
	//public float cauterizeChance     = 0.01f; // 1% base
	//public int   cauterizeHealAmount = 1;
//
	//public override void _Ready()
	//{
		//base._Ready();
		//ApplySaveData();
	//}
//
	//private void ApplySaveData()
	//{
		//PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
//
		//activeUltimate   = saveData.activeFireUltimate;
		//hasIgnite        = saveData.hasIgnite;
		//hasFireMultiShot = saveData.hasFireMultiShot;
		//hasCombustion    = saveData.hasCombustion;
		//hasPyromaniac    = saveData.hasPyromaniac;
		//hasCauterize     = saveData.hasCauterize;
//
		//// Ignite upgrades
		//igniteChance   += saveData.igniteChanceLevel   * 0.05f;
		//igniteDuration += saveData.igniteDurationLevel * 0.5f;
//
		//// Multishot upgrades
		//multishotCount  += saveData.fireMultishotCountLevel;
		//multiShotChance += saveData.fireMultishotChanceLevel * 0.1f;
//
		//// Combustion upgrades
		//combustionChance += saveData.combustionChanceLevel * 0.05f;
		//combustionRadius += saveData.combustionRadiusLevel * 25f;
//
		//// Pyromaniac upgrades
		//pyromaniacMaxStacks      += saveData.pyromaniacStackCountLevel;
		//pyromaniacDamagePerStack += saveData.pyromaniacDamagePerStackLevel * 0.05f;
//
		//// Cauterize upgrades
		//cauterizeChance     += saveData.cauterizeChanceLevel * 0.01f;
		//cauterizeHealAmount += saveData.cauterizeHealAmountLevel;
//
		//// Shared stat bonuses
		//moveSpeed  += saveData.moveSpeedBonus;
		//maxHealth  += saveData.healthBonus;
		//health      = maxHealth;
		//fireRate   -= saveData.attackSpeedBonus;
		//damage     += saveData.damageBonus;
	//}
//}
