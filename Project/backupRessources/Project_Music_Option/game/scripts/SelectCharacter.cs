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
        ChangeToGameScene();
    }

    private void OnGirlButtonPressed()
    {
        GD.Print("Girl button pressed!");
        ChangeToGameScene();
    }

    private void ChangeToGameScene()
    {
        GD.Print("Attempting to change scene...");
        GetTree().ChangeSceneToFile("res://scenes/start_adventure.tscn");
    }
}