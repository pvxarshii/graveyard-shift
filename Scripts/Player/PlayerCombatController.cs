using Godot;

public partial class PlayerCombatController : Node2D
{
	// Exports
	[Export] public PackedScene MagicBulletScene;
	[Export] public float MagicBulletDamage = 15f;
	[Export] public float MagicBulletCooldown = 0.6f;

	// State
	private float CooldownTimer = 0f;

	// References
	private PlayerController Player;

	// Ready
	public override void _Ready()
	{
		Player = GetParent<PlayerController>();
	}

	// Physics Process
	public override void _PhysicsProcess(double delta)
	{
		if (!Player.IsAlive || Player.IsSpawning)
			return;

		HandleMagicBulletSpawning(delta);
	}

	// Magic Bullet Spawning
	private void HandleMagicBulletSpawning(double delta)
	{
		CooldownTimer += (float)delta;
		if (CooldownTimer < MagicBulletCooldown)
			return;

		CooldownTimer = 0f;
		SpawnMagicBullet();
	}

	// Magic Bullet
	private void SpawnMagicBullet()
	{
		if (MagicBulletScene == null)
			return;

		MagicBullet bullet = MagicBulletScene.Instantiate<MagicBullet>();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.Initialize(GlobalPosition, MagicBulletDamage);
	}
}
