using Godot;

namespace JeuVideo.Core
{
    /// <summary>
    /// Stratégie pour les actions de combat.
    /// </summary>
    public class CombatStrategy : ActionDeBase
    {
        private readonly ICombatable _source;
        private readonly ICombatable _cible;

        /// <summary>
        /// Constructeur de la stratégie de combat.
        /// </summary>
        /// <param name="idAction">Identifiant de l'action</param>
        /// <param name="nomAction">Nom de l'action</param>
        /// <param name="descriptionAction">Description de l'action</param>
        /// <param name="source">Entité qui effectue l'action</param>
        /// <param name="cible">Entité ciblée par l'action</param>
        public CombatStrategy(int idAction, string nomAction, string descriptionAction, 
            ICombatable source, ICombatable cible) 
            : base(idAction, nomAction, descriptionAction)
        {
            _source = source;
            _cible = cible;
        }

        /// <summary>
        /// Exécute l'action de combat.
        /// </summary>
        /// <returns>Le résultat du combat</returns>
        public override string Executer()
        {
            if (!_source.EstVivant || !_cible.EstVivant)
            {
                return "Impossible de combattre : une des entités n'est pas en état de combattre";
            }

            int degatsInfliges = _source.Attaquer(_cible);
            int degatsReels = _cible.Defendre(degatsInfliges);

            return $"{NomAction} : {degatsReels} points de dégâts infligés";
        }
    }
} 