using Godot;

/// <summary>
/// SaveSystem — Autoload singleton.
/// Saves and loads coins and PlayerSaveData to disk.
/// 
/// SETUP: Project → Project Settings → Autoload
///        Path: res://Scripts/CharacterScripts/SaveSystem.cs   Name: SaveSystem
/// </summary>
public partial class SaveSystem : Node
{
	private const string SavePath = "user://savegame.json";

	public static SaveSystem Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
			Save();
	}

	// ── Save ─────────────────────────────────────────────────────────

	public void Save()
	{
		GameManager gm      = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		var saveDict = new Godot.Collections.Dictionary
		{
			// Meta
			{ "coins",             gm.coins },
			{ "selectedCharacter", (int)data.selectedCharacter },

			// ── Ice Wizard unlocks ──
			{ "hasMultiShot",   data.hasMultiShot },
			{ "hasFreezeOnHit", data.hasFreezeOnHit },
			{ "hasFrostNova",   data.hasFrostNova },
			{ "hasIceSpike",    data.hasIceSpike },
			{ "hasBlizzard",    data.hasBlizzard },
			{ "hasFlashFreeze", data.hasFlashFreeze },
			{ "hasPermafrost",  data.hasPermafrost },
			{ "hasBrittle",     data.hasBrittle },
			{ "hasIceShield",   data.hasIceShield },
			{ "activeUltimate", (int)data.activeUltimate },

			// Shared stat bonuses
			{ "moveSpeedBonus",   data.moveSpeedBonus },
			{ "healthBonus",      data.healthBonus },
			{ "attackSpeedBonus", data.attackSpeedBonus },
			{ "damageBonus",      data.damageBonus },

			// Frost Nova upgrades
			{ "frostNovaDamageLevel",         data.frostNovaDamageLevel },
			{ "frostNovaRadiusLevel",         data.frostNovaRadiusLevel },
			{ "frostNovaFreezeDurationLevel", data.frostNovaFreezeDurationLevel },

			// Ice Spike upgrades
			{ "iceSpikeDamageLevel",         data.iceSpikeDamageLevel },
			{ "iceSpikeFreezeDurationLevel", data.iceSpikeFreezeDurationLevel },
			{ "iceSpikeSizeLevel",           data.iceSpikeSizeLevel },

			// Multishot upgrades
			{ "multishotCountLevel",  data.multishotCountLevel },
			{ "multishotChanceLevel", data.multishotChanceLevel },

			// Freeze on hit upgrades
			{ "freezeChanceLevel",   data.freezeChanceLevel },
			{ "freezeDurationLevel", data.freezeDurationLevel },

			// Blizzard upgrades
			{ "blizzardDurationLevel", data.blizzardDurationLevel },
			{ "blizzardSizeLevel",     data.blizzardSizeLevel },
			{ "blizzardChillLevel",    data.blizzardChillLevel },

			// Flash Freeze upgrades
			{ "flashFreezeDurationLevel", data.flashFreezeDurationLevel },
			{ "flashFreezeShatterLevel",  data.flashFreezeShatterLevel },
			{ "flashFreezeGlacialLevel",  data.flashFreezeGlacialLevel },

			// Brittle upgrades
			{ "brittleBonusDamageLevel", data.brittleBonusDamageLevel },
			{ "brittleShatterUnlocked",  data.brittleShatterUnlocked },

			// Ice Shield upgrades
			{ "iceShieldBlockChanceLevel",  data.iceShieldBlockChanceLevel },
			{ "iceShieldRetaliateUnlocked", data.iceShieldRetaliateUnlocked },

			// Permafrost upgrades
			{ "permafrostRadiusLevel", data.permafrostRadiusLevel },
			{ "permafrostChillLevel",  data.permafrostChillLevel },

			// ── Fire Wizard unlocks ──
			{ "activeFireUltimate", (int)data.activeFireUltimate },
			{ "hasFireMultiShot",  data.hasFireMultiShot },
			{ "hasIgnite",         data.hasIgnite },
			{ "hasCombustion",     data.hasCombustion },
			{ "hasPyromaniac",     data.hasPyromaniac },
			{ "hasCauterize",      data.hasCauterize },
			{ "hasMoltenShield",   data.hasMoltenShield },
			{ "hasInferno",        data.hasInferno },
			{ "hasMeteor",         data.hasMeteor },
			{ "hasFireNova",       data.hasFireNova },
			{ "hasFlameDash",      data.hasFlameDash },

			// Inferno upgrades
			{ "infernoDurationLevel", data.infernoDurationLevel },
			{ "infernoSizeLevel",     data.infernoSizeLevel },

			// Meteor upgrades
			{ "meteorDamageLevel",      data.meteorDamageLevel },
			{ "meteorBlastRadiusLevel", data.meteorBlastRadiusLevel },

			// Fire Nova upgrades
			{ "fireNovaDamageLevel",       data.fireNovaDamageLevel },
			{ "fireNovaBurnDurationLevel", data.fireNovaBurnDurationLevel },

			// Flame Dash upgrades
			{ "flameDashRangeLevel",        data.flameDashRangeLevel },
			{ "flameDashTrailDurationLevel", data.flameDashTrailDurationLevel },

			// Ignite upgrades
			{ "igniteChanceLevel",   data.igniteChanceLevel },
			{ "igniteDurationLevel", data.igniteDurationLevel },

			// Fire Multishot upgrades
			{ "fireMultishotCountLevel",  data.fireMultishotCountLevel },
			{ "fireMultishotChanceLevel", data.fireMultishotChanceLevel },

			// Combustion upgrades
			{ "combustionChanceLevel", data.combustionChanceLevel },
			{ "combustionRadiusLevel", data.combustionRadiusLevel },

			// Pyromaniac upgrades
			{ "pyromaniacStackCountLevel",    data.pyromaniacStackCountLevel },
			{ "pyromaniacDamagePerStackLevel", data.pyromaniacDamagePerStackLevel },

			// Cauterize upgrades
			{ "cauterizeChanceLevel",    data.cauterizeChanceLevel },
			{ "cauterizeHealAmountLevel", data.cauterizeHealAmountLevel },

			// Molten Shield upgrades
			{ "moltenShieldBlockChanceLevel",    data.moltenShieldBlockChanceLevel },
			{ "moltenShieldExplosionDamageLevel", data.moltenShieldExplosionDamageLevel },
		};

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
		file.StoreString(Json.Stringify(saveDict));
		GD.Print("Game saved.");
	}

	// ── Load ─────────────────────────────────────────────────────────

	public void Load()
	{
		if (!FileAccess.FileExists(SavePath))
		{
			GD.Print("No save file found, starting fresh.");
			return;
		}

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
		string jsonStr = file.GetAsText();

		var result = Json.ParseString(jsonStr);
		if (result.VariantType != Variant.Type.Dictionary)
		{
			GD.PrintErr("Save file corrupted.");
			return;
		}

		var saveDict = result.AsGodotDictionary();
		GameManager gm      = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		// Meta
		if (saveDict.ContainsKey("coins"))             gm.coins             = saveDict["coins"].AsInt32();
		if (saveDict.ContainsKey("selectedCharacter")) data.selectedCharacter = (PlayerSaveData.Character)saveDict["selectedCharacter"].AsInt32();

		// ── Ice Wizard unlocks ──
		if (saveDict.ContainsKey("hasMultiShot"))   data.hasMultiShot   = saveDict["hasMultiShot"].AsBool();
		if (saveDict.ContainsKey("hasFreezeOnHit")) data.hasFreezeOnHit = saveDict["hasFreezeOnHit"].AsBool();
		if (saveDict.ContainsKey("hasFrostNova"))   data.hasFrostNova   = saveDict["hasFrostNova"].AsBool();
		if (saveDict.ContainsKey("hasIceSpike"))    data.hasIceSpike    = saveDict["hasIceSpike"].AsBool();
		if (saveDict.ContainsKey("hasBlizzard"))    data.hasBlizzard    = saveDict["hasBlizzard"].AsBool();
		if (saveDict.ContainsKey("hasFlashFreeze")) data.hasFlashFreeze = saveDict["hasFlashFreeze"].AsBool();
		if (saveDict.ContainsKey("hasPermafrost"))  data.hasPermafrost  = saveDict["hasPermafrost"].AsBool();
		if (saveDict.ContainsKey("hasBrittle"))     data.hasBrittle     = saveDict["hasBrittle"].AsBool();
		if (saveDict.ContainsKey("hasIceShield"))   data.hasIceShield   = saveDict["hasIceShield"].AsBool();
		if (saveDict.ContainsKey("activeUltimate"))
			data.activeUltimate = (IceWizardStats.UltimateAbility)saveDict["activeUltimate"].AsInt32();

		// Shared stat bonuses
		if (saveDict.ContainsKey("moveSpeedBonus"))   data.moveSpeedBonus   = saveDict["moveSpeedBonus"].AsSingle();
		if (saveDict.ContainsKey("healthBonus"))      data.healthBonus      = saveDict["healthBonus"].AsInt32();
		if (saveDict.ContainsKey("attackSpeedBonus")) data.attackSpeedBonus = saveDict["attackSpeedBonus"].AsSingle();
		if (saveDict.ContainsKey("damageBonus"))      data.damageBonus      = saveDict["damageBonus"].AsInt32();

		// Frost Nova upgrades
		if (saveDict.ContainsKey("frostNovaDamageLevel"))         data.frostNovaDamageLevel         = saveDict["frostNovaDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("frostNovaRadiusLevel"))         data.frostNovaRadiusLevel         = saveDict["frostNovaRadiusLevel"].AsInt32();
		if (saveDict.ContainsKey("frostNovaFreezeDurationLevel")) data.frostNovaFreezeDurationLevel = saveDict["frostNovaFreezeDurationLevel"].AsInt32();

		// Ice Spike upgrades
		if (saveDict.ContainsKey("iceSpikeDamageLevel"))         data.iceSpikeDamageLevel         = saveDict["iceSpikeDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("iceSpikeFreezeDurationLevel")) data.iceSpikeFreezeDurationLevel = saveDict["iceSpikeFreezeDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("iceSpikeSizeLevel"))           data.iceSpikeSizeLevel           = saveDict["iceSpikeSizeLevel"].AsInt32();

		// Multishot upgrades
		if (saveDict.ContainsKey("multishotCountLevel"))  data.multishotCountLevel  = saveDict["multishotCountLevel"].AsInt32();
		if (saveDict.ContainsKey("multishotChanceLevel")) data.multishotChanceLevel = saveDict["multishotChanceLevel"].AsInt32();

		// Freeze on hit upgrades
		if (saveDict.ContainsKey("freezeChanceLevel"))   data.freezeChanceLevel   = saveDict["freezeChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("freezeDurationLevel")) data.freezeDurationLevel = saveDict["freezeDurationLevel"].AsInt32();

		// Blizzard upgrades
		if (saveDict.ContainsKey("blizzardDurationLevel")) data.blizzardDurationLevel = saveDict["blizzardDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("blizzardSizeLevel"))     data.blizzardSizeLevel     = saveDict["blizzardSizeLevel"].AsInt32();
		if (saveDict.ContainsKey("blizzardChillLevel"))    data.blizzardChillLevel    = saveDict["blizzardChillLevel"].AsInt32();

		// Flash Freeze upgrades
		if (saveDict.ContainsKey("flashFreezeDurationLevel")) data.flashFreezeDurationLevel = saveDict["flashFreezeDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("flashFreezeShatterLevel"))  data.flashFreezeShatterLevel  = saveDict["flashFreezeShatterLevel"].AsInt32();
		if (saveDict.ContainsKey("flashFreezeGlacialLevel"))  data.flashFreezeGlacialLevel  = saveDict["flashFreezeGlacialLevel"].AsInt32();

		// Brittle upgrades
		if (saveDict.ContainsKey("brittleBonusDamageLevel")) data.brittleBonusDamageLevel = saveDict["brittleBonusDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("brittleShatterUnlocked"))  data.brittleShatterUnlocked  = saveDict["brittleShatterUnlocked"].AsBool();

		// Ice Shield upgrades
		if (saveDict.ContainsKey("iceShieldBlockChanceLevel"))  data.iceShieldBlockChanceLevel  = saveDict["iceShieldBlockChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("iceShieldRetaliateUnlocked")) data.iceShieldRetaliateUnlocked = saveDict["iceShieldRetaliateUnlocked"].AsBool();

		// Permafrost upgrades
		if (saveDict.ContainsKey("permafrostRadiusLevel")) data.permafrostRadiusLevel = saveDict["permafrostRadiusLevel"].AsInt32();
		if (saveDict.ContainsKey("permafrostChillLevel"))  data.permafrostChillLevel  = saveDict["permafrostChillLevel"].AsInt32();

		// ── Fire Wizard unlocks ──
		if (saveDict.ContainsKey("activeFireUltimate"))
			data.activeFireUltimate = (PlayerSaveData.FireUltimateAbility)saveDict["activeFireUltimate"].AsInt32();
		if (saveDict.ContainsKey("hasFireMultiShot")) data.hasFireMultiShot = saveDict["hasFireMultiShot"].AsBool();
		if (saveDict.ContainsKey("hasIgnite"))        data.hasIgnite        = saveDict["hasIgnite"].AsBool();
		if (saveDict.ContainsKey("hasCombustion"))    data.hasCombustion    = saveDict["hasCombustion"].AsBool();
		if (saveDict.ContainsKey("hasPyromaniac"))    data.hasPyromaniac    = saveDict["hasPyromaniac"].AsBool();
		if (saveDict.ContainsKey("hasCauterize"))     data.hasCauterize     = saveDict["hasCauterize"].AsBool();
		if (saveDict.ContainsKey("hasMoltenShield"))  data.hasMoltenShield  = saveDict["hasMoltenShield"].AsBool();
		if (saveDict.ContainsKey("hasInferno"))       data.hasInferno       = saveDict["hasInferno"].AsBool();
		if (saveDict.ContainsKey("hasMeteor"))        data.hasMeteor        = saveDict["hasMeteor"].AsBool();
		if (saveDict.ContainsKey("hasFireNova"))      data.hasFireNova      = saveDict["hasFireNova"].AsBool();
		if (saveDict.ContainsKey("hasFlameDash"))     data.hasFlameDash     = saveDict["hasFlameDash"].AsBool();

		// Inferno upgrades
		if (saveDict.ContainsKey("infernoDurationLevel")) data.infernoDurationLevel = saveDict["infernoDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("infernoSizeLevel"))     data.infernoSizeLevel     = saveDict["infernoSizeLevel"].AsInt32();

		// Meteor upgrades
		if (saveDict.ContainsKey("meteorDamageLevel"))      data.meteorDamageLevel      = saveDict["meteorDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("meteorBlastRadiusLevel")) data.meteorBlastRadiusLevel = saveDict["meteorBlastRadiusLevel"].AsInt32();

		// Fire Nova upgrades
		if (saveDict.ContainsKey("fireNovaDamageLevel"))       data.fireNovaDamageLevel       = saveDict["fireNovaDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("fireNovaBurnDurationLevel")) data.fireNovaBurnDurationLevel = saveDict["fireNovaBurnDurationLevel"].AsInt32();

		// Flame Dash upgrades
		if (saveDict.ContainsKey("flameDashRangeLevel"))         data.flameDashRangeLevel         = saveDict["flameDashRangeLevel"].AsInt32();
		if (saveDict.ContainsKey("flameDashTrailDurationLevel")) data.flameDashTrailDurationLevel = saveDict["flameDashTrailDurationLevel"].AsInt32();

		// Ignite upgrades
		if (saveDict.ContainsKey("igniteChanceLevel"))   data.igniteChanceLevel   = saveDict["igniteChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("igniteDurationLevel")) data.igniteDurationLevel = saveDict["igniteDurationLevel"].AsInt32();

		// Fire Multishot upgrades
		if (saveDict.ContainsKey("fireMultishotCountLevel"))  data.fireMultishotCountLevel  = saveDict["fireMultishotCountLevel"].AsInt32();
		if (saveDict.ContainsKey("fireMultishotChanceLevel")) data.fireMultishotChanceLevel = saveDict["fireMultishotChanceLevel"].AsInt32();

		// Combustion upgrades
		if (saveDict.ContainsKey("combustionChanceLevel")) data.combustionChanceLevel = saveDict["combustionChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("combustionRadiusLevel")) data.combustionRadiusLevel = saveDict["combustionRadiusLevel"].AsInt32();

		// Pyromaniac upgrades
		if (saveDict.ContainsKey("pyromaniacStackCountLevel"))     data.pyromaniacStackCountLevel     = saveDict["pyromaniacStackCountLevel"].AsInt32();
		if (saveDict.ContainsKey("pyromaniacDamagePerStackLevel")) data.pyromaniacDamagePerStackLevel = saveDict["pyromaniacDamagePerStackLevel"].AsInt32();

		// Cauterize upgrades
		if (saveDict.ContainsKey("cauterizeChanceLevel"))     data.cauterizeChanceLevel     = saveDict["cauterizeChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("cauterizeHealAmountLevel")) data.cauterizeHealAmountLevel = saveDict["cauterizeHealAmountLevel"].AsInt32();

		// Molten Shield upgrades
		if (saveDict.ContainsKey("moltenShieldBlockChanceLevel"))     data.moltenShieldBlockChanceLevel     = saveDict["moltenShieldBlockChanceLevel"].AsInt32();
		if (saveDict.ContainsKey("moltenShieldExplosionDamageLevel")) data.moltenShieldExplosionDamageLevel = saveDict["moltenShieldExplosionDamageLevel"].AsInt32();

		GD.Print("Game loaded. Coins: " + gm.coins);
	}

	// ── Delete Save ───────────────────────────────────────────────────

	public void DeleteSave()
	{
		if (FileAccess.FileExists(SavePath))
			DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SavePath));
		GD.Print("Save deleted.");
	}
}
