using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D {
	
	float playerSpeed = 100;
	

	public override void _PhysicsProcess(double delta) {
		Vector2 inputVector = Vector2.Zero;
		if (Input.IsActionPressed("ui_right"))
			inputVector.X += 1;
		if (Input.IsActionPressed("ui_left"))
			inputVector.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			inputVector.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			inputVector.Y -= 1;
		inputVector = inputVector.Normalized();
		Velocity = inputVector * playerSpeed;
		MoveAndSlide();
	}

}
