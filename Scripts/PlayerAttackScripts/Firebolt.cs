using Godot;


public partial class FireBolt : Projectile
{
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
		base._Ready();
	}
}
