using Godot;
using System;

public partial class Npc : CharacterBody2D
{
	[Export] public string npc_name { get; set; }
	[Export] public string npc_id { get; set; }
	[Export] public string[] dialog_lines { get; set; }
}
