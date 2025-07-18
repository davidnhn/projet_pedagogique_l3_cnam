using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Interface pour tous les objets avec lesquels on peut interagir dans le jeu.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Méthode appelée lorsqu'un personnage interagit avec cet objet.
        /// </summary>
        /// <returns>Le résultat de l'interaction</returns>
        string Interagir();
    }
} 