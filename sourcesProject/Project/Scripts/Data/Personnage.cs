using Godot;
using JeuVideo.Core;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un personnage dans le jeu.
    /// </summary>
    public class Personnage : CharacterBody2D, IInteractable, ICombatable
    {
        // Propriétés de base
        public int IdPersonnage { get; set; }
        public int Vie { get; set; }
        public int Experience { get; set; }
        public string NomPersonnage { get; set; }
        public string EtatPersonnage { get; set; }
        
        // Statistiques
        public int Force { get; set; } = 10;
        public int Agilite { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int VieMax { get; set; } = 100;
        public int Mana { get; set; } = 50;
        public int ManaMax { get; set; } = 50;
        
        // Relations
        public TypePersonnage TypePersonnage { get; set; }
        public ActionStrategy ActionStrategy { get; set; }
        public Inventaire Inventaire { get; set; }
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();
        public List<Quete> Quetes { get; set; } = new List<Quete>();
        
        // Propriété calculée pour ICombatable
        public bool EstVivant => Vie > 0;

        /// <summary>
        /// Appelé quand le nœud entre dans la scène.
        /// </summary>
        public override void _Ready()
        {
            // Initialisation Godot
            base._Ready();
            
            // Initialiser l'inventaire si nécessaire
            if (Inventaire == null)
            {
                Inventaire = Inventaire.GetInstance();
            }
            
            // Appliquer les bonus du type de personnage
            if (TypePersonnage != null)
            {
                var bonus = TypePersonnage.GetBonus();
                Force += bonus["Force"];
                Agilite += bonus["Agilite"];
                Intelligence += bonus["Intelligence"];
                VieMax += bonus["Vie"];
                Vie = VieMax;
            }
        }

        /// <summary>
        /// Effectue l'action définie par la stratégie actuelle.
        /// </summary>
        /// <returns>Le résultat de l'action</returns>
        public string EffectuerAction()
        {
            if (ActionStrategy == null)
            {
                return "Aucune action définie";
            }
            
            return ActionStrategy.Executer();
        }

        /// <summary>
        /// Fait gagner de l'expérience au personnage.
        /// </summary>
        /// <param name="montant">Montant d'expérience à gagner</param>
        /// <returns>Indique si le personnage a gagné un niveau</returns>
        public bool GagnerExperience(int montant)
        {
            int experiencePrecedente = Experience;
            Experience += montant;
            
            // Simple système de niveau : 100 XP par niveau
            int niveauPrecedent = experiencePrecedente / 100;
            int nouveauNiveau = Experience / 100;
            
            if (nouveauNiveau > niveauPrecedent)
            {
                // Gains de niveau
                VieMax += 10;
                Vie = VieMax;
                ManaMax += 5;
                Mana = ManaMax;
                Force += 1;
                Agilite += 1;
                Intelligence += 1;
                
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Utilise un objet de l'inventaire.
        /// </summary>
        /// <param name="objet">L'objet à utiliser</param>
        /// <returns>Le résultat de l'utilisation</returns>
        public string UtiliserObjet(Objet objet)
        {
            if (objet == null)
            {
                return "Objet invalide";
            }
            
            return objet.AppliquerEffet(this);
        }
        
        #region Implémentation de IInteractable
        
        /// <summary>
        /// Méthode d'interaction avec ce personnage.
        /// </summary>
        /// <returns>Le résultat de l'interaction</returns>
        public string Interagir()
        {
            return $"{NomPersonnage} vous salue !";
        }
        
        #endregion
        
        #region Implémentation de ICombatable
        
        /// <summary>
        /// Attaque une autre entité.
        /// </summary>
        /// <param name="cible">La cible de l'attaque</param>
        /// <returns>Les dégâts potentiels</returns>
        public int Attaquer(ICombatable cible)
        {
            // Calcul des dégâts basé sur la force et un peu d'aléatoire
            System.Random rnd = new System.Random();
            int degatsBase = Force;
            int variation = rnd.Next(-2, 3); // Entre -2 et +2
            
            return degatsBase + variation;
        }
        
        /// <summary>
        /// Défend contre une attaque.
        /// </summary>
        /// <param name="degats">Les dégâts potentiels</param>
        /// <returns>Les dégâts réellement subis</returns>
        public int Defendre(int degats)
        {
            // Réduction des dégâts basée sur l'agilité
            int reduction = Agilite / 5;
            int degatsReels = System.Math.Max(1, degats - reduction);
            
            Vie -= degatsReels;
            if (Vie < 0)
            {
                Vie = 0;
                EtatPersonnage = "Inconscient";
            }
            
            return degatsReels;
        }
        
        #endregion
    }
} 