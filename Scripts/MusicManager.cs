using Godot;

public partial class MusicManager : Node
{
	private AudioStreamPlayer player;
	private string currentTrackPath = "";

	private const string menuMusicPath = "res://Assets/Audio/menuMusic.mp3";
	private const float defaultVolumePercent = 0.5f;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		int masterBus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(masterBus, Mathf.LinearToDb(defaultVolumePercent));

		player = new AudioStreamPlayer();
		player.Bus = "Music";
		player.ProcessMode = ProcessModeEnum.Always;
		AddChild(player);
	}

	public void PlayMenuMusic()
	{
		PlayTrack(menuMusicPath);
	}

	// Picks the right track for the level being loaded based on its scene path.
	// Called from EnemyManager._Ready so every level starts its own music.
	public void PlayLevelMusic(string scenePath)
	{
		string trackPath = null;
		if (scenePath.Contains("Level1"))
			trackPath = "res://Assets/Audio/Level1 - music.mp3";
		else if (scenePath.Contains("Level2"))
			trackPath = "res://Assets/Audio/Level2 - music.mp3";
		else if (scenePath.Contains("Level3"))
			trackPath = "res://Assets/Audio/Level3 - music.mp3";

		if (trackPath != null)
			PlayTrack(trackPath);
	}

	// Loads, loops, and plays level track. (wont restart track if level restarts)
	private void PlayTrack(string path)
	{
		if (currentTrackPath == path && player.Playing) return;

		var stream = GD.Load<AudioStream>(path);
		if (stream is AudioStreamMP3 mp3)
			mp3.Loop = true;
		else if (stream is AudioStreamOggVorbis ogg)
			ogg.Loop = true;

		player.Stream = stream;
		currentTrackPath = path;
		player.Play();
	}

	public void StopMenuMusic()
	{
		player.Stop();
		currentTrackPath = "";
	}
}
