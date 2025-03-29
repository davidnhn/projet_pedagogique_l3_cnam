using Godot;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente une quête dans le jeu.
    /// </summary>
    public class Quete : Resource
    {
        public int IdQuete { get; set; }
        public string NomQuete { get; set; }
        public string DescriptionQuete { get; set; }
        public string EtatQuete { get; set; } // "Non commencé", "En cours", "Terminé", "Échoué"
        
        // Caractéristiques de la quête
        public int RecompenseExperience { get; set; }
        public List<Objet> RecompensesObjets { get; set; } = new List<Objet>();
        public List<string> ObjectifsQuete { get; set; } = new List<string>();
        public List<bool> ObjectifsRealises { get; set; } = new List<bool>();

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Quete() 
        {
            EtatQuete = "Non commencé";
        }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="idQuete">ID de la quête</param>
        /// <param name="nomQuete">Nom de la quête</param>
        /// <param name="descriptionQuete">Description de la quête</param>
        /// <param name="etatQuete">État initial de la quête</param>
        public Quete(int idQuete, string nomQuete, string descriptionQuete, string etatQuete = "Non commencé")
        {
            IdQuete = idQuete;
            NomQuete = nomQuete;
            DescriptionQuete = descriptionQuete;
            EtatQuete = etatQuete;
        }

        /// <summary>
        /// Change l'état de la quête.
        /// </summary>
        /// <param name="nouvelEtat">Le nouvel état</param>
        /// <returns>Un message indiquant le changement</returns>
        public string ChangerEtat(string nouvelEtat)
        {
            string ancienEtat = EtatQuete;
            EtatQuete = nouvelEtat;
            
            return $"La quête \"{NomQuete}\" est passée de l'état \"{ancienEtat}\" à \"{nouvelEtat}\"";
        }

        /// <summary>
        /// Ajoute un objectif à la quête.
        /// </summary>
        /// <param name="objectif">Description de l'objectif</param>
        public void AjouterObjectif(string objectif)
        {
            if (!string.IsNullOrEmpty(objectif))
            {
                ObjectifsQuete.Add(objectif);
                ObjectifsRealises.Add(false);
            }
        }

        /// <summary>
        /// Valide un objectif de la quête.
        /// </summary>
        /// <param name="indexObjectif">Index de l'objectif à valider</param>
        /// <returns>Un message indiquant la validation ou non</returns>
        public string ValiderObjectif(int indexObjectif)
        {
            if (indexObjectif >= 0 && indexObjectif < ObjectifsQuete.Count)
            {
                if (!ObjectifsRealises[indexObjectif])
                {
                    ObjectifsRealises[indexObjectif] = true;
                    
                    // Vérifie si tous les objectifs sont réalisés
                    bool tousPondrales = true;
                    foreach (bool realise in ObjectifsRealises)
                    {
                        if (!realise)
                        {
                            tousPondrales = false;
                            break;
                        }
                    }
                    
                    if (tousPondrales && EtatQuete != "Terminé")
                    {
                        ChangerEtat("Terminé");
                        return $"Objectif \"{ObjectifsQuete[indexObjectif]}\" validé. Tous les objectifs sont accomplis ! Quête terminée.";
                    }
                    
                    return $"Objectif \"{ObjectifsQuete[indexObjectif]}\" validé.";
                }
                else
                {
                    return $"L'objectif \"{ObjectifsQuete[indexObjectif]}\" est déjà validé.";
                }
            }
            
            return "Objectif invalide.";
        }

        /// <summary>
        /// Ajoute une récompense objet à la quête.
        /// </summary>
        /// <param name="objet">L'objet de récompense</param>
        public void AjouterRecompenseObjet(Objet objet)
        {
            if (objet != null && !RecompensesObjets.Contains(objet))
            {
                RecompensesObjets.Add(objet);
            }
        }

        /// <summary>
        /// Obtient la description complète de la quête avec ses objectifs.
        /// </summary>
        /// <returns>Description détaillée</returns>
        public string ObtenirDescriptionComplete()
        {
            string description = $"{NomQuete} - {DescriptionQuete}\n";
            description += $"État: {EtatQuete}\n\n";
            
            if (ObjectifsQuete.Count > 0)
            {
                description += "Objectifs:\n";
                for (int i = 0; i < ObjectifsQuete.Count; i++)
                {
                    string statut = ObjectifsRealises[i] ? "[X]" : "[ ]";
                    description += $"{statut} {ObjectifsQuete[i]}\n";
                }
            }
            
            description += $"\nRécompense: {RecompenseExperience} points d'expérience\n";
            
            if (RecompensesObjets.Count > 0)
            {
                description += "Objets de récompense:\n";
                foreach (var objet in RecompensesObjets)
                {
                    description += $"- {objet.NomObjet}\n";
                }
            }
            
            return description;
        }
    }
} 