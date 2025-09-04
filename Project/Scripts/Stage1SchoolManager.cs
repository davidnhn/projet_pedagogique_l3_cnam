using Godot;
using System.Collections.Generic;

public partial class Stage1SchoolManager : Node2D
{
    private Control _optionsMenu;
    private Node2D _player;
    private Button[] _menuButtons;
    private int _currentSelection = 0;
    private bool _isOptionsMenuVisible = false;

    private static readonly Dictionary<Classe, string> _classToScenePath = new()
    {
        { Classe.Bollywood, "res://scenes/players/Indian.tscn" },
        { Classe.CalculatorThief, "res://scenes/players/CalculatorThief.tscn" },
        { Classe.Nudist, "res://scenes/players/Camper.tscn" },
        { Classe.MathTeacher, "res://scenes/players/MathTeacher.tscn" },
        { Classe.MilitaryGirl, "res://scenes/players/MilitaryGirl.tscn" },
        { Classe.Stewardess, "res://scenes/players/FlightAttendant.tscn" }
        // Classe.Ai not mapped; fallback will be used
    };

    private static readonly Dictionary<Classe, string> _classToSpriteFrames = new()
    {
        { Classe.Bollywood, "res://scenes/players/SpriteFrame/Indian.tres" },
        { Classe.CalculatorThief, "res://scenes/players/SpriteFrame/CalculatorThief.tres" },
        { Classe.Nudist, "res://scenes/players/SpriteFrame/Camper.tres" },
        { Classe.MathTeacher, "res://scenes/players/SpriteFrame/MathTeacher.tres" },
        { Classe.MilitaryGirl, "res://scenes/players/SpriteFrame/MilitaryGirl.tres" },
        { Classe.Stewardess, "res://scenes/players/SpriteFrame/FlightAttendant.tres" }
    };

    private void ApplyPlayerSpriteFrames()
    {
        if (_player == null) { GD.PushWarning("Player node is null when applying frames."); return; }
        var selectedClass = GameData.Instance?.CharacterClass;
        string framesPath = null;
        if (selectedClass.HasValue && _classToSpriteFrames.TryGetValue(selectedClass.Value, out framesPath))
        {
            GD.Print($"Selected class: {selectedClass.Value}");
        }
        else
        {
            GD.PushWarning("No selected class or mapping not found; falling back to Indian frames.");
            framesPath = "res://scenes/players/SpriteFrame/Indian.tres";
        }

        GD.Print($"Loading SpriteFrames from: {framesPath}");
        var frames = ResourceLoader.Load<SpriteFrames>(framesPath);
        if (frames == null)
        {
            GD.PushWarning($"SpriteFrames not found at {framesPath}");
            return;
        }

		AnimatedSprite2D anim = null;
		anim = _player.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		if (anim == null)
		{
			var found = _player.FindChild("AnimatedSprite2D", true, false);
			anim = found as AnimatedSprite2D;
		}

		if (anim != null)
		{
			GD.Print($"Applying frames to AnimatedSprite2D on Player: {framesPath}");
			anim.SpriteFrames = frames;
			if (frames.HasAnimation("idle"))
			{
				anim.Play("idle");
			}
		}
		else
		{
			GD.PushWarning("AnimatedSprite2D not found under Player node (recursive search).");
		}
    }

    private void EnsurePlayerInstance()
    {
        // Try to find existing Player (any Node2D)
        _player = GetNodeOrNull<Node2D>("Player");
        if (_player != null)
        {
            Global.Player = _player;
            return;
        }

        // Instantiate based on selected class
        var selectedClass = GameData.Instance?.CharacterClass;
        string path;
        if (selectedClass.HasValue && _classToScenePath.TryGetValue(selectedClass.Value, out path))
        {
            var packed = ResourceLoader.Load<PackedScene>(path);
            if (packed != null)
            {
                var instance = packed.Instantiate<Node2D>();
                if (instance != null)
                {
                    instance.Name = "Player";
                    AddChild(instance);
                    _player = instance;
                    Global.Player = _player;
                }
            }
        }
        else
        {
            // Fallback: default to Indian
            const string defaultPath = "res://scenes/players/Indian.tscn";
            var packed = ResourceLoader.Load<PackedScene>(defaultPath);
            if (packed != null)
            {
                var instance = packed.Instantiate<Node2D>();
                if (instance != null)
                {
                    instance.Name = "Player";
                    AddChild(instance);
                    _player = instance;
                    Global.Player = _player;
                    GD.PushWarning("No class selected or mapping missing; defaulted to Indian player.");
                }
            }
        }
    }

