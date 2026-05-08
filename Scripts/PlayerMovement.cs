using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D {
	
	GameManager gameManager;
	
	float playerSpeed = 100;
	AnimatedSprite2D sprite;
	
	public override void _Ready() {
	gameManager = GetNode<GameManager>("/root/GameManager");
	gameManager.Reset();
	sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	sprite.AnimationFinished += OnAnimationFinished;
	}
	

	public override void _PhysicsProcess(double delta) {
		if(gameManager.isDead) {
			if (sprite.Animation != "Death" && sprite.Animation != "Dead")
				sprite.Play("Death");
				return;
		}
		
		
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
		
		if (inputVector != Vector2.Zero){
			sprite.Play("Walking");
			}
			else{
				sprite.Stop();
		}
		
		
	}	
	
	private void OnAnimationFinished() {
		if (gameManager.isDead && sprite.Animation == "Death") {
		sprite.Play("Dead");
	}
	}		
	
	

}
