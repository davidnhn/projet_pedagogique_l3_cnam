using Godot;
using System;

namespace JeuVideo.Data
{
    /// <summary>
    /// Classe représentant un memento de sauvegarde (pattern Memento).
    /// </summary>
    public partial class SauvegardeMemento : Resource
    {
        // État interne du memento (privé)
        private readonly DateTime _date;
        private readonly Position _position;
        private readonly int _vie;
        private readonly int _experience;
        private readonly string _etatPersonnage;
        private readonly int _idZone;
        private readonly int _idPersonnage;

        /// <summary>
        /// Constructeur du memento.
        /// </summary>
        /// <param name="date">Date de la sauvegarde</param>
        /// <param name="position">Position du personnage</param>
        /// <param name="vie">Vie du personnage</param>
        /// <param name="experience">Expérience du personnage</param>
        /// <param name="etatPersonnage">État du personnage</param>
        /// <param name="idZone">ID de la zone</param>
        /// <param name="idPersonnage">ID du personnage</param>
        public SauvegardeMemento(DateTime date, Position position, int vie, int experience, 
            string etatPersonnage, int idZone, int idPersonnage)
        {
            _date = date;
            _position = position;
            _vie = vie;
            _experience = experience;
            _etatPersonnage = etatPersonnage;
            _idZone = idZone;
            _idPersonnage = idPersonnage;
        }

        /// <summary>
        /// Obtient l'état interne du memento.
        /// Cette méthode ne doit être utilisée que par la classe Sauvegarde.
        /// </summary>
        /// <returns>Tuple contenant les états sauvegardés</returns>
        internal (DateTime date, Position position, int vie, int experience, 
            string etatPersonnage, int idZone, int idPersonnage) GetState()
        {
            return (_date, _position, _vie, _experience, _etatPersonnage, _idZone, _idPersonnage);
        }

        /// <summary>
        /// Représentation textuelle du memento (pour debug).
        /// </summary>
        /// <returns>Description de la sauvegarde</returns>
        public override string ToString()
        {
            return $"Sauvegarde du {_date:dd/MM/yyyy HH:mm:ss} - " +
                   $"Position: ({_position.X}, {_position.Y}) - " +
                   $"Vie: {_vie} - XP: {_experience}";
        }
    }
} 