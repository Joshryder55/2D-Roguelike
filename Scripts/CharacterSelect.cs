using Godot;

public partial class CharacterSelect : Control
{
	GameManager gameManager;
	
	private OptionButton characterOptionButton;
	private OptionButton levelOptionButton;
	private Button confirmButton;
	private Button backButton;

	private TextureRect characterPreviewTexture;
	private TextureRect levelPreviewTexture;

	private readonly string iceWizardPreviewPath = "res://Assets/IceWizard.png";

	public override void _Ready()
	{
		
		gameManager = GetNode<GameManager>("/root/GameManager");


		characterOptionButton = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/CharacterOptionButton");
		levelOptionButton = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/LevelOptionButton");
		confirmButton = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/ConfirmButton");
		backButton = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/BackButton");

		characterPreviewTexture = GetNode<TextureRect>("CenterContainer/HBoxContainer/PreviewVBox/CharacterPreviewTexture");
		levelPreviewTexture = GetNode<TextureRect>("CenterContainer/HBoxContainer/PreviewVBox/LevelPreviewTexture");

		SetupCharacterOptions();
		SetupLevelOptions();
		UpdatePreviews();

		characterOptionButton.ItemSelected += OnCharacterSelected;
		levelOptionButton.ItemSelected += OnLevelSelected;
		confirmButton.Pressed += OnConfirmPressed;
		backButton.Pressed += OnBackPressed;

		GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();
	}

	private void SetupCharacterOptions()
	{
		characterOptionButton.Clear();

		characterOptionButton.AddItem("Ice Wizard");
	}

	private void SetupLevelOptions()
	{
		levelOptionButton.Clear();

		levelOptionButton.AddItem("Level 1");
		
		// TODO: Unlock Level 2 after beating Level 1
		// Uncomment when unlock system is ready:
		// PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		// if (saveData.hasCompletedLevel1)
		// {
		//     levelOptionButton.AddItem("Level 2");
		// }


		// For now add Level 2 & 3 always for testing
		levelOptionButton.AddItem("Level 2");
		levelOptionButton.AddItem("Level 3");
		
	}

	private void OnCharacterSelected(long index)
	{
		UpdatePreviews();
	}

	private void OnLevelSelected(long index)
	{
		UpdatePreviews();
	}

	private void UpdatePreviews()
	{
		string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
		string selectedLevel = levelOptionButton.GetItemText(levelOptionButton.Selected);

		if (selectedCharacter == "Ice Wizard")
		{
			characterPreviewTexture.Texture = GD.Load<Texture2D>(iceWizardPreviewPath);
		}

		if (selectedLevel == "Level 1")
		{
			// No level image yet, so leave this blank for now.
			levelPreviewTexture.Texture = null;
		}
		
		if(selectedLevel == "Level 2"){
			
			levelPreviewTexture.Texture = null;
		}
		
		if(selectedLevel == "Level 3"){
			
			levelPreviewTexture.Texture = null;
		}
	}

	private void OnConfirmPressed()
{
	string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
	string selectedLevel = levelOptionButton.GetItemText(levelOptionButton.Selected);
	GD.Print("Selected character: " + selectedCharacter);
	GD.Print("Selected level: " + selectedLevel);

	GetNode<MusicManager>("/root/MusicManager").StopMenuMusic();

	// Set selected character in save data
	PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
	switch (selectedCharacter) {
		case "Ice Wizard":
			saveData.selectedCharacter = PlayerSaveData.Character.IceWizard;
			break;
		case "Fire Wizard":
			saveData.selectedCharacter = PlayerSaveData.Character.FireWizard;
			break;
	}

	switch (selectedLevel)
	{
		case "Level 1":
			gameManager.currentLevel = "res://Scenes/Level1.tscn";
			GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
			break;
		case "Level 2":
			gameManager.currentLevel = "res://Scenes/Level2.tscn";
			GetTree().ChangeSceneToFile("res://Scenes/Level2.tscn");
			break;
		case "Level 3":
			gameManager.currentLevel = "res://Scenes/Level3.tscn";
			GetTree().ChangeSceneToFile("res://Scenes/Level3.tscn");
			break;
	}
}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
