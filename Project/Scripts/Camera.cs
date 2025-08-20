using Godot;

public partial class Camera : Godot.Camera2D
{
	private Vector2? _lastMousePosition = null;
	private bool _dragging = false;

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_middle_click"))
		{
			_dragging = true;
			_lastMousePosition = GetGlobalMousePosition();
		}

		if (@event.IsActionReleased("ui_middle_click"))
		{
			_dragging = false;
		}

		if (_dragging && _lastMousePosition.HasValue)
		{
			Vector2 currentMousePosition = GetGlobalMousePosition();
			Vector2 dragVector = _lastMousePosition.Value - currentMousePosition;
			Position += dragVector;
			_lastMousePosition = currentMousePosition;
		}
	}
}
