using Godot;
using JeuVideo.Core;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente un bot (PNJ) dans le jeu.
    /// </summary>
    public class Bot : CharacterBody2D, IInteractable, ICombatable
    {
        public int IdBot { get; set; }
        public string NiveauBot { get; set; }
        public string DialogueBot { get; set; }
        public string NomBot { get; set; }
        public string AntagonisteBoot { get; set; }
        
        // Statistiques
        public int Vie { get; set; } = 50;
        public int VieMax { get; set; } = 50;
        public int Force { get; set; } = 5;
        public int Agilite { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        
        // Propriété calculée pour ICombatable
        public bool EstVivant => Vie > 0;
        
        // Relations
        public List<Quete> QuetesDisponibles { get; set; } = new List<Quete>();

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Bot() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idBot">ID du bot</param>
        /// <param name="niveauBot">Niveau du bot (Débutant, Expert, etc.)</param>
        /// <param name="dialogueBot">Dialogue de base du bot</param>
        /// <param name="nomBot">Nom du bot</param>
        /// <param name="antagonisteBot">Si le bot est un antagoniste</param>
        public Bot(int idBot, string niveauBot, string dialogueBot, string nomBot, string antagonisteBot)
        {
            IdBot = idBot;
            NiveauBot = niveauBot;
            DialogueBot = dialogueBot;
            NomBot = nomBot;
            AntagonisteBoot = antagonisteBot;
            
            // Ajuster les statistiques en fonction du niveau
            switch (niveauBot.ToLower())
            {
                case "expert":
                    VieMax = 100;
                    Vie = VieMax;
                    Force = 15;
                    Agilite = 15;
                    Intelligence = 15;
                    break;
                case "intermédiaire":
                case "intermediaire":
                    VieMax = 75;
                    Vie = VieMax;
                    Force = 10;
                    Agilite = 10;
                    Intelligence = 10;
                    break;
                default: // débutant
                    // Valeurs par défaut
                    break;
            }
        }

        /// <summary>
        /// Génère un dialogue en fonction du contexte.
        /// </summary>
        /// <param name="contexte">Contexte de l'interaction</param>
        /// <returns>Le dialogue généré</returns>
        public string GenererDialogue(string contexte = "")
        {
            // Simple logique de dialogue
            if (string.IsNullOrEmpty(contexte))
            {
                return DialogueBot;
            }
            
            if (contexte.ToLower().Contains("quête") || contexte.ToLower().Contains("quete"))
            {
                if (QuetesDisponibles.Count > 0)
                {
                    return $"J'ai une quête pour vous : {QuetesDisponibles[0].NomQuete}";
                }
                else
                {
                    return "Désolé, je n'ai pas de quête à vous proposer pour le moment.";
                }
            }
            
            if (contexte.ToLower().Contains("combat"))
            {
                if (AntagonisteBoot == "Oui")
                {
                    return "Préparez-vous au combat !";
                }
                else
                {
                    return "Je ne suis pas un ennemi, je ne veux pas me battre.";
                }
            }
            
            return DialogueBot;
        }

        /// <summary>
        /// Réagit à une action du personnage.
        /// </summary>
        /// <param name="action">L'action effectuée</param>
        /// <returns>La réaction du bot</returns>
        public string Reagir(string action)
        {
            if (action.ToLower().Contains("attaquer") || action.ToLower().Contains("frappe"))
            {
                if (AntagonisteBoot == "Oui")
                {
                    return "Vous allez regretter cette attaque !";
                }
                else
                {
                    return "Pourquoi m'attaquez-vous ? Je suis pacifique !";
                }
            }
            
            if (action.ToLower().Contains("parler") || action.ToLower().Contains("discuter"))
            {
                return GenererDialogue();
            }
            
            if (action.ToLower().Contains("aider") || action.ToLower().Contains("service"))
            {
                if (QuetesDisponibles.Count > 0)
                {
                    return $"Merci de votre aide ! Voici une quête : {QuetesDisponibles[0].NomQuete}";
                }
                else
                {
                    return "Merci pour votre offre d'aide, mais je n'ai pas besoin d'assistance pour le moment.";
                }
            }
            
            return "Je ne comprends pas ce que vous voulez faire.";
        }
        
        #region Implémentation de IInteractable
        
        /// <summary>
        /// Méthode d'interaction avec ce bot.
        /// </summary>
        /// <returns>Le résultat de l'interaction</returns>
        public string Interagir()
        {
            return GenererDialogue();
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
            }
            
            return degatsReels;
        }
        
        #endregion
    }
} 