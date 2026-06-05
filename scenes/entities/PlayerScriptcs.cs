using Godot;
using System;
using static Godot.WebSocketPeer;

public enum PlayerState { IDLE, RUN };

public partial class PlayerScriptcs : CharacterBody2D
{
	[Export] public float Speed = 300.0f;

	private AnimatedSprite2D _sprite;
    private PlayerState _state = PlayerState.IDLE;
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

	public override void _PhysicsProcess(double delta)
	{
        MovementLoop();
        AnimationLoop();
        UpdateAnimation();
	}

	private void MovementLoop()
	{
        Vector2 velocity = Velocity;

        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("walk_left", "walk_right", "walk_up", "walk_down");
        if (direction != Vector2.Zero)
            velocity = direction * Speed;
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void AnimationLoop()
    {
        if(Velocity != Vector2.Zero && _state == PlayerState.IDLE)
            _state = PlayerState.RUN;
        else if(Velocity == Vector2.Zero && _state == PlayerState.RUN)
            _state = PlayerState.IDLE;
    }

    private void UpdateAnimation()
    {
        //flip player sprite properly
        if (_state == PlayerState.RUN || _state == PlayerState.IDLE)
        {
            if (Velocity.X < -0.01)
                _sprite.FlipH = true;
            else if (Velocity.X > 0.01)
                _sprite.FlipH = false;
        }

        switch (_state)
        {
            case PlayerState.IDLE:
                _sprite.Play("idle");
                break;
            case PlayerState.RUN:
                _sprite.Play("run");
                break;
        }
    }
}
