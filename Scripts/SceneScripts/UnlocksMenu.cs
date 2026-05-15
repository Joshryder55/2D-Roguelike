using Godot;
using System.Collections.Generic;

public partial class UnlocksMenu : Control
{
	private Button coreButton;

	private Button blizzardButton;
	private Button flashFreezeButton;
	private Button permafrostButton;
	private Button brittleButton;
	private Button iceShieldButton;

	private Button moveSpeedButton;
	private Button attackSpeedButton;
	private Button multishotButton;
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
		coreButton = GetNode<Button>("TreeArea/CoreButton");

		blizzardButton = GetNode<Button>("TreeArea/BlizzardButton");
		flashFreezeButton = GetNode<Button>("TreeArea/FlashFreezeButton");
		permafrostButton = GetNode<Button>("TreeArea/PermafrostButton");
		brittleButton = GetNode<Button>("TreeArea/BrittleButton");
		iceShieldButton = GetNode<Button>("TreeArea/IceShieldButton");

		moveSpeedButton = GetNode<Button>("TreeArea/MoveSpeedButton");
		attackSpeedButton = GetNode<Button>("TreeArea/AttackSpeedButton");
		multishotButton = GetNode<Button>("TreeArea/MultishotButton");
		healthButton = GetNode<Button>("TreeArea/HealthButton");
		damageButton = GetNode<Button>("TreeArea/DamageButton");

		backButton = GetNode<Button>("BackButton");
		descriptionLabel = GetNode<Label>("DescriptionLabel");

		SetupSkills();
		ConnectButtons();
		UpdateAllButtonText();

		descriptionLabel.Text = "Select a skill to view details.\nClick a skill to upgrade it for free for now.";
	}

	private void SetupSkills()
	{
		AddSkill("Blizzard", 1, 100);
		AddSkill("Flash Freeze", 1, 100);
		AddSkill("Permafrost", 3, 75);
		AddSkill("Brittle", 3, 75);
		AddSkill("Ice Shield", 3, 75);

		AddSkill("Move Speed", 5, 50);
		AddSkill("Attack Speed", 5, 50);
		AddSkill("Multishot", 3, 100);
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
		permafrostButton.Pressed += () => UpgradeSkill("Permafrost");
		brittleButton.Pressed += () => UpgradeSkill("Brittle");
		iceShieldButton.Pressed += () => UpgradeSkill("Ice Shield");

		moveSpeedButton.Pressed += () => UpgradeSkill("Move Speed");
		attackSpeedButton.Pressed += () => UpgradeSkill("Attack Speed");
		multishotButton.Pressed += () => UpgradeSkill("Multishot");
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
		// if (GameManager.Coins < cost)
		// {
		//     descriptionLabel.Text = skillName + "\n\nNot enough coins.";
		//     return;
		// }
		//
		// GameManager.Coins -= cost;

		skillLevels[skillName]++;

		ApplySkillEffectPlaceholder(skillName);

		UpdateAllButtonText();
		ShowSkillDescription(skillName);

		GD.Print(skillName + " upgraded to level " + skillLevels[skillName]);
	}

	private void ApplySkillEffectPlaceholder(string skillName)
	{
		int level = skillLevels[skillName];

		// Later, actual gameplay changes go here.
		// Examples:
		//
		// Damage:
		// GameManager.damageBonus = level * 0.10f;
		//
		// Health:
		// GameManager.maxHealth += 10;
		//
		// Move Speed:
		// GameManager.moveSpeedBonus = level * 0.05f;
		//
		// Attack Speed:
		// GameManager.attackCooldownMultiplier = 1.0f - level * 0.05f;
		//
		// Multishot:
		// GameManager.extraProjectiles = level;
		//
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

			case "Permafrost":
				return "Passive. A slow aura surrounds the player. Nearby enemies move slower.";

			case "Brittle":
				return "Passive. Frozen enemies take bonus damage when hit.";

			case "Ice Shield":
				return "Passive. Chance to block incoming damage when hit.";

			case "Move Speed":
				return "Passive. Increases the player's movement speed.";

			case "Attack Speed":
				return "Passive. Increases how often the player attacks.";

			case "Multishot":
				return "Passive. Adds extra projectiles or attacks.";

			case "Health":
				return "Passive. Increases the player's maximum health.";

			case "Damage":
				return "Passive. Increases the player's attack damage.";

			default:
				return "No description yet.";
		}
	}

	private void UpdateAllButtonText()
	{
		blizzardButton.Text = GetSkillButtonText("Blizzard");
		flashFreezeButton.Text = GetSkillButtonText("Flash Freeze");
		permafrostButton.Text = GetSkillButtonText("Permafrost");
		brittleButton.Text = GetSkillButtonText("Brittle");
		iceShieldButton.Text = GetSkillButtonText("Ice Shield");

		moveSpeedButton.Text = GetSkillButtonText("Move Speed");
		attackSpeedButton.Text = GetSkillButtonText("Attack Speed");
		multishotButton.Text = GetSkillButtonText("Multishot");
		healthButton.Text = GetSkillButtonText("Health");
		damageButton.Text = GetSkillButtonText("Damage");
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
