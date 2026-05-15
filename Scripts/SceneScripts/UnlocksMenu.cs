using Godot;
using System.Collections.Generic;

public partial class UnlocksMenu : Control
{
	
	GameManager gameManager;
	
	private Button coreButton;

	//Ultimates
	private Button blizzardButton;
	private Button flashFreezeButton;
	private Button frostNovaButton;
	private Button iceSpikeButton;
	
	//Passives
	private Button permafrostButton;
	private Button brittleButton;
	private Button iceShieldButton;
	private Button multishotButton;
	private Button chanceToFreezeButton;

	//Core Stats
	private Button moveSpeedButton;
	private Button attackSpeedButton;
	private Button healthButton;
	private Button damageButton;

	private Button backButton;
	private Label descriptionLabel;

	// Skill levels for now.
	// Later, these can move into GameManager so they persist between scenes/runs.
	private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

	// Max level per skill.
	private Dictionary<string, int> skillMaxLevels = new Dictionary<string, int>();

	// Placeholder coin costs.
	// Later, these can be checked against player coins.
	private Dictionary<string, int> skillCosts = new Dictionary<string, int>();

	public override void _Ready()
	{
		
		gameManager = GetNode<GameManager>("/root/GameManager");
		
		coreButton = GetNode<Button>("TreeArea/CoreButton");

		blizzardButton = GetNode<Button>("TreeArea/Ultimate/BlizzardButton");
		flashFreezeButton = GetNode<Button>("TreeArea/Ultimate/FlashFreezeButton");
		frostNovaButton = GetNode<Button>("TreeArea/Ultimate/FrostNovaButton");
		iceSpikeButton = GetNode<Button>("TreeArea/Ultimate/IceSpikeButton");
		
		permafrostButton = GetNode<Button>("TreeArea/Passive/PermafrostButton");
		brittleButton = GetNode<Button>("TreeArea/Passive/BrittleButton");
		iceShieldButton = GetNode<Button>("TreeArea/Passive/IceShieldButton");
		multishotButton = GetNode<Button>("TreeArea/Passive/MultishotButton");
		chanceToFreezeButton = GetNode<Button>("TreeArea/Passive/ChancetoFreezeButton");

		moveSpeedButton = GetNode<Button>("TreeArea/CoreStats/MoveSpeedButton");
		attackSpeedButton = GetNode<Button>("TreeArea/CoreStats/AttackSpeedButton");
		healthButton = GetNode<Button>("TreeArea/CoreStats/HealthButton");
		damageButton = GetNode<Button>("TreeArea/CoreStats/DamageButton");

		backButton = GetNode<Button>("BackButton");
		descriptionLabel = GetNode<Label>("DescriptionLabel");

		SetupSkills();
		RestoreSkillLevels();
		ConnectButtons();
		UpdateAllButtonText();
		SetupToolTips();
		HideSubButtons();
		RestoreUnlockedSubButtons();

		descriptionLabel.Text = "Select a skill to view details.\nClick a skill to upgrade it for free for now.";
	}

	private void SetupSkills()
	{
		
		//Ultimates
		AddSkill("Blizzard", 1, 100);
		
		AddSkill("Flash Freeze", 1, 100);
		
		AddSkill("Frost Nova", 1, 25);
		// Frost Nova sub-upgrades
		AddSkill("Frost Nova Damage", 5, 50);
		AddSkill("Frost Nova Radius", 5, 50);
		AddSkill("Frost Nova Freeze Duration", 5, 50);
		
		AddSkill("Ice Spike", 1 , 25);
		// Ice Spike sub-upgrades
		AddSkill("Ice Spike Damage", 5, 50);
		AddSkill("Ice Spike Freeze Duration", 5, 50);
		AddSkill("Ice Spike Size", 5, 50);


		//Passives
		AddSkill("Multishot", 1, 100);
		// Multishot sub-upgrades
		AddSkill("Multishot Count", 5, 50);
		AddSkill("Multishot Chance", 5, 50);
		
		AddSkill("Permafrost", 1, 75);
		
		AddSkill("Brittle", 1, 75);
		
		AddSkill("Ice Shield", 1, 75);
		
		AddSkill("Chance to Freeze", 1, 25);
		// Chance to Freeze sub-upgrades
		AddSkill("Freeze Chance", 5, 50);
		AddSkill("Freeze Duration", 5, 50);
		
		
		//Core Skills
		AddSkill("Move Speed", 5, 50);
		AddSkill("Attack Speed", 5, 50);
		AddSkill("Health", 5, 50);
		AddSkill("Damage", 5, 50);
	}

