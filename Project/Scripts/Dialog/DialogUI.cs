using Godot;

public partial class DialogUI : Control
{
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
	}

	// You can add your methods for controlling the dialog UI below
}
