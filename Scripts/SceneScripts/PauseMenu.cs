using Godot;

public partial class PauseMenu : Control
{
	private Control _mainPanel;
	private Control _optionsPanel;

	private Button _resumeButton;
	private Button _optionsButton;
	private Button _mainMenuButton;
	private Button _quitButton;

	private HSlider _musicSlider;
	private HSlider _sfxSlider;
	private HSlider _attackSlider;
	private Button _backButton;

	private SoundManager _soundManager;

	public override void _Ready()
	{
		_mainPanel    = GetNode<Control>("CenterContainer");
		_optionsPanel = GetNode<Control>("OptionsPanel");

		_resumeButton   = GetNode<Button>("CenterContainer/VBoxContainer/ResumeButton");
		_optionsButton  = GetNode<Button>("CenterContainer/VBoxContainer/OptionsButton");
		_mainMenuButton = GetNode<Button>("CenterContainer/VBoxContainer/MainMenuButton");
		_quitButton     = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		_musicSlider  = GetNode<HSlider>("OptionsPanel/VBoxContainer/MusicSlider");
		_sfxSlider    = GetNode<HSlider>("OptionsPanel/VBoxContainer/SfxSlider");
		_attackSlider = GetNode<HSlider>("OptionsPanel/VBoxContainer/AttackSlider");
		_backButton   = GetNode<Button>("OptionsPanel/VBoxContainer/BackButton");

		_resumeButton.Pressed   += OnResumePressed;
		_optionsButton.Pressed  += OnOptionsPressed;
		_mainMenuButton.Pressed += OnMainMenuPressed;
		_quitButton.Pressed     += OnQuitPressed;
		_backButton.Pressed     += OnOptionsBackPressed;

		_soundManager = GetNode<SoundManager>("/root/SoundManager");

		// Seed sliders from the current saved volumes
		_musicSlider.Value  = _soundManager.MusicVolume * 100f;
		_sfxSlider.Value    = _soundManager.SfxVolume * 100f;
		_attackSlider.Value = _soundManager.AttackVolume * 100f;
		_musicSlider.ValueChanged  += OnMusicVolumeChanged;
		_sfxSlider.ValueChanged    += OnSfxVolumeChanged;
		_attackSlider.ValueChanged += OnAttackVolumeChanged;

		Visible     = false;
		ProcessMode = ProcessModeEnum.Always;
		_mainPanel.ProcessMode    = ProcessModeEnum.Always;
		_optionsPanel.ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_game"))
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
		}
	}

	private void TogglePause()
	{
		ApplyPause(!GetTree().Paused);
	}

	private void ApplyPause(bool pausing)
	{
		GetTree().Paused = pausing;
		Visible          = pausing;
		ShowOptions(false);
		// No GameManager calls needed — GameManager._Process checks
		// GetTree().Paused itself, so the timer stops automatically
	}

	private void ShowOptions(bool showing)
	{
		_optionsPanel.Visible = showing;
		_mainPanel.Visible    = !showing;
	}

	private void OnResumePressed()
	{
		ApplyPause(false);
	}

	private void OnOptionsPressed()
	{
		ShowOptions(true);
	}

	private void OnOptionsBackPressed()
	{
		ShowOptions(false);
	}

	private void OnMusicVolumeChanged(double value)
	{
		_soundManager.SetMusicVolume((float)value / 100f);
	}

	private void OnSfxVolumeChanged(double value)
	{
		_soundManager.SetSfxVolume((float)value / 100f);
	}

	private void OnAttackVolumeChanged(double value)
	{
		_soundManager.SetAttackVolume((float)value / 100f);
	}

	private void OnMainMenuPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