    public override void _Ready()
    {
        // Récupérer la référence au menu d'options
        _optionsMenu = GetNode<Control>("OptionsMenuInGame");
        
        if (_optionsMenu == null)
        {
            GD.PrintErr("OptionsMenuInGame not found in Stage1School scene!");
            return;
        }

        // Ensure player exists and matches selection
        EnsurePlayerInstance();
        
        // Récupérer le personnage
        _player ??= GetNodeOrNull<Node2D>("Player");
        CallDeferred(nameof(ApplyPlayerSpriteFrames));
        
        // Récupérer les boutons du menu
        _menuButtons = new Button[]
        {
            _optionsMenu.GetNode<Button>("VBoxContainer/ItemButtonInGame"),
            _optionsMenu.GetNode<Button>("VBoxContainer/SaveButtonInGame"),
            _optionsMenu.GetNode<Button>("VBoxContainer/ExitButtonInGame")
        };
        
        // Cacher le menu d'options au démarrage
        _optionsMenu.Visible = false;
        
        GD.Print("Stage1SchoolManager initialized successfully!");
        
        // Appeler la méthode de positionnement après l'initialisation
        CallDeferred(nameof(HandleSavedPosition));
    }

    private void HandleSavedPosition()
    {
        // Vérifier si on vient d'un chargement de sauvegarde
        if (GameData.Instance.PlayerPosition != Vector2.Zero)
        {
            GD.Print($"Loading saved position: {GameData.Instance.PlayerPosition}");
            if (_player != null)
            {
                _player.GlobalPosition = GameData.Instance.PlayerPosition;
            }
            // Réinitialiser la position pour éviter de la réutiliser lors du prochain démarrage
            GameData.Instance.PlayerPosition = Vector2.Zero;
        }
        else
        {
            GD.Print("Starting new game - using default player position");
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Si l'inventaire est ouvert, ignorer la touche X
        if (GetNodeOrNull<Control>("UIInventory") != null)
            return;

        // Détecter la touche X pour ouvrir/fermer le menu
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.X)
        {
            GD.Print($"X key pressed! Menu visible: {_isOptionsMenuVisible}");
            ToggleOptionsMenu();
            GetViewport().SetInputAsHandled();
        }
        
        // Navigation dans le menu avec les flèches haut/bas
        if (_isOptionsMenuVisible)
        {
            if (@event is InputEventKey inputKey && inputKey.Pressed)
            {
                switch (inputKey.Keycode)
                {
                    case Key.Up:
                        GD.Print("Up arrow pressed");
                        MoveSelection(-1);
                        GetViewport().SetInputAsHandled();
                        break;
                    case Key.Down:
                        GD.Print("Down arrow pressed");
                        MoveSelection(1);
                        GetViewport().SetInputAsHandled();
                        break;
                    case Key.Enter:
                        GD.Print("Enter pressed");
                        SelectCurrentOption();
                        GetViewport().SetInputAsHandled();
                        break;
                }
            }
        }
    }

