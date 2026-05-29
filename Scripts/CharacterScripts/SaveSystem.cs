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
			{ "coins",             gm.coins },
			{ "selectedCharacter", (int)data.selectedCharacter },
			// Wizard unlock flags
			{ "hasUnlockedIceWizard",  data.hasUnlockedIceWizard },
			{ "hasUnlockedFireWizard", data.hasUnlockedFireWizard },

			// Ice Wizard unlocks
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

			// Fire Wizard unlocks
			{ "activeFireUltimate", (int)data.activeFireUltimate },
			{ "hasFireMultiShot",  data.hasFireMultiShot },
			{ "hasIgnite",         data.hasIgnite },
			{ "hasCombustion",     data.hasCombustion },
			{ "hasPyromaniac",     data.hasPyromaniac },
			{ "hasCauterize",      data.hasCauterize },
			{ "hasInferno",        data.hasInferno },
			{ "hasMeteor",         data.hasMeteor },
			{ "hasFireNova",       data.hasFireNova },
			{ "hasFlameDash",      data.hasFlameDash },

			// Inferno upgrades
			{ "infernoDurationLevel",  data.infernoDurationLevel },
			{ "infernoSizeLevel",      data.infernoSizeLevel },
			{ "infernoIntensityLevel", data.infernoIntensityLevel },

			// Meteor upgrades
			{ "meteorDamageLevel",      data.meteorDamageLevel },
			{ "meteorBlastRadiusLevel", data.meteorBlastRadiusLevel },
			{ "meteorCraterLevel",      data.meteorCraterLevel },

			// Fire Nova upgrades
			{ "fireNovaDamageLevel",       data.fireNovaDamageLevel },
			{ "fireNovaBurnDurationLevel", data.fireNovaBurnDurationLevel },
			{ "fireNovaSpeedLevel",        data.fireNovaSpeedLevel },

			// Flame Dash upgrades
			{ "flameDashRangeLevel",         data.flameDashRangeLevel },
			{ "flameDashTrailDurationLevel", data.flameDashTrailDurationLevel },
			{ "flameDashTrailDamageLevel",   data.flameDashTrailDamageLevel },

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
			{ "pyromaniacStackCountLevel",     data.pyromaniacStackCountLevel },
			{ "pyromaniacDamagePerStackLevel", data.pyromaniacDamagePerStackLevel },

			// Cauterize upgrades
			{ "cauterizeChanceLevel",     data.cauterizeChanceLevel },
			{ "cauterizeHealAmountLevel", data.cauterizeHealAmountLevel },
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

		if (saveDict.ContainsKey("coins"))             gm.coins               = saveDict["coins"].AsInt32();
		if (saveDict.ContainsKey("selectedCharacter")) data.selectedCharacter = (PlayerSaveData.Character)saveDict["selectedCharacter"].AsInt32();
		// Wizard unlock flags
		if (saveDict.ContainsKey("hasUnlockedIceWizard"))  data.hasUnlockedIceWizard  = saveDict["hasUnlockedIceWizard"].AsBool();
		if (saveDict.ContainsKey("hasUnlockedFireWizard")) data.hasUnlockedFireWizard = saveDict["hasUnlockedFireWizard"].AsBool();

		// Ice Wizard unlocks
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

		// Fire Wizard unlocks
		if (saveDict.ContainsKey("activeFireUltimate"))
			data.activeFireUltimate = (PlayerSaveData.FireUltimateAbility)saveDict["activeFireUltimate"].AsInt32();
		if (saveDict.ContainsKey("hasFireMultiShot")) data.hasFireMultiShot = saveDict["hasFireMultiShot"].AsBool();
		if (saveDict.ContainsKey("hasIgnite"))        data.hasIgnite        = saveDict["hasIgnite"].AsBool();
		if (saveDict.ContainsKey("hasCombustion"))    data.hasCombustion    = saveDict["hasCombustion"].AsBool();
		if (saveDict.ContainsKey("hasPyromaniac"))    data.hasPyromaniac    = saveDict["hasPyromaniac"].AsBool();
		if (saveDict.ContainsKey("hasCauterize"))     data.hasCauterize     = saveDict["hasCauterize"].AsBool();
		if (saveDict.ContainsKey("hasInferno"))       data.hasInferno       = saveDict["hasInferno"].AsBool();
		if (saveDict.ContainsKey("hasMeteor"))        data.hasMeteor        = saveDict["hasMeteor"].AsBool();
		if (saveDict.ContainsKey("hasFireNova"))      data.hasFireNova      = saveDict["hasFireNova"].AsBool();
		if (saveDict.ContainsKey("hasFlameDash"))     data.hasFlameDash     = saveDict["hasFlameDash"].AsBool();

		// Inferno upgrades
		if (saveDict.ContainsKey("infernoDurationLevel"))  data.infernoDurationLevel  = saveDict["infernoDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("infernoSizeLevel"))      data.infernoSizeLevel      = saveDict["infernoSizeLevel"].AsInt32();
		if (saveDict.ContainsKey("infernoIntensityLevel")) data.infernoIntensityLevel = saveDict["infernoIntensityLevel"].AsInt32();

		// Meteor upgrades
		if (saveDict.ContainsKey("meteorDamageLevel"))      data.meteorDamageLevel      = saveDict["meteorDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("meteorBlastRadiusLevel")) data.meteorBlastRadiusLevel = saveDict["meteorBlastRadiusLevel"].AsInt32();
		if (saveDict.ContainsKey("meteorCraterLevel"))      data.meteorCraterLevel      = saveDict["meteorCraterLevel"].AsInt32();

		// Fire Nova upgrades
		if (saveDict.ContainsKey("fireNovaDamageLevel"))       data.fireNovaDamageLevel       = saveDict["fireNovaDamageLevel"].AsInt32();
		if (saveDict.ContainsKey("fireNovaBurnDurationLevel")) data.fireNovaBurnDurationLevel = saveDict["fireNovaBurnDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("fireNovaSpeedLevel"))        data.fireNovaSpeedLevel        = saveDict["fireNovaSpeedLevel"].AsInt32();

		// Flame Dash upgrades
		if (saveDict.ContainsKey("flameDashRangeLevel"))         data.flameDashRangeLevel         = saveDict["flameDashRangeLevel"].AsInt32();
		if (saveDict.ContainsKey("flameDashTrailDurationLevel")) data.flameDashTrailDurationLevel = saveDict["flameDashTrailDurationLevel"].AsInt32();
		if (saveDict.ContainsKey("flameDashTrailDamageLevel"))   data.flameDashTrailDamageLevel   = saveDict["flameDashTrailDamageLevel"].AsInt32();

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

		GD.Print("Game loaded. Coins: " + gm.coins);
	}

	// ── Delete Save ───────────────────────────────────────────────────

	public void DeleteSave()
	{
		if (FileAccess.FileExists(SavePath))
			DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SavePath));
		GD.Print("Save deleted.");
	}

	// ── Reset All Data ────────────────────────────────────────────────

	public void ResetData()
	{
		GameManager gm      = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		// GameManager
		gm.coins         = 0;
		gm.score         = 0;
		gm.xp            = 0;
		gm.level         = 1;
		gm.xpToNextLevel = 100;
		gm.gameTime      = 0f;
		gm.isDead        = false;

		// Character selection
		data.selectedCharacter     = PlayerSaveData.Character.None;
		data.hasUnlockedIceWizard  = false;
		data.hasUnlockedFireWizard = false;
		data.activeUltimate        = IceWizardStats.UltimateAbility.None;
		data.activeFireUltimate    = PlayerSaveData.FireUltimateAbility.None;

		// Ice Wizard unlocks
		data.hasMultiShot  = false; data.hasFreezeOnHit = false;
		data.hasFrostNova  = false; data.hasIceSpike    = false;
		data.hasBlizzard   = false; data.hasFlashFreeze = false;
		data.hasPermafrost = false; data.hasBrittle     = false;
		data.hasIceShield  = false;

		// Shared stat bonuses
		data.moveSpeedBonus = 0; data.healthBonus = 0;
		data.attackSpeedBonus = 0; data.damageBonus = 0;

		// Ice Wizard upgrade levels
		data.frostNovaDamageLevel = 0; data.frostNovaRadiusLevel = 0;
		data.frostNovaFreezeDurationLevel = 0;
		data.iceSpikeDamageLevel = 0; data.iceSpikeFreezeDurationLevel = 0;
		data.iceSpikeSizeLevel = 0;
		data.multishotCountLevel = 0; data.multishotChanceLevel = 0;
		data.freezeChanceLevel = 0; data.freezeDurationLevel = 0;
		data.blizzardDurationLevel = 0; data.blizzardSizeLevel = 0;
		data.blizzardChillLevel = 0;
		data.flashFreezeDurationLevel = 0; data.flashFreezeShatterLevel = 0;
		data.flashFreezeGlacialLevel = 0;
		data.brittleBonusDamageLevel = 0; data.brittleShatterUnlocked = false;
		data.iceShieldBlockChanceLevel = 0; data.iceShieldRetaliateUnlocked = false;
		data.permafrostRadiusLevel = 0; data.permafrostChillLevel = 0;

		// Fire Wizard unlocks
		data.hasFireMultiShot = false; data.hasIgnite     = false;
		data.hasCombustion    = false; data.hasPyromaniac = false;
		data.hasCauterize     = false;
		data.hasInferno = false; data.hasMeteor    = false;
		data.hasFireNova = false; data.hasFlameDash = false;

		// Fire Wizard upgrade levels
		data.infernoDurationLevel = 0; data.infernoSizeLevel = 0;
		data.infernoIntensityLevel = 0;
		data.meteorDamageLevel = 0; data.meteorBlastRadiusLevel = 0;
		data.meteorCraterLevel = 0;
		data.fireNovaDamageLevel = 0; data.fireNovaBurnDurationLevel = 0;
		data.fireNovaSpeedLevel = 0;
		data.flameDashRangeLevel = 0; data.flameDashTrailDurationLevel = 0;
		data.flameDashTrailDamageLevel = 0;
		data.igniteChanceLevel = 0; data.igniteDurationLevel = 0;
		data.fireMultishotCountLevel = 0; data.fireMultishotChanceLevel = 0;
		data.combustionChanceLevel = 0; data.combustionRadiusLevel = 0;
		data.pyromaniacStackCountLevel = 0; data.pyromaniacDamagePerStackLevel = 0;
		data.cauterizeChanceLevel = 0; data.cauterizeHealAmountLevel = 0;

		DeleteSave();
		GD.Print("Game fully reset.");
	}
}
