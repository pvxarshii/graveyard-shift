using Godot;

public partial class PlayerCombatController : Node2D
{
	// Exports
	[Export] public float Damage = 15f;
	[Export] public float Cooldown = 0.6f;
	[Export] public float Range = 700f;
	[Export] public PackedScene BulletScene;

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

		HandleAttack(delta);
	}

	// Attack
	private void HandleAttack(double delta)
	{
		CooldownTimer += (float)delta;
		if (CooldownTimer < Cooldown)
			return;

		Node2D target = FindNearestEnemy();
		if (target == null || BulletScene == null)
			return;

		CooldownTimer = 0f;

		MagicBullet bullet = BulletScene.Instantiate<MagicBullet>();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
		bullet.Initialize(target, Damage);
	}

	// Targeting
	private Node2D FindNearestEnemy()
	{
		Node2D nearest = null;
		float nearestDistance = Range;

		foreach (Node node in GetTree().GetNodesInGroup("Enemy"))
		{
			if (node is not Node2D enemy)
				continue;

			float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearest = enemy;
			}
		}

		return nearest;
	}
}
