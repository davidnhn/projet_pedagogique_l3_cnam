using Godot;
using System;

public partial class Npc : CharacterBody2D
{
	[Export] public string npc_name { get; set; }
	[Export] public string npc_id { get; set; }

	// Dialog vars
	public DialogManager DialogManager { get; set; }
	[Export] public Dialog DialogResource { get; set; }

	public override void _Ready()
	{
		if (DialogResource != null)
		{
			DialogResource.LoadFromJson("res://resources/Dialog/dialog_data.json");
		}
		if (Name == "Mallak")
		{
			var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			anim.Animation = "walk_right"; 
			anim.Stop();
			anim.Frame = 0; 
		}
				if (Name == "Charpentier")
		{
			var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			anim.Animation = "walk_left"; 
			anim.Stop();
			anim.Frame = 0; 
		}
		DialogManager = GetTree().GetFirstNodeInGroup("dialog_manager") as DialogManager;
	}

	public int current_branch_index = 0;
	public string current_state = "start";

	public void StartDialog()
	{
		if (DialogResource == null)
		{
			GD.PrintErr("DialogResource not set for this NPC!");
			return;
		}

		var npcDialogs = DialogResource.GetNpcDialog(npc_id);
		if (npcDialogs.Count == 0)
		{
			return;
		}

		if (DialogManager != null)
		{
			DialogManager.show_dialog(this);
		}
		else
		{
			GD.PrintErr("DialogManager node not found in scene. Did you add it to the 'dialog_manager' group?");
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
