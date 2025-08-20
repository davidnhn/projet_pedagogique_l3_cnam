using Godot;

namespace JeuVideo.Core
{
	/// <summary>
	/// Stratégie pour les actions de magie.
	/// </summary>
	public class MagieStrategy : ActionDeBase
	{
		private readonly int _coutMana;
		private readonly int _puissance;
		private readonly string _type;
		
		/// <summary>
		/// Constructeur de la stratégie de magie.
		/// </summary>
		/// <param name="idAction">Identifiant de l'action</param>
		/// <param name="nomAction">Nom de l'action</param>
		/// <param name="descriptionAction">Description de l'action</param>
		/// <param name="coutMana">Coût en mana pour utiliser cette magie</param>
		/// <param name="puissance">Puissance de l'effet magique</param>
		/// <param name="type">Type de magie (feu, glace, soin, etc.)</param>
		public MagieStrategy(int idAction, string nomAction, string descriptionAction, 
			int coutMana, int puissance, string type) 
			: base(idAction, nomAction, descriptionAction)
		{
			_coutMana = coutMana;
			_puissance = puissance;
			_type = type;
		}

		/// <summary>
		/// Exécute l'action de magie.
		/// </summary>
		/// <returns>Le résultat de la magie</returns>
		public override string Executer()
		{
			// Dans une implémentation réelle, on vérifierait le mana du personnage
			// et on appliquerait les effets en fonction du type de magie
			return $"Magie {NomAction} de type {_type} lancée avec une puissance de {_puissance}";
		}
	}
} 
