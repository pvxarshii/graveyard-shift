using Godot;

public partial class EnemyController : CharacterBody2D
{
	// Exports
	[Export] public float Speed = 100f;
	[Export] public float MaxHealth = 50f;
	[Export] public float Damage = 1f;
	[Export] public float AttackInterval = 1f;

	// State
	public float CurrentHealth { get; private set; }
	public bool IsAlive { get; private set; } = true;
	public bool PlayerInRange { get; set; } = false;
	private float attackCooldownTimer = 0f;

	// References
	private AnimatedSprite2D AnimSprite;
	protected PlayerController Player;
	private Area2D AttackArea;

	// Ready
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		AnimSprite = GetNode<AnimatedSprite2D>("EnemySprite");
		Player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		AttackArea = GetNode<Area2D>("AttackArea");
		AttackArea.BodyEntered += OnBodyEntered;
		AttackArea.BodyExited += OnBodyExited;
	}

	// Physics Process
	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive)
			return;

		HandleMovement();
		HandleAttack(delta);
		HandleSpriteFlip();
		HandleAnimation();
	}

	// Movement
	public virtual void HandleMovement()
	{
		if (Player == null || PlayerInRange)
		{
			Velocity = Vector2.Zero;
			return;
		}

		Vector2 direction = GlobalPosition.DirectionTo(Player.GlobalPosition);
		Velocity = direction * Speed;
		MoveAndSlide();
	}

	// Sprite Flip
	protected virtual void HandleSpriteFlip()
	{
		if (Velocity.X != 0)
			AnimSprite.FlipH = Velocity.X < 0;
	}

	// Animation
	protected virtual void HandleAnimation()
	{
		if (PlayerInRange)
			AnimSprite.Play("Attack");
		else if (Velocity != Vector2.Zero)
			AnimSprite.Play("Run");
		else
			AnimSprite.Play("Idle");
	}

	// Health
	public virtual void TakeDamage(float amount)
	{
		if (!IsAlive || amount <= 0f)
			return;

		CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);

		if (CurrentHealth <= 0f)
			Die();
	}

	// Attack
	protected virtual void HandleAttack(double delta)
	{
		if (!PlayerInRange || Player == null)
		{
			attackCooldownTimer = 0f;
			return;
		}

		attackCooldownTimer += (float)delta;
		if (attackCooldownTimer >= AttackInterval)
		{
			attackCooldownTimer = 0f;
			Player.TakeDamage(Damage);
			GD.Print($"Dealt {Damage} damage to player!");
		}
	}

	// Death
	public virtual void Die()
	{
		IsAlive = false;
		Velocity = Vector2.Zero;
		QueueFree();
	}

	// Signal Handlers
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
			PlayerInRange = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("Player"))
			PlayerInRange = false;
	}
}
