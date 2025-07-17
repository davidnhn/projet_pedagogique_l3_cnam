using Godot;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente une zone de jeu.
    /// </summary>
    public partial class Zone : Node2D
    {
        public int IdZone { get; set; }
        public string NomZone { get; set; }
        public string DescriptionZone { get; set; }
        public bool EstDecouverte { get; set; }
        
        // Relations
        public List<Bot> Bots { get; set; } = new List<Bot>();
        public List<Combat> Combats { get; set; } = new List<Combat>();
        
        // Caractéristiques de la zone
        public int DifficulteZone { get; set; } = 1;
        public string TypeEnvironnement { get; set; } = "Neutre"; // Forêt, Montagne, Désert, etc.
        
        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Zone() 
        {
            EstDecouverte = false;
        }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idZone">ID de la zone</param>
        /// <param name="nomZone">Nom de la zone</param>
        /// <param name="descriptionZone">Description de la zone</param>
        public Zone(int idZone, string nomZone, string descriptionZone)
        {
            IdZone = idZone;
            NomZone = nomZone;
            DescriptionZone = descriptionZone;
            EstDecouverte = false;
        }

        /// <summary>
        /// Découvre la zone et renvoie sa description.
        /// </summary>
        /// <returns>La description de la zone</returns>
        public string DecouvrirZone()
        {
            if (!EstDecouverte)
            {
                EstDecouverte = true;
                return $"Vous découvrez {NomZone}! {DescriptionZone}";
            }
            else
            {
                return $"Vous êtes dans {NomZone}. {DescriptionZone}";
            }
        }

        /// <summary>
        /// Ajoute un bot à la zone.
        /// </summary>
        /// <param name="bot">Le bot à ajouter</param>
        public void AjouterBot(Bot bot)
        {
            if (bot != null && !Bots.Contains(bot))
            {
                Bots.Add(bot);
                AddChild(bot);
            }
        }

        /// <summary>
        /// Initialise un combat dans la zone.
        /// </summary>
        /// <param name="personnage">Le personnage impliqué</param>
        /// <param name="bot">Le bot opposant</param>
        /// <returns>Le combat créé</returns>
        public Combat InitierCombat(Personnage personnage, Bot bot)
        {
            if (personnage != null && bot != null)
            {
                Combat nouveauCombat = new Combat
                {
                    IdCombat = Combats.Count + 1,
                    ResultatCombat = "En cours",
                    Zone = this,
                    Personnage = personnage,
                    Bot = bot
                };
                
                Combats.Add(nouveauCombat);
                return nouveauCombat;
            }
            
            return null;
        }

        /// <summary>
        /// Obtient une description détaillée de la zone.
        /// </summary>
        /// <returns>Description détaillée</returns>
        public string ObtenirDescriptionDetaillee()
        {
            string description = $"{NomZone} - {DescriptionZone}\n";
            description += $"Difficulté: {DifficulteZone}\n";
            description += $"Type d'environnement: {TypeEnvironnement}\n";
            
            if (Bots.Count > 0)
            {
                description += "Personnages présents:\n";
                foreach (var bot in Bots)
                {
                    description += $"- {bot.NomBot}\n";
                }
            }
            else
            {
                description += "Aucun personnage présent.\n";
            }
            
            return description;
        }
    }
} 