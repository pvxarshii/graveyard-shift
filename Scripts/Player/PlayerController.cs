using Godot;

public partial class PlayerController : CharacterBody2D
{
	// Exports
	[Export] public float Speed = 400f;
	[Export] public float MaxHealth = 1000f;

	// State
	public float CurrentHealth { get; private set; }
	public bool IsAlive { get; private set; } = true;

	// Signals
	[Signal] public delegate void PlayerHealthChangedEventHandler(float currentHealth, float maxHealth);
	[Signal] public delegate void PlayerDiedEventHandler();

	// References
	private AnimatedSprite2D AnimSprite;

	// Ready
	public override void _Ready()
	{
		AddToGroup("Player");
		AnimSprite = GetNode<AnimatedSprite2D>("PlayerSprite");
		CurrentHealth = MaxHealth;
		EmitSignal(SignalName.PlayerHealthChanged, CurrentHealth, MaxHealth);
	}

	// Physics Process
	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive)
			return;

		HandleMovement();
		HandleSpriteFlip();
		HandleAnimation();
	}

	// Movement
	public virtual void HandleMovement()
	{
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
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
		if (Velocity != Vector2.Zero)
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
		EmitSignal(SignalName.PlayerHealthChanged, CurrentHealth, MaxHealth);

		if (CurrentHealth <= 0f)
			Die();
	}

	// Death
	public virtual void Die()
	{
		IsAlive = false;
		Velocity = Vector2.Zero;
		EmitSignal(SignalName.PlayerDied);
	}
}
