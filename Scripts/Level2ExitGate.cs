using Godot;

public partial class Level2ExitGate : Area2D {
	public override void _Ready() {
		Visible = false;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body) {
		if (body.IsInGroup("player")) {
			CallDeferred("ChangeScene");
		}
	}

	private void ChangeScene() {
		GetTree().ChangeSceneToFile("res://Scenes/LoreScreens/Level3Lore.tscn");
	}
}
