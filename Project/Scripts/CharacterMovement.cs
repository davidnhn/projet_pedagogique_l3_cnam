using Godot;
using System;

public partial class CharacterMovement : Node2D
{
	private Character _character;

	private Vector2 _position;
	public new Vector2 Position
	{
		get => _position;
		set
		{
			_position = value;
			GlobalPosition = _position;
		}
	}

	public Vector2 Direction { get; private set; }
	private Vector2 _velocity;
	public bool IsMoving { get; private set; }

	private Area2D _collisionArea;
	private CollisionShape2D _collisionShape;
	private bool _isColliding;

	public override void _Ready()
	{
		_character = GetParent<Character>();
		if (_character == null)
		{
			GD.PrintErr("Character parent not found!");
			return;
		}

		IsMoving = false;
		_isColliding = false;
		SetupCollisionArea();
	}

	private void SetupCollisionArea()
	{
		_collisionArea = new Area2D();
		_collisionShape = new CollisionShape2D();
		var shape = new RectangleShape2D
		{
			Size = new Vector2(32, 32)
		};

		_collisionShape.Shape = shape;
		_collisionArea.AddChild(_collisionShape);
		AddChild(_collisionArea);

		_collisionArea.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
		_collisionArea.Connect("area_exited", new Callable(this, nameof(OnAreaExited)));

	}

	public override void _Process(double delta)
	{
		if (_character == null || !_character.IsAlive)
			return;

		HandleInput();
		UpdateMovement((float)delta);
	}

	private void HandleInput()
	{
		Vector2 inputDirection = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
			inputDirection.X += 1;
		if (Input.IsActionPressed("ui_left"))
			inputDirection.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			inputDirection.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			inputDirection.Y -= 1;

		if (inputDirection != Vector2.Zero)
		{
			Direction = inputDirection.Normalized();
			IsMoving = true;
		}
		else
		{
			IsMoving = false;
		}
	}

	private void UpdateMovement(float delta)
	{
		if (!IsMoving || _isColliding) return;

		_velocity = Direction * _character.Speed * delta;
		Position += _velocity;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("obstacles"))
		{
			_isColliding = true;
			GD.Print("Collision détectée !");
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area.IsInGroup("obstacles"))
		{
			_isColliding = false;
		}
	}

	public void SavePosition()
	{
		GD.Print($"Position sauvegardée : {Position}");
	}
}
