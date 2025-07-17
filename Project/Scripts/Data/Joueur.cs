using Godot;
using System.Collections.Generic;
using JeuVideo.Core;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un joueur du jeu.
    /// </summary>
    public partial class Joueur : Node
    {
        public int IdJoueur { get; private set; }
        public string PseudoJoueur { get; private set; }
        public List<Personnage> Personnages { get; private set; } = new List<Personnage>();
        public List<Sauvegarde> Sauvegardes { get; private set; } = new List<Sauvegarde>();

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Joueur() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idJoueur">ID du joueur</param>
        /// <param name="pseudoJoueur">Pseudo du joueur</param>
        public Joueur(int idJoueur, string pseudoJoueur)
        {
            IdJoueur = idJoueur;
            PseudoJoueur = pseudoJoueur;
        }

        /// <summary>
        /// Crée un nouveau personnage pour ce joueur.
        /// </summary>
        /// <param name="nomPersonnage">Nom du personnage</param>
        /// <param name="typePersonnage">Type du personnage</param>
        /// <returns>Le personnage créé</returns>
        public Personnage CreerPersonnage(string nomPersonnage, TypePersonnage typePersonnage)
        {
            var personnage = new Personnage
            {
                IdPersonnage = Personnages.Count + 1,
                NomPersonnage = nomPersonnage,
                Vie = 100,
                Experience = 0,
                EtatPersonnage = "Actif",
                TypePersonnage = typePersonnage
            };

            Personnages.Add(personnage);
            return personnage;
        }

        /// <summary>
        /// Crée une nouvelle sauvegarde.
        /// </summary>
        /// <param name="personnage">Le personnage à sauvegarder</param>
        /// <param name="zone">La zone actuelle</param>
        /// <param name="positionX">Position X dans la zone</param>
        /// <param name="positionY">Position Y dans la zone</param>
        /// <returns>La sauvegarde créée</returns>
        public Sauvegarde CreerSauvegarde(Personnage personnage, Zone zone, int positionX, int positionY)
        {
            var sauvegarde = new Sauvegarde
            {
                IdSauvegarde = Sauvegardes.Count + 1,
                DateSauvegarde = System.DateTime.Now,
                PositionXJoueur = positionX,
                PositionYJoueur = positionY,
                Zone = zone,
                Personnage = personnage
            };

            Sauvegardes.Add(sauvegarde);
            return sauvegarde;
        }
    }
} 