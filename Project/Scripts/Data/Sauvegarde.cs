using Godot;
using System;

namespace JeuVideo.Data
{
    /// <summary>
    /// Classe représentant une sauvegarde de jeu (Originator dans le pattern Memento).
    /// </summary>
    public partial class Sauvegarde : Resource
    {
        public int IdSauvegarde { get; set; }
        public DateTime DateSauvegarde { get; set; }
        public int PositionXJoueur { get; set; }
        public int PositionYJoueur { get; set; }
        
        // Relations
        public Zone Zone { get; set; }
        public Personnage Personnage { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Sauvegarde()
        {
            DateSauvegarde = DateTime.Now;
        }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idSauvegarde">ID de la sauvegarde</param>
        /// <param name="dateSauvegarde">Date de la sauvegarde</param>
        /// <param name="positionX">Position X du joueur</param>
        /// <param name="positionY">Position Y du joueur</param>
        /// <param name="zone">Zone actuelle</param>
        /// <param name="personnage">Personnage sauvegardé</param>
        public Sauvegarde(int idSauvegarde, DateTime dateSauvegarde, int positionX, int positionY, 
            Zone zone, Personnage personnage)
        {
            IdSauvegarde = idSauvegarde;
            DateSauvegarde = dateSauvegarde;
            PositionXJoueur = positionX;
            PositionYJoueur = positionY;
            Zone = zone;
            Personnage = personnage;
        }

        /// <summary>
        /// Crée un memento de l'état actuel.
        /// </summary>
        /// <returns>Le memento créé</returns>
        public SauvegardeMemento CreerMemento()
        {
            Position position = new Position(PositionXJoueur, PositionYJoueur);
            int idZone = Zone != null ? Zone.IdZone : 0;
            int idPersonnage = Personnage != null ? Personnage.IdPersonnage : 0;
            
            return new SauvegardeMemento(
                DateSauvegarde,
                position,
                Personnage?.Vie ?? 0,
                Personnage?.Experience ?? 0,
                Personnage?.EtatPersonnage ?? "Inconnu",
                idZone,
                idPersonnage
            );
        }

        /// <summary>
        /// Restaure l'état à partir d'un memento.
        /// </summary>
        /// <param name="memento">Le memento à restaurer</param>
        /// <returns>Message de confirmation</returns>
        public string Restaurer(SauvegardeMemento memento)
        {
            if (memento == null)
            {
                return "Impossible de restaurer: memento invalide";
            }
            
            var state = memento.GetState();
            
            DateSauvegarde = state.date;
            PositionXJoueur = state.position.X;
            PositionYJoueur = state.position.Y;
            
            if (Personnage != null)
            {
                Personnage.Vie = state.vie;
                Personnage.Experience = state.experience;
                Personnage.EtatPersonnage = state.etatPersonnage;
            }
            
            // Note: en pratique, il faudrait recharger la zone et le personnage depuis la base de données
            // à partir des IDs stockés dans le memento
            
            return $"Sauvegarde restaurée du {state.date:dd/MM/yyyy HH:mm:ss}";
        }

        /// <summary>
        /// Obtient une position à partir des coordonnées sauvegardées.
        /// </summary>
        /// <returns>Objet Position</returns>
        public Position ObtenirPosition()
        {
            return new Position(PositionXJoueur, PositionYJoueur);
        }

        /// <summary>
        /// Obtient une description de la sauvegarde.
        /// </summary>
        /// <returns>Description de la sauvegarde</returns>
        public string ObtenirDescription()
        {
            string nomZone = Zone != null ? Zone.NomZone : "Zone inconnue";
            string nomPersonnage = Personnage != null ? Personnage.NomPersonnage : "Personnage inconnu";
            
            return $"Sauvegarde #{IdSauvegarde} - {DateSauvegarde:dd/MM/yyyy HH:mm:ss}\n" +
                   $"Personnage: {nomPersonnage}\n" +
                   $"Zone: {nomZone}\n" +
                   $"Position: ({PositionXJoueur}, {PositionYJoueur})";
        }
    }
} 