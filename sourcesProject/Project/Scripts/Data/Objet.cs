using Godot;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un objet dans le jeu.
    /// </summary>
    public class Objet : Resource
    {
        public int IdObjet { get; set; }
        public string NomObjet { get; set; }
        public string DescriptionNomObjet { get; set; }
        public string Effet { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Objet() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idObjet">ID de l'objet</param>
        /// <param name="nomObjet">Nom de l'objet</param>
        /// <param name="descriptionNomObjet">Description de l'objet</param>
        /// <param name="effet">Effet de l'objet</param>
        public Objet(int idObjet, string nomObjet, string descriptionNomObjet, string effet)
        {
            IdObjet = idObjet;
            NomObjet = nomObjet;
            DescriptionNomObjet = descriptionNomObjet;
            Effet = effet;
        }

        /// <summary>
        /// Applique l'effet de l'objet sur un personnage.
        /// </summary>
        /// <param name="personnage">Le personnage cible</param>
        /// <returns>Le résultat de l'application</returns>
        public string AppliquerEffet(Personnage personnage)
        {
            if (personnage == null)
            {
                return "Personnage invalide";
            }

            // Analyser le type d'effet et l'appliquer
            if (Effet.Contains("vie") || Effet.Contains("Vie"))
            {
                // Extraction de la valeur numérique
                int valeur = ExtractNumber(Effet);
                int ancienneVie = personnage.Vie;
                personnage.Vie = System.Math.Min(personnage.VieMax, personnage.Vie + valeur);
                int restauration = personnage.Vie - ancienneVie;
                
                return $"{NomObjet} a restauré {restauration} points de vie";
            }
            else if (Effet.Contains("mana") || Effet.Contains("Mana"))
            {
                int valeur = ExtractNumber(Effet);
                int ancienMana = personnage.Mana;
                personnage.Mana = System.Math.Min(personnage.ManaMax, personnage.Mana + valeur);
                int restauration = personnage.Mana - ancienMana;
                
                return $"{NomObjet} a restauré {restauration} points de mana";
            }
            else if (Effet.Contains("force") || Effet.Contains("Force"))
            {
                int valeur = ExtractNumber(Effet);
                personnage.Force += valeur;
                
                return $"{NomObjet} a augmenté la force de {valeur} points";
            }
            else if (Effet.Contains("agilité") || Effet.Contains("Agilité") || Effet.Contains("agilite") || Effet.Contains("Agilite"))
            {
                int valeur = ExtractNumber(Effet);
                personnage.Agilite += valeur;
                
                return $"{NomObjet} a augmenté l'agilité de {valeur} points";
            }
            else if (Effet.Contains("intelligence") || Effet.Contains("Intelligence"))
            {
                int valeur = ExtractNumber(Effet);
                personnage.Intelligence += valeur;
                
                return $"{NomObjet} a augmenté l'intelligence de {valeur} points";
            }

            return $"{NomObjet} utilisé sans effet particulier";
        }

        /// <summary>
        /// Extrait un nombre d'une chaîne de caractères.
        /// </summary>
        /// <param name="text">Le texte contenant un nombre</param>
        /// <returns>Le nombre extrait, ou 0 si aucun nombre n'est trouvé</returns>
        private int ExtractNumber(string text)
        {
            string digits = "";
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    digits += c;
                }
            }

            if (digits.Length > 0)
            {
                return int.Parse(digits);
            }
            
            return 0;
        }
    }
} 