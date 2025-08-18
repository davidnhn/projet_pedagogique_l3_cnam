using Godot;
using System.Collections.Generic;

public partial class InteractionManager : Node
{
	private static InteractionManager _instance;
	public static InteractionManager Instance => _instance;

	private Node _player;
	private Label _label;

	private const string BaseText = "[E] to ";

	private List<InteractionArea> _activeAreas = new List<InteractionArea>();
	private bool _canInteract = true;

	public override void _EnterTree()
	{
		if (_instance != null)
		{
			QueueFree(); // The singleton is already running, so destroy this instance.
			return;
		}
		_instance = this;
	}

	public override void _Ready()
	{
		// @onready var player = get_tree().get_first_node_in_group("player")
		_player = GetTree().GetFirstNodeInGroup("player");
		if (_player == null)
		{
			GD.PrintErr("InteractionManager: Player node not found in group 'player'.");
		}

		// @onready var label = $Label
		// This expects a child node named "Label" to exist on the node where this script is attached.
		_label = GetNode<Label>("Label");
		if (_label == null)
		{
			GD.PrintErr("InteractionManager: Child node of type Label named 'Label' not found.");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && _canInteract)
		{
			if (_activeAreas.Count > 0)
			{
				// We call an async method to handle the interaction so we can use 'await'.
				_handleInteraction();
			}
		}
	}

	private async void _handleInteraction()
	{
		_canInteract = false;
		if (_label != null)
		{
			_label.Hide();
		}

		// The _Process method already sorts the areas, so the closest is always at index 0.
		InteractionArea closestArea = _activeAreas[0];
		
		// Call the interaction logic on the area.
		closestArea.interact.Call();

		// The 'await' in the original GDScript pauses execution. Since the C# interact
		// method might not be awaitable by default, we can simulate the pause
		// by waiting for the interaction's own signal or just a frame.
		// Here, we'll wait for the interaction's callable to finish if it returns a signal,
		// or just continue after a frame if not. Awaiting a frame is a safe default.
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		_canInteract = true;
	}

	public override void _Process(double delta)
	{
		// Ensure we don't run this logic if the player or label aren't set up.
		if (_player == null || _label == null)
		{
			return;
		}

		if (_activeAreas.Count > 0 && _canInteract)
		{
			// Sort the list to find the closest area
			_activeAreas.Sort(_sortByDistanceToPlayer);

			InteractionArea closestArea = _activeAreas[0];
			
			// Update and position the label
			_label.Text = BaseText + closestArea.action_name;
			Vector2 newPosition = closestArea.GlobalPosition;
			newPosition.Y -= 36;
			newPosition.X -= _label.Size.X / 2;
			_label.GlobalPosition = newPosition;

			_label.Show();
		}
		else
		{
			_label.Hide();
		}
	}

	private int _sortByDistanceToPlayer(InteractionArea area1, InteractionArea area2)
	{
		float area1ToPlayer = _player.GetNode<Node2D>(".") .GlobalPosition.DistanceTo(area1.GlobalPosition);
		float area2ToPlayer = _player.GetNode<Node2D>(".") .GlobalPosition.DistanceTo(area2.GlobalPosition);
		return area1ToPlayer.CompareTo(area2ToPlayer);
	}

	/// <summary>
	/// Corresponds to GDScript's `register_area` function.
	/// Adds a detected InteractionArea to the list of active areas.
	/// </summary>
	public void RegisterArea(InteractionArea area)
	{
		_activeAreas.Add(area);
	}

	/// <summary>
	/// Corresponds to GDScript's `unregister_area` function.
	/// Removes an InteractionArea from the list when it's no longer active.
	/// </summary>
	public void UnregisterArea(InteractionArea area)
	{
		_activeAreas.Remove(area);
	}
}
