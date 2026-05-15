using Godot;

public partial class OptionsMenu : Control
{
	private HSlider volumeSlider;
	private CheckBox fullscreenCheckBox;
	private Button backButton;

	public override void _Ready()
	{
		volumeSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/VolumeSlider");
		fullscreenCheckBox = GetNode<CheckBox>("CenterContainer/VBoxContainer/FullscreenCheckBox");
		backButton = GetNode<Button>("CenterContainer/VBoxContainer/BackButton");

		volumeSlider.ValueChanged += OnVolumeChanged;
		fullscreenCheckBox.Toggled += OnFullscreenToggled;
		backButton.Pressed += OnBackPressed;

		volumeSlider.Value = 80;

		fullscreenCheckBox.ButtonPressed =
			DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen ||
			DisplayServer.WindowGetMode() == DisplayServer.WindowMode.ExclusiveFullscreen;
	}

	private void OnVolumeChanged(double value)
	{
		float volumePercent = (float)value / 100f;
		int masterBusIndex = AudioServer.GetBusIndex("Master");

		if (volumePercent <= 0)
		{
			AudioServer.SetBusMute(masterBusIndex, true);
		}
		else
		{
			AudioServer.SetBusMute(masterBusIndex, false);
			AudioServer.SetBusVolumeDb(masterBusIndex, Mathf.LinearToDb(volumePercent));
		}

		GD.Print("Volume changed to: " + value);
	}

	private void OnFullscreenToggled(bool enabled)
	{
		GD.Print("Fullscreen toggled: " + enabled);

		if (enabled)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	private void OnBackPressed()
	{
		GD.Print("Back pressed");
		GetTree().ChangeSceneToFile("res://Scenes/Menus/MainMenu.tscn");
	}
}
