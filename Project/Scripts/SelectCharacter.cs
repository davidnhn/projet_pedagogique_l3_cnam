using Godot;
using System;

public partial class SelectCharacter : Node2D
{
	private TextureButton boyButton;
	private TextureButton girlButton;

	public override void _Ready()
	{
		GD.Print("SelectCharacter script is ready!");
		
		try
		{
			// Get buttons through TextureRect path
			boyButton = GetNode<TextureButton>("TextureRect/Boy_Button");
			girlButton = GetNode<TextureButton>("TextureRect/Girl_Button");
			GD.Print("Found buttons successfully!");
		}
		catch (Exception e)
		{
			GD.PrintErr("Could not find buttons. Error: " + e.Message);
			return;
		}

		// Connect button signals
		boyButton.Pressed += OnBoyButtonPressed;
		girlButton.Pressed += OnGirlButtonPressed;
	}

	private void OnBoyButtonPressed()
	{
		GD.Print("Boy button pressed!");
		GameData.Instance.CharacterGender = "Boy";
		
		int characterId = DatabaseManager.Instance.GetCharacterByPlayerId(GameData.Instance.PlayerId);
		if (characterId == -1)
		{
			characterId = DatabaseManager.Instance.CreateCharacter(GameData.Instance.PlayerId, GameData.Instance.PlayerName + "'s character", 1);
		}
		
		GD.Print($"Created or found character with ID: {characterId}");
		ChangeToGameScene();
	}

	private void OnGirlButtonPressed()
	{
		GD.Print("Girl button pressed!");
		GameData.Instance.CharacterGender = "Girl";

		int characterId = DatabaseManager.Instance.GetCharacterByPlayerId(GameData.Instance.PlayerId);
		if (characterId == -1)
		{
			characterId = DatabaseManager.Instance.CreateCharacter(GameData.Instance.PlayerId, GameData.Instance.PlayerName + "'s character", 2);
		}

		GD.Print($"Created or found character with ID: {characterId}");
		ChangeToGameScene();
	}

	private void ChangeToGameScene()
	{
		GD.Print("Attempting to change scene...");
		GetTree().ChangeSceneToFile("res://scenes/name_generator.tscn");
	}
}
