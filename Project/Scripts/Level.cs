using Godot;
using System;

public partial class Level : Node2D
{
	private Character _player;
	private Enemy _enemy;

	public override void _Ready()
	{
		_player = GetNode<Character>("Player");
		_enemy = GetNode<Enemy>("Enemy");

		if (_player == null || _enemy == null)
		{
			GD.PrintErr("Player or Enemy not found in the scene!");
			return;
		}

		_enemy.Target = _player;
	}

	public override void _Process(double delta)
	{
		if (_player != null && _enemy != null && _player.IsAlive && _enemy.IsAlive)
		{
			var distance = _player.GlobalPosition.DistanceTo(_enemy.GlobalPosition);
			if (distance < 50) 
			{
				_enemy.AttackCharacter(_player);
			}
		}
	}
} 
