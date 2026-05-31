using Godot;

public partial class LevelManager : Node2D
{
	[Export] public NodePath PlayerSpawnPath = "PlayerSpawn";

	private readonly string iceWizardScene  = "res://Scenes/Characters/IceWizard.tscn";
	private readonly string fireWizardScene = "res://Scenes/Characters/FireWizard.tscn";

	public override void _Ready()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		string scenePath = saveData.selectedCharacter == PlayerSaveData.Character.FireWizard
			? fireWizardScene
			: iceWizardScene;

		PackedScene wizardScene   = GD.Load<PackedScene>(scenePath);
		CharacterBody2D player    = wizardScene.Instantiate<CharacterBody2D>();

		Node2D spawnPoint         = GetNodeOrNull<Node2D>(PlayerSpawnPath);
		player.GlobalPosition     = spawnPoint != null
			? spawnPoint.GlobalPosition
			: new Vector2(387, 216);

		AddChild(player);

		// Use Node base type to find EnemyManager then cast
		Node enemyNode = GetNodeOrNull<Node>("EnemyManager");
		if (enemyNode is EnemyManager enemyManager)
		{
			enemyManager.Player = player;
			GD.Print("EnemyManager player assigned.");
		}
		else
		{
			GD.PrintErr("LevelManager: EnemyManager not found! Children: ");
			foreach (Node child in GetChildren())
				GD.Print(" - " + child.Name + " (" + child.GetType().Name + ")");
		}

		GD.Print("Spawned: " + scenePath);
	}
}
