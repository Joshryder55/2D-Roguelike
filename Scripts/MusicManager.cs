using Godot;

// Autoload (/root/MusicManager). Persists across scene changes so menu music
// plays continuously instead of restarting each time a menu screen loads.
public partial class MusicManager : Node
{
	private AudioStreamPlayer player;
	private const string menuMusicPath = "res://Assets/Audio/menuMusic.mp3";
	private const float defaultVolumePercent = 0.5f;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		int masterBus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(masterBus, Mathf.LinearToDb(defaultVolumePercent));

		player = new AudioStreamPlayer();
		var stream = GD.Load<AudioStream>(menuMusicPath);
		if (stream is AudioStreamMP3 mp3)
			mp3.Loop = true;
		player.Stream = stream;
		AddChild(player);
	}

	public void PlayMenuMusic()
	{
		if (player.Playing) return;
		player.Play();
	}

	public void StopMenuMusic()
	{
		player.Stop();
	}
}
