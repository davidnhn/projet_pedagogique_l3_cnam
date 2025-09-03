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
	private string _firstOptionText = null;
	private Button _closeButton;

	public override void _Ready()
	{
		_panel = GetNode<Panel>("CanvasLayer/Panel");
		_dialogSpeaker = GetNode<Label>("CanvasLayer/Panel/DialogBox/DialogSpeaker");
		_dialogText = GetNode<RichTextLabel>("CanvasLayer/Panel/DialogBox/DialogText");
		_dialogOptions = GetNode<VBoxContainer>("CanvasLayer/Panel/DialogBox/DialogOptions");
		_closeButton = GetNode<Button>("CanvasLayer/Panel/CloseButton");
		if (_closeButton != null)
		{
			_closeButton.Pressed += () => { GD.Print("[DialogUI] Close button pressed"); GetViewport().SetInputAsHandled(); HideDialog(); };
		}

		var canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
		if (canvasLayer != null)
		{
			canvasLayer.Layer = 100;
		}

		// Ensure UI processes and captures input reliably
		ProcessMode = Node.ProcessModeEnum.Always;
		MouseFilter = MouseFilterEnum.Ignore;
		_panel.MouseFilter = MouseFilterEnum.Stop;

		HideDialog();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") || @event.IsActionPressed("ui_cancel"))
		{
			GD.Print("[DialogUI] _Input received action");
		}
		if (_panel.Visible && @event.IsActionPressed("ui_cancel"))
		{
			GetViewport().SetInputAsHandled();
			HideDialog();
			return;
		}
		if (_panel.Visible && @event.IsActionPressed("ui_accept"))
		{
			GetViewport().SetInputAsHandled();
			if (!string.IsNullOrEmpty(_firstOptionText))
			{
				GD.Print($"[DialogUI] _Input accept -> selecting '{_firstOptionText}'");
				_on_option_selected(_firstOptionText);
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_panel.Visible)
		{
			return;
		}
		if (@event is InputEventKey key && key.Pressed && !key.Echo)
		{
			if (key.Keycode == Key.Space || key.Keycode == Key.Enter)
			{
				GD.Print("[DialogUI] _UnhandledInput Space/Enter");
				GetViewport().SetInputAsHandled();
				if (!string.IsNullOrEmpty(_firstOptionText))
				{
					_on_option_selected(_firstOptionText);
				}
			}
		}
	}

	// You can add your methods for controlling the dialog UI below
	public void ShowDialog(string speaker, string text, Godot.Collections.Dictionary options)
	{
		Visible = true;
		_panel.Visible = true;
		_dialogSpeaker.Text = speaker;
		_dialogText.Text = text;
		_firstOptionText = null;

		// Remove existing options
		foreach (Node child in _dialogOptions.GetChildren())
		{
			child.QueueFree();
		}

		// Populate options
		if (options != null)
		{
			bool first = true;
			foreach (var key in options.Keys)
			{
				var optionText = key.AsString();
				var button = new Button();
				button.Text = optionText;
				button.AddThemeFontSizeOverride("font_size", 20);
				button.FocusMode = FocusModeEnum.All;
				button.Pressed += () => { GD.Print($"[DialogUI] Option selected: {optionText}"); GetViewport().SetInputAsHandled(); _on_option_selected(optionText); };
				_dialogOptions.AddChild(button);
				if (first)
				{
					_firstOptionText = optionText;
					button.GrabFocus();
					first = false;
				}
			}
		}
	}

	public void HideDialog()
	{
		_panel.Visible = false;
		Visible = false;
		EmitSignal(SignalName.DialogClosed);
	}

	private void _on_option_selected(string option)
	{
		EmitSignal(SignalName.OptionSelected, option);
	}

	public bool TryTriggerFirstOption()
	{
		if (!string.IsNullOrEmpty(_firstOptionText))
		{
			GD.Print($"[DialogUI] Programmatic select: {_firstOptionText}");
			_on_option_selected(_firstOptionText);
			return true;
		}
		return false;
	}

	//close dialog
	public void _on_close_button_pressed()
	{
		GD.Print("[DialogUI] Close button pressed");
		GetViewport().SetInputAsHandled();
		HideDialog();
	}
}
