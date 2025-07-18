using Godot;
using System;

public partial class Character : Node2D
{
	// Propriétés de base du personnage
	public new string Name { get; set; }
	public int Health { get; set; }
	public int MaxHealth { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public float Speed { get; set; }

	// Gestion du sprite
	private Sprite2D _sprite;
	public string SpritePath { get; set; } = "res://assets/images/Character.jpg";

	// État du personnage
	public bool IsAlive { get; set; }

	// Référence au composant de mouvement
	private CharacterMovement _movement;

	public override void _Ready()
	{
		// Initialisation des propriétés du personnage
		MaxHealth = 100;
		Health = MaxHealth;
		Attack = 10;
		Defense = 5;
		Speed = 200;
		IsAlive = true;

		// Initialisation du sprite
		_sprite = new Sprite2D();
		AddChild(_sprite);
		
		// Charger l'image
		LoadSprite(SpritePath);

		// Initialisation du composant de mouvement
		_movement = GetNode<CharacterMovement>("CharacterMovement");
		if (_movement == null)
		{
			GD.PrintErr("CharacterMovement component not found!");
		}
	}

	/// <summary>
	/// Charge l'image du personnage.
	/// </summary>
	/// <param name="path">Chemin vers l'image</param>
	public void LoadSprite(string path)
	{
		if (string.IsNullOrEmpty(path))
			return;

		var texture = GD.Load<Texture2D>(path);
		if (texture != null)
		{
			_sprite.Texture = texture;
			SpritePath = path;
		}
		else
		{
			GD.PrintErr($"Impossible de charger l'image : {path}");
		}
	}

	// Méthodes de base
	public virtual void TakeDamage(int damage)
	{
		int actualDamage = Math.Max(1, damage - Defense);
		Health = Math.Max(0, Health - actualDamage);
		
		if (Health <= 0)
		{
			Die();
		}
	}

	public void Heal(int amount)
	{
		Health = Math.Min(MaxHealth, Health + amount);
	}

	private void Die()
	{
		IsAlive = false;
		// TODO: Ajouter des effets de mort, animations, etc.
	}

	// Méthode pour attaquer un autre personnage
	public void AttackCharacter(Character target)
	{
		if (!IsAlive || !target.IsAlive) return;
		
		target.TakeDamage(Attack);
	}
} 
