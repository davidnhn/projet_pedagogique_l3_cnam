using Godot;
using System;

public partial class Npc : CharacterBody2D
{
	[Export] public string npc_name { get; set; }
	[Export] public string npc_id { get; set; }

	// Dialog vars
	public DialogManager DialogManager { get; set; }
	[Export] public Dialog DialogResource { get; set; }
	[Export] public bool AutoStartOnProximity { get; set; } = false;
	[Export] public float ProximityRadius { get; set; } = 48f;
	private bool _dialogActive = false;
	private bool _waitForExit = false;

	public override void _Ready()
	{
		if (DialogResource != null)
		{
			DialogResource.LoadFromJson("res://resources/Dialog/dialog_data.json");
		}
		
		DialogManager = GetNode<DialogManager>("DialogManager");
		if (DialogManager != null)
		{
			DialogManager.Connect(DialogManager.SignalName.DialogClosed, new Callable(this, nameof(OnDialogClosed)));
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
		DialogManager = GetNode<DialogManager>("DialogManager");
	}

	public override void _Process(double delta)
	{
		if (!AutoStartOnProximity || _dialogActive)
		{
			return;
		}
		var playerNode2D = Global.Player as Node2D;
		if (playerNode2D == null)
		{
			return;
		}
		float distance = GlobalPosition.DistanceTo(playerNode2D.GlobalPosition);
		if (_waitForExit)
		{
			// Require the player to leave the radius before allowing auto-start again
			if (distance > ProximityRadius * 1.1f)
			{
				_waitForExit = false;
			}
			return;
		}
		if (distance <= ProximityRadius)
		{
			if (playerNode2D is Character ch)
			{
				ch.SetMovementEnabled(false);
			}
			StartDialog();
			_dialogActive = true;
		}
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

	private void OnDialogClosed()
	{
		_dialogActive = false;
		_waitForExit = true;
		if (Global.Player is Character ch)
		{
			ch.SetMovementEnabled(true);
		}
	}
}
