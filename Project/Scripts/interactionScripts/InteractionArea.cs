using Godot;
using System;

public partial class InteractionArea : Area2D
{
	[Export]
	public string action_name = "interact";

	[Export]
	public string[] Lines { get; set; }
	
	[Export]
	public AudioStream SpeechSound { get; set; }

	[Export]
	public Sprite2D Sprite { get; set; }

	public Callable interact { get; set; }

	public override void _Ready()
	{
		interact = new Callable(this, MethodName._on_interact);
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (InteractionManager.Instance != null)
		{
			InteractionManager.Instance.RegisterArea(this);
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (InteractionManager.Instance != null)
		{
			InteractionManager.Instance.UnregisterArea(this);
		}
	}

	private async void _on_interact()
	{
		// Check if the DialogManager instance is available before using it.
		if (DialogManager.Instance == null)
		{
			GD.PrintErr("DialogManager singleton not found. Did you forget to set it up in Autoload?");
			return;
		}

		DialogManager.Instance.StartDialog(GlobalPosition, Lines, SpeechSound);
		
		var bodies = GetOverlappingBodies();
		if (Sprite != null && bodies.Count > 0)
		{
			var interactor = bodies[0];
			Sprite.FlipH = interactor.GlobalPosition.X < GlobalPosition.X;
		}

		await ToSignal(DialogManager.Instance, DialogManager.SignalName.DialogFinished);
	}
}
