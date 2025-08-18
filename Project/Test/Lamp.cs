using Godot;
using System;

public partial class Lamp : Area2D
{
	[Export]
	private InteractionArea _interactionArea;

	[Export]
	private Sprite2D _sprite;

	public override void _Ready()
	{
		if (_interactionArea != null)
		{
			_interactionArea.interact = new Callable(this, MethodName._toggle_lamp);
			_interactionArea.action_name = "toggle ?";
		}
	}

	private void _toggle_lamp()
	{
		if (_sprite != null)
		{
			// This is a ternary operator. It's a compact way of writing an if/else statement.
			// It reads: "set frame to 1 if the current frame is 0, otherwise set it to 0."
			_sprite.Frame = (_sprite.Frame == 0) ? 1 : 0;
		}
	}
}
