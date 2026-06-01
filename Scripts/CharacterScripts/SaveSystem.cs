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
	private bool hasLoaded = false;

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

	// ── Save ──────────────────────────────────────────────────────────

	public void Save()
	{
		GameManager    gm   = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		var saveDict = new Godot.Collections.Dictionary
		{
			{ "coins", gm.coins },

			// Selected ultimate
			{ "selectedUltimate", (int)data.selectedUltimate },

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

			// Level completion
			{ "hasCompletedLevel1", data.hasCompletedLevel1 },
			{ "hasCompletedLevel2", data.hasCompletedLevel2 },
		};

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
		file.StoreString(Json.Stringify(saveDict));
		GD.Print("Game saved.");
	}

	// ── Load ──────────────────────────────────────────────────────────

	public void Load()
	{
		if (hasLoaded) return;
		hasLoaded = true;

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
		GameManager    gm   = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		if (saveDict.ContainsKey("coins")) gm.coins = saveDict["coins"].AsInt32();

		// Selected ultimate
		if (saveDict.ContainsKey("selectedUltimate"))
			data.selectedUltimate = (IceWizardStats.UltimateAbility)saveDict["selectedUltimate"].AsInt32();

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

		// Level completion
		if (saveDict.ContainsKey("hasCompletedLevel1")) data.hasCompletedLevel1 = saveDict["hasCompletedLevel1"].AsBool();
		if (saveDict.ContainsKey("hasCompletedLevel2")) data.hasCompletedLevel2 = saveDict["hasCompletedLevel2"].AsBool();

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
		GameManager    gm   = GetNode<GameManager>("/root/GameManager");
		PlayerSaveData data = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		gm.coins         = 0;
		gm.score         = 0;
		gm.xp            = 0;
		gm.level         = 1;
		gm.xpToNextLevel = 100;
		gm.gameTime      = 0f;
		gm.isDead        = false;

		data.selectedUltimate = IceWizardStats.UltimateAbility.None;
		data.hasMultiShot     = false;
		data.hasFreezeOnHit   = false;
		data.hasFrostNova     = false;
		data.hasIceSpike      = false;
		data.hasBlizzard      = false;
		data.hasFlashFreeze   = false;
		data.hasPermafrost    = false;
		data.hasBrittle       = false;
		data.hasIceShield     = false;

		data.moveSpeedBonus   = 0;
		data.healthBonus      = 0;
		data.attackSpeedBonus = 0;
		data.damageBonus      = 0;

		data.frostNovaDamageLevel         = 0;
		data.frostNovaRadiusLevel         = 0;
		data.frostNovaFreezeDurationLevel = 0;
		data.iceSpikeDamageLevel          = 0;
		data.iceSpikeFreezeDurationLevel  = 0;
		data.iceSpikeSizeLevel            = 0;
		data.multishotCountLevel          = 0;
		data.multishotChanceLevel         = 0;
		data.freezeChanceLevel            = 0;
		data.freezeDurationLevel          = 0;
		data.blizzardDurationLevel        = 0;
		data.blizzardSizeLevel            = 0;
		data.blizzardChillLevel           = 0;
		data.flashFreezeDurationLevel     = 0;
		data.flashFreezeShatterLevel      = 0;
		data.flashFreezeGlacialLevel      = 0;
		data.brittleBonusDamageLevel      = 0;
		data.brittleShatterUnlocked       = false;
		data.iceShieldBlockChanceLevel    = 0;
		data.iceShieldRetaliateUnlocked   = false;
		data.permafrostRadiusLevel        = 0;
		data.permafrostChillLevel         = 0;
		data.hasCompletedLevel1           = false;
		data.hasCompletedLevel2           = false;

		DeleteSave();
		hasLoaded = false;
		GD.Print("Game fully reset.");
	}
}
