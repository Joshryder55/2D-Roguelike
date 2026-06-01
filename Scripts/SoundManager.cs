using Godot;
using System.Collections.Generic;


// Music defaults a little quieter than sound effects.
public partial class SoundManager : Node
{
	private const string settingsPath = "user://audio_settings.cfg";

	// Sound effect definition. Start/Length isolate a single clip out of a sound file
	// that contains several (Length = 0 plays the whole stream from Start).
	private struct SfxDef
	{
		public string Path;
		public float Start;   // seconds into the file to begin playback
		public float Length;  // seconds to play before stopping (0 = to end)
		public string Bus;    // null/empty -> "SFX"
		public float Pitch;   // 0 -> 1.0
		public float VolumeDb; // volume offset in dB (0 = unchanged)
	}

	private static readonly Dictionary<string, SfxDef> sfxDefs = new()
	{
		// damageTaken.wav holds 10 damage clips about ~1.6s apart; we use the 4th (~4.88s in).
		{ "DamageTaken", new SfxDef { Path = "res://Assets/Audio/damageTaken.wav", Start = 4.86f, Length = 0.45f } },
		{ "GameOver",    new SfxDef { Path = "res://Assets/Audio/gameover.mp3" } },
		{ "LevelUp",     new SfxDef { Path = "res://Assets/Audio/LevelUp.mp3" } },
		{ "XPCollect",   new SfxDef { Path = "res://Assets/Audio/XPCollect.mp3" } },
		{ "IceBolt",     new SfxDef { Path = "res://Assets/Audio/IceBolt.ogg", Bus = "Attacks", Pitch = 1.45f, VolumeDb = -4f } },
		{ "EnemyFreeze", new SfxDef { Path = "res://Assets/Audio/EnemyFreeze.ogg", Bus = "Attacks" } },
		{ "IceSpike",    new SfxDef { Path = "res://Assets/Audio/IceSpike.ogg", Bus = "Attacks" } },
		{ "FrostNova",   new SfxDef { Path = "res://Assets/Audio/FrostNova.mp3", Bus = "Attacks", VolumeDb = 4f } },
		{ "Blizzard",    new SfxDef { Path = "res://Assets/Audio/Blizzard.mp3", Bus = "Attacks" } },
		{ "FlashFreeze", new SfxDef { Path = "res://Assets/Audio/FlashFreeze.mp3", Bus = "Attacks", VolumeDb = 4f } },
		{ "BossAttackLvl1", new SfxDef { Path = "res://Assets/Audio/BossAttackLvl1.mp3" } },
		{ "BossAttackLvl2", new SfxDef { Path = "res://Assets/Audio/BossAttackLvl2.mp3" } },
		{ "BossAttackLvl3", new SfxDef { Path = "res://Assets/Audio/BossAttackLvl3.mp3" } },
	};

	private readonly Dictionary<string, AudioStream> streams = new();

	// Defaults: music a little quieter than sound effects. Attack sounds also reduced volume at default
	public float MusicVolume { get; private set; } = 0.75f;
	public float SfxVolume { get; private set; } = 1.0f;
	public float AttackVolume { get; private set; } = 0.5f;

	private bool wasPaused = false;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		foreach (var pair in sfxDefs)
			streams[pair.Key] = GD.Load<AudioStream>(pair.Value.Path);

