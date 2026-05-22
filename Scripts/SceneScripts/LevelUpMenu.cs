using Godot;
using System;
using System.Collections.Generic;

public partial class LevelUpMenu : CanvasLayer
{
	private CharacterStats playerStats;

	private struct Upgrade
	{
		public string name;
		public string description;
		public Action<CharacterStats> apply;
	}

	// Pop-up screen for leveling in-run. Lets player choose core stats to upgrade.
	private static readonly List<Upgrade> allUpgrades = new List<Upgrade> {
		new Upgrade {
			name = "Health",
			description = "+15 Max HP and restore 15 HP",
			apply = s => { s.maxHealth += 15; s.health = Mathf.Min(s.health + 15, s.maxHealth); }
		},
		new Upgrade {
			name = "Damage",
			description = "+3 Attack Damage",
			apply = s => { s.damageBonus += 3; }
		},
		new Upgrade {
			name = "Move Speed",
			description = "+10 Movement Speed",
			apply = s => { s.playerSpeed += 10f; }
		},
		new Upgrade {
			name = "Attack Speed",
			description = "Attack Speed +10%",
			apply = s => { s.fireRate = Mathf.Max(0.1f, s.fireRate * 0.9f); }
		},
	};

	public override void _Ready()
	{
		Layer = 10;
		ProcessMode = ProcessModeEnum.Always;

		var playerNode = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		playerStats = playerNode?.GetNode<CharacterStats>("Stats");

		GetTree().Paused = true;
		buildUI(allUpgrades);
	}

	private void buildUI(List<Upgrade> choices)
	{
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		Vector2 center = viewportSize / 2f;

		// overlay (Dark)
		var overlay = new ColorRect();
		overlay.Color = new Color(0f, 0f, 0f, 0.65f);
		overlay.Size = viewportSize;
		overlay.Position = Vector2.Zero;
		AddChild(overlay);

		// Panel
		float panelW = 560f;
		float panelH = 400f;
		var panel = new Panel();
		panel.Size = new Vector2(panelW, panelH);
		panel.Position = center - new Vector2(panelW / 2f, panelH / 2f);
		AddChild(panel);

		var margin = new MarginContainer();
		margin.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		margin.AddThemeConstantOverride("margin_left", 24);
		margin.AddThemeConstantOverride("margin_right", 24);
		margin.AddThemeConstantOverride("margin_top", 18);
		margin.AddThemeConstantOverride("margin_bottom", 18);
		panel.AddChild(margin);

		var vbox = new VBoxContainer();
		vbox.AddThemeConstantOverride("separation", 12);
		margin.AddChild(vbox);

		var title = new Label();
		title.Text = "Level Up!  Choose an Upgrade";
		title.HorizontalAlignment = HorizontalAlignment.Center;
		title.AddThemeFontSizeOverride("font_size", 28);
		vbox.AddChild(title);

		foreach (var upgrade in choices)
		{
			var btn = new Button();
			btn.Text = upgrade.name + "\n" + upgrade.description;
			btn.CustomMinimumSize = new Vector2(0, 68);
			var captured = upgrade;
			btn.Pressed += () => onUpgradeChosen(captured);
			vbox.AddChild(btn);
		}
	}

	private void onUpgradeChosen(Upgrade upgrade)
	{
		upgrade.apply(playerStats);
		GetTree().Paused = false;
		QueueFree();
	}
}
