using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Interface pour toutes les entités pouvant combattre dans le jeu.
    /// </summary>
    public interface ICombatable
    {
        /// <summary>
        /// Méthode appelée pour attaquer une autre entité.
        /// </summary>
        /// <param name="cible">La cible de l'attaque</param>
        /// <returns>Les dégâts infligés</returns>
        int Attaquer(ICombatable cible);

        /// <summary>
        /// Méthode appelée pour se défendre contre une attaque.
        /// </summary>
        /// <param name="degats">Les dégâts de l'attaque</param>
        /// <returns>Les dégâts effectivement subis après défense</returns>
        int Defendre(int degats);

        /// <summary>
        /// Propriété indiquant si l'entité est encore en vie.
        /// </summary>
        bool EstVivant { get; }
    }
} 