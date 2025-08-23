using Godot;

public partial class DialogManager : Node2D
{
	[Export] public NodePath DialogUiPath { get; set; }
	
	private DialogUI _dialogUI;
	public Npc Npc { get; set; }

	public override void _Ready()
	{
		_dialogUI = GetNode<DialogUI>(DialogUiPath);
	}
}
