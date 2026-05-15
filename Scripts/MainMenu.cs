using Godot;

public partial class MainMenu : Control
{
	private Button startButton;
	private Button unlocksButton;
	private Button optionsButton;
	private Button quitButton;

	public override void _Ready()
	{
		startButton = GetNode<Button>("CenterContainer/VBoxContainer/StartButton");
		unlocksButton = GetNode<Button>("CenterContainer/VBoxContainer/UnlocksButton");
		optionsButton = GetNode<Button>("CenterContainer/VBoxContainer/OptionsButton");
		quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		startButton.Pressed += OnStartPressed;
		unlocksButton.Pressed += OnUnlocksPressed;
		optionsButton.Pressed += OnOptionsPressed;
		quitButton.Pressed += OnQuitPressed;
	}

	private void OnStartPressed()
	{
		GD.Print("Start pressed");
		GetTree().ChangeSceneToFile("res://Scenes/Menus/CharacterSelect.tscn");
	}

	private void OnUnlocksPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/UnlocksMenu.tscn");
	}

	private void OnOptionsPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/OptionsMenu.tscn");
	}

	private void OnQuitPressed()
	{
		GD.Print("Quit pressed");
		GetTree().Quit();
	}
}
