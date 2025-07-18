using Godot;
using System;

public partial class GameData : Node
{
    // Instance unique du singleton
    public static GameData Instance { get; private set; }
    
    // Données du joueur
    private string _playerName = "";
    private int _playerId = -1;
    private string _characterGender = "";
    private Classe _characterClass;
    
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

    public Classe CharacterClass
    {
        get => _characterClass;
        set
        {
            _characterClass = value;
            GD.Print($"Character class set to: {_characterClass}");
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
        Instance = this;
    }
} 