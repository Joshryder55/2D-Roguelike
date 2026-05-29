using Godot;

public partial class CharacterSelect : Control
{
	private const int FireWizardUnlockCost = 500;

	private OptionButton characterOptionButton;
	private OptionButton levelOptionButton;
	private Button confirmButton;
	private Button backButton;
	private TextureRect characterPreviewTexture;
	private TextureRect levelPreviewTexture;
	private Label coinsLabel;
	private Label lockLabel;

	private readonly string iceWizardPreviewPath = "res://Assets/IceWizard.png";

	private GameManager gameManager;
	private PlayerSaveData saveData;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		saveData    = GetNode<PlayerSaveData>("/root/PlayerSaveData");

		characterOptionButton  = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/CharacterOptionButton");
		levelOptionButton      = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/LevelOptionButton");
		confirmButton          = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/ConfirmButton");
		backButton             = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/BackButton");
		characterPreviewTexture = GetNode<TextureRect>("CenterContainer/HBoxContainer/PreviewVBox/CharacterPreviewTexture");
		levelPreviewTexture    = GetNode<TextureRect>("CenterContainer/HBoxContainer/PreviewVBox/LevelPreviewTexture");

		// Add coins label and lock label in code
		coinsLabel = new Label();
		coinsLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopRight);
		coinsLabel.OffsetLeft = -200; coinsLabel.OffsetTop = 10;
		coinsLabel.OffsetRight = -10; coinsLabel.OffsetBottom = 40;
		AddChild(coinsLabel);

		lockLabel = new Label();
		lockLabel.HorizontalAlignment = HorizontalAlignment.Center;
		lockLabel.Modulate = new Color(1f, 0.4f, 0.4f);
		GetNode("CenterContainer/HBoxContainer/VBoxContainer").AddChild(lockLabel);

		SetupCharacterOptions();
		SetupLevelOptions();
		UpdatePreviews();
		UpdateCoinsLabel();

		characterOptionButton.ItemSelected += OnCharacterSelected;
		levelOptionButton.ItemSelected     += OnLevelSelected;
		confirmButton.Pressed              += OnConfirmPressed;
		backButton.Pressed                 += OnBackPressed;

		GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();
	}

	private void SetupCharacterOptions()
	{
		characterOptionButton.Clear();
		characterOptionButton.AddItem("Ice Wizard");

		// Fire Wizard — show as locked if not purchased
		if (saveData.selectedCharacter == PlayerSaveData.Character.FireWizard ||
			gameManager.coins >= FireWizardUnlockCost)
			characterOptionButton.AddItem("Fire Wizard");
		else
			characterOptionButton.AddItem("Fire Wizard (500 coins to unlock)");
	}

	private void SetupLevelOptions()
	{
		levelOptionButton.Clear();
		levelOptionButton.AddItem("Level 1");
		levelOptionButton.AddItem("Level 2");
	}

	private void OnCharacterSelected(long index)
	{
		UpdatePreviews();
		UpdateCoinsLabel();
	}

	private void OnLevelSelected(long index)
	{
		UpdatePreviews();
	}

	private void UpdatePreviews()
	{
		string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
		string selectedLevel     = levelOptionButton.GetItemText(levelOptionButton.Selected);

		// Update character preview
		if (selectedCharacter.Contains("Ice Wizard"))
			characterPreviewTexture.Texture = GD.Load<Texture2D>(iceWizardPreviewPath);
		else
			characterPreviewTexture.Texture = null; // Fire Wizard image when available

		levelPreviewTexture.Texture = null;

		// Show lock warning
		bool isFireWizard     = selectedCharacter.Contains("Fire Wizard");
		bool fireWizardOwned  = saveData.selectedCharacter == PlayerSaveData.Character.FireWizard
								|| saveData.selectedCharacter == PlayerSaveData.Character.None;
		bool canAfford        = gameManager.coins >= FireWizardUnlockCost;

		if (isFireWizard && saveData.selectedCharacter == PlayerSaveData.Character.IceWizard && !canAfford)
			lockLabel.Text = "Not enough coins! Fire Wizard costs 500 coins.";
		else if (isFireWizard && saveData.selectedCharacter == PlayerSaveData.Character.IceWizard && canAfford)
			lockLabel.Text = "Selecting Fire Wizard will cost 500 coins.";
		else
			lockLabel.Text = "";
	}

	private void UpdateCoinsLabel()
	{
		coinsLabel.Text = "Coins: " + gameManager.coins;
	}

	private void OnConfirmPressed()
	{
		string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
		string selectedLevel     = levelOptionButton.GetItemText(levelOptionButton.Selected);

		bool isFireWizard = selectedCharacter.Contains("Fire Wizard");
		bool isIceWizard  = selectedCharacter.Contains("Ice Wizard");

		// First time picking — free choice
		if (saveData.selectedCharacter == PlayerSaveData.Character.None)
		{
			saveData.selectedCharacter = isFireWizard
				? PlayerSaveData.Character.FireWizard
				: PlayerSaveData.Character.IceWizard;
			GetNode<SaveSystem>("/root/SaveSystem").Save();
		}
		// Switching to the other wizard — costs coins
		else if (isFireWizard && saveData.selectedCharacter == PlayerSaveData.Character.IceWizard)
		{
			if (gameManager.coins < FireWizardUnlockCost) {
				lockLabel.Text = "Not enough coins! Fire Wizard costs 500 coins.";
				return;
			}
			gameManager.coins -= FireWizardUnlockCost;
			saveData.selectedCharacter = PlayerSaveData.Character.FireWizard;
			GetNode<SaveSystem>("/root/SaveSystem").Save();
		}
		else if (isIceWizard && saveData.selectedCharacter == PlayerSaveData.Character.FireWizard)
		{
			if (gameManager.coins < FireWizardUnlockCost) {
				lockLabel.Text = "Not enough coins! Ice Wizard costs 500 coins.";
				return;
			}
			gameManager.coins -= FireWizardUnlockCost;
			saveData.selectedCharacter = PlayerSaveData.Character.IceWizard;
			GetNode<SaveSystem>("/root/SaveSystem").Save();
		}

		GD.Print("Selected character: " + selectedCharacter);
		GD.Print("Selected level: " + selectedLevel);

		GetNode<MusicManager>("/root/MusicManager").StopMenuMusic();

		switch (selectedLevel)
		{
			case "Level 1":
				GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
				break;
			case "Level 2":
				GetTree().ChangeSceneToFile("res://Scenes/Level2.tscn");
				break;
		}
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
