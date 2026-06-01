using Godot;
public partial class MainMenu : Control
{
	private Button startButton;
	private Button unlocksButton;
	private Button optionsButton;
	private Button quitButton;

	// How to Play
	private Button howToPlayButton;
	private Control howToPlayPanel;

	public override void _Ready()
	{
		GetNode<SaveSystem>("/root/SaveSystem").Load();

		startButton   = GetNode<Button>("CenterContainer/VBoxContainer/StartButton");
		unlocksButton = GetNode<Button>("CenterContainer/VBoxContainer/UnlocksButton");
		optionsButton = GetNode<Button>("CenterContainer/VBoxContainer/OptionsButton");
		quitButton    = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");
		
		var newGameButton = new Button();
		newGameButton.Text = "New Game";
		newGameButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomLeft);
		newGameButton.OffsetLeft   = 10;
		newGameButton.OffsetTop    = -45;
		newGameButton.OffsetRight  = 130;
		newGameButton.OffsetBottom = -10;
		newGameButton.Pressed += OnNewGamePressed;
		AddChild(newGameButton);

		startButton.Pressed   += OnStartPressed;
		unlocksButton.Pressed += OnUnlocksPressed;
		optionsButton.Pressed += OnOptionsPressed;
		quitButton.Pressed    += OnQuitPressed;
		
		GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();

		BuildHowToPlayButton();
		BuildHowToPlayPanel();
	}

	private void BuildHowToPlayButton()
	{
		howToPlayButton = new Button();
		howToPlayButton.Text = "How to Play";
		howToPlayButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomRight);
		howToPlayButton.OffsetLeft   = -130;
		howToPlayButton.OffsetTop    = -45;
		howToPlayButton.OffsetRight  = -10;
		howToPlayButton.OffsetBottom = -10;
		howToPlayButton.Pressed += OnHowToPlayPressed;
		AddChild(howToPlayButton);
	}

	private void BuildHowToPlayPanel()
	{
		howToPlayPanel = new Control();
		howToPlayPanel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		howToPlayPanel.Visible = false;
		AddChild(howToPlayPanel);

		var bg = new ColorRect();
		bg.Color = new Color(0f, 0f, 0f, 0.85f);
		bg.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		howToPlayPanel.AddChild(bg);

		var box = new PanelContainer();
		box.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
		box.OffsetLeft   = -400;
		box.OffsetTop    = -300;
		box.OffsetRight  =  400;
		box.OffsetBottom =  300;
		howToPlayPanel.AddChild(box);

		var outerVBox = new VBoxContainer();
		outerVBox.AddThemeConstantOverride("separation", 8);
		box.AddChild(outerVBox);

		var title = new Label();
		title.Text = "How to Play";
		title.HorizontalAlignment = HorizontalAlignment.Center;
		title.AddThemeFontSizeOverride("font_size", 22);
		outerVBox.AddChild(title);

		outerVBox.AddChild(new HSeparator());

		var content = new Label();
		content.AutowrapMode = TextServer.AutowrapMode.Word;
		content.AddThemeFontSizeOverride("font_size", 14);
		content.CustomMinimumSize = new Vector2(750, 0);
		content.Text =
			"MOVEMENT\n" +
			"  WASD — Move your wizard\n\n" +
			"COMBAT\n" +
			"  Your wizard shoots automatically at nearby enemies. Survive as long as possible!\n\n" +
			"ULTIMATE\n" +
			"  Space — Activate your ultimate ability. Charge it by defeating enemies.\n" +
			"  Once you purchase an ultimate, don't forget to equip it using the Set Active button in the Unlocks menu.\n\n" +
			"LEVELING UP\n" +
			"  Collect XP orbs dropped by enemies to level up. Each level up lets you choose a stat upgrade mid-run.\n\n" +
			"COINS\n" +
			"  Enemies drop coins — collect them during your run. Coins persist after death and can be spent\n" +
			"  in the Unlocks menu to permanently upgrade your wizard.\n\n" +
			"UNLOCKS MENU\n" +
			"  Spend coins on permanent upgrades between runs. Unlock multiple ultimates and choose which\n" +
			"  one to bring into battle using the Set Active button.";
		outerVBox.AddChild(content);

		outerVBox.AddChild(new HSeparator());

		var closeButton = new Button();
		closeButton.Text = "Close";
		closeButton.CustomMinimumSize = new Vector2(0, 34);
		closeButton.Pressed += () => howToPlayPanel.Visible = false;
		outerVBox.AddChild(closeButton);
	}

	private void OnHowToPlayPressed()  { howToPlayPanel.Visible = true; }
	private void OnStartPressed()      { GD.Print("Start pressed"); GetTree().ChangeSceneToFile("res://Scenes/Menus/CharacterSelect.tscn"); }
	private void OnUnlocksPressed()    { GetTree().ChangeSceneToFile("res://Scenes/Menus/UnlocksMenu.tscn"); }
	private void OnOptionsPressed()    { GetTree().ChangeSceneToFile("res://Scenes/Menus/OptionsMenu.tscn"); }
	private void OnQuitPressed()       { GD.Print("Quit pressed"); GetTree().Quit(); }
	private void OnNewGamePressed() {
		GetNode<SaveSystem>("/root/SaveSystem").ResetData();
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
