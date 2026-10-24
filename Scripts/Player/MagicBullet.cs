using Godot;

public partial class MagicBullet : Area2D
{
	// Exports
	[Export] public float Speed = 800f;
	[Export] public float MaxLifetime = 3f;

	// State
	private Node2D Target;
	private float Damage;
	private float Lifetime = 0f;
	private Vector2 Direction = Vector2.Right;
	private bool HasHit = false;

	// Ready
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	// Setup
	public void Initialize(Node2D target, float damage)
	{
		Target = target;
		Damage = damage;
		Direction = GlobalPosition.DirectionTo(target.GlobalPosition);
		Rotation = Direction.Angle();
	}

	// Physics Process
	public override void _PhysicsProcess(double delta)
	{
		Lifetime += (float)delta;
		if (Lifetime >= MaxLifetime)
		{
			QueueFree();
			return;
		}

		if (Target != null && GodotObject.IsInstanceValid(Target))
		{
			Direction = GlobalPosition.DirectionTo(Target.GlobalPosition);
			Rotation = Direction.Angle();
		}

		GlobalPosition += Direction * Speed * (float)delta;
	}

	// Hit
	private void OnAreaEntered(Area2D area)
	{
		if (HasHit)
			return;

		if (area.GetParent() is not EnemyController enemy)
			return;

		HasHit = true;
		enemy.TakeDamage(Damage);
		QueueFree();
	}
}
