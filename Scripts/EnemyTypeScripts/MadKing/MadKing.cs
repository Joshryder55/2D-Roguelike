using Godot;
using System;

public partial class MadKing : Enemy {
	public override float speed { get; set; } = 0f;
	public override int contactDamage { get; set; } = 15;
	public override bool immuneToAilments { get; set; } = true;

	[Export] public PackedScene MinionScene;
	[Export] public PackedScene ProjectileScene;

	private enum KingState { Idle, Casting, Raising }
	private KingState currentState = KingState.Idle;

	private float castCooldown = 4f;
	private float summonCooldown = 10f;
	private float castTimer = 0f;
	private float summonTimer = 0f;

	private bool projectileSpawned = false;
	private bool minionSpawned = false;

	private Vector2 altarPosition;

public override async void _Ready() {
	base._Ready();
	
	GetNode<ProgressBar>("ProgressBar").Visible = true;
	GetNode<ProgressBar>("ProgressBar").MaxValue = 1000;

	await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	Player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
	
	Node2D altar = GetTree().CurrentScene.GetNodeOrNull<Node2D>("Altar");
	GD.Print("Altar found: " + (altar != null));
	if (altar != null)
		altarPosition = altar.GlobalPosition;
	else
		altarPosition = GlobalPosition;

	if (sprite.SpriteFrames.HasAnimation("Still"))
		sprite.Play("Still");

	castTimer = 0f;
	summonTimer = 15f;  // wait before first summon
	sprite.AnimationFinished += OnAnimationFinished;
}

	public override void _PhysicsProcess(double delta) {
		if (Player == null) return;
		if (gameManager.isDead) return;

		Velocity = Vector2.Zero;

		if (Player.GlobalPosition.X < GlobalPosition.X)
			sprite.FlipH = true;
		else
			sprite.FlipH = false;

		if (currentState == KingState.Idle) {
			castTimer -= (float)delta;
			summonTimer -= (float)delta;

			if (summonTimer <= 0f) {
				currentState = KingState.Raising;
				minionSpawned = false;
				summonTimer = summonCooldown;
				sprite.Play("Raise");
			} else if (castTimer <= 0f) {
				currentState = KingState.Casting;
				projectileSpawned = false;
				castTimer = castCooldown;
				sprite.Play("Cast");
			}
		}

		if (sprite.Animation == "Cast" && sprite.Frame == 5 && !projectileSpawned) {
			SpawnProjectile();
			projectileSpawned = true;
		}

		if (sprite.Animation == "Raise" && sprite.Frame == 8 && !minionSpawned) {
			SpawnMinions();
			minionSpawned = true;
		}
	}

	private void OnAnimationFinished() {
		if (sprite.Animation == "Cast" || sprite.Animation == "Raise") {
			currentState = KingState.Idle;
			sprite.Play("Still");
		}
	}

	private void SpawnProjectile() {
		if (ProjectileScene == null || Player == null) return;

		CorruptionArrow arrow = ProjectileScene.Instantiate<CorruptionArrow>();
		arrow.GlobalPosition = GlobalPosition;
		arrow.direction = (Player.GlobalPosition - GlobalPosition).Normalized();
		GetTree().CurrentScene.CallDeferred("add_child", arrow);

		GetNode<SoundManager>("/root/SoundManager").PlaySfx("BossAttackLvl3");
	}

	private void SpawnMinions() {
		if (MinionScene == null) return;

		int count = 3;
		for (int i = 0; i < count; i++) {
			Enemy minion = MinionScene.Instantiate<Enemy>();
			minion.Player = Player;

			float angle = (float)(i * Math.PI * 2 / count);
			Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 100f;
			minion.GlobalPosition = altarPosition + offset;

			if (minion is CorruptedSkeleton skeleton)
				skeleton.kingRef = this;

			GetTree().CurrentScene.CallDeferred("add_child", minion);
		}
	}

	public void TakeMinionDamage(int amount) {
		EnemyHealth health = GetNode<EnemyHealth>("EnemyHealth");
		health.health -= amount;
		GetNode<ProgressBar>("ProgressBar").Value = health.health;
		if (health.health <= 0)
			health.Die();
	}


}
