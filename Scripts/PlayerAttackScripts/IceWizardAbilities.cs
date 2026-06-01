using Godot;

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
		player      = GetParent<CharacterBody2D>();
		gameManager = GetNode<GameManager>("/root/GameManager");
		iceStats    = GetParent().GetNode<IceWizardStats>("Stats");
	}

	public override void _Process(double delta)
	{
		if (iceStats.hasPermafrost)
			ApplyPermafrost();

		if (Input.IsActionJustPressed("Ultimate"))
		{
			if (iceStats.activeUltimate != IceWizardStats.UltimateAbility.None &&
				iceStats.ultimateCharge >= iceStats.GetUltimateChargeRequired())
			{
				switch (iceStats.activeUltimate)
				{
					case IceWizardStats.UltimateAbility.FrostNova:
						if (iceStats.hasFrostNova) {
							gameManager.ultimateIsActive = true;
							FireFrostNova();
						}
						break;
					case IceWizardStats.UltimateAbility.IceSpike:
						if (iceStats.hasIceSpike) {
							FireIceSpikeWithDelay();
						}
						break;
					case IceWizardStats.UltimateAbility.Blizzard:
						if (iceStats.hasBlizzard) {
							gameManager.ultimateIsActive = true;
							FireBlizzard();
						}
						break;
					case IceWizardStats.UltimateAbility.FlashFreeze:
						if (iceStats.hasFlashFreeze) FireFlashFreeze();
						break;
				}

				iceStats.ultimateCharge = 0;
			}
		}
	}

	// ── Ultimates ────────────────────────────────────────────────────

	public void FireFrostNova()
	{
		FrostNova frostNova = FrostNovaScene.Instantiate<FrostNova>();
		frostNova.iceStats = iceStats;
		frostNova.GlobalPosition = player.GlobalPosition;
		GetTree().CurrentScene.AddChild(frostNova);
	}

	public async void FireIceSpikeWithDelay()
	{
		gameManager.ultimateIsActive = true;
		FireIceSpike();

		// Keep ultimateIsActive true for 3 seconds to cover spike travel time
		await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);
		gameManager.ultimateIsActive = false;
	}

	public void FireIceSpike()
	{
		CharacterBody2D nearestEnemy = FindNearestEnemy();
		if (nearestEnemy == null) return;

		GetNode<SoundManager>("/root/SoundManager").PlaySfx("IceSpike");

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

	public async void FireFlashFreeze()
	{
		AudioStreamPlayer flashFreezeSound = GetNode<SoundManager>("/root/SoundManager").PlaySfxLooping("FlashFreeze");

		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is not CharacterBody2D body) continue;
			Enemy enemy = body as Enemy;
			if (enemy == null) continue;
			if (enemy.immuneToAilments) continue;

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
					enemy.currentStatus = Enemy.StatusEffect.None;
					enemy.sprite.Play("Walking");

					if (iceStats.flashFreezeGlacialDuration > 0f)
					{
						enemy.speed = originalSpeed * 0.5f;

						Timer glacialTimer = new Timer();
						glacialTimer.WaitTime = iceStats.flashFreezeGlacialDuration;
						glacialTimer.OneShot = true;
						glacialTimer.Timeout += () => {
							if (IsInstanceValid(enemy) && enemy.currentStatus == Enemy.StatusEffect.None)
								enemy.speed = originalSpeed;
							glacialTimer.QueueFree();
						};
						enemy.AddChild(glacialTimer);
						glacialTimer.Start();
					}
					else
					{
						enemy.speed = originalSpeed;
					}
				}
				freezeTimer.QueueFree();
			};
			enemy.AddChild(freezeTimer);
			freezeTimer.Start();
		}

		DoFlashFreezeEffect();
		gameManager.ultimateIsActive = false;

		await ToSignal(GetTree().CreateTimer(iceStats.flashFreezeDuration), SceneTreeTimer.SignalName.Timeout);
		if (IsInstanceValid(flashFreezeSound)) flashFreezeSound.QueueFree();
	}

	// ── Passives ─────────────────────────────────────────────────────

	private void ApplyPermafrost()
	{
		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is not CharacterBody2D body) continue;
			Enemy enemy = body as Enemy;
			if (enemy == null) continue;
			if (enemy.immuneToAilments) continue;

			float dist = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);

			if (dist <= iceStats.permafrostRadius)
			{
				if (enemy.currentStatus == Enemy.StatusEffect.None)
					enemy.speed = Mathf.MoveToward(enemy.speed, enemy.baseSpeed * iceStats.permafrostSlowFactor, 50f);
			}
			else
			{
				if (enemy.currentStatus == Enemy.StatusEffect.None)
					enemy.speed = Mathf.MoveToward(enemy.speed, enemy.baseSpeed, 50f);
			}
		}
	}

	// ── Effects ──────────────────────────────────────────────────────

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

	// ── Helpers ──────────────────────────────────────────────────────

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
