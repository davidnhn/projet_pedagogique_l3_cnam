using Godot;
using System;

public partial class Enemy : Character
{
	// Propriétés spécifiques à l'ennemi
	public int ExperienceValue { get; private set; }
	public float DetectionRange { get; private set; }
	public bool IsAggressive { get; private set; }

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

		// Initialisation du composant de mouvement
		var movement = GetNode<CharacterMovement>("CharacterMovement");
		if (movement == null)
		{
			GD.PrintErr("CharacterMovement component not found!");
		}
	}

	// Méthode pour suivre le joueur
	public void FollowPlayer(Character player)
	{
		if (!IsAlive || player == null || !player.IsAlive) return;

		var distance = GlobalPosition.DistanceTo(player.GlobalPosition);
		if (distance <= DetectionRange)
		{
			var direction = (player.GlobalPosition - GlobalPosition).Normalized();
			var movement = GetNode<CharacterMovement>("CharacterMovement");
			if (movement != null)
			{
				// Déplacer l'ennemi vers le joueur
				movement.Position += direction * Speed * (float)GetProcessDeltaTime();
			}
		}
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
