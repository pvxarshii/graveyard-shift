using Godot;

public partial class PlayerController : CharacterBody2D
{
    // Exports
    [Export] public float Speed = 200f;

    // Refrence
    private AnimatedSprite2D AnimSprite;

    // Ready
    public override void _Ready()
    {
        AnimSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    // Delta
    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();
        HandleSpriteFlip();
        HandleAnimation();
    }

    // Movement
    private void HandleMovement()
    {
        Vector2 direction = Input.GetVector(
            "move_left",
            "move_right",
            "move_up",
            "move_down"
        );

        Velocity = direction * Speed;
        MoveAndSlide();
    }

    // Sprite Flip
    private void HandleSpriteFlip()
    {
        if (Velocity.X != 0)
            AnimSprite.FlipH = Velocity.X < 0;
    }

    // Animations
    private void HandleAnimation()
    {
        if (Velocity != Vector2.Zero)
            AnimSprite.Play("Run");
        else
            AnimSprite.Play("Idle");
    }
}