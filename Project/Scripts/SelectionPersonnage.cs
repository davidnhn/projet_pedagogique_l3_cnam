using Godot;
using System;
using System.Collections.Generic;

public partial class SelectionPersonnage : Control
{
	private Dictionary<string, TextureButton> personnages = new();
	private string personnageSelectionne = "";

	public override void _Ready()
	{
		// Associe chaque bouton à son nom
		personnages["Campeur"] = GetNode<TextureButton>("VBoxContainer/GridContainer/Campeur");
		personnages["ProfesseurMath"] = GetNode<TextureButton>("VBoxContainer/GridContainer/ProfesseurMath");
		personnages["Soldat"] = GetNode<TextureButton>("VBoxContainer/GridContainer/Soldat");
		personnages["HotesseAir"] = GetNode<TextureButton>("VBoxContainer/GridContainer/HotesseAir");
		personnages["VoleurCalculette"] = GetNode<TextureButton>("VBoxContainer/GridContainer/VoleurCalculette");
		personnages["Indien"] = GetNode<TextureButton>("VBoxContainer/GridContainer/Indien");

		foreach (var kvp in personnages)
		{
			string nom = kvp.Key;
			kvp.Value.Pressed += () => OnPersonnageSelectionne(nom);
		}

		GetNode<Button>("VBoxContainer/Valider").Pressed += OnValider;
	}

	private void OnPersonnageSelectionne(string nom)
	{
		personnageSelectionne = nom;

		foreach (var bouton in personnages.Values)
			bouton.Modulate = new Color(1, 1, 1); // Couleur normale

		personnages[nom].Modulate = new Color(0.6f, 1f, 0.6f); // Couleur sélectionnée
	}

	private void OnValider()
	{
		if (string.IsNullOrEmpty(personnageSelectionne))
		{
			GD.Print("Aucun personnage sélectionné.");
			return;
		}

		GD.Print($"Personnage sélectionné : {personnageSelectionne}");
		// Tu peux ensuite charger une autre scène ou affecter ce choix au joueur
	}
}
