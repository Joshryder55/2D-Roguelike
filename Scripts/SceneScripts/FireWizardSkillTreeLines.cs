using Godot;
public partial class FireWizardSkillTreeLines : Control
{
	private Button coreButton;

	private Button infernoButton;
	private Button infernoDuration;
	private Button infernoSize;
	private Button infernoIntensity;

	private Button meteorButton;
	private Button meteorDamage;
	private Button meteorBlastRadius;
	private Button meteorCrater;

	private Button fireNovaButton;
	private Button fireNovaDamage;
	private Button fireNovaBurnDuration;
	private Button fireNovaSpeed;

	private Button flameDashButton;
	private Button flameDashRange;
	private Button flameDashTrailDuration;
	private Button flameDashTrailDamage;

	private Button igniteButton;
	private Button igniteChance;
	private Button igniteDuration;

	private Button multishotButton;
	private Button multishotChance;
	private Button multishotCount;

	private Button combustionButton;
	private Button combustionChance;
	private Button combustionRadius;

	private Button pyromaniacButton;
	private Button pyromaniacStackCount;
	private Button pyromaniacDamagePerStack;

	private Button cauterizeButton;
	private Button cauterizeChance;
	private Button cauterizeHealAmount;

	private Button moltenShieldButton;
	private Button moltenShieldBlockChance;
	private Button moltenShieldExplosionDamage;

	public override void _Ready()
	{
		GD.Print("FireWizardSkillTreeLines loaded. Size: " + Size);
		MouseFilter = MouseFilterEnum.Ignore;

		coreButton = GetNodeOrNull<Button>("../CoreButton");

		infernoButton    = GetNodeOrNull<Button>("../Ultimate/InfernoButton");
		infernoDuration  = GetNodeOrNull<Button>("../Ultimate/InfernoDuration");
		infernoSize      = GetNodeOrNull<Button>("../Ultimate/InfernoSize");
		infernoIntensity = GetNodeOrNull<Button>("../Ultimate/InfernoIntensity");

		meteorButton      = GetNodeOrNull<Button>("../Ultimate/MeteorButton");
		meteorDamage      = GetNodeOrNull<Button>("../Ultimate/MeteorDamage");
		meteorBlastRadius = GetNodeOrNull<Button>("../Ultimate/MeteorBlastRadius");
		meteorCrater      = GetNodeOrNull<Button>("../Ultimate/MeteorCrater");

		fireNovaButton       = GetNodeOrNull<Button>("../Ultimate/FireNovaButton");
		fireNovaDamage       = GetNodeOrNull<Button>("../Ultimate/FireNovaDamage");
		fireNovaBurnDuration = GetNodeOrNull<Button>("../Ultimate/FireNovaBurnDuration");
		fireNovaSpeed        = GetNodeOrNull<Button>("../Ultimate/FireNovaSpeed");

		flameDashButton          = GetNodeOrNull<Button>("../Ultimate/FlameDashButton");
		flameDashRange           = GetNodeOrNull<Button>("../Ultimate/FlameDashRange");
		flameDashTrailDuration   = GetNodeOrNull<Button>("../Ultimate/FlameDashTrailDuration");
		flameDashTrailDamage     = GetNodeOrNull<Button>("../Ultimate/FlameDashTrailDamage");

		igniteButton   = GetNodeOrNull<Button>("../Passive/IgniteButton");
		igniteChance   = GetNodeOrNull<Button>("../Passive/IgniteChance");
		igniteDuration = GetNodeOrNull<Button>("../Passive/IgniteDuration");

		multishotButton = GetNodeOrNull<Button>("../Passive/MultishotButton");
		multishotChance = GetNodeOrNull<Button>("../Passive/MultishotChance");
		multishotCount  = GetNodeOrNull<Button>("../Passive/MultishotCount");

		combustionButton = GetNodeOrNull<Button>("../Passive/CombustionButton");
		combustionChance = GetNodeOrNull<Button>("../Passive/CombustionChance");
		combustionRadius = GetNodeOrNull<Button>("../Passive/CombustionRadius");

		pyromaniacButton         = GetNodeOrNull<Button>("../Passive/PyromaniacButton");
		pyromaniacStackCount     = GetNodeOrNull<Button>("../Passive/PyromaniacStackCount");
		pyromaniacDamagePerStack = GetNodeOrNull<Button>("../Passive/PyromaniacDamagePerStack");

		cauterizeButton     = GetNodeOrNull<Button>("../Passive/CauterizeButton");
		cauterizeChance     = GetNodeOrNull<Button>("../Passive/CauterizeChance");
		cauterizeHealAmount = GetNodeOrNull<Button>("../Passive/CauterizeHealAmount");

		moltenShieldButton           = GetNodeOrNull<Button>("../Passive/MoltenShieldButton");
		moltenShieldBlockChance      = GetNodeOrNull<Button>("../Passive/MoltenShieldBlockChance");
		moltenShieldExplosionDamage  = GetNodeOrNull<Button>("../Passive/MoltenShieldExplosionDamage");

		CheckMissingNodes();
		QueueRedraw();
	}

	public override void _Process(double delta) { QueueRedraw(); }

