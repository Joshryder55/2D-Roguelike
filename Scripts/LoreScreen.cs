using Godot;

public partial class LoreScreen : Control {
	[Export] public string LoreText = "";
	[Export] public string NextScene = "";
	[Export] public float DisplayTime = 30f;

	private float timer = 0f;
	private bool transitioning = false;

public override void _Ready() {
	if (!string.IsNullOrEmpty(LoreText))
		GetNode<Label>("LoreLabel").Text = LoreText;
	
	GetNode<Button>("SkipButton").Pressed += OnSkipPressed;

	Modulate = new Color(1, 1, 1, 0);
	Tween tween = CreateTween();
	tween.TweenProperty(this, "modulate:a", 1.0f, 1.5f);
}

	public override void _Process(double delta) {
		if (transitioning) return;
		timer += (float)delta;
		if (timer >= DisplayTime)
			StartTransition();
	}

	private void OnSkipPressed() {
		StartTransition();
	}

	private void StartTransition() {
		if (transitioning) return;
		transitioning = true;

		Tween tween = CreateTween();
		tween.TweenProperty(this, "modulate:a", 0.0f, 1.5f);
		tween.TweenCallback(Callable.From(() => {
			GetTree().ChangeSceneToFile(NextScene);
		}));
	}
}