	private void AddSkill(string skillName, int maxLevel, int coinCost)
	{
		skillLevels[skillName] = 0;
		skillMaxLevels[skillName] = maxLevel;
		skillCosts[skillName] = coinCost;
	}

	private void ConnectButtons()
	{
		coreButton.Pressed += OnCorePressed;

		blizzardButton.Pressed += () => UpgradeSkill("Blizzard");
		
		flashFreezeButton.Pressed += () => UpgradeSkill("Flash Freeze");
		
		frostNovaButton.Pressed += () => UpgradeSkill("Frost Nova");
		GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage").Pressed += () => UpgradeSkill("Frost Nova Damage");
		GetNode<Button>("TreeArea/Ultimate/FrostNovaSize").Pressed += () => UpgradeSkill("Frost Nova Radius");
		GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration").Pressed += () => UpgradeSkill("Frost Nova Freeze Duration");

		iceSpikeButton.Pressed += () => UpgradeSkill("Ice Spike");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage").Pressed += () => UpgradeSkill("Ice Spike Damage");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration").Pressed += () => UpgradeSkill("Ice Spike Freeze Duration");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeSize").Pressed += () => UpgradeSkill("Ice Spike Size");

		
		permafrostButton.Pressed += () => UpgradeSkill("Permafrost");
		
		brittleButton.Pressed += () => UpgradeSkill("Brittle");
		
		iceShieldButton.Pressed += () => UpgradeSkill("Ice Shield");
		
		multishotButton.Pressed += () => UpgradeSkill("Multishot");
		GetNode<Button>("TreeArea/Passive/MultishotCount").Pressed += () => UpgradeSkill("Multishot Count");
		GetNode<Button>("TreeArea/Passive/MultishotChance").Pressed += () => UpgradeSkill("Multishot Chance");

		
		chanceToFreezeButton.Pressed += () => UpgradeSkill("Chance to Freeze");
		GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance").Pressed += () => UpgradeSkill("Freeze Chance");
		GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration").Pressed += () => UpgradeSkill("Freeze Duration");

		moveSpeedButton.Pressed += () => UpgradeSkill("Move Speed");
		attackSpeedButton.Pressed += () => UpgradeSkill("Attack Speed");
		healthButton.Pressed += () => UpgradeSkill("Health");
		damageButton.Pressed += () => UpgradeSkill("Damage");