		LoadSettings();
		ApplyBus("Music", MusicVolume);
		ApplyBus("SFX", SfxVolume);
		ApplyBus("Attacks", AttackVolume);
	}

	public override void _Process(double delta)
	{
		// Cut all gameplay sound effects on pause
		bool paused = GetTree().Paused;
		if (paused && !wasPaused)
			StopGameplaySfx();
		wasPaused = paused;
	}

	private void StopGameplaySfx()
	{
		foreach (Node child in GetChildren())
		{
			if (child is AudioStreamPlayer p && p.ProcessMode == ProcessModeEnum.Pausable)
				p.QueueFree();
		}
	}


	// playWhilePaused: true only for (level-up, game over) that have screens which pause game
	public void PlaySfx(string name, bool playWhilePaused = false)
	{
		if (!sfxDefs.TryGetValue(name, out SfxDef def))
		{
			GD.PrintErr($"SoundManager: unknown sfx '{name}'");
			return;
		}

		var sfxPlayer = new AudioStreamPlayer();
		sfxPlayer.Stream = streams[name];
		sfxPlayer.Bus = string.IsNullOrEmpty(def.Bus) ? "SFX" : def.Bus;
		sfxPlayer.PitchScale = def.Pitch > 0f ? def.Pitch : 1.0f;
		sfxPlayer.VolumeDb = def.VolumeDb;
		sfxPlayer.ProcessMode = playWhilePaused ? ProcessModeEnum.Always : ProcessModeEnum.Pausable;
		AddChild(sfxPlayer);

		if (def.Length > 0f)
		{
			// To isolate one clip from sound file: start at the offset, then stop after Length.
			var stopTimer = new Timer();
			stopTimer.OneShot = true;
			stopTimer.WaitTime = def.Length;
			stopTimer.ProcessMode = sfxPlayer.ProcessMode;
			sfxPlayer.AddChild(stopTimer);
			stopTimer.Timeout += sfxPlayer.QueueFree;
			sfxPlayer.Play(def.Start);
			stopTimer.Start();
		}
		else
		{
			sfxPlayer.Finished += sfxPlayer.QueueFree;
			sfxPlayer.Play(def.Start);
		}
	}

	public AudioStreamPlayer PlaySfxLooping(string name)
	{
		if (!sfxDefs.TryGetValue(name, out SfxDef def))
		{
			GD.PrintErr($"SoundManager: unknown sfx '{name}'");
			return null;
		}

		AudioStream stream = streams[name];
		if (stream is AudioStreamMP3 mp3) mp3.Loop = true;
		else if (stream is AudioStreamOggVorbis ogg) ogg.Loop = true;

		var sfxPlayer = new AudioStreamPlayer();
		sfxPlayer.Stream = stream;
		sfxPlayer.Bus = string.IsNullOrEmpty(def.Bus) ? "SFX" : def.Bus;
		sfxPlayer.PitchScale = def.Pitch > 0f ? def.Pitch : 1.0f;
		sfxPlayer.VolumeDb = def.VolumeDb;
		sfxPlayer.ProcessMode = ProcessModeEnum.Pausable;

		AddChild(sfxPlayer);
		sfxPlayer.Play(def.Start);
		return sfxPlayer;
	}

	public void SetMusicVolume(float linear)
	{
		MusicVolume = Mathf.Clamp(linear, 0f, 1f);
		ApplyBus("Music", MusicVolume);
		SaveSettings();
	}

	public void SetSfxVolume(float linear)
	{
		SfxVolume = Mathf.Clamp(linear, 0f, 1f);
		ApplyBus("SFX", SfxVolume);
		SaveSettings();
	}

	public void SetAttackVolume(float linear)
	{
		AttackVolume = Mathf.Clamp(linear, 0f, 1f);
		ApplyBus("Attacks", AttackVolume);
		SaveSettings();
	}

	private static void ApplyBus(string busName, float linear)
	{
		int idx = AudioServer.GetBusIndex(busName);
		if (idx < 0) return;

		if (linear <= 0f)
		{
			AudioServer.SetBusMute(idx, true);
		}
		else
		{
			AudioServer.SetBusMute(idx, false);
			AudioServer.SetBusVolumeDb(idx, Mathf.LinearToDb(linear));
		}
	}

	private void LoadSettings()
	{
		var config = new ConfigFile();
		if (config.Load(settingsPath) != Error.Ok) return;

		MusicVolume = (float)config.GetValue("audio", "music", MusicVolume);
		SfxVolume = (float)config.GetValue("audio", "sfx", SfxVolume);
		AttackVolume = (float)config.GetValue("audio", "attacks", AttackVolume);
	}

	private void SaveSettings()
	{
		var config = new ConfigFile();
		config.SetValue("audio", "music", MusicVolume);
		config.SetValue("audio", "sfx", SfxVolume);
		config.SetValue("audio", "attacks", AttackVolume);
		config.Save(settingsPath);
	}
}
