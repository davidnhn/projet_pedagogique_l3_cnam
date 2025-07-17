using Godot;
using System;

public partial class GameData : Node
{
    // Instance unique du singleton
    private static GameData _instance;
    
    // Données du joueur
    private string _playerName = "";
    private int _playerId = -1;
    private string _characterGender = "";
    
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

    public int PlayerId
    {
        get => _playerId;
        set
        {
            _playerId = value;
            GD.Print($"Player ID set to: {_playerId}");
        }
    }

    public string CharacterGender
    {
        get => _characterGender;
        set
        {
            _characterGender = value;
            GD.Print($"Character gender set to: {_characterGender}");
        }
    }

    // Custom names for world generation
    public string PersonalName1 { get; set; }
    public string PersonalName2 { get; set; }
    public string PersonalName3 { get; set; }
    public string ProfessionName { get; set; }
    public string UnusualObjectName { get; set; }
    public string PlaceName { get; set; }

    // Three extra custom names
    public string ExtraName1 { get; set; }
    public string ExtraName2 { get; set; }
    public string ExtraName3 { get; set; }

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