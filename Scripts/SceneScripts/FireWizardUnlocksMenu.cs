using Godot;
using System;
using System.Collections.Generic;

public partial class FireWizardUnlocksMenu : Control
{
	public bool openedFromGame = false;
	public event Action OnClose;

	private GameManager gameManager;
	private PlayerSaveData saveData;
	private Label descriptionLabel;
	private Label coinsLabel;
	private Button backButton;
	private Button coreButton;

	// Ultimates
	private Button infernoButton;
	private Button infernoDurationButton;
	private Button infernoSizeButton;
	private Button infernoIntensityButton;

	private Button meteorButton;
	private Button meteorDamageButton;
	private Button meteorBlastRadiusButton;
	private Button meteorCraterButton;

	private Button fireNovaButton;
	private Button fireNovaDamageButton;
	private Button fireNovaBurnDurationButton;
	private Button fireNovaSpeedButton;

	private Button flameDashButton;
	private Button flameDashRangeButton;
	private Button flameDashTrailDurationButton;
	private Button flameDashTrailDamageButton;

	// Passives
	private Button igniteButton;
	private Button igniteChanceButton;
	private Button igniteDurationButton;

	private Button multishotButton;
	private Button multishotCountButton;
	private Button multishotChanceButton;

	private Button combustionButton;
	private Button combustionChanceButton;
	private Button combustionRadiusButton;

	private Button pyromaniacButton;
	private Button pyromaniacStackCountButton;
	private Button pyromaniacDamagePerStackButton;

	private Button cauterizeButton;
	private Button cauterizeChanceButton;
	private Button cauterizeHealAmountButton;

	// Core Stats
	private Button moveSpeedButton;
	private Button attackSpeedButton;
	private Button healthButton;
	private Button damageButton;

	// Respec
	private Button respecButton;
	private Label respecConfirmLabel;
	private Button respecYesButton;
	private Button respecNoButton;
	private const int RespecCost = 10;

	private Dictionary<string, int> skillLevels    = new Dictionary<string, int>();
	private Dictionary<string, int> skillMaxLevels = new Dictionary<string, int>();
	private Dictionary<string, int> skillCosts     = new Dictionary<string, int>();

	// Match Ice Wizard escalating cost system
	private static readonly HashSet<string> escalatingCostSkills =
		new HashSet<string> {
			"Move Speed", "Attack Speed", "Health", "Damage",
			"Inferno Duration", "Inferno Size", "Inferno Intensity",
			"Meteor Damage", "Meteor Blast Radius", "Meteor Crater",
			"Fire Nova Damage", "Fire Nova Burn Duration", "Fire Nova Speed",
			"Flame Dash Range", "Flame Dash Trail Duration", "Flame Dash Trail Damage",
			"Ignite Chance", "Ignite Duration",
			"Multishot Count", "Multishot Chance",
			"Combustion Chance", "Combustion Radius",
			"Pyromaniac Stack Count", "Pyromaniac Damage Per Stack",
			"Cauterize Chance", "Cauterize Heal Amount"
		};

	private bool isCoreStatSkill(string skillName)
	{
		return skillName == "Move Speed" || skillName == "Attack Speed"
			|| skillName == "Health" || skillName == "Damage";
	}

	private int getActualCost(string skillName)
	{
		int baseCost = skillCosts[skillName];
		int level    = skillLevels[skillName];

		if (isCoreStatSkill(skillName))
			return baseCost * (level + 1);

		if (escalatingCostSkills.Contains(skillName))
			return (int)(baseCost * Math.Pow(1.8, level));

		return baseCost;
	}

	private int getTotalCostForSkill(string skillName)
	{
		int level    = skillLevels[skillName];
		int baseCost = skillCosts[skillName];

		if (isCoreStatSkill(skillName))
			return baseCost * level * (level + 1) / 2;

		if (escalatingCostSkills.Contains(skillName)) {
			int total = 0;
			for (int i = 0; i < level; i++)
				total += (int)(baseCost * Math.Pow(1.8, i));
			return total;
		}

		return level * baseCost;
	}

