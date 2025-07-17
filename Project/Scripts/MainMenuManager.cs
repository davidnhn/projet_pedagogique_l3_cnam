using Godot;
using System;

// Cette classe gère le menu principal du jeu
// Elle hérite de Control, qui est une classe de base pour les interfaces utilisateur dans Godot
public partial class MainMenuManager : Control
{
	// Variable pour stocker la référence au bouton de sortie
	private Button exitButton;
	// Variable pour stocker la référence au bouton de démarrage
	private Button startButton;
	// Variable pour stocker la référence au bouton Options
	private Button optionsButton;

	// Cette méthode est appelée automatiquement quand la scène est chargée
	// C'est l'équivalent du "constructeur" pour les nodes Godot
	public override void _Ready()
	{
		// Recherche le bouton Exit_Button dans l'arbre des nodes
		// Le chemin "VBoxContainer/Exit_Button" correspond à la structure dans l'éditeur
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");
		
		// Recherche le bouton Start_Button dans l'arbre des nodes
		startButton = GetNode<Button>("VBoxContainer/Start_Button");
		
		// Ajout de la récupération du bouton Options
		optionsButton = GetNode<Button>("VBoxContainer/Options_Button");
		if (optionsButton == null)
		{
			GD.PrintErr("Options_Button not found! Check the node path.");
			return;
		}
		
		// Vérifie si les boutons ont été trouvés
		if (exitButton == null)
		{
			GD.PrintErr("Exit_Button not found! Check the node path.");
			return;
		}

		if (startButton == null)
		{
			GD.PrintErr("Start_Button not found! Check the node path.");
			return;
		}
		
		// Message de confirmation si les boutons sont trouvés
		GD.Print("Buttons found and connected!");

		// Connecte le signal "Pressed" du bouton à notre méthode OnExitButtonPressed
		// Cette méthode sera appelée chaque fois que le bouton sera cliqué
		exitButton.Pressed += OnExitButtonPressed;
		
		// Connecte le signal "Pressed" du bouton Start à notre méthode OnStartButtonPressed
		startButton.Pressed += OnStartButtonPressed;
		
		// Connecte le signal "Pressed" du bouton Options à notre méthode OnOptionsButtonPressed
		optionsButton.Pressed += OnOptionsButtonPressed;
	}

	// Méthode appelée quand le bouton Exit est pressé
	private void OnExitButtonPressed()
	{
		// Affiche un message de debug pour confirmer que le bouton a été pressé
		GD.Print("Exit button pressed!");
		// Quitte le jeu
		GetTree().Quit();
	}

	// Méthode appelée quand le bouton Start est pressé
	private void OnStartButtonPressed()
	{
		GD.Print("Start button pressed!");
		GetTree().ChangeSceneToFile("res://scenes/enter_name.tscn");
	}

	// Ajout de la méthode pour le bouton Options
	private void OnOptionsButtonPressed()
	{
		GD.Print("Options button pressed!");
		// Change la scène vers le menu d'options (à adapter selon ton projet)
		GetTree().ChangeSceneToFile("res://scenes/options_menu.tscn");
	}
}
