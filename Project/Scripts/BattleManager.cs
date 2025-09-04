using Godot;

/// <summary>
/// Gestionnaire principal du système de combat au tour par tour.
/// Contrôle l'interface utilisateur, la logique de combat et les interactions entre le joueur et l'ennemi.
/// </summary>
public partial class BattleManager : Control
{
	// ===== PROPRIÉTÉS EXPORTÉES =====
	[Export]
	public BaseEnemy Enemy { get; set; } = null; // Ressource de l'ennemi (configurée dans l'éditeur)
	
	// ===== STATS DU JOUEUR =====
	private int _playerMaxHealth = 100;    // Santé maximale du joueur
	private int _playerDamage = 20;        // Dégâts infligés par le joueur
	
	// ===== VARIABLES D'ÉTAT DU COMBAT =====
	private int _currentPlayerHealth = 0;  // Santé actuelle du joueur
	private int _currentEnemyHealth = 0;   // Santé actuelle de l'ennemi
	private bool _isDefending = false;     // Indique si le joueur se défend
	
	// ===== RÉFÉRENCES AUX NŒUDS DE L'INTERFACE =====
	private ProgressBar _enemyHealthBar;   // Barre de vie de l'ennemi
	private ProgressBar _playerHealthBar;  // Barre de vie du joueur
	private Label _enemyHealthLabel;       // Label affichant la santé de l'ennemi
	private Label _playerHealthLabel;      // Label affichant la santé du joueur
	private TextureRect _enemySprite;      // Sprite de l'ennemi
	private Control _actionsPanel;         // Panel contenant les boutons d'action
	private Label _messageLabel;           // Label pour afficher les messages de combat
	
	/// <summary>
	/// Appelé quand le nœud est prêt. Initialise les références et démarre le combat.
	/// </summary>
	public override void _Ready()
	{
		// Récupérer les références aux nœuds avec les bons chemins
		_enemyHealthBar = GetNode<ProgressBar>("EnemyContainer/ProgressBar");
		_playerHealthBar = GetNode<ProgressBar>("PlayerHealthPanel/PlayerData/ProgressBar");
		_enemyHealthLabel = GetNode<Label>("EnemyContainer/ProgressBar/Label");
		_playerHealthLabel = GetNode<Label>("PlayerHealthPanel/PlayerData/ProgressBar/HPLabel");
		_enemySprite = GetNode<TextureRect>("EnemyContainer/Enemy");
		_actionsPanel = GetNode<Control>("ActionsPanel");
		
		// Créer un label pour afficher les messages
		CreateMessageLabel();
		
		// If Enemy not set in editor, try to use pending enemy from GameData
		if (Enemy == null && GameData.Instance != null && GameData.Instance.PendingEnemy != null)
		{
			Enemy = GameData.Instance.PendingEnemy;
			GameData.Instance.PendingEnemy = null;
		}
		
		// Provide minimal fallback to avoid null
		if (Enemy == null)
		{
			Enemy = new BaseEnemy { EnemyName = "Adversaire", Health = 50, Damage = 8 };
		}
		
		// Initialiser le combat
		InitializeBattle();
	}
	
