using Godot;
using System;

public partial class GameData : Node
{
    // Instance unique du singleton
    private static GameData _instance;
    
    // Données du joueur
    private string _playerName = "";
    
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameData();
            }
            return _instance;
        }
    }
    
    // Propriété pour accéder au nom du joueur
    public string PlayerName
    {
        get => _playerName;
        set
        {
            _playerName = value;
            GD.Print($"Player name set to: {_playerName}");
        }
    }
    
    public override void _Ready()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        // S'assure que le node persiste entre les scènes
        ProcessMode = ProcessModeEnum.Always;
    }
} 