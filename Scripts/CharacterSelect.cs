using Godot;
public partial class CharacterSelect : Control
{
	GameManager gameManager;
	
	private OptionButton characterOptionButton;
	private OptionButton levelOptionButton;
	private Button confirmButton;
	private Button backButton;
	private TextureRect characterPreviewTexture;
	private readonly string iceWizardPreviewPath = "res://Assets/IceWizard.png";
	public override void _Ready()
	{
		
		gameManager = GetNode<GameManager>("/root/GameManager");
		characterOptionButton = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/CharacterOptionButton");
		levelOptionButton = GetNode<OptionButton>("CenterContainer/HBoxContainer/VBoxContainer/LevelOptionButton");
		confirmButton = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/ConfirmButton");
		backButton = GetNode<Button>("CenterContainer/HBoxContainer/VBoxContainer/BackButton");
		characterPreviewTexture = GetNode<TextureRect>("CenterContainer/HBoxContainer/PreviewVBox/CharacterPreviewTexture");
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
		PlayerSaveData saveData = GetNode<PlayerSaveData>("/root/PlayerSaveData");
		levelOptionButton.Clear();
		levelOptionButton.AddItem("Level 1");
		if (saveData.hasCompletedLevel1)
			levelOptionButton.AddItem("Level 2");
		if (saveData.hasCompletedLevel2)
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
		if (selectedCharacter == "Ice Wizard")
		{
			characterPreviewTexture.Texture = GD.Load<Texture2D>(iceWizardPreviewPath);
		}
	}
	private void OnConfirmPressed()
	{
		string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
		string selectedLevel = levelOptionButton.GetItemText(levelOptionButton.Selected);
		GD.Print("Selected character: " + selectedCharacter);
		GD.Print("Selected level: " + selectedLevel);

		gameManager.ResetForNewRun();
	  
		GetNode<MusicManager>("/root/MusicManager").StopMenuMusic();
	
		switch (selectedLevel)
		{
			case "Level 1":
				gameManager.currentLevel = "res://Scenes/Level1.tscn";
				GetTree().ChangeSceneToFile("res://Scenes/LoreScreens/Level1Lore.tscn");
				break;
			case "Level 2":
				gameManager.currentLevel = "res://Scenes/Level2.tscn";
				GetTree().ChangeSceneToFile("res://Scenes/LoreScreens/Level2Lore.tscn");
				break;
			case "Level 3":
				gameManager.currentLevel = "res://Scenes/Level3.tscn";
				GetTree().ChangeSceneToFile("res://Scenes/LoreScreens/Level3Lore.tscn");
				break;
		}
	}
	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
