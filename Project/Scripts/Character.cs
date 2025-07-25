using Godot;
using System;

public partial class Character : CharacterBody2D
{
	[Export]
	public float Speed { get; protected set; } = 200.0f;
	[Export]
	public int MaxHealth { get; protected set; } = 100;
	[Export]
	public int Health { get; protected set; }
	[Export]
	public int Attack { get; protected set; } = 10;
	[Export]
	public int Defense { get; protected set; } = 5;

	public bool IsAlive { get; protected set; } = true;
	public string SpritePath { get; protected set; }

	public override void _Ready()
	{
		Health = MaxHealth;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}
		
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection.Normalized() * Speed;
		
		MoveAndSlide();
	}

	public virtual void TakeDamage(int damage)
	{
		int actualDamage = Mathf.Max(0, damage - Defense);
		Health -= actualDamage;
		GD.Print($"{Name} took {actualDamage} damage. Health is now {Health}");

		if (Health <= 0)
		{
			Health = 0;
			IsAlive = false;
			Die();
		}
	}

	public virtual void AttackCharacter(Character target)
	{
		if (target != null && target.IsAlive)
		{
			GD.Print($"{Name} attacks {target.Name}.");
			target.TakeDamage(Attack);
		}
	}

	protected virtual void Die()
	{
		GD.Print($"{Name} has died.");
		// The character's death can be handled here, e.g., by playing an animation
		// and then calling QueueFree(). For now, we'll just log it.
	}
} 
