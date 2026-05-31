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

	// Wizard select popup (shown when both wizards are unlocked)
	private Panel wizardSelectPanel;
	private Button iceWizardTreeButton;
	private Button fireWizardTreeButton;
	private Button wizardSelectCancelButton;

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

		// Build New Game confirmation popup in code
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

		// Build Wizard Select popup in code
		wizardSelectPanel = new Panel();
		wizardSelectPanel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
		wizardSelectPanel.CustomMinimumSize = new Vector2(350, 180);
		wizardSelectPanel.Visible = false;
		AddChild(wizardSelectPanel);

		var wsMargin = new MarginContainer();
		wsMargin.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		wsMargin.AddThemeConstantOverride("margin_left", 20);
		wsMargin.AddThemeConstantOverride("margin_right", 20);
		wsMargin.AddThemeConstantOverride("margin_top", 20);
		wsMargin.AddThemeConstantOverride("margin_bottom", 20);
		wizardSelectPanel.AddChild(wsMargin);

		var wsVbox = new VBoxContainer();
		wsVbox.AddThemeConstantOverride("separation", 12);
		wsMargin.AddChild(wsVbox);

		var wsLabel = new Label();
		wsLabel.Text = "Which skill tree would you like to view?";
		wsLabel.HorizontalAlignment = HorizontalAlignment.Center;
		wsVbox.AddChild(wsLabel);

		var wsHbox = new HBoxContainer();
		wsHbox.Alignment = BoxContainer.AlignmentMode.Center;
		wsHbox.AddThemeConstantOverride("separation", 12);
		wsVbox.AddChild(wsHbox);

		iceWizardTreeButton = new Button();
		iceWizardTreeButton.Text = "Ice Wizard";
		iceWizardTreeButton.Pressed += () => {
			wizardSelectPanel.Visible = false;
			GetTree().ChangeSceneToFile("res://Scenes/Menus/UnlocksMenu.tscn");
		};
		wsHbox.AddChild(iceWizardTreeButton);

		fireWizardTreeButton = new Button();
		fireWizardTreeButton.Text = "Fire Wizard";
		fireWizardTreeButton.Pressed += () => {
			wizardSelectPanel.Visible = false;
			GetTree().ChangeSceneToFile("res://Scenes/Menus/FireWizardUnlocksMenu.tscn");
		};
		wsHbox.AddChild(fireWizardTreeButton);

		wizardSelectCancelButton = new Button();
		wizardSelectCancelButton.Text = "Cancel";
		wizardSelectCancelButton.Pressed += () => wizardSelectPanel.Visible = false;
		wsVbox.AddChild(wizardSelectCancelButton);

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
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		// Both wizards unlocked — show picker
		if (saveData.hasUnlockedIceWizard && saveData.hasUnlockedFireWizard)
		{
			wizardSelectPanel.Visible = true;
			return;
		}

		// Route to whichever tree they own
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
