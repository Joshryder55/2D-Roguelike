using Godot;

public partial class OptionsMenu : Control
{
	private HSlider musicSlider;
	private HSlider sfxSlider;
	private HSlider attackSlider;
	private CheckBox fullscreenCheckBox;
	private Button backButton;

	private SoundManager soundManager;

	public override void _Ready()
	{
		musicSlider        = GetNode<HSlider>("CenterContainer/VBoxContainer/MusicSlider");
		sfxSlider          = GetNode<HSlider>("CenterContainer/VBoxContainer/SfxSlider");
		attackSlider       = GetNode<HSlider>("CenterContainer/VBoxContainer/AttackSlider");
		fullscreenCheckBox = GetNode<CheckBox>("CenterContainer/VBoxContainer/FullscreenCheckBox");
		backButton         = GetNode<Button>("CenterContainer/VBoxContainer/BackButton");

		soundManager = GetNode<SoundManager>("/root/SoundManager");

		musicSlider.Value  = soundManager.MusicVolume * 100f;
		sfxSlider.Value    = soundManager.SfxVolume * 100f;
		attackSlider.Value = soundManager.AttackVolume * 100f;
		musicSlider.ValueChanged  += OnMusicVolumeChanged;
		sfxSlider.ValueChanged    += OnSfxVolumeChanged;
		attackSlider.ValueChanged += OnAttackVolumeChanged;

		fullscreenCheckBox.Toggled += OnFullscreenToggled;
		backButton.Pressed += OnBackPressed;

		fullscreenCheckBox.ButtonPressed =
			DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen ||
			DisplayServer.WindowGetMode() == DisplayServer.WindowMode.ExclusiveFullscreen;

		GetNode<MusicManager>("/root/MusicManager").PlayMenuMusic();
	}

	private void OnMusicVolumeChanged(double value)
	{
		soundManager.SetMusicVolume((float)value / 100f);
	}

	private void OnSfxVolumeChanged(double value)
	{
		soundManager.SetSfxVolume((float)value / 100f);
	}

	private void OnAttackVolumeChanged(double value)
	{
		soundManager.SetAttackVolume((float)value / 100f);
	}

	private void OnFullscreenToggled(bool enabled)
	{
		if (enabled)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
