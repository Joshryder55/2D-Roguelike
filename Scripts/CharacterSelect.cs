using Godot;

public partial class CharacterSelect : Control
{
	private const int FireWizardUnlockCost = 500;

	private OptionButton characterOptionButton;
	private OptionButton levelOptionButton;
	private Button confirmButton;
	private Button backButton;
	private Button buyButton; // shown when selecting a locked wizard
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

		// Buy button — appears when selecting a locked wizard
		buyButton = new Button();
		buyButton.Text = "Unlock for 500 coins";
		buyButton.Visible = false;
		buyButton.Pressed += OnBuyPressed;
		GetNode("CenterContainer/HBoxContainer/VBoxContainer").AddChild(buyButton);

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
		characterOptionButton.AddItem("Fire Wizard");
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

		if (selectedLevel == "Level 1")
			levelPreviewTexture.Texture = null;
		if (selectedLevel == "Level 2")
			levelPreviewTexture.Texture = null;

		bool isFireWizard = selectedCharacter.Contains("Fire Wizard");
		bool isIceWizard  = selectedCharacter.Contains("Ice Wizard");
		bool canAfford    = gameManager.coins >= FireWizardUnlockCost;

		// No character picked yet — both free, no lock prompt
		if (saveData.selectedCharacter == PlayerSaveData.Character.None)
		{
			lockLabel.Text        = "";
			buyButton.Visible     = false;
			confirmButton.Visible = true;
			return;
		}

		// Check if selected wizard is locked
		bool selectedIsLocked =
			(isFireWizard && !saveData.hasUnlockedFireWizard) ||
			(isIceWizard  && !saveData.hasUnlockedIceWizard);

		if (selectedIsLocked)
		{
			lockLabel.Text        = canAfford
				? "This wizard costs 500 coins to unlock."
				: "Not enough coins! This wizard costs 500 coins.";
			buyButton.Visible     = canAfford;
			confirmButton.Visible = false;
		}
		else
		{
			lockLabel.Text        = "";
			buyButton.Visible     = false;
			confirmButton.Visible = true;
		}
	}

	private void OnBuyPressed()
	{
		if (gameManager.coins < FireWizardUnlockCost) return;

		string selected   = characterOptionButton.GetItemText(characterOptionButton.Selected);
		bool isFireWizard = selected.Contains("Fire Wizard");

		gameManager.coins -= FireWizardUnlockCost;

		if (isFireWizard)
			saveData.hasUnlockedFireWizard = true;
		else
			saveData.hasUnlockedIceWizard = true;

		GetNode<SaveSystem>("/root/SaveSystem").Save();
		UpdateCoinsLabel();
		UpdatePreviews();
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

		// First time picking — free choice, lock the other
		if (saveData.selectedCharacter == PlayerSaveData.Character.None)
		{
			saveData.selectedCharacter = isFireWizard
				? PlayerSaveData.Character.FireWizard
				: PlayerSaveData.Character.IceWizard;

			// Chosen wizard is unlocked, other is locked
			saveData.hasUnlockedFireWizard = isFireWizard;
			saveData.hasUnlockedIceWizard  = isIceWizard;

			GetNode<SaveSystem>("/root/SaveSystem").Save();
		}
		else
		{
			// Switching character — only possible if already unlocked (confirm only shows when unlocked)
			saveData.selectedCharacter = isFireWizard
				? PlayerSaveData.Character.FireWizard
				: PlayerSaveData.Character.IceWizard;
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
