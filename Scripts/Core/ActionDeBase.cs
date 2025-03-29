using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Classe abstraite de base pour toutes les actions dans le jeu.
    /// </summary>
    public abstract class ActionDeBase : ActionStrategy
    {
        public int IdAction { get; protected set; }
        public string NomAction { get; protected set; }
        public string DescriptionAction { get; protected set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        /// <param name="idAction">Identifiant de l'action</param>
        /// <param name="nomAction">Nom de l'action</param>
        /// <param name="descriptionAction">Description de l'action</param>
        protected ActionDeBase(int idAction, string nomAction, string descriptionAction)
        {
            IdAction = idAction;
            NomAction = nomAction;
            DescriptionAction = descriptionAction;
        }

        /// <summary>
        /// Implémentation de base de la méthode Executer.
        /// </summary>
        /// <returns>Le résultat de l'exécution</returns>
        public virtual string Executer()
        {
            return $"Action {NomAction} exécutée";
        }
    }
} 