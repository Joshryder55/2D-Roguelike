using Godot;

public partial class PowerUpXPVacuum : Area2D
{
	GameManager gameManager;
	CharacterBody2D player;

	float attractRadius = 120.0f;
	float moveSpeed = 180.0f;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		if (player == null || gameManager.isDead) return;

		if (GlobalPosition.DistanceTo(player.GlobalPosition) < attractRadius)
		{
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			GlobalPosition += direction * moveSpeed * (float)delta;
		}
	}

	void OnBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("player")) return;

		// Vacuum up every XP orb currently on the map
		int totalXP = 0;
		foreach (Node node in GetTree().GetNodesInGroup("xp_orbs"))
		{
			if (node is XPOrb orb && IsInstanceValid(orb))
			{
				totalXP += orb.xpValue;
				orb.QueueFree();
			}
		}

		if (totalXP > 0)
			gameManager.AddXP(totalXP);

		QueueFree();
	}
}
