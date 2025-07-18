using Godot;
using System;
using System.Collections.Generic;

public partial class SelectCharacter : Control
{
	private readonly Dictionary<Classe, (int health, int mana, int strength, string weapon)> _classStats = new()
	{
		{ Classe.Bollywood, (100, 50, 10, "Fists of Fury") },
		{ Classe.CalculatorThief, (80, 70, 15, "Sharp Calculator") },
		{ Classe.Nudist, (120, 20, 20, "Bare Hands") },
		{ Classe.MathTeacher, (90, 80, 5, "Ruler of Doom") },
		{ Classe.MilitaryGirl, (110, 40, 25, "Standard Issue Rifle") },
		{ Classe.Stewardess, (95, 60, 12, "Serving Tray") },
		{ Classe.Ai, (150, 150, 30, "Laser Beam") }
	};

	public override void _Ready()
	{
		GD.Print("SelectCharacter script is ready!");

		// Connect buttons to their respective class selection methods
		GetNode<Button>("Panel/TextureContainer/ContainerBollywoodActor/ButtonBollywoodActor").Pressed += () => OnClassButtonPressed(Classe.Bollywood);
		GetNode<Button>("Panel/TextureContainer/ContainerCalculatorThief/ButtonCalculatorThief").Pressed += () => OnClassButtonPressed(Classe.CalculatorThief);
		GetNode<Button>("Panel/TextureContainer/ContainerNudiste/ButtonNudist").Pressed += () => OnClassButtonPressed(Classe.Nudist);
		GetNode<Button>("Panel/TextureContainer/ContainerMathTeacher/ButtonMathTeacher").Pressed += () => OnClassButtonPressed(Classe.MathTeacher);
		GetNode<Button>("Panel/TextureContainer/ContainerMilitaryGirl/ButtonMilitaryGirl").Pressed += () => OnClassButtonPressed(Classe.MilitaryGirl);
		GetNode<Button>("Panel/TextureContainer/ContainerStewardess/ButtonStewardess").Pressed += () => OnClassButtonPressed(Classe.Stewardess);
		
		// Assuming there will be a button for the AI class, named "AiButton"
		// If you create a button for the AI class, uncomment the following line and ensure the path is correct.
		// GetNode<Button>("Panel/TextureContainer/ContainerAi/AiButton").Pressed += () => OnClassButtonPressed(Classe.Ai);
	}

	private void OnClassButtonPressed(Classe selectedClass)
	{
		GD.Print($"{selectedClass} button pressed!");
		GameData.Instance.CharacterClass = selectedClass;

		var stats = _classStats[selectedClass];
		// Here you would apply the stats to the player character
		// For example:
		// GameData.Instance.PlayerHealth = stats.health;
		// GameData.Instance.PlayerMana = stats.mana;
		// GameData.Instance.PlayerStrength = stats.strength;
		// GameData.Instance.PlayerWeapon = stats.weapon;

		// For now, let's just print them
		GD.Print($"Stats - Health: {stats.health}, Mana: {stats.mana}, Strength: {stats.strength}, Weapon: {stats.weapon}");

		int characterId = DatabaseManager.Instance.GetCharacterByPlayerId(GameData.Instance.PlayerId);
		if (characterId == -1)
		{
			// The last parameter to CreateCharacter was an int, presumably for character type.
			// I'll cast the Classe enum to an int here.
			characterId = DatabaseManager.Instance.CreateCharacter(GameData.Instance.PlayerId, GameData.Instance.PlayerName + "'s character", (int)selectedClass + 1);
		}

		GD.Print($"Created or found character with ID: {characterId}");
		ChangeToNameGeneratorScene();
	}

	private void ChangeToNameGeneratorScene()
	{
		GD.Print("Attempting to change scene to name_generator...");
		GetTree().ChangeSceneToFile("res://scenes/name_generator.tscn");
	}
}
