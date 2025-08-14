using Godot;
using System;
using System.Threading.Tasks;

public partial class DialogManager : Node
{
	[Signal]
	public delegate void DialogFinishedEventHandler();

	private static DialogManager _instance;
	public static DialogManager Instance => _instance;

	private Label _dialogLabel;
	private Panel _dialogContainer;

	public override void _EnterTree()
	{
		// This ensures there is only one instance of the DialogManager.
		if (_instance != null && _instance != this)
		{
			QueueFree(); 
			return;
		}
		_instance = this;
	}

	public override void _Ready()
	{
		// Get references to the nodes in the scene.
		_dialogLabel = GetNode<Label>("dialogLabel");
		_dialogContainer = GetNode<Panel>("dialogContainer");

		// Ensure the dialog is hidden by default.
		_dialogContainer.Hide();
	}

	/// <summary>
	/// Starts the dialog process. In a real game, this would show a UI,
	/// display lines one by one, and wait for player input.
	/// </summary>
	public void StartDialog(Vector2 position, string[] lines, AudioStream speechSound)
	{
		// Position and show the dialog box.
		_dialogContainer.GlobalPosition = position;
		_dialogContainer.Show();
		
		// This starts the process of showing lines and waiting.
		ProcessDialog(lines);
	}

	private async void ProcessDialog(string[] lines)
	{
		foreach (var line in lines)
		{
			_dialogLabel.Text = line;
			// Wait for player input or a timer.
			await ToSignal(GetTree().CreateTimer(1.5), SceneTreeTimer.SignalName.Timeout);
		}
		
		// Hide the dialog and notify that it's finished.
		_dialogContainer.Hide();
		EmitSignal(SignalName.DialogFinished);
	}
}
