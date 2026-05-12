using Godot;

public partial class CharacterSelect : Control
{
	private OptionButton characterOptionButton;
	private OptionButton levelOptionButton;
	private Button confirmButton;
	private Button backButton;

	private TextureRect characterPreviewTexture;
	private TextureRect levelPreviewTexture;

	private readonly string iceWizardPreviewPath = "res://Assets/IceWizard.png";

	public override void _Ready()
	{
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
	}

	private void OnConfirmPressed()
	{
		string selectedCharacter = characterOptionButton.GetItemText(characterOptionButton.Selected);
		string selectedLevel = levelOptionButton.GetItemText(levelOptionButton.Selected);

		GD.Print("Selected character: " + selectedCharacter);
		GD.Print("Selected level: " + selectedLevel);

		GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