	public override void _Draw()
	{
		Color lineColor = new Color(1.0f, 0.4f, 0.1f, 1.0f);
		float lineWidth = 4.0f;

		DrawConnection(coreButton, infernoButton,    lineColor, lineWidth);
		DrawConnection(coreButton, meteorButton,     lineColor, lineWidth);
		DrawConnection(coreButton, fireNovaButton,   lineColor, lineWidth);
		DrawConnection(coreButton, flameDashButton,  lineColor, lineWidth);
		DrawConnection(coreButton, igniteButton,     lineColor, lineWidth);
		DrawConnection(coreButton, multishotButton,  lineColor, lineWidth);
		DrawConnection(coreButton, combustionButton, lineColor, lineWidth);
		DrawConnection(coreButton, pyromaniacButton, lineColor, lineWidth);
		DrawConnection(coreButton, cauterizeButton,  lineColor, lineWidth);
		DrawConnection(coreButton, moltenShieldButton, lineColor, lineWidth);

		if (infernoDuration  != null && infernoDuration.Visible)  DrawConnection(infernoButton, infernoDuration,  lineColor, lineWidth);
		if (infernoSize      != null && infernoSize.Visible)      DrawConnection(infernoButton, infernoSize,      lineColor, lineWidth);
		if (infernoIntensity != null && infernoIntensity.Visible) DrawConnection(infernoButton, infernoIntensity, lineColor, lineWidth);

		if (meteorDamage      != null && meteorDamage.Visible)      DrawConnection(meteorButton, meteorDamage,      lineColor, lineWidth);
		if (meteorBlastRadius != null && meteorBlastRadius.Visible) DrawConnection(meteorButton, meteorBlastRadius, lineColor, lineWidth);
		if (meteorCrater      != null && meteorCrater.Visible)      DrawConnection(meteorButton, meteorCrater,      lineColor, lineWidth);

		if (fireNovaDamage       != null && fireNovaDamage.Visible)       DrawConnection(fireNovaButton, fireNovaDamage,       lineColor, lineWidth);
		if (fireNovaBurnDuration != null && fireNovaBurnDuration.Visible) DrawConnection(fireNovaButton, fireNovaBurnDuration, lineColor, lineWidth);
		if (fireNovaSpeed        != null && fireNovaSpeed.Visible)        DrawConnection(fireNovaButton, fireNovaSpeed,        lineColor, lineWidth);

		if (flameDashRange         != null && flameDashRange.Visible)         DrawConnection(flameDashButton, flameDashRange,         lineColor, lineWidth);
		if (flameDashTrailDuration != null && flameDashTrailDuration.Visible) DrawConnection(flameDashButton, flameDashTrailDuration, lineColor, lineWidth);
		if (flameDashTrailDamage   != null && flameDashTrailDamage.Visible)   DrawConnection(flameDashButton, flameDashTrailDamage,   lineColor, lineWidth);

		if (igniteChance   != null && igniteChance.Visible)   DrawConnection(igniteButton, igniteChance,   lineColor, lineWidth);
		if (igniteDuration != null && igniteDuration.Visible) DrawConnection(igniteButton, igniteDuration, lineColor, lineWidth);

		if (multishotChance != null && multishotChance.Visible) DrawConnection(multishotButton, multishotChance, lineColor, lineWidth);
		if (multishotCount  != null && multishotCount.Visible)  DrawConnection(multishotButton, multishotCount,  lineColor, lineWidth);

		if (combustionChance != null && combustionChance.Visible) DrawConnection(combustionButton, combustionChance, lineColor, lineWidth);
		if (combustionRadius != null && combustionRadius.Visible) DrawConnection(combustionButton, combustionRadius, lineColor, lineWidth);

		if (pyromaniacStackCount     != null && pyromaniacStackCount.Visible)     DrawConnection(pyromaniacButton, pyromaniacStackCount,     lineColor, lineWidth);
		if (pyromaniacDamagePerStack != null && pyromaniacDamagePerStack.Visible) DrawConnection(pyromaniacButton, pyromaniacDamagePerStack, lineColor, lineWidth);

		if (cauterizeChance     != null && cauterizeChance.Visible)     DrawConnection(cauterizeButton, cauterizeChance,     lineColor, lineWidth);
		if (cauterizeHealAmount != null && cauterizeHealAmount.Visible) DrawConnection(cauterizeButton, cauterizeHealAmount, lineColor, lineWidth);

		if (moltenShieldBlockChance     != null && moltenShieldBlockChance.Visible)     DrawConnection(moltenShieldButton, moltenShieldBlockChance,     lineColor, lineWidth);
		if (moltenShieldExplosionDamage != null && moltenShieldExplosionDamage.Visible) DrawConnection(moltenShieldButton, moltenShieldExplosionDamage, lineColor, lineWidth);
	}

	private void DrawConnection(Control fromNode, Control toNode, Color color, float width)
	{
		if (fromNode == null || toNode == null) return;
		Vector2 lineLayerGlobalPosition = GetGlobalRect().Position;
		Vector2 fromCenter = fromNode.GetGlobalRect().GetCenter() - lineLayerGlobalPosition;
		Vector2 toCenter   = toNode.GetGlobalRect().GetCenter()   - lineLayerGlobalPosition;
		DrawLine(fromCenter, toCenter, color, width);
	}

	private void CheckMissingNodes()
	{
		if (coreButton == null)       GD.PrintErr("Missing CoreButton");
		if (infernoButton == null)    GD.PrintErr("Missing InfernoButton");
		if (meteorButton == null)     GD.PrintErr("Missing MeteorButton");
		if (fireNovaButton == null)   GD.PrintErr("Missing FireNovaButton");
		if (flameDashButton == null)  GD.PrintErr("Missing FlameDashButton");
		if (igniteButton == null)     GD.PrintErr("Missing IgniteButton");
		if (multishotButton == null)  GD.PrintErr("Missing MultishotButton");
		if (combustionButton == null) GD.PrintErr("Missing CombustionButton");
		if (pyromaniacButton == null) GD.PrintErr("Missing PyromaniacButton");
		if (cauterizeButton == null)  GD.PrintErr("Missing CauterizeButton");
		if (moltenShieldButton == null) GD.PrintErr("Missing MoltenShieldButton");
	}
}
