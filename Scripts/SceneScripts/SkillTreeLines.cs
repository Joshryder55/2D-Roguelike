using Godot;
public partial class SkillTreeLines : Control
{
	private Button coreButton;

	private Button blizzardButton;
	private Button blizzardDuration;
	private Button blizzardSize;
	private Button blizzardChill;

	private Button flashFreezeButton;
	private Button flashFreezeDuration;
	private Button flashFreezeShatter;
	private Button flashFreezeGlacial;

	private Button frostNovaButton;
	private Button frostNovaSize;
	private Button frostNovaDamage;
	private Button frostNovaFreezeDuration;

	private Button iceSpikeButton;
	private Button iceSpikeSize;
	private Button iceSpikeDamage;
	private Button iceSpikeFreezeDuration;

	private Button permafrostButton;
	private Button permafrostRadius;
	private Button permafrostChill;

	private Button brittleButton;
	private Button brittleBonusDamage;
	private Button brittleShatter;

	private Button iceShieldButton;
	private Button iceShieldBlockChance;
	private Button iceShieldRetaliate;

	private Button multishotButton;
	private Button multishotChance;
	private Button multishotCount;

	private Button chanceToFreezeButton;
	private Button chanceToFreezeChance;
	private Button chanceToFreezeDuration;

	public override void _Ready()
	{
		GD.Print("SkillTreeLines loaded. Size: " + Size);
		MouseFilter = MouseFilterEnum.Ignore;

		coreButton = GetNodeOrNull<Button>("../CoreButton");

		blizzardButton   = GetNodeOrNull<Button>("../Ultimate/BlizzardButton");
		blizzardDuration = GetNodeOrNull<Button>("../Ultimate/BlizzardDuration");
		blizzardSize     = GetNodeOrNull<Button>("../Ultimate/BlizzardSize");
		blizzardChill    = GetNodeOrNull<Button>("../Ultimate/BlizzardChill");

		flashFreezeButton   = GetNodeOrNull<Button>("../Ultimate/FlashFreezeButton");
		flashFreezeDuration = GetNodeOrNull<Button>("../Ultimate/FlashFreezeDuration");
		flashFreezeShatter  = GetNodeOrNull<Button>("../Ultimate/FlashFreezeShatter");
		flashFreezeGlacial  = GetNodeOrNull<Button>("../Ultimate/FlashFreezeGlacial");

		frostNovaButton         = GetNodeOrNull<Button>("../Ultimate/FrostNovaButton");
		frostNovaSize           = GetNodeOrNull<Button>("../Ultimate/FrostNovaSize");
		frostNovaDamage         = GetNodeOrNull<Button>("../Ultimate/FrostNovaDamage");
		frostNovaFreezeDuration = GetNodeOrNull<Button>("../Ultimate/FrostNovaFreezeDuration");

		iceSpikeButton         = GetNodeOrNull<Button>("../Ultimate/IceSpikeButton");
		iceSpikeSize           = GetNodeOrNull<Button>("../Ultimate/IceSpikeSize");
		iceSpikeDamage         = GetNodeOrNull<Button>("../Ultimate/IceSpikeDamage");
		iceSpikeFreezeDuration = GetNodeOrNull<Button>("../Ultimate/IceSpikeFreezeDuration");

		permafrostButton = GetNodeOrNull<Button>("../Passive/PermafrostButton");
		permafrostRadius = GetNodeOrNull<Button>("../Passive/PermafrostRadius");
		permafrostChill  = GetNodeOrNull<Button>("../Passive/PermafrostChill");

		brittleButton      = GetNodeOrNull<Button>("../Passive/BrittleButton");
		brittleBonusDamage = GetNodeOrNull<Button>("../Passive/BrittleBonusDamage");
		brittleShatter     = GetNodeOrNull<Button>("../Passive/BrittleShatter");

		iceShieldButton      = GetNodeOrNull<Button>("../Passive/IceShieldButton");
		iceShieldBlockChance = GetNodeOrNull<Button>("../Passive/IceShieldBlockChance");
		iceShieldRetaliate   = GetNodeOrNull<Button>("../Passive/IceShieldRetaliate");

		multishotButton = GetNodeOrNull<Button>("../Passive/MultishotButton");
		multishotChance = GetNodeOrNull<Button>("../Passive/MultishotChance");
		multishotCount  = GetNodeOrNull<Button>("../Passive/MultishotCount");

		chanceToFreezeButton   = GetNodeOrNull<Button>("../Passive/ChancetoFreezeButton");
		chanceToFreezeChance   = GetNodeOrNull<Button>("../Passive/ChancetoFreezeChance");
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

		// Core to all main buttons
		DrawConnection(coreButton, blizzardButton, lineColor, lineWidth);
		DrawConnection(coreButton, flashFreezeButton, lineColor, lineWidth);
		DrawConnection(coreButton, frostNovaButton, lineColor, lineWidth);
		DrawConnection(coreButton, iceSpikeButton, lineColor, lineWidth);
		DrawConnection(coreButton, permafrostButton, lineColor, lineWidth);
		DrawConnection(coreButton, brittleButton, lineColor, lineWidth);
		DrawConnection(coreButton, iceShieldButton, lineColor, lineWidth);
		DrawConnection(coreButton, multishotButton, lineColor, lineWidth);
		DrawConnection(coreButton, chanceToFreezeButton, lineColor, lineWidth);

		// Blizzard upgrades
		if (blizzardDuration != null && blizzardDuration.Visible)
			DrawConnection(blizzardButton, blizzardDuration, lineColor, lineWidth);
		if (blizzardSize != null && blizzardSize.Visible)
			DrawConnection(blizzardButton, blizzardSize, lineColor, lineWidth);
		if (blizzardChill != null && blizzardChill.Visible)
			DrawConnection(blizzardButton, blizzardChill, lineColor, lineWidth);

		// Flash Freeze upgrades
		if (flashFreezeDuration != null && flashFreezeDuration.Visible)
			DrawConnection(flashFreezeButton, flashFreezeDuration, lineColor, lineWidth);
		if (flashFreezeShatter != null && flashFreezeShatter.Visible)
			DrawConnection(flashFreezeButton, flashFreezeShatter, lineColor, lineWidth);
		if (flashFreezeGlacial != null && flashFreezeGlacial.Visible)
			DrawConnection(flashFreezeButton, flashFreezeGlacial, lineColor, lineWidth);

		// Frost Nova upgrades
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

		// Permafrost upgrades
		if (permafrostRadius != null && permafrostRadius.Visible)
			DrawConnection(permafrostButton, permafrostRadius, lineColor, lineWidth);
		if (permafrostChill != null && permafrostChill.Visible)
			DrawConnection(permafrostButton, permafrostChill, lineColor, lineWidth);

		// Brittle upgrades
		if (brittleBonusDamage != null && brittleBonusDamage.Visible)
			DrawConnection(brittleButton, brittleBonusDamage, lineColor, lineWidth);
		if (brittleShatter != null && brittleShatter.Visible)
			DrawConnection(brittleButton, brittleShatter, lineColor, lineWidth);

		// Ice Shield upgrades
		if (iceShieldBlockChance != null && iceShieldBlockChance.Visible)
			DrawConnection(iceShieldButton, iceShieldBlockChance, lineColor, lineWidth);
		if (iceShieldRetaliate != null && iceShieldRetaliate.Visible)
			DrawConnection(iceShieldButton, iceShieldRetaliate, lineColor, lineWidth);

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
		if (coreButton == null)        GD.PrintErr("Missing CoreButton");
		if (blizzardButton == null)    GD.PrintErr("Missing BlizzardButton");
		if (flashFreezeButton == null) GD.PrintErr("Missing FlashFreezeButton");
		if (frostNovaButton == null)   GD.PrintErr("Missing FrostNovaButton");
		if (iceSpikeButton == null)    GD.PrintErr("Missing IceSpikeButton");
		if (permafrostButton == null)  GD.PrintErr("Missing PermafrostButton");
		if (brittleButton == null)     GD.PrintErr("Missing BrittleButton");
		if (iceShieldButton == null)   GD.PrintErr("Missing IceShieldButton");
		if (multishotButton == null)   GD.PrintErr("Missing MultishotButton");
		if (chanceToFreezeButton == null) GD.PrintErr("Missing ChanceToFreezeButton");
	}
}
