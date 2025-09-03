using Godot;
using System;

[GlobalClass]
public partial class Dialog : Resource
{
    [Export] public Godot.Collections.Dictionary Dialogs = new();
    

    // Load dialog data
    public void LoadFromJson(string filePath)
    {
        var data = FileAccess.GetFileAsString(filePath);
        if (string.IsNullOrEmpty(data))
        {
            GD.PrintErr($"Could not read file or file is empty: {filePath}");
            return;
        }

        var parsedVariant = Json.ParseString(data);

        if (parsedVariant.VariantType == Variant.Type.Dictionary)
        {
            Dialogs = parsedVariant.AsGodotDictionary();
        }
        else
        {
            GD.PrintErr($"Failed to parse JSON from '{filePath}' or it is not a dictionary.");
        }
    }

    // Return individual NPC dialogs
    public Godot.Collections.Array GetNpcDialog(string npcId)
    {
        if (Dialogs.TryGetValue(npcId, out var npcDialogVariant))
        {
            var npcDialogData = npcDialogVariant.AsGodotDictionary();
            if (npcDialogData.TryGetValue("tree", out var treeVariant))
            {
                return treeVariant.AsGodotArray();
            }
            if (npcDialogData.TryGetValue("trees", out var treesVariant))
            {
                return treesVariant.AsGodotArray();
            }
        }

        return new Godot.Collections.Array();
    }
}
    