using Godot;

public partial class PauseMenu : Control
{
	private Button _resumeButton;
	private Button _mainMenuButton;
	private Button _quitButton;

	public override void _Ready()
	{
		_resumeButton   = GetNode<Button>("CenterContainer/VBoxContainer/ResumeButton");
		_mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");
		_quitButton     = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		_resumeButton.Pressed   += OnResumePressed;
		_mainMenuButton.Pressed += OnMainMenuPressed;
		_quitButton.Pressed     += OnQuitPressed;

		Visible     = false;
		ProcessMode = ProcessModeEnum.Always;
		GetNode<Control>("CenterContainer").ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_game"))
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
		}
	}

	private void TogglePause()
	{
		ApplyPause(!GetTree().Paused);
	}

	private void ApplyPause(bool pausing)
	{
		GetTree().Paused = pausing;
		Visible          = pausing;
		// No GameManager calls needed — GameManager._Process checks
		// GetTree().Paused itself, so the timer stops automatically
	}

	private void OnResumePressed()
	{
		ApplyPause(false);
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
