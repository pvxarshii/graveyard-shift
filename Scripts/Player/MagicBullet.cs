using Godot;

public partial class MagicBullet : Area2D
{
	// Exports
	[Export] public float Speed = 800f;
	[Export] public float MaxLifetime = 3f;
	[Export] public float SearchRange = 700f;

	// State
	public float Damage { get; set; } = 15f;
	private Node2D Target;
	private Vector2 Direction = Vector2.Right;
	private float Lifetime = 0f;
	private bool HasHit = false;

	// Ready
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	// Setup
	public void Initialize(Vector2 spawnPosition, float damage)
	{
		GlobalPosition = spawnPosition;
		Damage = damage;
		Target = FindNearestEnemy();

		if (Target == null)
		{
			QueueFree();
			return;
		}

		HandleTargeting();
	}

	// Physics Process
	public override void _PhysicsProcess(double delta)
	{
		HandleLifetime(delta);
		HandleMovement(delta);
	}

	// Lifetime
	private void HandleLifetime(double delta)
	{
		Lifetime += (float)delta;
		if (Lifetime >= MaxLifetime)
			QueueFree();
	}

	// Movement
	private void HandleMovement(double delta)
	{
		HandleTargeting();
		GlobalPosition += Direction * Speed * (float)delta;
	}

	// Targeting
	private void HandleTargeting()
	{
		if (Target == null || !IsInstanceValid(Target))
			return;

		Direction = GlobalPosition.DirectionTo(Target.GlobalPosition);
		Rotation = Direction.Angle();
	}

	// Nearest Enemy
	private Node2D FindNearestEnemy()
	{
		Node2D nearest = null;
		float nearestDistance = SearchRange;

		foreach (Node node in GetTree().GetNodesInGroup("Enemy"))
		{
			Node2D enemy = node as Node2D;
			if (enemy == null)
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

	// Hit
	private void OnAreaEntered(Area2D area)
	{
		if (HasHit)
			return;

		EnemyController enemy = area.GetParent() as EnemyController;
		if (enemy == null)
			return;

		HasHit = true;
		enemy.TakeDamage(Damage);
		QueueFree();
	}
}
