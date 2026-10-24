using Godot;

public partial class EnemyController : CharacterBody2D
{
	// Exports
	[Export] public float Speed = 100f;
	[Export] public float MaxHealth = 50f;
	[Export] public float Damage = 1f;
	[Export] public float AttackInterval = 1f;
	[Export] public float SeparationRadius = 200f;
	[Export] public float SeparationForce = 1000f;

	// State
	public float CurrentHealth { get; private set; }
	public bool IsAlive { get; private set; } = true;
	public bool PlayerInRange { get; set; } = false;
	private float AttackCooldownTimer = 0f;

	// Signals
	[Signal] public delegate void EnemyHealthChangedEventHandler(float currentHealth, float maxHealth);
	[Signal] public delegate void EnemyDiedEventHandler();

	// References
	private AnimatedSprite2D AnimSprite;
	private AnimatedSprite2D DeathSprite;
	protected PlayerController Player;
	private Area2D EnemyArea;

	// Ready
	public override void _Ready()
	{
		AddToGroup("Enemy");
		CurrentHealth = MaxHealth;
		AnimSprite = GetNode<AnimatedSprite2D>("EnemySprite");
		DeathSprite = GetNodeOrNull<AnimatedSprite2D>("DeathSprite");
		Player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		EnemyArea = GetNode<Area2D>("EnemyArea");
		EnemyArea.BodyEntered += OnBodyEntered;
		EnemyArea.BodyExited += OnBodyExited;

		if (DeathSprite != null)
		{
			DeathSprite.Visible = false;
			DeathSprite.AnimationFinished += OnDeathAnimationFinished;
		}

		EmitSignal(SignalName.EnemyHealthChanged, CurrentHealth, MaxHealth);
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
		HandleSeparation();
		MoveAndSlide();
	}

	// Seperation
	protected virtual void HandleSeparation()
	{
		Vector2 pushAway = Vector2.Zero;

		foreach (var enemy in GetTree().GetNodesInGroup("Enemy"))
		{
			if (enemy == this)
				continue;

			var enemyBody = enemy as Node2D;
			if (enemyBody == null)
				continue;

			float dist = GlobalPosition.DistanceTo(enemyBody.GlobalPosition);
			if (dist < SeparationRadius && dist > 0.01f)
				pushAway += GlobalPosition.DirectionTo(enemyBody.GlobalPosition) * (1f - dist / SeparationRadius);
		}

		Velocity -= pushAway * SeparationForce * (float)GetPhysicsProcessDeltaTime();
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
		EmitSignal(SignalName.EnemyHealthChanged, CurrentHealth, MaxHealth);

		if (CurrentHealth <= 0f)
			Die();
	}

	// Attack
	protected virtual void HandleAttack(double delta)
	{
		if (!PlayerInRange || Player == null)
		{
			AttackCooldownTimer = 0f;
			return;
		}

		AttackCooldownTimer += (float)delta;
		if (AttackCooldownTimer >= AttackInterval)
		{
			AttackCooldownTimer = 0f;
			Player.TakeDamage(Damage);
			GD.Print($"Dealt {Damage} damage to player!");
		}
	}

	// Death
	public virtual void Die()
	{
		IsAlive = false;
		Velocity = Vector2.Zero;
		RemoveFromGroup("Enemy");
		EmitSignal(SignalName.EnemyDied);
		EnemyArea.SetDeferred(Area2D.PropertyName.Monitoring, false);
		EnemyArea.SetDeferred(Area2D.PropertyName.Monitorable, false);

		if (DeathSprite != null)
		{
			AnimSprite.Stop();
			AnimSprite.Visible = false;
			DeathSprite.FlipH = AnimSprite.FlipH;
			DeathSprite.Visible = true;
			DeathSprite.Play("Death");
		}
		else
		{
			QueueFree();
		}
	}

	// Death Animation
	private void OnDeathAnimationFinished()
	{
		if (IsAlive)
			return;

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
