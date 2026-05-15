using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		GetNode<Button>("Control/RestartButton").Pressed +=OnRestartPressed;
		GetNode<Button>("Control/TitleScreenButton").Pressed += OnTitleScreenPressed;
	}

	private void OnRestartPressed(){
		GD.Print("Restart CLicked");
				
		//Change this to the character and level select screen when that is created.
		GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
	}
	
	private void OnTitleScreenPressed(){
		//Hook up the same way as above when the title screen is created 
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
