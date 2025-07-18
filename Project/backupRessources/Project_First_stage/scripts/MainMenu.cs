using Godot;
using System;

// Cette classe gère le menu principal du jeu
// Elle hérite de Control, qui est une classe de base pour les interfaces utilisateur dans Godot
public partial class MainMenu : Control
{
	// Variable pour stocker la référence au bouton de sortie
	private Button exitButton;
	// Variable pour stocker la référence au bouton de démarrage
	private Button startButton;

	// Cette méthode est appelée automatiquement quand la scène est chargée
	// C'est l'équivalent du "constructeur" pour les nodes Godot
	public override void _Ready()
	{
		// Recherche le bouton Exit_Button dans l'arbre des nodes
		// Le chemin "VBoxContainer/Exit_Button" correspond à la structure dans l'éditeur
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");
		
		// Recherche le bouton Start_Button dans l'arbre des nodes
		startButton = GetNode<Button>("VBoxContainer/Start_Button");
		
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
		// Affiche un message de debug pour confirmer que le bouton a été pressé
		GD.Print("Start button pressed!");
		// Change la scène vers enter_name.tscn qui se trouve dans le dossier scenes
		GetTree().ChangeSceneToFile("res://scenes/enter_name.tscn");
	}
}
