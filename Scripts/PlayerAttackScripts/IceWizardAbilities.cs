using Godot;
using System;

public partial class IceWizardAbilities : Node
{
	CharacterBody2D player;
	GameManager gameManager;
	IceWizardStats iceStats;

	[Export] public PackedScene FrostNovaScene;
	[Export] public PackedScene IceSpikeScene;
	[Export] public PackedScene BlizzardScene;

	public override void _Ready()
	{
		player = GetParent<CharacterBody2D>();
		gameManager = GetNode<GameManager>("/root/GameManager");
		iceStats = GetParent().GetNode<IceWizardStats>("Stats");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Ultimate"))
		{
			if (iceStats.activeUltimate != IceWizardStats.UltimateAbility.None &&
				iceStats.ultimateCharge >= iceStats.GetUltimateChargeRequired())
			{
				GD.Print("Setting ultimateIsActive to true");
				gameManager.ultimateIsActive = true;

				switch (iceStats.activeUltimate)
				{
					case IceWizardStats.UltimateAbility.FrostNova:
						if (iceStats.hasFrostNova) FireFrostNova();
						break;
					case IceWizardStats.UltimateAbility.IceSpike:
						if (iceStats.hasIceSpike) FireIceSpike();
						break;
					case IceWizardStats.UltimateAbility.Blizzard:
						if (iceStats.hasBlizzard) FireBlizzard();
						break;
					case IceWizardStats.UltimateAbility.FlashFreeze:
						if (iceStats.hasFlashFreeze) FireFlashFreeze();
						break;
				}

				iceStats.ultimateCharge = 0;
			}
		}
	}

	public void FireFrostNova()
	{
		FrostNova frostNova = FrostNovaScene.Instantiate<FrostNova>();
		frostNova.iceStats = iceStats;
		frostNova.GlobalPosition = player.GlobalPosition;
		GetTree().CurrentScene.AddChild(frostNova);
	}

	public void FireIceSpike()
	{
		CharacterBody2D nearestEnemy = FindNearestEnemy();
		if (nearestEnemy == null) return;

		IceSpike iceSpike = IceSpikeScene.Instantiate<IceSpike>();
		iceSpike.characterStats = iceStats;
		iceSpike.Direction = (nearestEnemy.GlobalPosition - player.GlobalPosition).Normalized();
		iceSpike.GlobalPosition = player.GlobalPosition;
		GetTree().CurrentScene.AddChild(iceSpike);
	}

	public void FireBlizzard()
	{
		Blizzard blizzard = BlizzardScene.Instantiate<Blizzard>();
		blizzard.iceStats = iceStats;
		blizzard.GlobalPosition = player.GetGlobalMousePosition();
		GetTree().CurrentScene.AddChild(blizzard);
	}

	public void FireFlashFreeze()
	{
		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is not CharacterBody2D body) continue;
			Enemy enemy = body as Enemy;
			if (enemy == null) continue;

			float originalSpeed = enemy.baseSpeed;
			enemy.speed = 0;
			enemy.currentStatus = Enemy.StatusEffect.Frozen;
			enemy.sprite.Play("Frozen");

			Timer freezeTimer = new Timer();
			freezeTimer.WaitTime = iceStats.flashFreezeDuration;
			freezeTimer.OneShot = true;
			freezeTimer.Timeout += () => {
				if (IsInstanceValid(enemy))
				{
					enemy.speed = originalSpeed;
					enemy.currentStatus = Enemy.StatusEffect.None;
					enemy.sprite.Play("Walking");
				}
				freezeTimer.QueueFree();
			};
			enemy.AddChild(freezeTimer);
			freezeTimer.Start();
		}

		// White flash screen effect
		DoFlashFreezeEffect();

		gameManager.ultimateIsActive = false;
	}

	private void DoFlashFreezeEffect()
	{
		CanvasLayer canvas = GetTree().CurrentScene.GetNodeOrNull<CanvasLayer>("CanvasLayer");
		if (canvas == null) return;

		ColorRect flash = new ColorRect();
		flash.Color = new Color(0.85f, 0.95f, 1.0f, 0.9f);
		flash.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		flash.MouseFilter = Control.MouseFilterEnum.Ignore;
		canvas.AddChild(flash);

		Tween tween = flash.CreateTween();
		tween.TweenProperty(flash, "color:a", 0.9f, 0.05f);
		tween.TweenProperty(flash, "color:a", 0.0f, 0.35f);
		tween.TweenCallback(Callable.From(() => flash.QueueFree()));
	}

	private CharacterBody2D FindNearestEnemy()
	{
		CharacterBody2D nearest = null;
		float nearestDistance = 750.0f;

		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is CharacterBody2D enemy && IsInstanceValid(enemy) && enemy.IsInsideTree())
			{
				float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearest = enemy;
				}
			}
		}
		return nearest;
	}
}