		backButton.Pressed += OnBackPressed;
	}

	private void UpgradeSkill(string skillName)
	{
		int currentLevel = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost = skillCosts[skillName];

		if (currentLevel >= maxLevel)
		{
			descriptionLabel.Text = skillName + "\n\nAlready max level.";
			return;
		}

		// Later coin check goes here.
		// Example:
		 if (gameManager.coins < cost)
		 {
			 descriptionLabel.Text = skillName + "\n\nNot enough coins.";
			 return;
		 }
		
		 gameManager.coins -= cost;

		skillLevels[skillName]++;

		ApplySkillEffectPlaceholder(skillName);

		UpdateAllButtonText();
		ShowSkillDescription(skillName);

		GD.Print(skillName + " upgraded to level " + skillLevels[skillName]);
	}

	private void ApplySkillEffectPlaceholder(string skillName)
	{
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		int level = skillLevels[skillName];

		switch (skillName) {
			// Ultimates
			case "Blizzard": saveData.hasBlizzard = true; break;
			
			case "Flash Freeze": saveData.hasFlashFreeze = true; break;
			
			case "Frost Nova":
				saveData.hasFrostNova = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.FrostNova;
				GetNode<Button>("TreeArea/Ultimate/FrostNovaSize").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration").Visible = true;
				break;
				
			case "Frost Nova Damage": saveData.frostNovaDamageLevel++; break;
			
			case "Frost Nova Radius": saveData.frostNovaRadiusLevel++; break;
			
			case "Frost Nova Freeze Duration": saveData.frostNovaFreezeDurationLevel++; break;
			
				
			case "Ice Spike":
				saveData.hasIceSpike = true;
				saveData.activeUltimate = IceWizardStats.UltimateAbility.IceSpike;
				GetNode<Button>("TreeArea/Ultimate/IceSpikeSize").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration").Visible = true;
				break;
				
			case "Ice Spike Damage": saveData.iceSpikeDamageLevel++; break;
			
			case "Ice Spike Freeze Duration": saveData.iceSpikeFreezeDurationLevel++; break;
			
			case "Ice Spike Size": saveData.iceSpikeSizeLevel++; break;
				

			// Passives
			case "Permafrost": saveData.hasPermafrost = true; break;
			
			case "Brittle": saveData.hasBrittle = true; break;
			
			case "Ice Shield": saveData.hasIceShield = true; break;
			
			case "Multishot":
				saveData.hasMultiShot = true;
				GetNode<Button>("TreeArea/Passive/MultishotChance").Visible = true;
				GetNode<Button>("TreeArea/Passive/MultishotCount").Visible = true;
				break;
				
			case "Multishot Count": saveData.multishotCountLevel++; break;
			case "Multishot Chance": saveData.multishotChanceLevel++; break;
				
			case "Chance to Freeze":
				saveData.hasFreezeOnHit = true;
				GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance").Visible = true;
				GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration").Visible = true;
				break;
				
			case "Freeze Chance": saveData.freezeChanceLevel++; break;
			case "Freeze Duration": saveData.freezeDurationLevel++; break;

			// Core Stats
			case "Move Speed": saveData.moveSpeedBonus += 10f; break;
			case "Health": saveData.healthBonus += 2; break;
			case "Attack Speed": saveData.attackSpeedBonus += 0.05f; break;
			case "Damage": saveData.damageBonus += 5; break;
}
		// For now, this just prints.
		GD.Print("Placeholder effect applied for " + skillName + " at level " + level);
	}

	private void ShowSkillDescription(string skillName)
	{
		int level = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];
		int cost = skillCosts[skillName];

		string description = GetSkillDescription(skillName);

		descriptionLabel.Text =
			skillName +
			"\nLevel: " + level + " / " + maxLevel +
			"\nCoins Needed: " +
			"\n\n" + description;
	}

	private string GetSkillDescription(string skillName)
	{
		switch (skillName)
		{
			case "Blizzard":
				return "Ultimate. Place a persistent AOE snowstorm that damages and slows enemies inside.";

			case "Flash Freeze":
				return "Ultimate. Instantly freezes all enemies on screen briefly.";
				
			case "Frost Nova":
				return "Ultimate. Release Cold Winds that damage and freeze enemies.";
				
			case "Ice Spike":
				return "Ultimate. Release a large deadly spike that penetrates enemies.";
				

			case "Permafrost":
				return "Passive. A slow aura surrounds the player. Nearby enemies move slower.";

			case "Brittle":
				return "Passive. Frozen enemies take bonus damage when hit.";

			case "Ice Shield":
				return "Passive. Chance to block incoming damage when hit.";
				
			case "Multishot":
				return "Passive. Adds extra projectiles or attacks.";
				
			case "Chance to Freeze" :
				return "Passive. Percent Chance to Freeze enemies.";	
				

			case "Move Speed":
				return "Passive. Increases the player's movement speed.";

			case "Attack Speed":
				return "Passive. Increases how often the player attacks.";

			case "Health":
				return "Passive. Increases the player's maximum health.";

			case "Damage":
				return "Passive. Increases the player's attack damage.";

			default:
				return "No description yet.";
		}
	}
	
	private void SetupToolTips() {
		blizzardButton.TooltipText = GetSkillDescription("Blizzard");
		
		flashFreezeButton.TooltipText = GetSkillDescription("Flash Freeze");
		
		frostNovaButton.TooltipText = GetSkillDescription("Frost Nova");
		
		iceSpikeButton.TooltipText = GetSkillDescription("Ice Spike");
		
		permafrostButton.TooltipText = GetSkillDescription("Permafrost");
		
		brittleButton.TooltipText = GetSkillDescription("Brittle");
		
		iceShieldButton.TooltipText = GetSkillDescription("Ice Shield");
		
		multishotButton.TooltipText = GetSkillDescription("Multishot");
		
		chanceToFreezeButton.TooltipText = GetSkillDescription("Chance to Freeze");
		
		moveSpeedButton.TooltipText = GetSkillDescription("Move Speed");
		
		attackSpeedButton.TooltipText = GetSkillDescription("Attack Speed");
		
		healthButton.TooltipText = GetSkillDescription("Health");
		
		damageButton.TooltipText = GetSkillDescription("Damage");
		
	}
	
		private void HideSubButtons() {
			// Frost Nova
			GetNode<Button>("TreeArea/Ultimate/FrostNovaSize").Visible = false;
			GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage").Visible = false;
			GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration").Visible = false;
			
			// Ice Spike
			GetNode<Button>("TreeArea/Ultimate/IceSpikeSize").Visible = false;
			GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage").Visible = false;
			GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration").Visible = false;
			
			// Multishot
			GetNode<Button>("TreeArea/Passive/MultishotChance").Visible = false;
			GetNode<Button>("TreeArea/Passive/MultishotCount").Visible = false;
			
			// Chance to Freeze
			GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance").Visible = false;
			GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration").Visible = false;
		}
	
		private void RestoreUnlockedSubButtons() {
			PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
			
			if (saveData.hasFrostNova) {
				GetNode<Button>("TreeArea/Ultimate/FrostNovaSize").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration").Visible = true;
			}
			if (saveData.hasIceSpike) {
				GetNode<Button>("TreeArea/Ultimate/IceSpikeSize").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage").Visible = true;
				GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration").Visible = true;
			}
			if (saveData.hasMultiShot) {
				GetNode<Button>("TreeArea/Passive/MultishotChance").Visible = true;
				GetNode<Button>("TreeArea/Passive/MultishotCount").Visible = true;
			}
			if (saveData.hasFreezeOnHit) {
				GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance").Visible = true;
				GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration").Visible = true;
			}
	}

	private void UpdateAllButtonText()
	{
		blizzardButton.Text = GetSkillButtonText("Blizzard");
		flashFreezeButton.Text = GetSkillButtonText("Flash Freeze");
		frostNovaButton.Text = GetSkillButtonText("Frost Nova");
		iceSpikeButton.Text = GetSkillButtonText("Ice Spike");
		
		permafrostButton.Text = GetSkillButtonText("Permafrost");
		brittleButton.Text = GetSkillButtonText("Brittle");
		iceShieldButton.Text = GetSkillButtonText("Ice Shield");
		multishotButton.Text = GetSkillButtonText("Multishot");
		chanceToFreezeButton.Text = GetSkillButtonText("Chance to Freeze");

		GetNode<Button>("TreeArea/Ultimate/FrostNovaDamage").Text = GetSkillButtonText("Frost Nova Damage");
		GetNode<Button>("TreeArea/Ultimate/FrostNovaSize").Text = GetSkillButtonText("Frost Nova Radius");
		GetNode<Button>("TreeArea/Ultimate/FrostNovaFreezeDuration").Text = GetSkillButtonText("Frost Nova Freeze Duration");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeDamage").Text = GetSkillButtonText("Ice Spike Damage");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeSize").Text = GetSkillButtonText("Ice Spike Size");
		GetNode<Button>("TreeArea/Ultimate/IceSpikeFreezeDuration").Text = GetSkillButtonText("Ice Spike Freeze Duration");
		GetNode<Button>("TreeArea/Passive/MultishotCount").Text = GetSkillButtonText("Multishot Count");
		GetNode<Button>("TreeArea/Passive/MultishotChance").Text = GetSkillButtonText("Multishot Chance");
		GetNode<Button>("TreeArea/Passive/ChancetoFreezeChance").Text = GetSkillButtonText("Freeze Chance");
		GetNode<Button>("TreeArea/Passive/ChancetoFreezeDuration").Text = GetSkillButtonText("Freeze Duration");


		moveSpeedButton.Text = GetSkillButtonText("Move Speed");
		attackSpeedButton.Text = GetSkillButtonText("Attack Speed");
		healthButton.Text = GetSkillButtonText("Health");
		damageButton.Text = GetSkillButtonText("Damage");
	}
	
	private void RestoreSkillLevels() {
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		
		// Unlock levels
		if (saveData.hasFrostNova) skillLevels["Frost Nova"] = 1;
		if (saveData.hasIceSpike) skillLevels["Ice Spike"] = 1;
		if (saveData.hasMultiShot) skillLevels["Multishot"] = 1;
		if (saveData.hasFreezeOnHit) skillLevels["Chance to Freeze"] = 1;
		if (saveData.hasBlizzard) skillLevels["Blizzard"] = 1;
		if (saveData.hasFlashFreeze) skillLevels["Flash Freeze"] = 1;
		if (saveData.hasPermafrost) skillLevels["Permafrost"] = 1;
		if (saveData.hasBrittle) skillLevels["Brittle"] = 1;
		if (saveData.hasIceShield) skillLevels["Ice Shield"] = 1;
		
		// Sub-upgrade levels
		skillLevels["Frost Nova Damage"] = saveData.frostNovaDamageLevel;
		skillLevels["Frost Nova Radius"] = saveData.frostNovaRadiusLevel;
		skillLevels["Frost Nova Freeze Duration"] = saveData.frostNovaFreezeDurationLevel;
		skillLevels["Ice Spike Damage"] = saveData.iceSpikeDamageLevel;
		skillLevels["Ice Spike Freeze Duration"] = saveData.iceSpikeFreezeDurationLevel;
		skillLevels["Ice Spike Size"] = saveData.iceSpikeSizeLevel;
		skillLevels["Multishot Count"] = saveData.multishotCountLevel;
		skillLevels["Multishot Chance"] = saveData.multishotChanceLevel;
		skillLevels["Freeze Chance"] = saveData.freezeChanceLevel;
		skillLevels["Freeze Duration"] = saveData.freezeDurationLevel;
		skillLevels["Move Speed"] = (int)(saveData.moveSpeedBonus / 10f);
		skillLevels["Health"] = saveData.healthBonus / 2;
		skillLevels["Attack Speed"] = (int)(saveData.attackSpeedBonus / 0.05f);
		skillLevels["Damage"] = saveData.damageBonus / 5;
	}

	private string GetSkillButtonText(string skillName)
	{
		int level = skillLevels[skillName];
		int maxLevel = skillMaxLevels[skillName];

		if (level >= maxLevel)
		{
			return skillName + "\nMAX";
		}

		return skillName + "\nLv. " + level;
	}

	private void OnCorePressed()
	{
		descriptionLabel.Text =
			"Ice Wizard Core\n\nThis is the center of the Ice Wizard skill tree.\n" +
			"Upgrade connected skills to improve the class.";
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
