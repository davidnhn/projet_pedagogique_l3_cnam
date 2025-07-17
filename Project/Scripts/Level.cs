using Godot;
using System;

public partial class Level : Node2D
{
	private Character _player;
	private Enemy _enemy;

	public override void _Ready()
	{
		// Récupérer les références au joueur et à l'ennemi
		_player = GetNode<Character>("Player");
		_enemy = GetNode<Enemy>("Enemy");

		if (_player == null || _enemy == null)
		{
			GD.PrintErr("Player or Enemy not found in the scene!");
		}
	}

	public override void _Process(double delta)
	{
		if (_player != null && _enemy != null)
		{
			// Faire suivre le joueur par l'ennemi
			_enemy.FollowPlayer(_player);

			// Vérifier la distance entre le joueur et l'ennemi pour l'attaque
			var distance = _player.GlobalPosition.DistanceTo(_enemy.GlobalPosition);
			if (distance < 50) // Distance d'attaque
			{
				_enemy.AttackCharacter(_player);
			}
		}
	}
} 
