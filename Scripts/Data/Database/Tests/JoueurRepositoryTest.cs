using Godot;
using System;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database.Tests
{
	public class JoueurRepositoryTest : Node
	{
		private JoueurRepository _repository;

		public override void _Ready()
		{
			_repository = new JoueurRepository();
			RunTests();
		}

		private async void RunTests()
		{
			try
			{
				GD.Print("Début des tests du JoueurRepository...");

				// Test de création
				GD.Print("\nTest de création d'un joueur...");
				var nouveauJoueur = new Joueur { PseudoJoueur = "TestPlayer" };
				var joueurCree = await _repository.CreateAsync(nouveauJoueur);
				GD.Print($"Joueur créé avec succès - ID: {joueurCree.IdJoueur}, Pseudo: {joueurCree.PseudoJoueur}");

				// Test de lecture par ID
				GD.Print("\nTest de lecture du joueur par ID...");
				var joueurLu = await _repository.GetByIdAsync(joueurCree.IdJoueur);
				GD.Print($"Joueur lu avec succès - ID: {joueurLu.IdJoueur}, Pseudo: {joueurLu.PseudoJoueur}");

				// Test de mise à jour
				GD.Print("\nTest de mise à jour du joueur...");
				joueurLu.PseudoJoueur = "TestPlayerUpdated";
				var updateReussi = await _repository.UpdateAsync(joueurLu);
				GD.Print($"Mise à jour {(updateReussi ? "réussie" : "échouée")}");

				// Test de lecture de tous les joueurs
				GD.Print("\nTest de lecture de tous les joueurs...");
				var tousLesJoueurs = await _repository.GetAllAsync();
				GD.Print($"Nombre total de joueurs: {tousLesJoueurs.Count}");
				foreach (var joueur in tousLesJoueurs)
				{
					GD.Print($"- ID: {joueur.IdJoueur}, Pseudo: {joueur.PseudoJoueur}");
				}

				// Test de suppression
				GD.Print("\nTest de suppression du joueur...");
				var deleteReussi = await _repository.DeleteAsync(joueurCree.IdJoueur);
				GD.Print($"Suppression {(deleteReussi ? "réussie" : "échouée")}");

				// Vérification finale
				var joueurSupprime = await _repository.GetByIdAsync(joueurCree.IdJoueur);
				GD.Print($"\nVérification finale - Le joueur {(joueurSupprime == null ? "a bien été supprimé" : "n'a pas été supprimé")}");

				GD.Print("\nTous les tests ont été exécutés avec succès!");
			}
			catch (Exception ex)
			{
				GD.PrintErr($"Erreur pendant les tests: {ex.Message}");
				GD.PrintErr($"Stack trace: {ex.StackTrace}");
			}
		}
	}
} 
