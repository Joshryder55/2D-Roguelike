using Godot;
using System;
using System.Collections.Generic;

public partial class UnlocksMenu : Control
{
	public bool openedFromGame = false;
	public event Action OnClose;
	// ─── Node References ───────────────────────────────────────────────
	private GameManager gameManager;
	private PlayerSaveData saveData;
	private Label descriptionLabel;
	private Label coinsLabel;
	private Button backButton;
	private Button coreButton;

	// Ultimates
	private Button blizzardButton;
	private Button flashFreezeButton;

	private Button frostNovaButton;
	private Button frostNovaDamageButton;
	private Button frostNovaRadiusButton;
	private Button frostNovaFreezeDurationButton;

	private Button iceSpikeButton;
	private Button iceSpikeDamageButton;
	private Button iceSpikeFreezeDurationButton;
	private Button iceSpikeSizeButton;

	// Passives
	private Button permafrostButton;
	private Button brittleButton;
	private Button iceShieldButton;

	private Button multishotButton;
	private Button multishotCountButton;
	private Button multishotChanceButton;

	private Button chanceToFreezeButton;
	private Button chanceToFreezeChanceButton;
	private Button chanceToFreezeDurationButton;

	// Core Stats
	private Button moveSpeedButton;
	private Button attackSpeedButton;
	private Button healthButton;
	private Button damageButton;
	
	
	//Respec
	private Button respecButton;
	private Label respecConfirmLabel;
	private Button respecYesButton;
	private Button respecNoButton;
	private bool isRespecConfirming = false;
	private const int RespecCost = 10;

	// ─── Skill Data ────────────────────────────────────────────────────
	private Dictionary<string, int> skillLevels = new Dictionary<string, int>();
	private Dictionary<string, int> skillMaxLevels = new Dictionary<string, int>();
	private Dictionary<string, int> skillCosts = new Dictionary<string, int>();

	// Core stats cost scaling: basically the cost scales like: baseCost * (statLevel + 1)
	private static readonly System.Collections.Generic.HashSet<string> escalatingCostSkills =
		new System.Collections.Generic.HashSet<string> { "Move Speed", "Attack Speed", "Health", "Damage" };

	private int getActualCost(string skillName)
	{
		if (escalatingCostSkills.Contains(skillName))
			return skillCosts[skillName] * (skillLevels[skillName] + 1);
		return skillCosts[skillName];
	}

	private int getTotalCostForSkill(string skillName)
	{
		int level = skillLevels[skillName];
		if (escalatingCostSkills.Contains(skillName))
		{
			int baseCost = skillCosts[skillName];
			return baseCost * level * (level + 1) / 2;
		}
		return level * skillCosts[skillName];
	}

	// ─── Ready ─────────────────────────────────────────────────────────
	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		descriptionLabel = GetNode<Label>("DescriptionLabel");
		coinsLabel = GetNode<Label>("CoinsLabel");
		backButton = GetNode<Button>("BackButton");
		coreButton = GetNode<Button>("TreeArea/CoreButton");

		// Ultimates
		blizzardButton = GetNode<Button>("TreeArea/Ultimate/BlizzardButton");
		flashFreezeButton = GetNode<Button>("TreeArea/Ultimate/FlashFreezeButton");

		frostNovaButton = GetNode<Button>("TreeArea/Ultimate/FrostNovaButton");
		frostNovaDamageButton = GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage");
		frostNovaRadiusButton = GetNode<Button>("TreeArea/Ultimate/FrostNovaSize");
		frostNovaFreezeDurationButton = GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration");

		iceSpikeButton = GetNode<Button>("TreeArea/Ultimate/IceSpikeButton");
		iceSpikeDamageButton = GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage");
		iceSpikeFreezeDurationButton = GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration");
		iceSpikeSizeButton = GetNode<Button>("TreeArea/Ultimate/IceSpikeSize");

