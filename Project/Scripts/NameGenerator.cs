using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NameGenerator : Control
{
	// Structure pour contenir les listes de mots pour un thème
	private class WordTheme
	{
		public List<string> PersonalNames { get; set; }
		public List<string> Professions { get; set; }
		public List<string> UnusualObjects { get; set; }
		public List<string> PlaceNames { get; set; }
		public List<string> ExtraNames { get; set; }
	}

	// Dictionnaire contenant tous les thèmes
	private readonly Dictionary<string, WordTheme> _themes;
	private readonly Random _random = new Random();

	// Références aux nœuds de la scène
	private LineEdit _personalNameInput1;
	private LineEdit _personalNameInput2;
	private LineEdit _personalNameInput3;
	private LineEdit _professionNameInput;
	private LineEdit _unusualObjectInput;
	private LineEdit _placeNameInput;
	private LineEdit _extraNameInput1;
	private LineEdit _extraNameInput2;
	private LineEdit _extraNameInput3;
	private Button _validateButton;

	public NameGenerator()
	{
		// Initialisation des thèmes et des listes de mots
		_themes = new Dictionary<string, WordTheme>
		{
			{
				"Informatique", new WordTheme
				{
					PersonalNames = new List<string> { "Ada", "Grace", "Linus", "Ken", "Dennis", "Bjarne", "Guido", "Satoshi" },
					Professions = new List<string> { "Développeur", "Sysadmin", "Pentesteur", "Architecte Cloud", "Data Scientist" },
					UnusualObjects = new List<string> { "Dongle Quantique", "Processeur Neuronal", "Câble RGB Déphasé", "Compiler Hanté" },
					PlaceNames = new List<string> { "La Ferme de Serveurs", "Le Lac de Données", "Le Pare-Feu Ancestral", "La Forêt Binaire" },
					ExtraNames = new List<string> { "Kernel", "Byte", "Pixel", "Stack", "Heap", "Thread", "Socket", "Proxy" }
				}
			},
			{
				"Culture Indienne", new WordTheme
				{
					PersonalNames = new List<string> { "Aarav", "Vivaan", "Aditya", "Arjun", "Krishna", "Priya", "Anika", "Diya" },
					Professions = new List<string> { "Pandit", "Yogi", "Maharaja", "Dabbawala", "Sitariste", "Marchand d'épices" },
					UnusualObjects = new List<string> { "Sitar Volant", "Turban Infini", "Lassi d'Immortalité", "Tapis Magique" },
					PlaceNames = new List<string> { "Le Palais des Vents", "Le Temple du Lotus", "Le Bazar aux Épices", "Les Ghats du Gange" },
					ExtraNames = new List<string> { "Karma", "Dharma", "Mantra", "Chakra", "Guru", "Avatar", "Raga", "Veda" }
				}
			},
			{
				"Mathématiques", new WordTheme
				{
					PersonalNames = new List<string> { "Euclide", "Pythagore", "Archimède", "Isaac", "Leonhard", "Blaise", "Sophie", "Emmy" },
					Professions = new List<string> { "Géomètre", "Algébriste", "Statisticien", "Actuaire", "Cryptographe", "Topologue" },
					UnusualObjects = new List<string> { "Compas Fractal", "Abacus Dimensionnel", "Règle à Calcul Temporelle", "Équation Vivante" },
					PlaceNames = new List<string> { "La Mer des Matrices", "La Forteresse Fractale", "Le Pont de Möbius", "La Sphère de Riemann" },
					ExtraNames = new List<string> { "Théorème", "Axiome", "Corollaire", "Lemme", "Intégrale", "Dérivée", "Vecteur", "Matrice" }
				}
			},
			{
				"Camping", new WordTheme
				{
					PersonalNames = new List<string> { "Jack", "Rose", "Bear", "Forrest", "River", "Willow", "Sunny", "Scout" },
					Professions = new List<string> { "Garde Forestier", "Guide de Montagne", "Campeur Pro", "Survivaliste", "Pisteur" },
					UnusualObjects = new List<string> { "Tente Auto-montante", "Sac de Couchage Chauffant", "Feu de Camp Éternel", "Gourde Sans Fond" },
					PlaceNames = new List<string> { "Le Lac de la Tranquillité", "Le Pic du Silence", "La Clairière aux Lucioles", "Le Sentier des Murmures" },
					ExtraNames = new List<string> { "Bivouac", "Randonnée", "Boussole", "Lanterne", "Hamac", "Mousqueton", "Trekking", "Cabane" }
				}
			}
		};
	}

	public override void _Ready()
	{
		// Récupérer les références aux nœuds
		_personalNameInput1 = GetNode<LineEdit>("Panel/GridContainer/PersonalNameInput1");
		_personalNameInput2 = GetNode<LineEdit>("Panel/GridContainer/PersonalNameInput2");
		_personalNameInput3 = GetNode<LineEdit>("Panel/GridContainer/PersonalNameInput3");
		_professionNameInput = GetNode<LineEdit>("Panel/GridContainer/ProfessionNameInput");
		_unusualObjectInput = GetNode<LineEdit>("Panel/GridContainer/UnusualObjectInput");
		_placeNameInput = GetNode<LineEdit>("Panel/GridContainer/PlaceNameInput");
		_extraNameInput1 = GetNode<LineEdit>("Panel/GridContainer/ExtraNameInput1");
		_extraNameInput2 = GetNode<LineEdit>("Panel/GridContainer/ExtraNameInput2");
		_extraNameInput3 = GetNode<LineEdit>("Panel/GridContainer/ExtraNameInput3");
		_validateButton = GetNode<Button>("Panel/ValidateButton");

		_validateButton.Pressed += OnValidateButtonPressed;
	}

	private void OnValidateButtonPressed()
	{
		// Choisir un thème au hasard
		var randomThemeKey = _themes.Keys.ElementAt(_random.Next(_themes.Count));
		var selectedTheme = _themes[randomThemeKey];
		GD.Print($"Thème choisi : {randomThemeKey}");

		// Récupérer le texte ou générer un nom si le champ est vide
		var personalName1 = GetOrCreateText(_personalNameInput1, selectedTheme.PersonalNames);
		var personalName2 = GetOrCreateText(_personalNameInput2, selectedTheme.PersonalNames);
		var personalName3 = GetOrCreateText(_personalNameInput3, selectedTheme.PersonalNames);
		var professionName = GetOrCreateText(_professionNameInput, selectedTheme.Professions);
		var unusualObjectName = GetOrCreateText(_unusualObjectInput, selectedTheme.UnusualObjects);
		var placeName = GetOrCreateText(_placeNameInput, selectedTheme.PlaceNames);
		var extraName1 = GetOrCreateText(_extraNameInput1, selectedTheme.ExtraNames);
		var extraName2 = GetOrCreateText(_extraNameInput2, selectedTheme.ExtraNames);
		var extraName3 = GetOrCreateText(_extraNameInput3, selectedTheme.ExtraNames);

		// Stocker les valeurs dans GameData
		GameData.Instance.PersonalName1 = personalName1;
		GameData.Instance.PersonalName2 = personalName2;
		GameData.Instance.PersonalName3 = personalName3;
		GameData.Instance.ProfessionName = professionName;
		GameData.Instance.UnusualObjectName = unusualObjectName;
		GameData.Instance.PlaceName = placeName;
		GameData.Instance.ExtraName1 = extraName1;
		GameData.Instance.ExtraName2 = extraName2;
		GameData.Instance.ExtraName3 = extraName3;

		GD.Print("Noms personnalisés enregistrés !");
		
		// Sauvegarder les noms dans la base de données
		DatabaseManager.Instance.UpdatePlayerCustomNames(
			GameData.Instance.PlayerId,
			GameData.Instance.PersonalName1,
			GameData.Instance.PersonalName2,
			GameData.Instance.PersonalName3,
			GameData.Instance.ProfessionName,
			GameData.Instance.UnusualObjectName,
			GameData.Instance.PlaceName,
			GameData.Instance.ExtraName1,
			GameData.Instance.ExtraName2,
			GameData.Instance.ExtraName3
		);
		
		GD.Print("Noms sauvegardés dans la base de données.");

		// Changer de scène pour commencer l'aventure
		GetTree().ChangeSceneToFile("res://scenes/start_adventure.tscn");
	}

	/// <summary>
	/// Récupère le texte d'un LineEdit. S'il est vide, choisit un mot au hasard dans la liste fournie.
	/// </summary>
	private string GetOrCreateText(LineEdit input, List<string> wordList)
	{
		if (!string.IsNullOrWhiteSpace(input.Text))
		{
			return input.Text.Trim();
		}
		else
		{
			string randomWord = wordList[_random.Next(wordList.Count)];
			input.Text = randomWord; // Met à jour l'interface pour que le joueur voie le nom généré
			return randomWord;
		}
	}
} 
