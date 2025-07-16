using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Interface pour le pattern Strategy des actions du personnage.
    /// </summary>
    public interface ActionStrategy
    {
        /// <summary>
        /// Exécute l'action choisie.
        /// </summary>
        /// <returns>Le résultat de l'exécution</returns>
        string Executer();
    }
} 