	public override void _Ready()
	{
		gameManager      = GetNode<GameManager>("/root/GameManager");
		saveData         = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		descriptionLabel = GetNode<Label>("DescriptionLabel");
		coinsLabel       = GetNode<Label>("CoinsLabel");
		backButton       = GetNode<Button>("BackButton");
		coreButton       = GetNode<Button>("TreeArea/CoreButton");

		// Ultimates
		infernoButton          = GetNode<Button>("TreeArea/Ultimate/InfernoButton");
		infernoDurationButton  = GetNode<Button>("TreeArea/Ultimate/InfernoDuration");
		infernoSizeButton      = GetNode<Button>("TreeArea/Ultimate/InfernoSize");
		infernoIntensityButton = GetNode<Button>("TreeArea/Ultimate/InfernoIntensity");

		meteorButton            = GetNode<Button>("TreeArea/Ultimate/MeteorButton");
		meteorDamageButton      = GetNode<Button>("TreeArea/Ultimate/MeteorDamage");
		meteorBlastRadiusButton = GetNode<Button>("TreeArea/Ultimate/MeteorBlastRadius");
		meteorCraterButton      = GetNode<Button>("TreeArea/Ultimate/MeteorCrater");

		fireNovaButton             = GetNode<Button>("TreeArea/Ultimate/FireNovaButton");
		fireNovaDamageButton       = GetNode<Button>("TreeArea/Ultimate/FireNovaDamage");
		fireNovaBurnDurationButton = GetNode<Button>("TreeArea/Ultimate/FireNovaBurnDuration");
		fireNovaSpeedButton        = GetNode<Button>("TreeArea/Ultimate/FireNovaSpeed");

		flameDashButton              = GetNode<Button>("TreeArea/Ultimate/FlameDashButton");
		flameDashRangeButton         = GetNode<Button>("TreeArea/Ultimate/FlameDashRange");
		flameDashTrailDurationButton = GetNode<Button>("TreeArea/Ultimate/FlameDashTrailDuration");
		flameDashTrailDamageButton   = GetNode<Button>("TreeArea/Ultimate/FlameDashTrailDamage");

		// Passives
		igniteButton         = GetNode<Button>("TreeArea/Passive/IgniteButton");
		igniteChanceButton   = GetNode<Button>("TreeArea/Passive/IgniteChance");
		igniteDurationButton = GetNode<Button>("TreeArea/Passive/IgniteDuration");

		multishotButton       = GetNode<Button>("TreeArea/Passive/MultishotButton");
		multishotCountButton  = GetNode<Button>("TreeArea/Passive/MultishotCount");
		multishotChanceButton = GetNode<Button>("TreeArea/Passive/MultishotChance");

		combustionButton       = GetNode<Button>("TreeArea/Passive/CombustionButton");
		combustionChanceButton = GetNode<Button>("TreeArea/Passive/CombustionChance");
		combustionRadiusButton = GetNode<Button>("TreeArea/Passive/CombustionRadius");

		pyromaniacButton               = GetNode<Button>("TreeArea/Passive/PyromaniacButton");
		pyromaniacStackCountButton     = GetNode<Button>("TreeArea/Passive/PyromaniacStackCount");
		pyromaniacDamagePerStackButton = GetNode<Button>("TreeArea/Passive/PyromaniacDamagePerStack");

		cauterizeButton           = GetNode<Button>("TreeArea/Passive/CauterizeButton");
		cauterizeChanceButton     = GetNode<Button>("TreeArea/Passive/CauterizeChance");
		cauterizeHealAmountButton = GetNode<Button>("TreeArea/Passive/CauterizeHealAmount");

		// Core Stats
		moveSpeedButton   = GetNode<Button>("TreeArea/CoreStats/MoveSpeedButton");
		attackSpeedButton = GetNode<Button>("TreeArea/CoreStats/AttackSpeedButton");
		healthButton      = GetNode<Button>("TreeArea/CoreStats/HealthButton");
		damageButton      = GetNode<Button>("TreeArea/CoreStats/DamageButton");

		respecButton       = GetNode<Button>("RespecButton");
		respecYesButton    = GetNode<Button>("RespecYesButton");
		respecNoButton     = GetNode<Button>("RespecNoButton");
		respecConfirmLabel = GetNode<Label>("RespecConfirmLabel");

		respecYesButton.Visible    = false;
		respecNoButton.Visible     = false;
		respecConfirmLabel.Visible = false;

		respecButton.Pressed    += OnRespecPressed;
		respecYesButton.Pressed += OnRespecConfirmed;
		respecNoButton.Pressed  += OnRespecCancelled;

		SetupSkills();
		RestoreSkillLevels();
		ConnectButtons();
		UpdateAllButtonText();
		SetupTooltips();
		HideSubButtons();
		RestoreUnlockedSubButtons();

		descriptionLabel.Text = "Select a skill to view details.";

		if (!openedFromGame)
			GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();

		// DEBUG
		var debugButton = new Button();
		debugButton.Text = "+1000 coin DEBUG";
		debugButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomRight);
		debugButton.OffsetLeft = -160; debugButton.OffsetTop = -40;
		debugButton.OffsetRight = 0;   debugButton.OffsetBottom = 0;
		debugButton.Pressed += () => { gameManager.coins += 1000; UpdateAllButtonText(); GetNode<SaveSystem>("/root/SaveSystem").Save(); };
		AddChild(debugButton);
	}

	private void SetupSkills()
	{
		AddSkill("Inferno", 1, 16);
		AddSkill("Inferno Duration", 5, 8);
		AddSkill("Inferno Size", 5, 8);
		AddSkill("Inferno Intensity", 5, 8);

		AddSkill("Meteor", 1, 16);
		AddSkill("Meteor Damage", 5, 8);
		AddSkill("Meteor Blast Radius", 5, 8);
		AddSkill("Meteor Crater", 5, 8);

		AddSkill("Fire Nova", 1, 5);
		AddSkill("Fire Nova Damage", 5, 8);
		AddSkill("Fire Nova Burn Duration", 5, 6);
		AddSkill("Fire Nova Speed", 5, 6);

		AddSkill("Flame Dash", 1, 5);
		AddSkill("Flame Dash Range", 5, 6);
		AddSkill("Flame Dash Trail Duration", 5, 6);
		AddSkill("Flame Dash Trail Damage", 5, 6);

		AddSkill("Ignite", 1, 3);
		AddSkill("Ignite Chance", 5, 5);
		AddSkill("Ignite Duration", 5, 5);

		AddSkill("Multishot", 1, 3);
		AddSkill("Multishot Count", 5, 5);
		AddSkill("Multishot Chance", 5, 5);

		AddSkill("Combustion", 1, 7);
		AddSkill("Combustion Chance", 5, 5);
		AddSkill("Combustion Radius", 5, 5);

		AddSkill("Pyromaniac", 1, 7);
		AddSkill("Pyromaniac Stack Count", 5, 5);
		AddSkill("Pyromaniac Damage Per Stack", 5, 5);

		AddSkill("Cauterize", 1, 10);
		AddSkill("Cauterize Chance", 5, 5);
		AddSkill("Cauterize Heal Amount", 5, 5);

		AddSkill("Move Speed", 50, 5);
		AddSkill("Attack Speed", 50, 5);
		AddSkill("Health", 50, 5);
		AddSkill("Damage", 50, 5);
	}

	private void AddSkill(string skillName, int maxLevel, int coinCost)
	{
		skillLevels[skillName]    = 0;
		skillMaxLevels[skillName] = maxLevel;
		skillCosts[skillName]     = coinCost;
	}

	private void ConnectButtons()
	{
		coreButton.Pressed += OnCorePressed;
		backButton.Pressed += OnBackPressed;

		infernoButton.Pressed          += () => UpgradeSkill("Inferno");
		infernoDurationButton.Pressed  += () => UpgradeSkill("Inferno Duration");
		infernoSizeButton.Pressed      += () => UpgradeSkill("Inferno Size");
		infernoIntensityButton.Pressed += () => UpgradeSkill("Inferno Intensity");

		meteorButton.Pressed            += () => UpgradeSkill("Meteor");
		meteorDamageButton.Pressed      += () => UpgradeSkill("Meteor Damage");
		meteorBlastRadiusButton.Pressed += () => UpgradeSkill("Meteor Blast Radius");
		meteorCraterButton.Pressed      += () => UpgradeSkill("Meteor Crater");

		fireNovaButton.Pressed             += () => UpgradeSkill("Fire Nova");
		fireNovaDamageButton.Pressed       += () => UpgradeSkill("Fire Nova Damage");
		fireNovaBurnDurationButton.Pressed += () => UpgradeSkill("Fire Nova Burn Duration");
		fireNovaSpeedButton.Pressed        += () => UpgradeSkill("Fire Nova Speed");

		flameDashButton.Pressed              += () => UpgradeSkill("Flame Dash");
		flameDashRangeButton.Pressed         += () => UpgradeSkill("Flame Dash Range");
		flameDashTrailDurationButton.Pressed += () => UpgradeSkill("Flame Dash Trail Duration");
		flameDashTrailDamageButton.Pressed   += () => UpgradeSkill("Flame Dash Trail Damage");

		igniteButton.Pressed         += () => UpgradeSkill("Ignite");
		igniteChanceButton.Pressed   += () => UpgradeSkill("Ignite Chance");
		igniteDurationButton.Pressed += () => UpgradeSkill("Ignite Duration");

		multishotButton.Pressed       += () => UpgradeSkill("Multishot");
		multishotCountButton.Pressed  += () => UpgradeSkill("Multishot Count");
		multishotChanceButton.Pressed += () => UpgradeSkill("Multishot Chance");

		combustionButton.Pressed       += () => UpgradeSkill("Combustion");
		combustionChanceButton.Pressed += () => UpgradeSkill("Combustion Chance");
		combustionRadiusButton.Pressed += () => UpgradeSkill("Combustion Radius");

		pyromaniacButton.Pressed               += () => UpgradeSkill("Pyromaniac");
		pyromaniacStackCountButton.Pressed     += () => UpgradeSkill("Pyromaniac Stack Count");
		pyromaniacDamagePerStackButton.Pressed += () => UpgradeSkill("Pyromaniac Damage Per Stack");

		cauterizeButton.Pressed           += () => UpgradeSkill("Cauterize");
		cauterizeChanceButton.Pressed     += () => UpgradeSkill("Cauterize Chance");
		cauterizeHealAmountButton.Pressed += () => UpgradeSkill("Cauterize Heal Amount");

		moveSpeedButton.Pressed   += () => UpgradeSkill("Move Speed");
		attackSpeedButton.Pressed += () => UpgradeSkill("Attack Speed");
		healthButton.Pressed      += () => UpgradeSkill("Health");
		damageButton.Pressed      += () => UpgradeSkill("Damage");
	}

	private void UpgradeSkill(string skillName)
	{
		int currentLevel = skillLevels[skillName];
		int maxLevel     = skillMaxLevels[skillName];
		int cost         = getActualCost(skillName);

		if (currentLevel >= maxLevel) { descriptionLabel.Text = skillName + "\n\nAlready max level."; return; }
		if (gameManager.coins < cost) { descriptionLabel.Text = skillName + "\n\nNot enough coins."; return; }

		gameManager.coins -= cost;
		skillLevels[skillName]++;
		ApplySkillEffect(skillName);
		GetNode<SaveSystem>("/root/SaveSystem").Save();
		UpdateAllButtonText();
		ShowSkillDescription(skillName);
	}

	private void ApplySkillEffect(string skillName)
	{
		switch (skillName)
		{
			case "Inferno":
				saveData.hasInferno = true;
				saveData.activeFireUltimate = PlayerSaveData.FireUltimateAbility.Inferno;
				infernoDurationButton.Visible  = true;
				infernoSizeButton.Visible      = true;
				infernoIntensityButton.Visible = true;
				break;
			case "Inferno Duration":  saveData.infernoDurationLevel++;  break;
			case "Inferno Size":      saveData.infernoSizeLevel++;      break;
			case "Inferno Intensity": saveData.infernoIntensityLevel++; break;

			case "Meteor":
				saveData.hasMeteor = true;
				saveData.activeFireUltimate = PlayerSaveData.FireUltimateAbility.Meteor;
				meteorDamageButton.Visible      = true;
				meteorBlastRadiusButton.Visible = true;
				meteorCraterButton.Visible      = true;
				break;
			case "Meteor Damage":       saveData.meteorDamageLevel++;      break;
			case "Meteor Blast Radius": saveData.meteorBlastRadiusLevel++; break;
			case "Meteor Crater":       saveData.meteorCraterLevel++;      break;

			case "Fire Nova":
				saveData.hasFireNova = true;
				saveData.activeFireUltimate = PlayerSaveData.FireUltimateAbility.FireNova;
				fireNovaDamageButton.Visible       = true;
				fireNovaBurnDurationButton.Visible = true;
				fireNovaSpeedButton.Visible        = true;
				break;
			case "Fire Nova Damage":        saveData.fireNovaDamageLevel++;       break;
			case "Fire Nova Burn Duration": saveData.fireNovaBurnDurationLevel++; break;
			case "Fire Nova Speed":         saveData.fireNovaSpeedLevel++;        break;

			case "Flame Dash":
				saveData.hasFlameDash = true;
				saveData.activeFireUltimate = PlayerSaveData.FireUltimateAbility.FlameDash;
				flameDashRangeButton.Visible         = true;
				flameDashTrailDurationButton.Visible = true;
				flameDashTrailDamageButton.Visible   = true;
				break;
			case "Flame Dash Range":          saveData.flameDashRangeLevel++;         break;
			case "Flame Dash Trail Duration": saveData.flameDashTrailDurationLevel++; break;
			case "Flame Dash Trail Damage":   saveData.flameDashTrailDamageLevel++;   break;

			case "Ignite":
				saveData.hasIgnite = true;
				igniteChanceButton.Visible   = true;
				igniteDurationButton.Visible = true;
				break;
			case "Ignite Chance":   saveData.igniteChanceLevel++;   break;
			case "Ignite Duration": saveData.igniteDurationLevel++; break;

			case "Multishot":
				saveData.hasFireMultiShot = true;
				multishotCountButton.Visible  = true;
				multishotChanceButton.Visible = true;
				break;
			case "Multishot Count":  saveData.fireMultishotCountLevel++;  break;
			case "Multishot Chance": saveData.fireMultishotChanceLevel++; break;

			case "Combustion":
				saveData.hasCombustion = true;
				combustionChanceButton.Visible = true;
				combustionRadiusButton.Visible = true;
				break;
			case "Combustion Chance": saveData.combustionChanceLevel++; break;
			case "Combustion Radius": saveData.combustionRadiusLevel++; break;

			case "Pyromaniac":
				saveData.hasPyromaniac = true;
				pyromaniacStackCountButton.Visible     = true;
				pyromaniacDamagePerStackButton.Visible = true;
				break;
			case "Pyromaniac Stack Count":      saveData.pyromaniacStackCountLevel++;     break;
			case "Pyromaniac Damage Per Stack": saveData.pyromaniacDamagePerStackLevel++; break;

			case "Cauterize":
				saveData.hasCauterize = true;
				cauterizeChanceButton.Visible     = true;
				cauterizeHealAmountButton.Visible = true;
				break;
			case "Cauterize Chance":     saveData.cauterizeChanceLevel++;     break;
			case "Cauterize Heal Amount": saveData.cauterizeHealAmountLevel++; break;

			case "Move Speed":   saveData.moveSpeedBonus   += 5f;    break;
			case "Health":       saveData.healthBonus       += 1;     break;
			case "Attack Speed": saveData.attackSpeedBonus  += 0.02f; break;
			case "Damage":       saveData.damageBonus       += 2;     break;
		}
	}

	private void ShowSkillDescription(string skillName)
	{
		descriptionLabel.Text =
			skillName +
			"\nLevel: " + skillLevels[skillName] + " / " + skillMaxLevels[skillName] +
			"\nCost: " + getActualCost(skillName) + " coins" +
			"\n\n" + GetSkillDescription(skillName);
	}

	private string GetSkillDescription(string skillName)
	{
		switch (skillName)
		{
			case "Inferno":           return "Ultimate. Place a persistent AOE firestorm at the cursor that damages and burns enemies inside.";
			case "Inferno Duration":  return "Increases Inferno duration by 0.6 seconds.";
			case "Inferno Size":      return "Increases Inferno radius by 50.";
			case "Inferno Intensity": return "Increases Inferno damage per tick.";

			case "Meteor":              return "Ultimate. Call down a massive explosion at the cursor dealing heavy damage in a blast radius.";
			case "Meteor Damage":       return "Increases Meteor damage by 5.";
			case "Meteor Blast Radius": return "Increases Meteor blast radius by 50.";
			case "Meteor Crater":       return "Leaves a burning crater on impact that damages enemies who walk through it.";

			case "Fire Nova":               return "Ultimate. Release a wave of fire that damages and burns enemies.";
			case "Fire Nova Damage":        return "Increases Fire Nova damage by 5.";
			case "Fire Nova Burn Duration": return "Increases Fire Nova burn duration by 1 second.";
			case "Fire Nova Speed":         return "Increases Fire Nova wave travel speed.";

			case "Flame Dash":               return "Ultimate. Dash through enemies leaving a trail of fire.";
			case "Flame Dash Range":         return "Increases Flame Dash range by 50.";
			case "Flame Dash Trail Duration": return "Increases the fire trail duration by 1 second.";
			case "Flame Dash Trail Damage":  return "Increases damage dealt by the fire trail.";

			case "Ignite":          return "Passive. Chance to set enemies on fire dealing damage over time.";
			case "Ignite Chance":   return "Increases Ignite chance by 5%.";
			case "Ignite Duration": return "Increases Ignite burn duration by 0.5 seconds.";

			case "Multishot":        return "Passive. Adds extra projectiles to each attack.";
			case "Multishot Count":  return "Adds 1 additional projectile per attack.";
			case "Multishot Chance": return "Increases multishot trigger chance by 10%.";

			case "Combustion":        return "Passive. Chance on enemy death to explode leaving a flame radius that burns enemies.";
			case "Combustion Chance": return "Increases Combustion trigger chance by 5%.";
			case "Combustion Radius": return "Increases Combustion explosion radius by 25.";

			case "Pyromaniac":                  return "Passive. Consecutive hits on the same burning enemy deal escalating damage up to 5 stacks.";
			case "Pyromaniac Stack Count":      return "Increases maximum stack count by 1.";
			case "Pyromaniac Damage Per Stack": return "Increases damage bonus per stack by 5%.";

			case "Cauterize":            return "Passive. Small chance on kill to restore a small amount of health.";
			case "Cauterize Chance":     return "Increases Cauterize proc chance by 1%.";
			case "Cauterize Heal Amount": return "Increases health restored per proc.";

			case "Move Speed":   return "Permanently increases movement speed by 5.";
			case "Attack Speed": return "Permanently increases attack speed by 2%.";
			case "Health":       return "Permanently increases maximum health by 1.";
			case "Damage":       return "Permanently increases attack damage by 2.";

			default: return "No description yet.";
		}
	}

	private void SetupTooltips()
	{
		infernoButton.TooltipText          = GetSkillDescription("Inferno");
		infernoDurationButton.TooltipText  = GetSkillDescription("Inferno Duration");
		infernoSizeButton.TooltipText      = GetSkillDescription("Inferno Size");
		infernoIntensityButton.TooltipText = GetSkillDescription("Inferno Intensity");

		meteorButton.TooltipText            = GetSkillDescription("Meteor");
		meteorDamageButton.TooltipText      = GetSkillDescription("Meteor Damage");
		meteorBlastRadiusButton.TooltipText = GetSkillDescription("Meteor Blast Radius");
		meteorCraterButton.TooltipText      = GetSkillDescription("Meteor Crater");

		fireNovaButton.TooltipText             = GetSkillDescription("Fire Nova");
		fireNovaDamageButton.TooltipText       = GetSkillDescription("Fire Nova Damage");
		fireNovaBurnDurationButton.TooltipText = GetSkillDescription("Fire Nova Burn Duration");
		fireNovaSpeedButton.TooltipText        = GetSkillDescription("Fire Nova Speed");

		flameDashButton.TooltipText              = GetSkillDescription("Flame Dash");
		flameDashRangeButton.TooltipText         = GetSkillDescription("Flame Dash Range");
		flameDashTrailDurationButton.TooltipText = GetSkillDescription("Flame Dash Trail Duration");
		flameDashTrailDamageButton.TooltipText   = GetSkillDescription("Flame Dash Trail Damage");

		igniteButton.TooltipText         = GetSkillDescription("Ignite");
		igniteChanceButton.TooltipText   = GetSkillDescription("Ignite Chance");
		igniteDurationButton.TooltipText = GetSkillDescription("Ignite Duration");

		multishotButton.TooltipText       = GetSkillDescription("Multishot");
		multishotCountButton.TooltipText  = GetSkillDescription("Multishot Count");
		multishotChanceButton.TooltipText = GetSkillDescription("Multishot Chance");

		combustionButton.TooltipText       = GetSkillDescription("Combustion");
		combustionChanceButton.TooltipText = GetSkillDescription("Combustion Chance");
		combustionRadiusButton.TooltipText = GetSkillDescription("Combustion Radius");

		pyromaniacButton.TooltipText               = GetSkillDescription("Pyromaniac");
		pyromaniacStackCountButton.TooltipText     = GetSkillDescription("Pyromaniac Stack Count");
		pyromaniacDamagePerStackButton.TooltipText = GetSkillDescription("Pyromaniac Damage Per Stack");

		cauterizeButton.TooltipText           = GetSkillDescription("Cauterize");
		cauterizeChanceButton.TooltipText     = GetSkillDescription("Cauterize Chance");
		cauterizeHealAmountButton.TooltipText = GetSkillDescription("Cauterize Heal Amount");

		moveSpeedButton.TooltipText   = GetSkillDescription("Move Speed");
		attackSpeedButton.TooltipText = GetSkillDescription("Attack Speed");
		healthButton.TooltipText      = GetSkillDescription("Health");
		damageButton.TooltipText      = GetSkillDescription("Damage");
	}

	private void HideSubButtons()
	{
		infernoDurationButton.Visible  = false;
		infernoSizeButton.Visible      = false;
		infernoIntensityButton.Visible = false;

		meteorDamageButton.Visible      = false;
		meteorBlastRadiusButton.Visible = false;
		meteorCraterButton.Visible      = false;

		fireNovaDamageButton.Visible       = false;
		fireNovaBurnDurationButton.Visible = false;
		fireNovaSpeedButton.Visible        = false;

		flameDashRangeButton.Visible         = false;
		flameDashTrailDurationButton.Visible = false;
		flameDashTrailDamageButton.Visible   = false;

		igniteChanceButton.Visible   = false;
		igniteDurationButton.Visible = false;

		multishotCountButton.Visible  = false;
		multishotChanceButton.Visible = false;

		combustionChanceButton.Visible = false;
		combustionRadiusButton.Visible = false;

		pyromaniacStackCountButton.Visible     = false;
		pyromaniacDamagePerStackButton.Visible = false;

		cauterizeChanceButton.Visible     = false;
		cauterizeHealAmountButton.Visible = false;
	}

	private void RestoreUnlockedSubButtons()
	{
		if (saveData.hasInferno) {
			infernoDurationButton.Visible  = true;
			infernoSizeButton.Visible      = true;
			infernoIntensityButton.Visible = true;
		}
		if (saveData.hasMeteor) {
			meteorDamageButton.Visible      = true;
			meteorBlastRadiusButton.Visible = true;
			meteorCraterButton.Visible      = true;
		}
		if (saveData.hasFireNova) {
			fireNovaDamageButton.Visible       = true;
			fireNovaBurnDurationButton.Visible = true;
			fireNovaSpeedButton.Visible        = true;
		}
		if (saveData.hasFlameDash) {
			flameDashRangeButton.Visible         = true;
			flameDashTrailDurationButton.Visible = true;
			flameDashTrailDamageButton.Visible   = true;
		}
		if (saveData.hasIgnite) {
			igniteChanceButton.Visible   = true;
			igniteDurationButton.Visible = true;
		}
		if (saveData.hasFireMultiShot) {
			multishotCountButton.Visible  = true;
			multishotChanceButton.Visible = true;
		}
		if (saveData.hasCombustion) {
			combustionChanceButton.Visible = true;
			combustionRadiusButton.Visible = true;
		}
		if (saveData.hasPyromaniac) {
			pyromaniacStackCountButton.Visible     = true;
			pyromaniacDamagePerStackButton.Visible = true;
		}
		if (saveData.hasCauterize) {
			cauterizeChanceButton.Visible     = true;
			cauterizeHealAmountButton.Visible = true;
		}
	}

	private void UpdateAllButtonText()
	{
		UpdateCoinDisplay();

		infernoButton.Text          = GetSkillButtonText("Inferno");
		infernoDurationButton.Text  = GetSkillButtonText("Inferno Duration");
		infernoSizeButton.Text      = GetSkillButtonText("Inferno Size");
		infernoIntensityButton.Text = GetSkillButtonText("Inferno Intensity");

		meteorButton.Text            = GetSkillButtonText("Meteor");
		meteorDamageButton.Text      = GetSkillButtonText("Meteor Damage");
		meteorBlastRadiusButton.Text = GetSkillButtonText("Meteor Blast Radius");
		meteorCraterButton.Text      = GetSkillButtonText("Meteor Crater");

		fireNovaButton.Text             = GetSkillButtonText("Fire Nova");
		fireNovaDamageButton.Text       = GetSkillButtonText("Fire Nova Damage");
		fireNovaBurnDurationButton.Text = GetSkillButtonText("Fire Nova Burn Duration");
		fireNovaSpeedButton.Text        = GetSkillButtonText("Fire Nova Speed");

		flameDashButton.Text              = GetSkillButtonText("Flame Dash");
		flameDashRangeButton.Text         = GetSkillButtonText("Flame Dash Range");
		flameDashTrailDurationButton.Text = GetSkillButtonText("Flame Dash Trail Duration");
		flameDashTrailDamageButton.Text   = GetSkillButtonText("Flame Dash Trail Damage");

		igniteButton.Text         = GetSkillButtonText("Ignite");
		igniteChanceButton.Text   = GetSkillButtonText("Ignite Chance");
		igniteDurationButton.Text = GetSkillButtonText("Ignite Duration");

		multishotButton.Text       = GetSkillButtonText("Multishot");
		multishotCountButton.Text  = GetSkillButtonText("Multishot Count");
		multishotChanceButton.Text = GetSkillButtonText("Multishot Chance");

		combustionButton.Text       = GetSkillButtonText("Combustion");
		combustionChanceButton.Text = GetSkillButtonText("Combustion Chance");
		combustionRadiusButton.Text = GetSkillButtonText("Combustion Radius");

		pyromaniacButton.Text               = GetSkillButtonText("Pyromaniac");
		pyromaniacStackCountButton.Text     = GetSkillButtonText("Pyromaniac Stack Count");
		pyromaniacDamagePerStackButton.Text = GetSkillButtonText("Pyromaniac Damage Per Stack");

		cauterizeButton.Text           = GetSkillButtonText("Cauterize");
		cauterizeChanceButton.Text     = GetSkillButtonText("Cauterize Chance");
		cauterizeHealAmountButton.Text = GetSkillButtonText("Cauterize Heal Amount");

		moveSpeedButton.Text   = GetSkillButtonText("Move Speed");
		attackSpeedButton.Text = GetSkillButtonText("Attack Speed");
		healthButton.Text      = GetSkillButtonText("Health");
		damageButton.Text      = GetSkillButtonText("Damage");
	}

	private string GetSkillButtonText(string skillName)
	{
		int level    = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost     = getActualCost(skillName);
		if (level >= maxLevel) return skillName + "\nMAX";
		return skillName + "\nLv." + level + " | " + cost + "g";
	}

	private void UpdateCoinDisplay()
	{
		coinsLabel.Text = "Coins: " + gameManager.coins;
	}

	private void RestoreSkillLevels()
	{
		if (saveData.hasInferno)  skillLevels["Inferno"] = 1;
		skillLevels["Inferno Duration"]  = saveData.infernoDurationLevel;
		skillLevels["Inferno Size"]      = saveData.infernoSizeLevel;
		skillLevels["Inferno Intensity"] = saveData.infernoIntensityLevel;

		if (saveData.hasMeteor)   skillLevels["Meteor"] = 1;
		skillLevels["Meteor Damage"]       = saveData.meteorDamageLevel;
		skillLevels["Meteor Blast Radius"] = saveData.meteorBlastRadiusLevel;
		skillLevels["Meteor Crater"]       = saveData.meteorCraterLevel;

		if (saveData.hasFireNova) skillLevels["Fire Nova"] = 1;
		skillLevels["Fire Nova Damage"]        = saveData.fireNovaDamageLevel;
		skillLevels["Fire Nova Burn Duration"] = saveData.fireNovaBurnDurationLevel;
		skillLevels["Fire Nova Speed"]         = saveData.fireNovaSpeedLevel;

		if (saveData.hasFlameDash) skillLevels["Flame Dash"] = 1;
		skillLevels["Flame Dash Range"]          = saveData.flameDashRangeLevel;
		skillLevels["Flame Dash Trail Duration"] = saveData.flameDashTrailDurationLevel;
		skillLevels["Flame Dash Trail Damage"]   = saveData.flameDashTrailDamageLevel;

		if (saveData.hasIgnite)       skillLevels["Ignite"] = 1;
		skillLevels["Ignite Chance"]   = saveData.igniteChanceLevel;
		skillLevels["Ignite Duration"] = saveData.igniteDurationLevel;

		if (saveData.hasFireMultiShot) skillLevels["Multishot"] = 1;
		skillLevels["Multishot Count"]  = saveData.fireMultishotCountLevel;
		skillLevels["Multishot Chance"] = saveData.fireMultishotChanceLevel;

		if (saveData.hasCombustion)   skillLevels["Combustion"] = 1;
		skillLevels["Combustion Chance"] = saveData.combustionChanceLevel;
		skillLevels["Combustion Radius"] = saveData.combustionRadiusLevel;

		if (saveData.hasPyromaniac)   skillLevels["Pyromaniac"] = 1;
		skillLevels["Pyromaniac Stack Count"]     = saveData.pyromaniacStackCountLevel;
		skillLevels["Pyromaniac Damage Per Stack"] = saveData.pyromaniacDamagePerStackLevel;

		if (saveData.hasCauterize)    skillLevels["Cauterize"] = 1;
		skillLevels["Cauterize Chance"]      = saveData.cauterizeChanceLevel;
		skillLevels["Cauterize Heal Amount"] = saveData.cauterizeHealAmountLevel;

		skillLevels["Move Speed"]   = (int)(saveData.moveSpeedBonus / 5f);
		skillLevels["Health"]       = saveData.healthBonus;
		skillLevels["Attack Speed"] = (int)(saveData.attackSpeedBonus / 0.02f);
		skillLevels["Damage"]       = saveData.damageBonus / 2;
	}

	public override void _Input(InputEvent @event)
	{
		if (openedFromGame && @event.IsActionPressed("pause_game"))
		{
			OnBackPressed();
			GetViewport().SetInputAsHandled();
		}
	}

	private void OnCorePressed()
	{
		descriptionLabel.Text =
			"Fire Wizard Core\n\nThis is the center of the Fire Wizard skill tree.\n" +
			"Upgrade connected skills to improve the class.";
	}

	private void OnBackPressed()
	{
		if (openedFromGame) { OnClose?.Invoke(); QueueFree(); }
		else GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}

	private void OnRespecPressed()
	{
		if (gameManager.coins < RespecCost) {
			descriptionLabel.Text = "Not enough coins to respec.\nCost: " + RespecCost + " coins.";
			return;
		}
		respecConfirmLabel.Text    = "Are you sure? This will reset all skills.\nCost: " + RespecCost + " coins.";
		respecConfirmLabel.Visible = true;
		respecYesButton.Visible    = true;
		respecNoButton.Visible     = true;
	}

	private void OnRespecConfirmed()
	{
		int totalSpent = 0;
		foreach (var skill in skillLevels) totalSpent += getTotalCostForSkill(skill.Key);
		int refund = totalSpent - RespecCost;
		gameManager.coins += refund;

		saveData.hasInferno = false; saveData.hasMeteor = false;
		saveData.hasFireNova = false; saveData.hasFlameDash = false;
		saveData.hasIgnite = false; saveData.hasFireMultiShot = false;
		saveData.hasCombustion = false; saveData.hasPyromaniac = false;
		saveData.hasCauterize = false;
		saveData.activeFireUltimate = PlayerSaveData.FireUltimateAbility.None;

		saveData.infernoDurationLevel = 0; saveData.infernoSizeLevel = 0; saveData.infernoIntensityLevel = 0;
		saveData.meteorDamageLevel = 0; saveData.meteorBlastRadiusLevel = 0; saveData.meteorCraterLevel = 0;
		saveData.fireNovaDamageLevel = 0; saveData.fireNovaBurnDurationLevel = 0; saveData.fireNovaSpeedLevel = 0;
		saveData.flameDashRangeLevel = 0; saveData.flameDashTrailDurationLevel = 0; saveData.flameDashTrailDamageLevel = 0;
		saveData.igniteChanceLevel = 0; saveData.igniteDurationLevel = 0;
		saveData.fireMultishotCountLevel = 0; saveData.fireMultishotChanceLevel = 0;
		saveData.combustionChanceLevel = 0; saveData.combustionRadiusLevel = 0;
		saveData.pyromaniacStackCountLevel = 0; saveData.pyromaniacDamagePerStackLevel = 0;
		saveData.cauterizeChanceLevel = 0; saveData.cauterizeHealAmountLevel = 0;
		saveData.moveSpeedBonus = 0; saveData.healthBonus = 0;
		saveData.attackSpeedBonus = 0; saveData.damageBonus = 0;

		SetupSkills(); HideSubButtons(); UpdateAllButtonText();

		respecConfirmLabel.Visible = false;
		respecYesButton.Visible    = false;
		respecNoButton.Visible     = false;
		descriptionLabel.Text = "Skills reset. " + refund + " coins refunded.";
	}

	private void OnRespecCancelled()
	{
		respecConfirmLabel.Visible = false;
		respecYesButton.Visible    = false;
		respecNoButton.Visible     = false;
		descriptionLabel.Text = "Respec cancelled.";
	}
}
