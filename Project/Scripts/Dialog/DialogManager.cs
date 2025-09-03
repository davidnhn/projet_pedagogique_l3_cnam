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
			string speaker = npc.npc_name;
			if (dialog.ContainsKey("speaker"))
			{
				speaker = dialog["speaker"].AsString();
			}
			_dialogUI.ShowDialog(speaker, dialog["text"].AsString(), dialog["options"].AsGodotDictionary());
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
		string requestedNextState = "start";
		if (options.ContainsKey(option))
		{
			requestedNextState = options[option].AsString();
		}

		// Compute current dialog index BEFORE changing state
		int currentIndex = -1;
		var npcDialogs = Npc.DialogResource.GetNpcDialog(Npc.npc_id);
		if (Npc.current_branch_index < npcDialogs.Count)
		{
			var branch = npcDialogs[Npc.current_branch_index].AsGodotDictionary();
			if (branch.TryGetValue("dialogs", out var dialogsVar))
			{
				var dialogs = dialogsVar.AsGodotArray();
				for (int i = 0; i < dialogs.Count; i++)
				{
					var d = dialogs[i].AsGodotDictionary();
					if (d.TryGetValue("state", out var st) && st.AsString() == Npc.current_state)
					{
						currentIndex = i;
						break;
					}
				}
			}
		}

		string nextStateToSet = requestedNextState;
		bool autoAdvanced = false;
		if (string.IsNullOrEmpty(requestedNextState) && currentIndex >= 0)
		{
			// Auto-advance to the next dialog entry in the current branch
			var branch = npcDialogs[Npc.current_branch_index].AsGodotDictionary();
			var dialogs = branch["dialogs"].AsGodotArray();
			if (currentIndex + 1 < dialogs.Count)
			{
				var nextDialog = dialogs[currentIndex + 1].AsGodotDictionary();
				if (nextDialog.TryGetValue("state", out var nextSt))
				{
					nextStateToSet = nextSt.AsString();
					autoAdvanced = true;
				}
			}
		}

		Npc.SetDialogState(nextStateToSet);

		if (nextStateToSet == "end")
		{
			if (Npc.current_branch_index < Npc.DialogResource.GetNpcDialog(Npc.npc_id).Count - 1)
			{
				Npc.SetDialogTree(Npc.current_branch_index + 1);
			}
			hide_dialog();
		}
		else if (nextStateToSet == "exit")
		{
			Npc.SetDialogState("start");
			hide_dialog();
		}
		else
		{
			show_dialog(Npc);
		}
	}
}
