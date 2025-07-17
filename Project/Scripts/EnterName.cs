using Godot;
using System;

public partial class EnterName : Control
{
    private LineEdit nameInput;
    private Button validateButton;
    private Label messageLabel;
    
    public override void _Ready()
    {
        // Récupère les références aux nodes
        nameInput = GetNode<LineEdit>("NameInput");
        validateButton = GetNode<Button>("Validate_Button");
        messageLabel = GetNode<Label>("MessageLabel");
        
        // Vérifie si les nodes ont été trouvés
        if (nameInput == null)
        {
            GD.PrintErr("NameInput not found! Check the node path.");
            return;
        }
        
        if (validateButton == null)
        {
            GD.PrintErr("Validate_Button not found! Check the node path.");
            return;
        }

        if (messageLabel == null)
        {
            GD.PrintErr("MessageLabel not found! Check the node path.");
            return;
        }
        
        // Connecte le signal du bouton
        validateButton.Connect("pressed", new Callable(this, nameof(OnValidateButtonPressed)));
    }
    
    private void OnValidateButtonPressed()
    {
        GD.Print("Validate button pressed"); // Debug log
        
        string playerName = nameInput.Text.Trim();
        
        // Vérifie si le nom n'est pas vide
        if (string.IsNullOrEmpty(playerName))
        {
            GD.Print("Name is empty"); // Debug log
            messageLabel.Text = "Veuillez entrer un nom !";
            messageLabel.Modulate = new Color(1, 0, 0); // Rouge pour l'erreur
            return;
        }

        if (DatabaseManager.Instance.PlayerExists(playerName))
        {
            messageLabel.Text = "Ce nom est déjà pris !";
            messageLabel.Modulate = new Color(1, 0, 0); // Rouge pour l'erreur
            return;
        }
        
        // Stocke le nom dans le singleton GameData
        GameData.Instance.PlayerName = playerName;
        
        int playerId = DatabaseManager.Instance.GetPlayerByName(playerName);
        if (playerId == -1)
        {
            // Player does not exist, create a new one
            playerId = DatabaseManager.Instance.CreatePlayer(playerName);
        }

        GameData.Instance.PlayerId = playerId;
        GD.Print($"Name stored: {playerName} with ID: {GameData.Instance.PlayerId}"); // Debug log
        
        // Arrêter la musique du menu si elle existe
        var musicManager = GetTree().Root.GetNode<MusicManager>("MusicManager");
        if (musicManager != null && musicManager.Playing)
        {
            musicManager.Stop();
            musicManager.QueueFree(); // Supprime le node singleton
        }
        
        // Change directement la scène vers select_character.tscn
        Error err = GetTree().ChangeSceneToFile("res://scenes/select_character.tscn");
        if (err != Error.Ok)
        {
            GD.PrintErr($"Failed to change scene. Error: {err}");
        }
    }
} 