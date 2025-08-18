using Godot;
using System.Threading.Tasks;

public partial class TestGuy : CharacterBody2D
{
	[Export]
	public float Speed { get; protected set; } = 200.0f;
	
	public bool IsAlive { get; protected set; } = true;

	[Export]
	private InteractionArea _interactionArea;

	[Export]
	private AnimatedSprite2D _sprite;

	[Export]
	private AudioStream _speechSound;

	private readonly string[] _lines = new string[]
	{
		"Hey there!"
	};

	public override void _Ready()
	{
		// This is the key part: this NPC is telling the InteractionArea
		// what to do when it gets interacted with.
		if (_interactionArea != null)
		{
			_interactionArea.interact = new Callable(this, MethodName._on_interact);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}
		
		// Ne pas traiter les touches si le jeu est en pause
		if (GetTree().Paused)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}
		
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection.Normalized() * Speed;
		
		MoveAndSlide();
	}

	private async void _on_interact()
	{
		if (DialogManager.Instance == null)
		{
			GD.PrintErr("DialogManager not found. Did you forget to add it to Autoload?");
			return;
		}

		// Start the dialog using the global manager
		DialogManager.Instance.StartDialog(GlobalPosition, _lines, _speechSound);
		
		// Flip the sprite based on who is interacting
		var bodies = _interactionArea.GetOverlappingBodies();
		if (_sprite != null && bodies.Count > 0)
		{
			var interactor = bodies[0]; // Assumes the player is the first body
			_sprite.FlipH = interactor.GlobalPosition.X < GlobalPosition.X;
		}

		// Wait for the dialog to finish before this method completes
		await ToSignal(DialogManager.Instance, DialogManager.SignalName.DialogFinished);
	}
}
