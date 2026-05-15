using Godot;
public partial class SkillTreeLines : Control
{
	private Button coreButton;
	private Button blizzardButton;
	private Button flashFreezeButton;
	private Button frostNovaButton;
	private Button iceSpikeButton;
	
	private Button iceSpikeSize;
	private Button iceSpikeDamage;
	private Button iceSpikeFreezeDuration;
	
	private Button frostNovaSize;
	private Button frostNovaDamage;
	private Button frostNovaFreezeDuration;
	
	private Button permafrostButton;
	private Button brittleButton;
	private Button iceShieldButton;
	private Button multishotButton;
	private Button chanceToFreezeButton;
	
	private Button multishotChance;
	private Button multishotCount;
	
	private Button chanceToFreezeChance;
	private Button chanceToFreezeDuration;

	public override void _Ready()
	{
		GD.Print("SkillTreeLines loaded. Size: " + Size);
		MouseFilter = MouseFilterEnum.Ignore;
		coreButton = GetNodeOrNull<Button>("../CoreButton");
		
		blizzardButton = GetNodeOrNull<Button>("../Ultimate/BlizzardButton");
		
		flashFreezeButton = GetNodeOrNull<Button>("../Ultimate/FlashFreezeButton");
		
		frostNovaButton = GetNodeOrNull<Button>("../Ultimate/FrostNovaButton");
		frostNovaSize = GetNodeOrNull<Button>("../Ultimate/FrostNovaSize");
		frostNovaDamage = GetNodeOrNull<Button>("../Ultimate/FrostNovaDamage");
		frostNovaFreezeDuration = GetNodeOrNull<Button>("../Ultimate/FrostNovaFreezeDuration");
		
		iceSpikeButton = GetNodeOrNull<Button>("../Ultimate/IceSpikeButton");
		iceSpikeSize = GetNodeOrNull<Button>("../Ultimate/IceSpikeSize");
		iceSpikeDamage = GetNodeOrNull<Button>("../Ultimate/IceSpikeDamage");
		iceSpikeFreezeDuration = GetNodeOrNull<Button>("../Ultimate/IceSpikeFreezeDuration");
		
		permafrostButton = GetNodeOrNull<Button>("../Passive/PermafrostButton");
		
		brittleButton = GetNodeOrNull<Button>("../Passive/BrittleButton");
		
		iceShieldButton = GetNodeOrNull<Button>("../Passive/IceShieldButton");
		
		multishotButton = GetNodeOrNull<Button>("../Passive/MultishotButton");
		multishotChance = GetNodeOrNull<Button>("../Passive/MultishotChance");
		multishotCount = GetNodeOrNull<Button>("../Passive/MultishotCount");
		
		chanceToFreezeButton = GetNodeOrNull<Button>("../Passive/ChanceToFreezeButton");
		chanceToFreezeChance = GetNodeOrNull<Button>("../Passive/ChancetoFreezeChance");
		chanceToFreezeDuration = GetNodeOrNull<Button>("../Passive/ChancetoFreezeDuration");
		
		
		

		chanceToFreezeChance = GetNodeOrNull<Button>("../Passive/ChancetoFreezeChance");
		chanceToFreezeDuration = GetNodeOrNull<Button>("../Passive/ChancetoFreezeDuration");
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
		
		DrawConnection(coreButton, blizzardButton, lineColor, lineWidth);
		DrawConnection(coreButton, flashFreezeButton, lineColor, lineWidth);
		DrawConnection(coreButton, frostNovaButton, lineColor, lineWidth);
		DrawConnection(coreButton, iceSpikeButton, lineColor, lineWidth);
		
		DrawConnection(coreButton, permafrostButton, lineColor, lineWidth);
		DrawConnection(coreButton, brittleButton, lineColor, lineWidth);
		DrawConnection(coreButton, iceShieldButton, lineColor, lineWidth);
		DrawConnection(coreButton, multishotButton, lineColor, lineWidth);
		DrawConnection(coreButton, chanceToFreezeButton, lineColor, lineWidth);
		
		// Frost Nova upgrades - only draw if visible
if (frostNovaDamage != null && frostNovaDamage.Visible)
	DrawConnection(frostNovaButton, frostNovaDamage, lineColor, lineWidth);
if (frostNovaSize != null && frostNovaSize.Visible)
	DrawConnection(frostNovaButton, frostNovaSize, lineColor, lineWidth);
if (frostNovaFreezeDuration != null && frostNovaFreezeDuration.Visible)
	DrawConnection(frostNovaButton, frostNovaFreezeDuration, lineColor, lineWidth);

// Ice Spike upgrades
if (iceSpikeDamage != null && iceSpikeDamage.Visible)
	DrawConnection(iceSpikeButton, iceSpikeDamage, lineColor, lineWidth);
if (iceSpikeSize != null && iceSpikeSize.Visible)
	DrawConnection(iceSpikeButton, iceSpikeSize, lineColor, lineWidth);
if (iceSpikeFreezeDuration != null && iceSpikeFreezeDuration.Visible)
	DrawConnection(iceSpikeButton, iceSpikeFreezeDuration, lineColor, lineWidth);

// Multishot upgrades
if (multishotChance != null && multishotChance.Visible)
	DrawConnection(multishotButton, multishotChance, lineColor, lineWidth);
if (multishotCount != null && multishotCount.Visible)
	DrawConnection(multishotButton, multishotCount, lineColor, lineWidth);

// Chance to Freeze upgrades
if (chanceToFreezeChance != null && chanceToFreezeChance.Visible)
	DrawConnection(chanceToFreezeButton, chanceToFreezeChance, lineColor, lineWidth);
if (chanceToFreezeDuration != null && chanceToFreezeDuration.Visible)
	DrawConnection(chanceToFreezeButton, chanceToFreezeDuration, lineColor, lineWidth);
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
		if (frostNovaButton == null) GD.PrintErr("Missing FrostNovaButton");
		if (iceSpikeButton == null) GD.PrintErr("Missing IceSpikeButton");
		if (permafrostButton == null) GD.PrintErr("Missing PermafrostButton");
		if (brittleButton == null) GD.PrintErr("Missing BrittleButton");
		if (iceShieldButton == null) GD.PrintErr("Missing IceShieldButton");
		if (multishotButton == null) GD.PrintErr("Missing MultishotButton");
		if (chanceToFreezeButton == null) GD.PrintErr("Missing ChanceToFreezeButton");
	}
	




}
