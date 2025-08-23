using Godot;
using System;

public partial class Npc : CharacterBody2D
{
	[Export] public string npc_name { get; set; }
	[Export] public string npc_id { get; set; }

	// Dialog vars
	[Export] public DialogManager DialogManager { get; set; }
	[Export] public Dialog DialogResource { get; set; }

	public override void _Ready()
	{
		if (DialogResource != null)
		{
			DialogResource.LoadFromJson("res://resources/Dialog/dialog_data.json");
		}
		
		if (DialogManager != null)
		{
			DialogManager.Npc = this;
		}
	}

	public int current_branch_index = 0;
	public string current_state = "default";

	public void StartDialog()
	{
		if (DialogResource == null)
		{
			GD.PrintErr("DialogResource not set for this NPC!");
			return;
		}

		var dialogTree = DialogResource.GetNpcDialog(npc_id);
		if (dialogTree.Count > 0)
		{
			// For now, let's just print the dialog tree.
			// You'll replace this with your actual dialog UI logic.
			GD.Print($"Found dialog for {npc_name}:");
			GD.Print(Json.Stringify(dialogTree));
		}
		else
		{
			GD.Print($"No dialog found for NPC with ID: {npc_id}");
		}
	}

	public Godot.Collections.Dictionary GetCurrentDialog()
	{
		if (DialogResource == null)
		{
			GD.PrintErr("DialogResource not set for this NPC!");
			return null;
		}

		var npcDialogs = DialogResource.GetNpcDialog(npc_id);

		if (current_branch_index < npcDialogs.Count)
		{
			var branch = npcDialogs[current_branch_index].AsGodotDictionary();
			if (branch.TryGetValue("dialogs", out var dialogsInBranchVariant))
			{
				var dialogsInBranch = dialogsInBranchVariant.AsGodotArray();
				foreach (var dialogVariant in dialogsInBranch)
				{
					var dialog = dialogVariant.AsGodotDictionary();
					if (dialog.TryGetValue("state", out var stateVariant) && stateVariant.AsString() == current_state)
					{
						return dialog;
					}
				}
			}
		}

		return null;
	}

	public void SetDialogTree(int branchIndex)
	{
		current_branch_index = branchIndex;
		current_state = "start";
	}

	public void SetDialogState(string state)
	{
		current_state = state;
	}
}