    private void ToggleOptionsMenu()
    {
        if (_optionsMenu == null) 
        {
            GD.PrintErr("OptionsMenu is null!");
            return;
        }
        
        GD.Print($"Before toggle - Menu visible: {_isOptionsMenuVisible}");
        
        _isOptionsMenuVisible = !_isOptionsMenuVisible;
        _optionsMenu.Visible = _isOptionsMenuVisible;
        
        // Positionner le menu à la position du personnage
        if (_isOptionsMenuVisible && _player != null)
        {
            // Convertir la position du personnage en position d'écran
            Vector2 playerScreenPos = GetViewport().GetCamera2D().GetScreenCenterPosition();
            Vector2 menuPos = _player.GlobalPosition - _optionsMenu.Size / 2;
            _optionsMenu.GlobalPosition = menuPos;
            
            GD.Print($"Menu positioned at player location: {menuPos}");
            
            // Initialiser la sélection
            _currentSelection = 0;
            UpdateSelection();
        }
        
        // Pause/Resume le jeu quand le menu est ouvert/fermé
        GetTree().Paused = _isOptionsMenuVisible;
        
        // Changer le ProcessMode seulement quand le menu est ouvert
        ProcessMode = _isOptionsMenuVisible ? Node.ProcessModeEnum.WhenPaused : Node.ProcessModeEnum.Inherit;
        
        GD.Print($"After toggle - Menu visible: {_isOptionsMenuVisible}, Game paused: {GetTree().Paused}");
    }

    private void MoveSelection(int direction)
    {
        _currentSelection += direction;
        
        // Boucler la sélection
        if (_currentSelection < 0)
            _currentSelection = _menuButtons.Length - 1;
        else if (_currentSelection >= _menuButtons.Length)
            _currentSelection = 0;
        
        GD.Print($"Selection moved to: {_currentSelection}");
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        // Mettre en évidence le bouton sélectionné
        for (int i = 0; i < _menuButtons.Length; i++)
        {
            if (_menuButtons[i] != null)
            {
                if (i == _currentSelection)
                {
                    _menuButtons[i].GrabFocus();
                    GD.Print($"Button {i} ({_menuButtons[i].Text}) is now selected");
                }
            }
        }
    }

    private void SelectCurrentOption()
    {
        if (_menuButtons == null || _currentSelection >= _menuButtons.Length)
            return;
        
        var selectedButton = _menuButtons[_currentSelection];
        if (selectedButton != null)
        {
            GD.Print($"Selected option: {selectedButton.Text}");
            
            // Gérer les actions selon l'option sélectionnée
            switch (selectedButton.Text.ToLower())
            {
                case "items":
                    GD.Print("Opening inventory...");
                    OpenInventoryOverlay();
                    break;
                case "save":
                    GD.Print("Saving game...");
                    SaveGame();
                    break;
                case "exit":
                    GD.Print("Returning to main menu...");
                    ReturnToMainMenu();
                    break;
            }
        }
    }

    private void SaveGame()
    {
        if (_player == null)
        {
            GD.PrintErr("Player not found for saving!");
            return;
        }

        // Récupérer les données du joueur depuis GameData
        var playerName = GameData.Instance?.PlayerName ?? "Unknown";
        var playerId = DatabaseManager.Instance?.GetPlayerByName(playerName) ?? -1;
        
        if (playerId == -1)
        {
            GD.PrintErr("Player not found in database!");
            return;
        }

        // Déterminer la zone actuelle - utiliser le vrai nom de zone de la base de données
        // Stage1School correspond à "Le Désert de la Cafet" dans la base de données
        var zoneId = DatabaseManager.Instance.GetZoneIdByName("Le Désert de la Cafet");
        if (zoneId == -1)
        {
            // Si la zone n'existe pas, utiliser l'ID 1 par défaut
            zoneId = 1;
        }

        // Sauvegarder dans la base de données
        var saveId = DatabaseManager.Instance.SaveGame(
            playerId,
            _player.GlobalPosition,
            zoneId
        );

        if (saveId > 0)
        {
            GD.Print($"Game saved successfully! Save ID: {saveId}");
        }
        else
        {
            GD.PrintErr("Failed to save game!");
        }
    }

    private void ReturnToMainMenu()
    {
        // Remettre le jeu en mode normal (pas en pause)
        GetTree().Paused = false;
        
        // Changer vers la scène du menu principal
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }

    private void OpenInventoryOverlay()
    {
        var inventoryScene = GD.Load<PackedScene>("res://scenes/inventory.tscn");
        if (inventoryScene != null)
        {
            var inventoryInstance = inventoryScene.Instantiate();
            var uiRoot = GetNode<CanvasLayer>("CanvasLayer");
            uiRoot.AddChild(inventoryInstance, true);
        }
        else
        {
            GD.PrintErr("Impossible de charger la scène d'inventaire !");
        }
    }
} 