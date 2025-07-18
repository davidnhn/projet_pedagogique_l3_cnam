using Godot;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un type de personnage dans le jeu.
    /// </summary>
    public partial class TypePersonnage : Resource
    {
        public int IdTypePersonnage { get; set; }
        public string NomTypePersonnage { get; set; }
        public string DescriptionTypePersonnage { get; set; }

        // Bonus spécifiques au type de personnage
        public int BonusForce { get; set; }
        public int BonusAgilite { get; set; }
        public int BonusIntelligence { get; set; }
        public int BonusVie { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public TypePersonnage() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idTypePersonnage">ID du type de personnage</param>
        /// <param name="nomTypePersonnage">Nom du type de personnage</param>
        /// <param name="descriptionTypePersonnage">Description du type de personnage</param>
        public TypePersonnage(int idTypePersonnage, string nomTypePersonnage, string descriptionTypePersonnage)
        {
            IdTypePersonnage = idTypePersonnage;
            NomTypePersonnage = nomTypePersonnage;
            DescriptionTypePersonnage = descriptionTypePersonnage;

            // Initialisation des bonus par défaut
            BonusForce = 0;
            BonusAgilite = 0;
            BonusIntelligence = 0;
            BonusVie = 0;

            // Configuration des bonus en fonction du type
            switch (nomTypePersonnage.ToLower())
            {
                case "guerrier":
                    BonusForce = 5;
                    BonusVie = 20;
                    break;
                case "mage":
                    BonusIntelligence = 5;
                    BonusVie = 10;
                    break;
                case "archer":
                    BonusAgilite = 5;
                    BonusVie = 15;
                    break;
            }
        }

        /// <summary>
        /// Récupère les bonus de stats pour ce type de personnage.
        /// </summary>
        /// <returns>Un dictionnaire avec les bonus</returns>
        public System.Collections.Generic.Dictionary<string, int> GetBonus()
        {
            return new System.Collections.Generic.Dictionary<string, int>
            {
                { "Force", BonusForce },
                { "Agilite", BonusAgilite },
                { "Intelligence", BonusIntelligence },
                { "Vie", BonusVie }
            };
        }
    }
} 