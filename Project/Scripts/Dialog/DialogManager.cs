using System;
using Godot;

public partial class DialogManager : Node2D
{
	[Signal]
	public delegate void DialogClosedEventHandler();
	[Export] public NodePath DialogUiPath { get; set; }

	private DialogUI _dialogUI;
	public Npc Npc { get; set; }

	public override void _Ready()
	{
		_dialogUI = GetNode<DialogUI>(DialogUiPath);
		_dialogUI.OptionSelected += handle_dialog_choice;
		_dialogUI.DialogClosed += OnDialogUiClosed;
	}

	private void OnDialogUiClosed()
	{
		EmitSignal(SignalName.DialogClosed);
	}

	public void show_dialog(Npc npc, string text = "", Godot.Collections.Dictionary options = null)
	{
		this.Npc = npc;

		if (text != "")
		{
			_dialogUI.ShowDialog(npc.npc_name, text, options);
		}
		else
		{
			var dialog = npc.GetCurrentDialog();
			if (dialog == null)
			{
				return;
			}            
			_dialogUI.ShowDialog(npc.npc_name, dialog["text"].AsString(), dialog["options"].AsGodotDictionary());
		}
	}

	public void hide_dialog()
	{
		_dialogUI.HideDialog();
	}

	public void handle_dialog_choice(string option)
	{
		var current_dialog = Npc.GetCurrentDialog();
		if (current_dialog == null)
		{
			return;
		}

		var options = current_dialog["options"].AsGodotDictionary();
		string next_state = "start";
		if (options.ContainsKey(option))
		{
			next_state = options[option].AsString();
		}
		Npc.SetDialogState(next_state);

		if (next_state == "end")
		{
			if (Npc.current_branch_index < Npc.DialogResource.GetNpcDialog(Npc.npc_id).Count - 1)
			{
				Npc.SetDialogTree(Npc.current_branch_index + 1);
			}
			hide_dialog();
		}
		else if (next_state == "exit")
		{
			Npc.SetDialogState("start");
			hide_dialog();
		}
		else if (next_state == "give_quests")
		{
			// TODO: Implement quest logic
		}
		else
		{
			show_dialog(Npc);
		}
	}
}
