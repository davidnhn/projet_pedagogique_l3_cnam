using Godot;
using System.Collections.Generic;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente l'inventaire d'un personnage (implémenté en Singleton).
    /// </summary>
    public class Inventaire : Resource
    {
        // Implémentation du Singleton
        private static Inventaire _instance;
        public static Inventaire GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Inventaire();
            }
            return _instance;
        }

        // Propriétés
        public int IdInventaire { get; set; }
        public int TailleMaxInventaire { get; set; } = 20;
        
        // Stockage des objets avec leur quantité
        private Dictionary<Objet, int> _objets = new Dictionary<Objet, int>();

        // Constructeur privé pour empêcher l'instanciation directe
        private Inventaire() 
        {
            IdInventaire = 1; // Par défaut
        }

        /// <summary>
        /// Constructeur utilisé pour créer un inventaire spécifique (non singleton).
        /// </summary>
        /// <param name="idInventaire">ID de l'inventaire</param>
        /// <param name="tailleMax">Taille maximale</param>
        public Inventaire(int idInventaire, int tailleMax)
        {
            IdInventaire = idInventaire;
            TailleMaxInventaire = tailleMax;
        }

        /// <summary>
        /// Ajoute un objet à l'inventaire.
        /// </summary>
        /// <param name="objet">L'objet à ajouter</param>
        /// <param name="quantite">La quantité à ajouter</param>
        /// <returns>Vrai si l'ajout a réussi, faux sinon</returns>
        public bool AjouterObjet(Objet objet, int quantite = 1)
        {
            if (objet == null || quantite <= 0)
            {
                return false;
            }

            // Vérifier si l'inventaire est plein (nombre d'objets différents)
            if (!_objets.ContainsKey(objet) && _objets.Count >= TailleMaxInventaire)
            {
                return false;
            }

            // Ajouter ou mettre à jour la quantité
            if (_objets.ContainsKey(objet))
            {
                _objets[objet] += quantite;
            }
            else
            {
                _objets.Add(objet, quantite);
            }

            return true;
        }

        /// <summary>
        /// Retire un objet de l'inventaire.
        /// </summary>
        /// <param name="objet">L'objet à retirer</param>
        /// <param name="quantite">La quantité à retirer</param>
        /// <returns>Vrai si le retrait a réussi, faux sinon</returns>
        public bool RetirerObjet(Objet objet, int quantite = 1)
        {
            if (objet == null || quantite <= 0 || !_objets.ContainsKey(objet))
            {
                return false;
            }

            // Vérifier si la quantité est suffisante
            if (_objets[objet] < quantite)
            {
                return false;
            }

            // Mettre à jour la quantité
            _objets[objet] -= quantite;

            // Supprimer l'entrée si la quantité atteint zéro
            if (_objets[objet] <= 0)
            {
                _objets.Remove(objet);
            }

            return true;
        }

        /// <summary>
        /// Obtient la quantité d'un objet dans l'inventaire.
        /// </summary>
        /// <param name="objet">L'objet recherché</param>
        /// <returns>La quantité, 0 si l'objet n'est pas présent</returns>
        public int ObtenirQuantite(Objet objet)
        {
            if (objet == null || !_objets.ContainsKey(objet))
            {
                return 0;
            }

            return _objets[objet];
        }

        /// <summary>
        /// Obtient tous les objets de l'inventaire avec leur quantité.
        /// </summary>
        /// <returns>Un dictionnaire des objets et leurs quantités</returns>
        public Dictionary<Objet, int> ObtenirTousLesObjets()
        {
            return new Dictionary<Objet, int>(_objets);
        }

        /// <summary>
        /// Vérifie si l'inventaire contient un objet spécifique.
        /// </summary>
        /// <param name="objet">L'objet à vérifier</param>
        /// <returns>Vrai si l'objet est présent, faux sinon</returns>
        public bool ContientObjet(Objet objet)
        {
            return objet != null && _objets.ContainsKey(objet);
        }
    }
} 