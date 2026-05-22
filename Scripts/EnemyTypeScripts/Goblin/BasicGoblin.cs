using Godot;
using System;

public partial class BasicGoblin : Enemy {
	public override float speed { get; set; } = 100f;
	public override int contactDamage { get; set; } = 4;

	[Export] public bool isLeader = true;

	private Vector2 erraticOffset = Vector2.Zero;
	private float erraticTimer = 0f;
	private float erraticInterval = 0.6f;
	private Random random = new Random();

	public override void _Ready() {
		base._Ready();
		RandomizeOffset();

		if (isLeader) {
			CallDeferred(nameof(SpawnGroup));
		}
	}

	private void SpawnGroup() {
		PackedScene goblinScene = GD.Load<PackedScene>("res://Scenes/Enemies/BasicGoblin.tscn");
		if (goblinScene == null) return;

		int extras = random.Next(2, 5); // 2-4 extras = 3-5 total
		for (int i = 0; i < extras; i++) {
			BasicGoblin companion = goblinScene.Instantiate<BasicGoblin>();
			companion.Player = Player;
			companion.isLeader = false;

			float angle = (float)(random.NextDouble() * Math.PI * 2);
			float dist = (float)(random.NextDouble() * 80 + 30);
			companion.GlobalPosition = GlobalPosition + new Vector2(
				Mathf.Cos(angle) * dist,
				Mathf.Sin(angle) * dist
			);

			GetTree().CurrentScene.AddChild(companion);
		}
	}

	private void RandomizeOffset() {
		float angle = (float)(random.NextDouble() * Math.PI * 2);
		float magnitude = (float)(random.NextDouble() * 70 + 30);
		erraticOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
		erraticInterval = (float)(random.NextDouble() * 0.8 + 0.3);
	}

	public override void _PhysicsProcess(double delta) {
		if (Player == null) return;

		if (gameManager.isDead) {
			Velocity = Vector2.Zero;
			sprite.Play("Still");
			return;
		}

		erraticTimer -= (float)delta;
		if (erraticTimer <= 0f) {
			RandomizeOffset();
			erraticTimer = erraticInterval;
		}

		Vector2 toPlayer = (Player.GlobalPosition - GlobalPosition).Normalized();
		// Blend toward player (70%) with erratic offset (30%) for zigzag movement
		Vector2 erraticDir = (toPlayer * 0.6f + erraticOffset.Normalized() * 0.4f).Normalized();
		Velocity = erraticDir * speed;
		MoveAndSlide();

		if (Player.GlobalPosition.X < GlobalPosition.X) {
			sprite.FlipH = true;
		} else {
			sprite.FlipH = false;
		}

		if (currentStatus == StatusEffect.None) {
			sprite.Play("Walking");
		}
	}
}
