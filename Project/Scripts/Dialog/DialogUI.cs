using Godot;

public partial class DialogUI : Control
{
	[Signal]
	public delegate void DialogClosedEventHandler();

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
			EmitSignal(SignalName.DialogClosed);
		}
	}

	// You can add your methods for controlling the dialog UI below
	public void ShowDialog()
	{
		_panel.Visible = true;
	}

	public void HideDialog()
	{
		_panel.Visible = false;
	}

//close dialog
	public void _on_close_button_pressed()
	{
		HideDialog();
		EmitSignal(SignalName.DialogClosed);//TODO: Check if this is correct
	}
}
