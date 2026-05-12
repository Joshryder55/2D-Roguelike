using Godot;
using System;

public partial class IceWizardAbilities : Node
{
	
	CharacterBody2D player;
	GameManager gameManager;
	IceWizardStats iceStats;
	[Export] public PackedScene FrostNovaScene;
	[Export] public PackedScene IceSpikeScene;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent<CharacterBody2D>();
		gameManager = GetNode<GameManager>("/root/GameManager");
		iceStats = GetParent().GetNode<IceWizardStats>("Stats");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Ultimate")) {
			if (iceStats.activeUltimate != IceWizardStats.UltimateAbility.None && 
				iceStats.ultimateCharge >= iceStats.GetUltimateChargeRequired()) {
				
				switch(iceStats.activeUltimate) {
					case IceWizardStats.UltimateAbility.FrostNova:
						if (iceStats.hasFrostNova)
						FireFrostNova();
						break;
					case IceWizardStats.UltimateAbility.IceSpike:
						if (iceStats.hasIceSpike)
						FireIceSpike();
						break;
				}
				
				iceStats.ultimateCharge = 0; // reset charge after use
			}
		}
	}
	
	public void FireFrostNova() {
		FrostNova frostNova = FrostNovaScene.Instantiate<FrostNova>();
		frostNova.iceStats = iceStats;
		frostNova.GlobalPosition = player.GlobalPosition;
		GetTree().CurrentScene.AddChild(frostNova);
		
		
	}
	
	public void FireIceSpike() {
	CharacterBody2D nearestEnemy = FindNearestEnemy();
	if (nearestEnemy == null) return;
	
	IceSpike iceSpike = IceSpikeScene.Instantiate<IceSpike>();
	iceSpike.characterStats = iceStats;
	iceSpike.Direction = (nearestEnemy.GlobalPosition - player.GlobalPosition).Normalized();
	iceSpike.GlobalPosition = player.GlobalPosition;
	GetTree().CurrentScene.AddChild(iceSpike);
}


private CharacterBody2D FindNearestEnemy() {
		CharacterBody2D nearest = null;
		float nearestDistance = 750.0f;

		foreach (Node node in GetTree().GetNodesInGroup("enemies")) {
			if (node is CharacterBody2D enemy && IsInstanceValid(enemy) && enemy.IsInsideTree()) {
				float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
				if (distance < nearestDistance) {
					nearestDistance = distance;
					nearest = enemy;
				}
			}
		}
		return nearest;
	}
	
	
	
	
	
}	
