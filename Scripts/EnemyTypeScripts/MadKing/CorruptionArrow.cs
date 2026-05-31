using Godot;

public partial class CorruptionArrow : Arrow {
	public override float speed { get; set; } = 150f;
	public override int damage { get; set; } = 20;

	public override void _Ready() {
		Direction = direction;
		startPosition = GlobalPosition;
		BodyEntered += OnBodyEntered;
		Rotation = Direction.Angle() + Mathf.Pi/2; // flip 180 degrees
	}
}
