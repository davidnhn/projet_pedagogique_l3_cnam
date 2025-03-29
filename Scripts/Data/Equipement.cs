using Godot;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un équipement dans le jeu.
    /// </summary>
    public class Equipement : Resource
    {
        public int IdEquipement { get; set; }
        public string NomEquipement { get; set; }
        public string TypeEquipement { get; set; } // Arme, Armure, Bouclier, Casque, etc.
        public int BonusStat { get; set; }
        
        // Statistiques affectées par l'équipement
        public int BonusVie { get; set; }
        public int BonusForce { get; set; }
        public int BonusAgilite { get; set; }
        public int BonusIntelligence { get; set; }
        public int BonusDefense { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Equipement() { }

        /// <summary>
        /// Constructeur avec paramètres de base.
        /// </summary>
        /// <param name="idEquipement">ID de l'équipement</param>
        /// <param name="nomEquipement">Nom de l'équipement</param>
        /// <param name="typeEquipement">Type de l'équipement</param>
        /// <param name="bonusStat">Bonus de statistique principal</param>
        public Equipement(int idEquipement, string nomEquipement, string typeEquipement, int bonusStat)
        {
            IdEquipement = idEquipement;
            NomEquipement = nomEquipement;
            TypeEquipement = typeEquipement;
            BonusStat = bonusStat;

            // Configurer les bonus en fonction du type d'équipement
            switch (typeEquipement.ToLower())
            {
                case "arme":
                    BonusForce = bonusStat;
                    break;
                case "armure":
                    BonusDefense = bonusStat;
                    BonusVie = bonusStat / 2;
                    break;
                case "bouclier":
                    BonusDefense = bonusStat;
                    break;
                case "casque":
                    BonusDefense = bonusStat / 2;
                    BonusIntelligence = bonusStat / 2;
                    break;
                case "gants":
                    BonusAgilite = bonusStat;
                    break;
                case "bottes":
                    BonusAgilite = bonusStat;
                    break;
                case "amulette":
                    BonusIntelligence = bonusStat;
                    break;
                default:
                    // Répartir le bonus équitablement
                    BonusForce = bonusStat / 4;
                    BonusAgilite = bonusStat / 4;
                    BonusIntelligence = bonusStat / 4;
                    BonusDefense = bonusStat / 4;
                    break;
            }
        }

        /// <summary>
        /// Constructeur avec paramètres détaillés.
        /// </summary>
        public Equipement(int idEquipement, string nomEquipement, string typeEquipement,
            int bonusVie, int bonusForce, int bonusAgilite, int bonusIntelligence, int bonusDefense)
        {
            IdEquipement = idEquipement;
            NomEquipement = nomEquipement;
            TypeEquipement = typeEquipement;
            BonusVie = bonusVie;
            BonusForce = bonusForce;
            BonusAgilite = bonusAgilite;
            BonusIntelligence = bonusIntelligence;
            BonusDefense = bonusDefense;
            
            // Calcul du bonus global pour comparaison
            BonusStat = bonusVie / 5 + bonusForce + bonusAgilite + bonusIntelligence + bonusDefense;
        }

        /// <summary>
        /// Applique les bonus de cet équipement à un personnage.
        /// </summary>
        /// <param name="personnage">Le personnage à qui appliquer les bonus</param>
        /// <returns>Description des bonus appliqués</returns>
        public string AppliquerBonus(Personnage personnage)
        {
            if (personnage == null)
            {
                return "Personnage invalide";
            }

            personnage.VieMax += BonusVie;
            personnage.Vie = System.Math.Min(personnage.Vie + BonusVie, personnage.VieMax);
            personnage.Force += BonusForce;
            personnage.Agilite += BonusAgilite;
            personnage.Intelligence += BonusIntelligence;
            // Note: La défense pourrait être ajoutée comme propriété au personnage

            return $"{NomEquipement} équipé avec succès";
        }

        /// <summary>
        /// Retire les bonus de cet équipement à un personnage.
        /// </summary>
        /// <param name="personnage">Le personnage à qui retirer les bonus</param>
        /// <returns>Description des bonus retirés</returns>
        public string RetirerBonus(Personnage personnage)
        {
            if (personnage == null)
            {
                return "Personnage invalide";
            }

            personnage.VieMax -= BonusVie;
            personnage.Vie = System.Math.Min(personnage.Vie, personnage.VieMax);
            personnage.Force -= BonusForce;
            personnage.Agilite -= BonusAgilite;
            personnage.Intelligence -= BonusIntelligence;

            return $"{NomEquipement} retiré avec succès";
        }
    }
} 