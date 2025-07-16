using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Stratégie pour les actions d'interaction.
    /// </summary>
    public class InteractionStrategy : ActionDeBase
    {
        private readonly IInteractable _cible;

        /// <summary>
        /// Constructeur de la stratégie d'interaction.
        /// </summary>
        /// <param name="idAction">Identifiant de l'action</param>
        /// <param name="nomAction">Nom de l'action</param>
        /// <param name="descriptionAction">Description de l'action</param>
        /// <param name="cible">Entité avec laquelle interagir</param>
        public InteractionStrategy(int idAction, string nomAction, string descriptionAction, 
            IInteractable cible) 
            : base(idAction, nomAction, descriptionAction)
        {
            _cible = cible;
        }

        /// <summary>
        /// Exécute l'action d'interaction.
        /// </summary>
        /// <returns>Le résultat de l'interaction</returns>
        public override string Executer()
        {
            string resultat = _cible.Interagir();
            return $"{NomAction} : {resultat}";
        }
    }
} 