		// Passives
		permafrostButton = GetNode<Button>("TreeArea/Passive/PermafrostButton");
		brittleButton = GetNode<Button>("TreeArea/Passive/BrittleButton");
		iceShieldButton = GetNode<Button>("TreeArea/Passive/IceShieldButton");

		multishotButton = GetNode<Button>("TreeArea/Passive/MultishotButton");
		multishotCountButton = GetNode<Button>("TreeArea/Passive/MultishotCount");
		multishotChanceButton = GetNode<Button>("TreeArea/Passive/MultishotChance");

		chanceToFreezeButton = GetNode<Button>("TreeArea/Passive/ChancetoFreezeButton");
		chanceToFreezeChanceButton = GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance");
		chanceToFreezeDurationButton = GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration");

		// Core Stats
		moveSpeedButton = GetNode<Button>("TreeArea/CoreStats/MoveSpeedButton");
		attackSpeedButton = GetNode<Button>("TreeArea/CoreStats/AttackSpeedButton");
		healthButton = GetNode<Button>("TreeArea/CoreStats/HealthButton");
		damageButton = GetNode<Button>("TreeArea/CoreStats/DamageButton");
		
		respecButton = GetNode<Button>("RespecButton");
		respecYesButton = GetNode<Button>("RespecYesButton");
		respecNoButton = GetNode<Button>("RespecNoButton");
		respecConfirmLabel = GetNode<Label>("RespecConfirmLabel");

		respecYesButton.Visible = false;
		respecNoButton.Visible = false;
		respecConfirmLabel.Visible = false;

		respecButton.Pressed += OnRespecPressed;
		respecYesButton.Pressed += OnRespecConfirmed;
		respecNoButton.Pressed += OnRespecCancelled;

		SetupSkills();
		RestoreSkillLevels();
		ConnectButtons();
		UpdateAllButtonText();
		SetupTooltips();
		HideSubButtons();
		RestoreUnlockedSubButtons();

		descriptionLabel.Text = "Select a skill to view details.";

		// Only play menu music in screens accessed from menu
		if (!openedFromGame)
			GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();

