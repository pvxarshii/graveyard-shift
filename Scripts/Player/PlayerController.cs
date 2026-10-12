using Godot;

public partial class PlayerController : CharacterBody2D
{
	[Export] public float Speed = 200f;

	private AnimatedSprite2D AnimSprite;

	public override void _Ready()
	{
		AnimSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector(
			"move_left",
			"move_right",
			"move_up",
            "move_down"
		);

		Velocity = direction * Speed;
		MoveAndSlide();

		// Flip sprite when moving horizontally.
		if (direction.X != 0)
			AnimSprite.FlipH = direction.X < 0;

		// Switch between idle and run animations.
		if (direction != Vector2.Zero)
			AnimSprite.Play("Run");
		else
			AnimSprite.Play("Idle");
	}
}
