using Godot;
using System;

public partial class Enemy : Character
{
	// Propriétés spécifiques à l'ennemi
	public int ExperienceValue { get; private set; }
	public float DetectionRange { get; private set; }
	public bool IsAggressive { get; private set; }
	public Character Target { get; set; }

	public override void _Ready()
	{
		// Initialisation des propriétés de base
		Name = "Enemy";
		MaxHealth = 50;
		Health = MaxHealth;
		Attack = 8;
		Defense = 3;
		Speed = 150;
		IsAlive = true;
		SpritePath = "res://assets/images/Enemy.jpg";

		// Initialisation des propriétés spécifiques à l'ennemi
		ExperienceValue = 100;
		DetectionRange = 200.0f;
		IsAggressive = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive || Target == null || !Target.IsAlive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		var distance = GlobalPosition.DistanceTo(Target.GlobalPosition);
		if (distance <= DetectionRange)
		{
			var direction = (Target.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * Speed;
		}
		else
		{
			Velocity = Vector2.Zero;
		}
		
		MoveAndSlide();
	}

	// Surcharge de la méthode TakeDamage pour ajouter des comportements spécifiques
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		
		// Comportement spécifique quand l'ennemi prend des dégâts
		if (IsAlive && Health < MaxHealth * 0.3f) // Ennemi en dessous de 30% de sa vie
		{
			// L'ennemi devient plus agressif
			Speed *= 1.2f;
			Attack = (int)(Attack * 1.2f);
		}
	}
} 
