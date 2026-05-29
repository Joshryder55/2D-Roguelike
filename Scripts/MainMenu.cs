using Godot;
public partial class MainMenu : Control
{
	private Button startButton;
	private Button unlocksButton;
	private Button optionsButton;
	private Button quitButton;
	private Button newGameButton;

	// New Game confirmation popup
	private Panel confirmPanel;
	private Button confirmYesButton;
	private Button confirmNoButton;

	public override void _Ready()
	{
		startButton   = GetNode<Button>("CenterContainer/VBoxContainer/StartButton");
		unlocksButton = GetNode<Button>("CenterContainer/VBoxContainer/UnlocksButton");
		optionsButton = GetNode<Button>("CenterContainer/VBoxContainer/OptionsButton");
		quitButton    = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");
		newGameButton = GetNode<Button>("CenterContainer/VBoxContainer/NewGameButton");

		startButton.Pressed   += OnStartPressed;
		unlocksButton.Pressed += OnUnlocksPressed;
		optionsButton.Pressed += OnOptionsPressed;
		quitButton.Pressed    += OnQuitPressed;
		newGameButton.Pressed += OnNewGamePressed;

		// Build confirmation popup in code
		confirmPanel = new Panel();
		confirmPanel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
		confirmPanel.CustomMinimumSize = new Vector2(400, 160);
		confirmPanel.Visible = false;
		AddChild(confirmPanel);

		var vbox = new VBoxContainer();
		vbox.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		vbox.AddThemeConstantOverride("separation", 12);
		confirmPanel.AddChild(vbox);

		var margin = new MarginContainer();
		margin.AddThemeConstantOverride("margin_left", 20);
		margin.AddThemeConstantOverride("margin_right", 20);
		margin.AddThemeConstantOverride("margin_top", 20);
		margin.AddThemeConstantOverride("margin_bottom", 20);
		vbox.AddChild(margin);

		var innerVbox = new VBoxContainer();
		innerVbox.AddThemeConstantOverride("separation", 12);
		margin.AddChild(innerVbox);

		var label = new Label();
		label.Text = "Start a New Game?\nThis will wipe your current progress.";
		label.HorizontalAlignment = HorizontalAlignment.Center;
		innerVbox.AddChild(label);

		var hbox = new HBoxContainer();
		hbox.Alignment = BoxContainer.AlignmentMode.Center;
		hbox.AddThemeConstantOverride("separation", 20);
		innerVbox.AddChild(hbox);

		confirmYesButton = new Button();
		confirmYesButton.Text = "Yes, New Game";
		confirmYesButton.Pressed += OnConfirmNewGame;
		hbox.AddChild(confirmYesButton);

		confirmNoButton = new Button();
		confirmNoButton.Text = "Cancel";
		confirmNoButton.Pressed += () => confirmPanel.Visible = false;
		hbox.AddChild(confirmNoButton);

		// Load save data
		GetNode<SaveSystem>("/root/SaveSystem").Load();
		GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();
	}

	private void OnStartPressed()
	{
		GD.Print("Start pressed");
		GetTree().ChangeSceneToFile("res://Scenes/Menus/CharacterSelect.tscn");
	}

	private void OnUnlocksPressed()
	{
		// Route to correct unlock tree based on selected character
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		if (saveData.selectedCharacter == PlayerSaveData.Character.FireWizard)
			GetTree().ChangeSceneToFile("res://Scenes/Menus/FireWizardUnlocksMenu.tscn");
		else
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

	private void OnNewGamePressed()
	{
		confirmPanel.Visible = true;
	}

	private void OnConfirmNewGame()
	{
	confirmPanel.Visible = false;
	// Fully reset everything in memory and on disk
	GetNode<SaveSystem>("/root/SaveSystem").ResetData();
	// Stay on main menu — player clicks Start to pick their wizard
	}
}