	/// <summary>
	/// Crée et configure le label de message de combat.
	/// Le label est ajouté comme enfant du PlayerHealthPanel pour une meilleure organisation.
	/// </summary>
	private void CreateMessageLabel()
	{
		_messageLabel = new Label();
		_messageLabel.Text = "";
		_messageLabel.HorizontalAlignment = HorizontalAlignment.Center;
		_messageLabel.VerticalAlignment = VerticalAlignment.Center;
		_messageLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1, 1));
		_messageLabel.AddThemeFontSizeOverride("font_size", 24);
		
		// Ajouter le label comme enfant du PlayerHealthPanel
		var playerHealthPanel = GetNode<Control>("PlayerHealthPanel");
		playerHealthPanel.AddChild(_messageLabel);
		
		// Positionner le label sous la barre de santé, centré horizontalement
		_messageLabel.Position = new Vector2(200, 60); // X décalé à droite (200), Y sous la barre
		_messageLabel.Size = new Vector2(400, 50); // Taille adaptée au panel
		
		_messageLabel.Visible = false;
	}
	
	/// <summary>
	/// Initialise le combat en configurant les barres de vie, l'ennemi et l'interface.
	/// </summary>
	private void InitializeBattle()
	{
		if (Enemy == null)
		{
			DisplayMessage("Erreur: Ennemi non configuré!");
			return;
		}
		
		// Configurer les barres de vie
		SetHealth(_enemyHealthBar, Enemy.Health, Enemy.Health);
		SetHealth(_playerHealthBar, _playerMaxHealth, _playerMaxHealth);
		
		// Configurer l'ennemi
		_enemySprite.Texture = Enemy.EnemyTexture;
		
		// Initialiser les variables de combat
		_currentPlayerHealth = _playerMaxHealth;
		_currentEnemyHealth = Enemy.Health;
		
		// Cacher le panel d'actions au début
		_actionsPanel.Visible = false;
		
		// Afficher le message de début de combat
		DisplayMessage($"Un {Enemy.EnemyName.ToUpper()} sauvage apparaît!");
		
		// Montrer le panel d'actions après un délai
		CallDeferred(nameof(ShowActionsPanel));
	}
	
	/// <summary>
	/// Affiche le panel d'actions et cache le message de combat.
	/// </summary>
	private void ShowActionsPanel()
	{
		_actionsPanel.Visible = true;
		_messageLabel.Visible = false;
	}
	
	/// <summary>
	/// Affiche un message de combat et cache temporairement le panel d'actions.
	/// Le message disparaît après 2 secondes et les actions réapparaissent.
	/// </summary>
	/// <param name="message">Le message à afficher</param>
	private void DisplayMessage(string message)
	{
		_messageLabel.Text = message;
		_messageLabel.Visible = true;
		_actionsPanel.Visible = false;
		
		// Masquer le message après 2 secondes
		GetTree().CreateTimer(2.0f).Timeout += () => {
			if (_messageLabel != null)
			{
				_messageLabel.Visible = false;
				_actionsPanel.Visible = true;
			}
		};
	}
	
	/// <summary>
	/// Met à jour une barre de vie et son label correspondant.
	/// </summary>
	/// <param name="progressBar">La barre de progression à mettre à jour</param>
	/// <param name="health">La santé actuelle</param>
	/// <param name="maxHealth">La santé maximale</param>
	private void SetHealth(ProgressBar progressBar, int health, int maxHealth)
	{
		progressBar.Value = health;
		progressBar.MaxValue = maxHealth;
		
		// Mettre à jour le label de santé correspondant
		if (progressBar == _enemyHealthBar)
		{
			_enemyHealthLabel.Text = $"Pv : {health}/{maxHealth}";
		}
		else if (progressBar == _playerHealthBar)
		{
			_playerHealthLabel.Text = $"HP: {health}/{maxHealth}";
		}
	}
	
	/// <summary>
	/// Gère le tour de l'ennemi. L'ennemi attaque le joueur avec gestion de la défense.
	/// </summary>
	private void EnemyTurn()
	{
		DisplayMessage($"{Enemy.EnemyName} se lance férocement sur toi!");
		
		if (_isDefending)
		{
			_isDefending = false;
			
			// Défense aléatoire : 40% de réussite, 60% d'échec
			if (GD.Randf() < 0.4f) // 40% de chance de réussite
			{
				DisplayMessage("Tu t'es défendu avec succès!");
			}
			else // 60% de chance d'échec
			{
				DisplayMessage("Ta défense a échoué!");
				_currentPlayerHealth = Mathf.Max(0, _currentPlayerHealth - Enemy.Damage);
				SetHealth(_playerHealthBar, _currentPlayerHealth, _playerMaxHealth);
				DisplayMessage($"{Enemy.EnemyName} t'a infligé {Enemy.Damage} dégâts!");
			}
		}
		else
		{
			// Attaque normale si le joueur ne se défend pas
			_currentPlayerHealth = Mathf.Max(0, _currentPlayerHealth - Enemy.Damage);
			SetHealth(_playerHealthBar, _currentPlayerHealth, _playerMaxHealth);
			DisplayMessage($"{Enemy.EnemyName} t'a infligé {Enemy.Damage} dégâts!");
		}
		
		// Vérifier si le joueur est mort
		if (_currentPlayerHealth <= 0)
		{
			DisplayMessage("Tu as été vaincu!");
			// Arrêter la scène après 3 secondes
			GetTree().CreateTimer(3.0f).Timeout += () => {
				GetTree().Quit();
			};
			return;
		}
	}
	
	// ===== MÉTHODES APPELÉES PAR LES BOUTONS D'ACTION =====
	
	/// <summary>
	/// Gère l'action de fuite. Affiche un message et ferme la scène.
	/// </summary>
	public void OnRunPressed()
	{
		DisplayMessage("Tu t'es échappé en sécurité!");
		// Ici tu peux ajouter la logique de fuite
		GetTree().Quit();
	}
	
	/// <summary>
	/// Gère l'action d'attaque. Le joueur attaque l'ennemi et peut le vaincre.
	/// </summary>
	public void OnAttackPressed()
	{
		DisplayMessage("Tu brandis ton épée perçante!");
		
		// Debug pour vérifier les valeurs
		GD.Print($"Santé ennemie avant attaque: {_currentEnemyHealth}");
		GD.Print($"Dégâts du joueur: {_playerDamage}");
		
		// Calculer les dégâts infligés
		_currentEnemyHealth = Mathf.Max(0, _currentEnemyHealth - _playerDamage);
		
		GD.Print($"Santé ennemie après attaque: {_currentEnemyHealth}");
		
		// Mettre à jour l'interface
		SetHealth(_enemyHealthBar, _currentEnemyHealth, Enemy.Health);
		
		DisplayMessage($"Tu as infligé {_playerDamage} dégâts!");
		
		// Vérifier si l'ennemi est vaincu
		if (_currentEnemyHealth <= 0)
		{
			DisplayMessage($"{Enemy.EnemyName} a été vaincu!");
			// Transition to Stage 2 after a short delay
			GetTree().CreateTimer(2.0f).Timeout += () => {
				GetTree().ChangeSceneToFile("res://scenes/stage/Stage2Laboratory.tscn");
			};
			return;
		}
		
		// Tour de l'ennemi après un délai
		GetTree().CreateTimer(2.0f).Timeout += EnemyTurn;
	}
	
	/// <summary>
	/// Gère l'action de défense. Le joueur se prépare à bloquer la prochaine attaque.
	/// La défense a 40% de chance de réussir et 60% d'échouer.
	/// </summary>
	public void OnDefendPressed()
	{
		_isDefending = true;
		DisplayMessage("Tu te prépares défensivement!");
		
		// Tour de l'ennemi après un délai
		GetTree().CreateTimer(2.0f).Timeout += EnemyTurn;
	}
}
