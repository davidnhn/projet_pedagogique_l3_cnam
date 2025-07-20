using Godot;

public partial class Stage1SchoolManager : Node2D
{
    private Control _optionsMenu;
    private Character _player;
    private Button[] _menuButtons;
    private int _currentSelection = 0;
    private bool _isOptionsMenuVisible = false;

    public override void _Ready()
    {
        // Récupérer la référence au menu d'options
        _optionsMenu = GetNode<Control>("OptionsMenuInGame");
        
        if (_optionsMenu == null)
        {
            GD.PrintErr("OptionsMenuInGame not found in Stage1School scene!");
            return;
        }
        
        // Récupérer le personnage
        _player = GetNode<Character>("Player");
        
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
    }

    public override void _Input(InputEvent @event)
    {
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
                    // Ici vous pouvez ajouter le code pour ouvrir l'inventaire
                    break;
                case "save":
                    GD.Print("Saving game...");
                    SaveGame();
                    break;
                case "exit":
                    GD.Print("Closing menu...");
                    ToggleOptionsMenu();
                    break;
            }
        }
    }

    private void SaveGame()
    {
        if (_player == null)
        {
            GD.PrintErr("Player not found for saving!");
            ShowSaveMessage("Erreur: Personnage non trouvé!", true);
            return;
        }

        // Récupérer les données du joueur depuis GameData
        var playerName = GameData.Instance?.PlayerName ?? "Unknown";
        var playerId = DatabaseManager.Instance?.GetPlayerByName(playerName) ?? -1;
        
        if (playerId == -1)
        {
            GD.PrintErr("Player not found in database!");
            ShowSaveMessage("Erreur: Joueur non trouvé dans la base de données!", true);
            return;
        }

        // Déterminer la zone actuelle (Stage1School = Zone 1)
        var zoneId = DatabaseManager.Instance.GetZoneIdByName("Stage1School");
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
            ShowSaveMessage("Partie sauvegardée avec succès!");
        }
        else
        {
            GD.PrintErr("Failed to save game!");
            ShowSaveMessage("Erreur lors de la sauvegarde!", true);
        }
    }

    private void ShowSaveMessage(string message, bool isError = false)
    {
        // Créer un label temporaire pour afficher le message
        var label = new Label();
        label.Text = message;
        label.AddThemeColorOverride("font_color", isError ? new Color(1, 0, 0) : new Color(0, 1, 0)); // Rouge si erreur, vert si succès
        label.Position = new Vector2(100, 100);
        label.Size = new Vector2(400, 50);
        
        AddChild(label);
        
        // Supprimer le message après 3 secondes
        var timer = new Timer();
        timer.WaitTime = 3.0f;
        timer.OneShot = true;
        timer.Timeout += () => {
            label.QueueFree();
            timer.QueueFree();
        };
        AddChild(timer);
        timer.Start();
    }
} 