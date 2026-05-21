using Godot;

public partial class PauseMenu : Control
{
	private Button resumeButton;
	private Button mainMenuButton;
	private Button unlocksButton;
	private Button quitButton;
	private CenterContainer centerContainer;
	private bool isShowingUnlocks = false;

	public override void _Ready()
	{
		centerContainer = GetNode<CenterContainer>("CenterContainer");
		resumeButton = GetNode<Button>("CenterContainer/VBoxContainer/ResumeButton");
		mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");
		unlocksButton = GetNode<Button>("CenterContainer/VBoxContainer/UnlocksButton");
		quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		resumeButton.Pressed += OnResumePressed;
		mainMenuButton.Pressed += OnMainMenuPressed;
		unlocksButton.Pressed += OnUnlocksPressed;
		quitButton.Pressed += OnQuitPressed;

		Visible = false;
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_game") && !isShowingUnlocks)
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
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

	private void OnUnlocksPressed()
	{
		isShowingUnlocks = true;
		centerContainer.Visible = false;
		PackedScene scene = GD.Load<PackedScene>("res://Scenes/Menus/UnlocksMenu.tscn");
		UnlocksMenu menu = scene.Instantiate<UnlocksMenu>();
		menu.openedFromGame = true;
		menu.ProcessMode = ProcessModeEnum.Always;
		menu.OnClose += OnUnlocksMenuClosed;
		AddChild(menu);
	}

	private void OnUnlocksMenuClosed()
	{
		isShowingUnlocks = false;
		centerContainer.Visible = true;
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
