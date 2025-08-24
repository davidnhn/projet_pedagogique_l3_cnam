using Godot;

public partial class DialogUI : Control
{
	[Signal]
	public delegate void DialogClosedEventHandler();
	[Signal]
	public delegate void OptionSelectedEventHandler(string option);

	private Panel _panel;
	private Label _dialogSpeaker;
	private RichTextLabel _dialogText;
	private VBoxContainer _dialogOptions;

	public override void _Ready()
	{
		_panel = GetNode<Panel>("CanvasLayer/Panel");
		_dialogSpeaker = GetNode<Label>("CanvasLayer/Panel/DialogBox/DialogSpeaker");
		_dialogText = GetNode<RichTextLabel>("CanvasLayer/Panel/DialogBox/DialogText");
		_dialogOptions = GetNode<VBoxContainer>("CanvasLayer/Panel/DialogBox/DialogOptions");

		HideDialog();
	}

	public override void _Input(InputEvent @event)
	{
		if (_panel.Visible && @event.IsActionPressed("ui_cancel"))
		{
			HideDialog();
		}
	}

	// You can add your methods for controlling the dialog UI below
	public void ShowDialog(string speaker, string text, Godot.Collections.Dictionary options)
	{
		_panel.Visible = true;
		_dialogSpeaker.Text = speaker;
		_dialogText.Text = text;

		// Remove existing options
		foreach (Node child in _dialogOptions.GetChildren())
		{
			child.QueueFree();
		}

		// Populate options
		if (options != null)
		{
			foreach (var key in options.Keys)
			{
				var optionText = key.AsString();
				var button = new Button();
				button.Text = optionText;
				button.AddThemeFontSizeOverride("font_size", 20);
				button.Pressed += () => _on_option_selected(optionText);
				_dialogOptions.AddChild(button);
			}
		}
	}

	public void HideDialog()
	{
		if (_panel.Visible)
		{
			_panel.Visible = false;
			EmitSignal(SignalName.DialogClosed);
		}
	}

	private void _on_option_selected(string option)
	{
		EmitSignal(SignalName.OptionSelected, option);
	}

//close dialog
	public void _on_close_button_pressed()
	{
		HideDialog();
	}
}
