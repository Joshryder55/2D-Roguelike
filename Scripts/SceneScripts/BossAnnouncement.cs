using Godot;

public partial class BossAnnouncement : Control {
	readonly Color incomingColor = new Color(1f, 0.25f, 0.25f);
	readonly Color slainColor = new Color(1f, 0.85f, 0.2f);
	readonly Color completeColor = new Color(0.4f, 1f, 0.5f);

	Label label;
	Timer hideTimer;
	bool showCompleteAfterHide = false;
	int pendingLevel = 1;

	public override void _Ready() {
		AddToGroup("bossAnnouncement");

		label = GetNode<Label>("Label");
		label.Modulate = new Color(1, 1, 1, 0);

		hideTimer = new Timer();
		hideTimer.OneShot = true;
		hideTimer.Timeout += FadeOut;
		AddChild(hideTimer);
	}

	public void AnnounceBossIncoming() {
		showCompleteAfterHide = false;
		ShowMessage("⚠  BOSS INCOMING  ⚠", incomingColor, 3.5f);
	}

	public void AnnounceBossSlain(int currentLevel) {
		pendingLevel = currentLevel;
		showCompleteAfterHide = true;
		ShowMessage("BOSS SLAIN", slainColor, 2.8f);
	}

	void ShowMessage(string text, Color color, float duration) {
		label.Text = text;
		label.AddThemeColorOverride("font_color", color);
		hideTimer.Stop();
		hideTimer.WaitTime = duration;
		hideTimer.Start();

		Tween tween = CreateTween();
		tween.TweenProperty(label, "modulate:a", 1f, 0.4f);
	}

	void FadeOut() {
		Tween tween = CreateTween();
		tween.TweenProperty(label, "modulate:a", 0f, 0.6f);
		if (showCompleteAfterHide) {
			showCompleteAfterHide = false;
			tween.TweenCallback(Callable.From(ShowLevelComplete));
		}
	}

	void ShowLevelComplete() {
		label.Text = $"LEVEL {pendingLevel} COMPLETE!\nEnter north gate to reach Level {pendingLevel + 1}";
		label.AddThemeColorOverride("font_color", completeColor);

		hideTimer.Stop();
		hideTimer.WaitTime = 5f;
		hideTimer.Start();

		Tween tween = CreateTween();
		tween.TweenProperty(label, "modulate:a", 1f, 0.5f);
	}
}
