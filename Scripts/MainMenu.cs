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
		GD.Print("Unlocks menu will be added later.");
	}

	private void OnOptionsPressed()
	{
		GD.Print("Options menu will be added later.");
	}

	private void OnQuitPressed()
	{
		GD.Print("Quit pressed");
		GetTree().Quit();
	}
}
