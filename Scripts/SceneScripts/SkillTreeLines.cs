using Godot;

public partial class SkillTreeLines : Control
{
	private Button coreButton;

	private Button blizzardButton;
	private Button flashFreezeButton;
	private Button permafrostButton;
	private Button brittleButton;
	private Button iceShieldButton;

	private Button moveSpeedButton;
	private Button attackSpeedButton;
	private Button multishotButton;
	private Button healthButton;
	private Button damageButton;

	public override void _Ready()
	{
		GD.Print("SkillTreeLines loaded. Size: " + Size);

		MouseFilter = MouseFilterEnum.Ignore;

		coreButton = GetNodeOrNull<Button>("../CoreButton");

		blizzardButton = GetNodeOrNull<Button>("../BlizzardButton");
		flashFreezeButton = GetNodeOrNull<Button>("../FlashFreezeButton");
		permafrostButton = GetNodeOrNull<Button>("../PermafrostButton");
		brittleButton = GetNodeOrNull<Button>("../BrittleButton");
		iceShieldButton = GetNodeOrNull<Button>("../IceShieldButton");

		moveSpeedButton = GetNodeOrNull<Button>("../MoveSpeedButton");
		attackSpeedButton = GetNodeOrNull<Button>("../AttackSpeedButton");
		multishotButton = GetNodeOrNull<Button>("../MultishotButton");
		healthButton = GetNodeOrNull<Button>("../HealthButton");
		damageButton = GetNodeOrNull<Button>("../DamageButton");

		CheckMissingNodes();

		QueueRedraw();
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		Color lineColor = new Color(0.2f, 0.8f, 1.0f, 1.0f);
		float lineWidth = 4.0f;

		// Top ice skills
		DrawConnection(coreButton, blizzardButton, lineColor, lineWidth);
		DrawConnection(coreButton, flashFreezeButton, lineColor, lineWidth);
		DrawConnection(coreButton, permafrostButton, lineColor, lineWidth);
		DrawConnection(coreButton, brittleButton, lineColor, lineWidth);
		DrawConnection(coreButton, iceShieldButton, lineColor, lineWidth);

		// Bottom passive/stat skills
		DrawConnection(coreButton, moveSpeedButton, lineColor, lineWidth);
		DrawConnection(coreButton, attackSpeedButton, lineColor, lineWidth);
		DrawConnection(coreButton, multishotButton, lineColor, lineWidth);
		DrawConnection(coreButton, healthButton, lineColor, lineWidth);
		DrawConnection(coreButton, damageButton, lineColor, lineWidth);
	}

	private void DrawConnection(Control fromNode, Control toNode, Color color, float width)
	{
		if (fromNode == null || toNode == null)
			return;

		Vector2 lineLayerGlobalPosition = GetGlobalRect().Position;

		Vector2 fromCenter = fromNode.GetGlobalRect().GetCenter() - lineLayerGlobalPosition;
		Vector2 toCenter = toNode.GetGlobalRect().GetCenter() - lineLayerGlobalPosition;

		DrawLine(fromCenter, toCenter, color, width);
	}

	private void CheckMissingNodes()
	{
		if (coreButton == null) GD.PrintErr("Missing CoreButton");

		if (blizzardButton == null) GD.PrintErr("Missing BlizzardButton");
		if (flashFreezeButton == null) GD.PrintErr("Missing FlashFreezeButton");
		if (permafrostButton == null) GD.PrintErr("Missing PermafrostButton");
		if (brittleButton == null) GD.PrintErr("Missing BrittleButton");
		if (iceShieldButton == null) GD.PrintErr("Missing IceShieldButton");

		if (moveSpeedButton == null) GD.PrintErr("Missing MoveSpeedButton");
		if (attackSpeedButton == null) GD.PrintErr("Missing AttackSpeedButton");
		if (multishotButton == null) GD.PrintErr("Missing MultishotButton");
		if (healthButton == null) GD.PrintErr("Missing HealthButton");
		if (damageButton == null) GD.PrintErr("Missing DamageButton");
	}
}
