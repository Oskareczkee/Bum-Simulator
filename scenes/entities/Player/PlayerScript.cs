using Godot;
using System;
using static Godot.WebSocketPeer;

public enum PlayerState { IDLE, RUN };

public partial class PlayerScript : CharacterBody2D
{
	[Signal] public delegate void UpdateIntoxicationBarEventHandler(int percent_value);
	[Signal] public delegate void GameOverEventHandler();

	[Export] public float Speed = 300.0f;

    private Hud HUD;

    public double intoxicationLossPerMinute = 40.0f;
    public double maxIntoxication = 400.0f;
    public double intoxication = 400.0f;

    private AnimatedSprite2D _sprite;
	private PlayerState _state = PlayerState.IDLE;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;

		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        HUD = GetNode<Control>("/root/GameManager/UI/HUD") as Hud;
        intoxication = maxIntoxication;

		UpdateIntoxicationBar += HUD.UpdateIntoxicationBar;
        GameOver += DisplayGameOver;
    }

	public override void _PhysicsProcess(double delta)
	{
		MovementLoop();
		AnimationLoop();
		UpdateAnimation();

		UpdateIntoxication(delta);
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

	private async void Death()
	{
        await ToSignal(GetTree().CreateTimer(0.4), SceneTreeTimer.SignalName.Timeout); //wait for animations or etc...
		this.ProcessMode = ProcessModeEnum.Disabled; //remove processing for player

        EmitSignal(SignalName.GameOver);
    }

	private void UpdateIntoxication(double delta)
	{
		if (intoxication == 0) return; //prevent calling this too much after emitting signal

		intoxication -= (intoxicationLossPerMinute / 60.0d) * delta;
		if(intoxication < 0) intoxication = 0;

		EmitSignal(SignalName.UpdateIntoxicationBar, (intoxication * 100) / maxIntoxication);

		if (intoxication == 0)
			Death();
	}

	private void DisplayGameOver()
	{
        GameManager gameManager = GetNode("/root/GameManager") as GameManager;
        gameManager.GameOver();
    }
}