		// DEBUG - give 1000 coins - remove/disable before release ***
		var debugButton = new Button();
		debugButton.Text = "+1000 coin DEBUG";
		debugButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomRight);
		debugButton.OffsetLeft = -160;
		debugButton.OffsetTop = -40;
		debugButton.OffsetRight = 0;
		debugButton.OffsetBottom = 0;
		debugButton.Pressed += () => { gameManager.coins += 1000; UpdateAllButtonText(); };
		AddChild(debugButton);
	}

	// ─── Skill Setup ───────────────────────────────────────────────────
	private void SetupSkills()
	{
		// Ultimates
		// high-tier
		AddSkill("Blizzard", 1, 16);
		AddSkill("Flash Freeze", 1, 16);

		// low-tier
		AddSkill("Frost Nova", 1, 5);
		AddSkill("Frost Nova Damage", 5, 8);
		AddSkill("Frost Nova Radius", 5, 6);
		AddSkill("Frost Nova Freeze Duration", 5, 6);

		AddSkill("Ice Spike", 1, 3);
		AddSkill("Ice Spike Damage", 5, 8);
		AddSkill("Ice Spike Freeze Duration", 5, 6);
		AddSkill("Ice Spike Size", 5, 6);

		// Passives
		AddSkill("Permafrost", 1, 7);
		AddSkill("Brittle", 1, 7);
		AddSkill("Ice Shield", 1, 10);

		AddSkill("Multishot", 1, 3);
		AddSkill("Multishot Count", 5, 5);
		AddSkill("Multishot Chance", 5, 5);

		AddSkill("Chance to Freeze", 1, 3);
		AddSkill("Freeze Chance", 5, 5);
		AddSkill("Freeze Duration", 5, 5);

		// Core Stats — permanent minor upgrades, escalating cost per level
		AddSkill("Move Speed", 5, 5);
		AddSkill("Attack Speed", 5, 5);
		AddSkill("Health", 5, 5);
		AddSkill("Damage", 5, 5);
	}

	private void AddSkill(string skillName, int maxLevel, int coinCost)
	{
		skillLevels[skillName] = 0;
		skillMaxLevels[skillName] = maxLevel;
		skillCosts[skillName] = coinCost;
	}

	// ─── Button Connections ────────────────────────────────────────────
	private void ConnectButtons()
	{
		coreButton.Pressed += OnCorePressed;
		backButton.Pressed += OnBackPressed;

		// Ultimates
		blizzardButton.Pressed += () => UpgradeSkill("Blizzard");
		flashFreezeButton.Pressed += () => UpgradeSkill("Flash Freeze");

		frostNovaButton.Pressed += () => UpgradeSkill("Frost Nova");
		frostNovaDamageButton.Pressed += () => UpgradeSkill("Frost Nova Damage");
		frostNovaRadiusButton.Pressed += () => UpgradeSkill("Frost Nova Radius");
		frostNovaFreezeDurationButton.Pressed += () => UpgradeSkill("Frost Nova Freeze Duration");

		iceSpikeButton.Pressed += () => UpgradeSkill("Ice Spike");
		iceSpikeDamageButton.Pressed += () => UpgradeSkill("Ice Spike Damage");
		iceSpikeFreezeDurationButton.Pressed += () => UpgradeSkill("Ice Spike Freeze Duration");
		iceSpikeSizeButton.Pressed += () => UpgradeSkill("Ice Spike Size");

		// Passives
		permafrostButton.Pressed += () => UpgradeSkill("Permafrost");
		brittleButton.Pressed += () => UpgradeSkill("Brittle");
		iceShieldButton.Pressed += () => UpgradeSkill("Ice Shield");

		multishotButton.Pressed += () => UpgradeSkill("Multishot");
		multishotCountButton.Pressed += () => UpgradeSkill("Multishot Count");
		multishotChanceButton.Pressed += () => UpgradeSkill("Multishot Chance");

		chanceToFreezeButton.Pressed += () => UpgradeSkill("Chance to Freeze");
		chanceToFreezeChanceButton.Pressed += () => UpgradeSkill("Freeze Chance");
		chanceToFreezeDurationButton.Pressed += () => UpgradeSkill("Freeze Duration");

		// Core Stats
		moveSpeedButton.Pressed += () => UpgradeSkill("Move Speed");
		attackSpeedButton.Pressed += () => UpgradeSkill("Attack Speed");
		healthButton.Pressed += () => UpgradeSkill("Health");
		damageButton.Pressed += () => UpgradeSkill("Damage");
	}

	// ─── Upgrade Logic ─────────────────────────────────────────────────
	private void UpgradeSkill(string skillName)
	{
		int currentLevel = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost = getActualCost(skillName);

		if (currentLevel >= maxLevel) {
			descriptionLabel.Text = skillName + "\n\nAlready max level.";
			return;
		}

		if (gameManager.coins < cost) {
			descriptionLabel.Text = skillName + "\n\nNot enough coins.";
			return;
		}

		gameManager.coins -= cost;
		skillLevels[skillName]++;
		ApplySkillEffect(skillName);
		UpdateAllButtonText();
		ShowSkillDescription(skillName);
	}

	private void ApplySkillEffect(string skillName)
	{
		switch (skillName) {
			// Ultimates
			case "Blizzard":
				saveData.hasBlizzard = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.Blizzard;
				break;
			case "Flash Freeze":
				saveData.hasFlashFreeze = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.FlashFreeze;
				break;

			case "Frost Nova":
				saveData.hasFrostNova = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.FrostNova;
				frostNovaDamageButton.Visible = true;
				frostNovaRadiusButton.Visible = true;
				frostNovaFreezeDurationButton.Visible = true;
				break;
			case "Frost Nova Damage":
				saveData.frostNovaDamageLevel++;
				break;
			case "Frost Nova Radius":
				saveData.frostNovaRadiusLevel++;
				break;
			case "Frost Nova Freeze Duration":
				saveData.frostNovaFreezeDurationLevel++;
				break;

			case "Ice Spike":
				saveData.hasIceSpike = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.IceSpike;
				iceSpikeDamageButton.Visible = true;
				iceSpikeFreezeDurationButton.Visible = true;
				iceSpikeSizeButton.Visible = true;
				break;
			case "Ice Spike Damage":
				saveData.iceSpikeDamageLevel++;
				break;
			case "Ice Spike Freeze Duration":
				saveData.iceSpikeFreezeDurationLevel++;
				break;
			case "Ice Spike Size":
				saveData.iceSpikeSizeLevel++;
				break;

			// Passives
			case "Permafrost":
				saveData.hasPermafrost = true;
				break;
			case "Brittle":
				saveData.hasBrittle = true;
				break;
			case "Ice Shield":
				saveData.hasIceShield = true;
				break;

			case "Multishot":
				saveData.hasMultiShot = true;
				multishotCountButton.Visible = true;
				multishotChanceButton.Visible = true;
				break;
			case "Multishot Count":
				saveData.multishotCountLevel++;
				break;
			case "Multishot Chance":
				saveData.multishotChanceLevel++;
				break;

			case "Chance to Freeze":
				saveData.hasFreezeOnHit = true;
				chanceToFreezeChanceButton.Visible = true;
				chanceToFreezeDurationButton.Visible = true;
				break;
			case "Freeze Chance":
				saveData.freezeChanceLevel++;
				break;
			case "Freeze Duration":
				saveData.freezeDurationLevel++;
				break;

			// Core Stats
			case "Move Speed":
				saveData.moveSpeedBonus += 5f;
				break;
			case "Health":
				saveData.healthBonus += 1;
				break;
			case "Attack Speed":
				saveData.attackSpeedBonus += 0.02f;
				break;
			case "Damage":
				saveData.damageBonus += 2;
				break;
		}
	}

	// ─── UI Updates ────────────────────────────────────────────────────
	private void ShowSkillDescription(string skillName)
	{
		int level = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost = getActualCost(skillName);
		string description = GetSkillDescription(skillName);

		descriptionLabel.Text =
			skillName +
			"\nLevel: " + level + " / " + maxLevel +
			"\nCost: " + cost + " coins" +
			"\n\n" + description;
	}

	private string GetSkillDescription(string skillName)
	{
		switch (skillName)
		{
			// Ultimates
			case "Blizzard":
				return "Ultimate. Place a persistent AOE snowstorm that damages and slows enemies inside.";
			case "Flash Freeze":
				return "Ultimate. Instantly freezes all enemies on screen briefly.";

			case "Frost Nova":
				return "Ultimate. Release Cold Winds that damage and freeze enemies.";
			case "Frost Nova Damage":
				return "Increases Frost Nova damage by 5.";
			case "Frost Nova Radius":
				return "Increases Frost Nova radius by 50.";
			case "Frost Nova Freeze Duration":
				return "Increases Frost Nova freeze duration by 1 second.";

			case "Ice Spike":
				return "Ultimate. Release a large deadly spike that penetrates enemies.";
			case "Ice Spike Damage":
				return "Increases Ice Spike damage by 5.";
			case "Ice Spike Freeze Duration":
				return "Increases Ice Spike freeze duration by 0.5 seconds.";
			case "Ice Spike Size":
				return "Increases Ice Spike size by 10%.";

			// Passives
			case "Permafrost":
				return "Passive. A slow aura surrounds the player. Nearby enemies move slower.";
			case "Brittle":
				return "Passive. Frozen enemies take bonus damage when hit.";
			case "Ice Shield":
				return "Passive. Chance to block incoming damage when hit.";

			case "Multishot":
				return "Passive. Adds extra projectiles to each attack.";
			case "Multishot Count":
				return "Adds 1 additional projectile per attack.";
			case "Multishot Chance":
				return "Increases multishot trigger chance by 10%.";

			case "Chance to Freeze":
				return "Passive. Percent chance to freeze enemies on hit.";
			case "Freeze Chance":
				return "Increases freeze chance by 5%.";
			case "Freeze Duration":
				return "Increases freeze duration by 0.5 seconds.";

			// Core Stats
			case "Move Speed":
				return "Permanently increases movement speed by 5.";
			case "Attack Speed":
				return "Permanently increases attack speed by 2%.";
			case "Health":
				return "Permanently increases maximum health by 1.";
			case "Damage":
				return "Permanently increases attack damage by 2.";

			default:
				return "No description yet.";
		}
	}

	private void SetupTooltips()
	{
		// Ultimates
		blizzardButton.TooltipText = GetSkillDescription("Blizzard");
		flashFreezeButton.TooltipText = GetSkillDescription("Flash Freeze");

		frostNovaButton.TooltipText = GetSkillDescription("Frost Nova");
		frostNovaDamageButton.TooltipText = GetSkillDescription("Frost Nova Damage");
		frostNovaRadiusButton.TooltipText = GetSkillDescription("Frost Nova Radius");
		frostNovaFreezeDurationButton.TooltipText = GetSkillDescription("Frost Nova Freeze Duration");

		iceSpikeButton.TooltipText = GetSkillDescription("Ice Spike");
		iceSpikeDamageButton.TooltipText = GetSkillDescription("Ice Spike Damage");
		iceSpikeFreezeDurationButton.TooltipText = GetSkillDescription("Ice Spike Freeze Duration");
		iceSpikeSizeButton.TooltipText = GetSkillDescription("Ice Spike Size");

		// Passives
		permafrostButton.TooltipText = GetSkillDescription("Permafrost");
		brittleButton.TooltipText = GetSkillDescription("Brittle");
		iceShieldButton.TooltipText = GetSkillDescription("Ice Shield");

		multishotButton.TooltipText = GetSkillDescription("Multishot");
		multishotCountButton.TooltipText = GetSkillDescription("Multishot Count");
		multishotChanceButton.TooltipText = GetSkillDescription("Multishot Chance");

		chanceToFreezeButton.TooltipText = GetSkillDescription("Chance to Freeze");
		chanceToFreezeChanceButton.TooltipText = GetSkillDescription("Freeze Chance");
		chanceToFreezeDurationButton.TooltipText = GetSkillDescription("Freeze Duration");

		// Core Stats
		moveSpeedButton.TooltipText = GetSkillDescription("Move Speed");
		attackSpeedButton.TooltipText = GetSkillDescription("Attack Speed");
		healthButton.TooltipText = GetSkillDescription("Health");
		damageButton.TooltipText = GetSkillDescription("Damage");
	}

	// ─── Sub Button Visibility ─────────────────────────────────────────
	private void HideSubButtons()
	{
		frostNovaDamageButton.Visible = false;
		frostNovaRadiusButton.Visible = false;
		frostNovaFreezeDurationButton.Visible = false;

		iceSpikeDamageButton.Visible = false;
		iceSpikeFreezeDurationButton.Visible = false;
		iceSpikeSizeButton.Visible = false;

		multishotCountButton.Visible = false;
		multishotChanceButton.Visible = false;

		chanceToFreezeChanceButton.Visible = false;
		chanceToFreezeDurationButton.Visible = false;
	}

	private void RestoreUnlockedSubButtons()
	{
		if (saveData.hasFrostNova) {
			frostNovaDamageButton.Visible = true;
			frostNovaRadiusButton.Visible = true;
			frostNovaFreezeDurationButton.Visible = true;
		}

		if (saveData.hasIceSpike) {
			iceSpikeDamageButton.Visible = true;
			iceSpikeFreezeDurationButton.Visible = true;
			iceSpikeSizeButton.Visible = true;
		}

		if (saveData.hasMultiShot) {
			multishotCountButton.Visible = true;
			multishotChanceButton.Visible = true;
		}

		if (saveData.hasFreezeOnHit) {
			chanceToFreezeChanceButton.Visible = true;
			chanceToFreezeDurationButton.Visible = true;
		}
	}

	// ─── Button Text ───────────────────────────────────────────────────
	private void UpdateAllButtonText()
	{
		UpdateCoinDisplay();

		// Ultimates
		blizzardButton.Text = GetSkillButtonText("Blizzard");
		flashFreezeButton.Text = GetSkillButtonText("Flash Freeze");

		frostNovaButton.Text = GetSkillButtonText("Frost Nova");
		frostNovaDamageButton.Text = GetSkillButtonText("Frost Nova Damage");
		frostNovaRadiusButton.Text = GetSkillButtonText("Frost Nova Radius");
		frostNovaFreezeDurationButton.Text = GetSkillButtonText("Frost Nova Freeze Duration");

		iceSpikeButton.Text = GetSkillButtonText("Ice Spike");
		iceSpikeDamageButton.Text = GetSkillButtonText("Ice Spike Damage");
		iceSpikeFreezeDurationButton.Text = GetSkillButtonText("Ice Spike Freeze Duration");
		iceSpikeSizeButton.Text = GetSkillButtonText("Ice Spike Size");

		// Passives
		permafrostButton.Text = GetSkillButtonText("Permafrost");
		brittleButton.Text = GetSkillButtonText("Brittle");
		iceShieldButton.Text = GetSkillButtonText("Ice Shield");

		multishotButton.Text = GetSkillButtonText("Multishot");
		multishotCountButton.Text = GetSkillButtonText("Multishot Count");
		multishotChanceButton.Text = GetSkillButtonText("Multishot Chance");

		chanceToFreezeButton.Text = GetSkillButtonText("Chance to Freeze");
		chanceToFreezeChanceButton.Text = GetSkillButtonText("Freeze Chance");
		chanceToFreezeDurationButton.Text = GetSkillButtonText("Freeze Duration");

		// Core Stats
		moveSpeedButton.Text = GetSkillButtonText("Move Speed");
		attackSpeedButton.Text = GetSkillButtonText("Attack Speed");
		healthButton.Text = GetSkillButtonText("Health");
		damageButton.Text = GetSkillButtonText("Damage");
	}

	private string GetSkillButtonText(string skillName)
	{
		int level = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost = getActualCost(skillName);

		if (level >= maxLevel)
			return skillName + "\nMAX";

		return skillName + "\nLv." + level + " | " + cost + "g";
	}

	private void UpdateCoinDisplay()
	{
		coinsLabel.Text = "Coins: " + gameManager.coins;
	}

	// ─── Restore Levels ────────────────────────────────────────────────
	private void RestoreSkillLevels()
	{
		if (saveData.hasBlizzard) skillLevels["Blizzard"] = 1;
		if (saveData.hasFlashFreeze) skillLevels["Flash Freeze"] = 1;

		if (saveData.hasFrostNova) skillLevels["Frost Nova"] = 1;
		skillLevels["Frost Nova Damage"] = saveData.frostNovaDamageLevel;
		skillLevels["Frost Nova Radius"] = saveData.frostNovaRadiusLevel;
		skillLevels["Frost Nova Freeze Duration"] = saveData.frostNovaFreezeDurationLevel;

		if (saveData.hasIceSpike) skillLevels["Ice Spike"] = 1;
		skillLevels["Ice Spike Damage"] = saveData.iceSpikeDamageLevel;
		skillLevels["Ice Spike Freeze Duration"] = saveData.iceSpikeFreezeDurationLevel;
		skillLevels["Ice Spike Size"] = saveData.iceSpikeSizeLevel;

		if (saveData.hasPermafrost) skillLevels["Permafrost"] = 1;
		if (saveData.hasBrittle) skillLevels["Brittle"] = 1;
		if (saveData.hasIceShield) skillLevels["Ice Shield"] = 1;

		if (saveData.hasMultiShot) skillLevels["Multishot"] = 1;
		skillLevels["Multishot Count"] = saveData.multishotCountLevel;
		skillLevels["Multishot Chance"] = saveData.multishotChanceLevel;

		if (saveData.hasFreezeOnHit) skillLevels["Chance to Freeze"] = 1;
		skillLevels["Freeze Chance"] = saveData.freezeChanceLevel;
		skillLevels["Freeze Duration"] = saveData.freezeDurationLevel;

		skillLevels["Move Speed"] = (int)(saveData.moveSpeedBonus / 5f);
		skillLevels["Health"] = saveData.healthBonus;
		skillLevels["Attack Speed"] = (int)(saveData.attackSpeedBonus / 0.02f);
		skillLevels["Damage"] = saveData.damageBonus / 2;
	}

	public override void _Input(InputEvent @event)
	{
		if (openedFromGame && @event.IsActionPressed("pause_game"))
		{
			OnBackPressed();
			GetViewport().SetInputAsHandled();
		}
	}

	// ─── Button Handlers ───────────────────────────────────────────────
	private void OnCorePressed()
	{
		descriptionLabel.Text =
			"Ice Wizard Core\n\nThis is the center of the Ice Wizard skill tree.\n" +
			"Upgrade connected skills to improve the class.";
	}

	private void OnBackPressed()
	{
		if (openedFromGame) {
			OnClose?.Invoke();
			QueueFree();
		} else {
			GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
		}
	}
	
	
	// ─── Respec ────────────────────────────────────────────────────────
	private void OnRespecPressed()
	{
		if (gameManager.coins < RespecCost) {
			descriptionLabel.Text = "Not enough coins to respec.\nCost: " + RespecCost + " coins.";
			return;
		}

		respecConfirmLabel.Text = "Are you sure? This will reset all skills.\nCost: " + RespecCost + " coins.";
		respecConfirmLabel.Visible = true;
		respecYesButton.Visible = true;
		respecNoButton.Visible = true;
	}

	private void OnRespecConfirmed()
	{
		// Calculate refund — all spent coins minus respec cost
		int totalSpent = 0;
		foreach (var skill in skillLevels) {
			totalSpent += getTotalCostForSkill(skill.Key);
		}
		int refund = totalSpent - RespecCost;
		gameManager.coins += refund;

		// Reset all save data
		saveData.hasBlizzard = false;
		saveData.hasFlashFreeze = false;
		saveData.hasFrostNova = false;
		saveData.hasIceSpike = false;
		saveData.hasPermafrost = false;
		saveData.hasBrittle = false;
		saveData.hasIceShield = false;
		saveData.hasMultiShot = false;
		saveData.hasFreezeOnHit = false;
		saveData.activeUltimate = IceWizardStats.UltimateAbility.None;

		saveData.frostNovaDamageLevel = 0;
		saveData.frostNovaRadiusLevel = 0;
		saveData.frostNovaFreezeDurationLevel = 0;
		saveData.iceSpikeDamageLevel = 0;
		saveData.iceSpikeFreezeDurationLevel = 0;
		saveData.iceSpikeSizeLevel = 0;
		saveData.multishotCountLevel = 0;
		saveData.multishotChanceLevel = 0;
		saveData.freezeChanceLevel = 0;
		saveData.freezeDurationLevel = 0;
		saveData.moveSpeedBonus = 0;
		saveData.healthBonus = 0;
		saveData.attackSpeedBonus = 0;
		saveData.damageBonus = 0;

		// Reset skill levels dictionary
		SetupSkills();
		HideSubButtons();
		UpdateAllButtonText();

		respecConfirmLabel.Visible = false;
		respecYesButton.Visible = false;
		respecNoButton.Visible = false;

		descriptionLabel.Text = "Skills reset. " + refund + " coins refunded.";
	}

	private void OnRespecCancelled()
	{
		respecConfirmLabel.Visible = false;
		respecYesButton.Visible = false;
		respecNoButton.Visible = false;
		descriptionLabel.Text = "Respec cancelled.";
	}
}
