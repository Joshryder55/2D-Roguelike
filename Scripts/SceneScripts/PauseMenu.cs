using Godot;

public partial class PauseMenu : Control
{
	private Button resumeButton;
	private Button mainMenuButton;
	private Button quitButton;

	public override void _Ready()
	{
		resumeButton = GetNode<Button>("CenterContainer/VBoxContainer/ResumeButton");
		mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");
		quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		resumeButton.Pressed += OnResumePressed;
		mainMenuButton.Pressed += OnMainMenuPressed;
		quitButton.Pressed += OnQuitPressed;

		Visible = false;
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_game"))
		{
			TogglePause();
		}
	}

	private void TogglePause()
	{
		bool shouldPause = !GetTree().Paused;

		GetTree().Paused = shouldPause;
		Visible = shouldPause;
	}

	private void OnResumePressed()
	{
		GetTree().Paused = false;
		Visible = false;
	}

	private void OnMainMenuPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
