using Godot;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un combat entre un personnage et un bot.
    /// </summary>
    public class Combat : Node
    {
        public int IdCombat { get; set; }
        public string ResultatCombat { get; set; } // "En cours", "Victoire", "Défaite"
        
        // Relations
        public Zone Zone { get; set; }
        public Personnage Personnage { get; set; }
        public Bot Bot { get; set; }
        
        // Informations sur le combat
        public int TourActuel { get; private set; } = 1;
        public List<string> JournalCombat { get; private set; } = new List<string>();
        
        // État du combat
        public bool EstTermine { get; private set; } = false;

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Combat() 
        {
            ResultatCombat = "En cours";
        }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idCombat">ID du combat</param>
        /// <param name="personnage">Le personnage impliqué</param>
        /// <param name="bot">Le bot opposant</param>
        /// <param name="zone">La zone du combat</param>
        public Combat(int idCombat, Personnage personnage, Bot bot, Zone zone)
        {
            IdCombat = idCombat;
            Personnage = personnage;
            Bot = bot;
            Zone = zone;
            ResultatCombat = "En cours";
            
            JournalCombat.Add($"Début du combat entre {personnage.NomPersonnage} et {bot.NomBot} dans {zone.NomZone}.");
        }

        /// <summary>
        /// Gère un tour de combat.
        /// </summary>
        /// <returns>Le résultat du tour</returns>
        public string GererTour()
        {
            if (EstTermine)
            {
                return $"Le combat est déjà terminé avec comme résultat: {ResultatCombat}";
            }
            
            string resultatTour = $"Tour {TourActuel}:\n";
            
            // Le personnage attaque en premier
            int degatsPersonnage = Personnage.Attaquer(Bot);
            int degatsReelsPersonnage = Bot.Defendre(degatsPersonnage);
            resultatTour += $"{Personnage.NomPersonnage} attaque {Bot.NomBot} et inflige {degatsReelsPersonnage} points de dégâts.\n";
            
            // Vérifier si le bot est vaincu
            if (!Bot.EstVivant)
            {
                EstTermine = true;
                ResultatCombat = "Victoire";
                resultatTour += $"{Bot.NomBot} a été vaincu!\n";
                resultatTour += $"{Personnage.NomPersonnage} gagne le combat!";
                JournalCombat.Add(resultatTour);
                return resultatTour;
            }
            
            // Le bot riposte
            int degatsBot = Bot.Attaquer(Personnage);
            int degatsReelsBot = Personnage.Defendre(degatsBot);
            resultatTour += $"{Bot.NomBot} attaque {Personnage.NomPersonnage} et inflige {degatsReelsBot} points de dégâts.\n";
            
            // Vérifier si le personnage est vaincu
            if (!Personnage.EstVivant)
            {
                EstTermine = true;
                ResultatCombat = "Défaite";
                resultatTour += $"{Personnage.NomPersonnage} a été vaincu!\n";
                resultatTour += $"{Bot.NomBot} gagne le combat!";
                JournalCombat.Add(resultatTour);
                return resultatTour;
            }
            
            // Passer au tour suivant
            TourActuel++;
            resultatTour += $"Fin du tour {TourActuel-1}. {Personnage.NomPersonnage}: {Personnage.Vie}/{Personnage.VieMax} PV, {Bot.NomBot}: {Bot.Vie}/{Bot.VieMax} PV.";
            JournalCombat.Add(resultatTour);
            
            return resultatTour;
        }

        /// <summary>
        /// Détermine le résultat du combat si non déjà déterminé.
        /// </summary>
        /// <returns>Le résultat final du combat</returns>
        public string DeterminerResultat()
        {
            if (!EstTermine)
            {
                // Continuer le combat jusqu'à ce qu'il soit terminé
                while (!EstTermine)
                {
                    GererTour();
                }
            }
            
            string resume = $"Résultat du combat: {ResultatCombat}\n";
            resume += $"Nombre de tours: {TourActuel}\n";
            
            // Décision des récompenses si victoire
            if (ResultatCombat == "Victoire")
            {
                int xpGagnee = Bot.VieMax + (Bot.Force + Bot.Agilite + Bot.Intelligence) * 2;
                bool aGagneNiveau = Personnage.GagnerExperience(xpGagnee);
                
                resume += $"{Personnage.NomPersonnage} gagne {xpGagnee} points d'expérience.\n";
                
                if (aGagneNiveau)
                {
                    resume += $"{Personnage.NomPersonnage} a gagné un niveau!";
                }
            }
            
            return resume;
        }

        /// <summary>
        /// Fuite du combat par le personnage.
        /// </summary>
        /// <returns>Le résultat de la tentative de fuite</returns>
        public string FuirCombat()
        {
            if (EstTermine)
            {
                return $"Le combat est déjà terminé avec comme résultat: {ResultatCombat}";
            }
            
            // Chance de fuite basée sur l'agilité
            System.Random rnd = new System.Random();
            int chanceFuite = 30 + Personnage.Agilite * 2;
            int resultatTirage = rnd.Next(1, 101); // Entre 1 et 100
            
            if (resultatTirage <= chanceFuite)
            {
                EstTermine = true;
                ResultatCombat = "Fuite";
                
                string message = $"{Personnage.NomPersonnage} parvient à fuir le combat!";
                JournalCombat.Add(message);
                return message;
            }
            else
            {
                // La fuite échoue, le bot attaque gratuitement
                int degatsBot = Bot.Attaquer(Personnage);
                int degatsReelsBot = Personnage.Defendre(degatsBot);
                
                string message = $"Tentative de fuite échouée! {Bot.NomBot} attaque et inflige {degatsReelsBot} points de dégâts.";
                JournalCombat.Add(message);
                
                // Vérifier si le personnage est vaincu
                if (!Personnage.EstVivant)
                {
                    EstTermine = true;
                    ResultatCombat = "Défaite";
                    message += $"\n{Personnage.NomPersonnage} a été vaincu!";
                }
                
                return message;
            }
        }

        /// <summary>
        /// Obtient l'historique complet du combat.
        /// </summary>
        /// <returns>L'historique du combat</returns>
        public string ObtenirHistorique()
        {
            string historique = $"Combat #{IdCombat} - {ResultatCombat}\n";
            historique += $"{Personnage.NomPersonnage} vs {Bot.NomBot} dans {Zone.NomZone}\n\n";
            
            foreach (string entree in JournalCombat)
            {
                historique += entree + "\n---\n";
            }
            
            return historique;
        }
    }
} 