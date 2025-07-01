using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    /// <summary>
    /// Service d'API du jeu qui orchestrera les différents repositories.
    /// Exemple d'implémentation pour montrer comment utiliser les repositories ensemble.
    /// </summary>
    public class GameApiService
    {
        private readonly JoueurRepository _joueurRepository;
        private readonly PersonnageRepository _personnageRepository;
        private readonly ObjetRepository _objetRepository;
        private readonly ZoneRepository _zoneRepository;
        private readonly QueteRepository _queteRepository;
        private readonly BotRepository _botRepository;
        private readonly CombatRepository _combatRepository;
        private readonly InventaireRepository _inventaireRepository;
        private readonly SauvegardeRepository _sauvegardeRepository;

        public GameApiService(
            JoueurRepository joueurRepository,
            PersonnageRepository personnageRepository,
            ObjetRepository objetRepository,
            ZoneRepository zoneRepository,
            QueteRepository queteRepository,
            BotRepository botRepository,
            CombatRepository combatRepository,
            InventaireRepository inventaireRepository,
            SauvegardeRepository sauvegardeRepository)
        {
            _joueurRepository = joueurRepository;
            _personnageRepository = personnageRepository;
            _objetRepository = objetRepository;
            _zoneRepository = zoneRepository;
            _queteRepository = queteRepository;
            _botRepository = botRepository;
            _combatRepository = combatRepository;
            _inventaireRepository = inventaireRepository;
            _sauvegardeRepository = sauvegardeRepository;
        }

        #region Gestion des Joueurs et Personnages

        /// <summary>
        /// Crée un nouveau joueur avec un personnage initial.
        /// </summary>
        public async Task<Joueur> CreerNouveauJoueurAsync(string pseudoJoueur, string nomPersonnage, int typePersonnageId)
        {
            try
            {
                // Créer le joueur
                var nouveauJoueur = new Joueur { PseudoJoueur = pseudoJoueur };
                var joueurCree = await _joueurRepository.CreateAsync(nouveauJoueur);

                if (joueurCree != null)
                {
                    // Créer le personnage initial
                    var nouveauPersonnage = new Personnage
                    {
                        NomPersonnage = nomPersonnage,
                        Vie = 100,
                        Experience = 0,
                        EtatPersonnage = "Actif"
                    };

                    await _personnageRepository.CreateAsync(nouveauPersonnage);
                }

                return joueurCree;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la création du joueur : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Récupère toutes les informations d'un joueur avec ses personnages.
        /// </summary>
        public async Task<(Joueur joueur, List<Personnage> personnages)> GetJoueurCompletAsync(int joueurId)
        {
            try
            {
                var joueur = await _joueurRepository.GetByIdAsync(joueurId);
                var personnages = await _personnageRepository.GetByJoueurIdAsync(joueurId);

                return (joueur, personnages);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la récupération du joueur : {ex.Message}");
                return (null, new List<Personnage>());
            }
        }

        #endregion

        #region Gestion des Zones et Exploration

        /// <summary>
        /// Récupère les informations d'une zone avec ses bots.
        /// </summary>
        public async Task<(Zone zone, List<Bot> bots)> GetZoneAvecBotsAsync(int zoneId)
        {
            try
            {
                var zone = await _zoneRepository.GetByIdAsync(zoneId);
                
                // Note: Il faudrait une méthode GetBotsByZoneIdAsync dans BotRepository
                // Pour l'exemple, on récupère tous les bots
                var bots = await _botRepository.GetAllAsync();

                return (zone, bots);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la récupération de la zone : {ex.Message}");
                return (null, new List<Bot>());
            }
        }

        #endregion

        #region Gestion des Combats

        /// <summary>
        /// Initie un combat entre un personnage et un bot en utilisant la logique métier existante.
        /// </summary>
        public async Task<Combat> InitierCombatAsync(int personnageId, int botId, int zoneId)
        {
            try
            {
                // 1. Récupérer les entités (Services de persistance)
                var personnage = await _personnageRepository.GetByIdAsync(personnageId);
                var bot = await _botRepository.GetByIdAsync(botId);
                var zone = await _zoneRepository.GetByIdAsync(zoneId);

                if (personnage == null || bot == null || zone == null)
                {
                    throw new ArgumentException("Entités introuvables");
                }

                // 2. Utiliser la logique métier de Zone pour créer le combat
                var combat = zone.InitierCombat(personnage, bot);
                
                // 3. Persister le combat (Service de persistance)
                var combatCree = await _combatRepository.CreateAsync(combat);

                return combatCree;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de l'initiation du combat : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gère un tour de combat en utilisant la logique métier de Combat.
        /// </summary>
        public async Task<(string resultat, bool combatTermine)> GererTourCombatAsync(int combatId)
        {
            try
            {
                // 1. Récupérer le combat (Service de persistance)
                var combat = await _combatRepository.GetByIdAsync(combatId);
                
                if (combat == null)
                {
                    return ("Combat introuvable", false);
                }

                // 2. Utiliser la logique métier de Combat
                string resultatTour = combat.GererTour();
                bool estTermine = combat.EstTermine;

                // 3. Mettre à jour en base (Service de persistance)
                await _combatRepository.UpdateAsync(combat);

                return (resultatTour, estTermine);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors du tour de combat : {ex.Message}");
                return ("Erreur", false);
            }
        }

        /// <summary>
        /// Tente une fuite de combat en utilisant la logique métier.
        /// </summary>
        public async Task<string> TenterFuiteCombatAsync(int combatId)
        {
            try
            {
                var combat = await _combatRepository.GetByIdAsync(combatId);
                
                if (combat == null)
                {
                    return "Combat introuvable";
                }

                // Utiliser la logique métier de Combat pour la fuite
                string resultatFuite = combat.FuirCombat();
                
                // Sauvegarder l'état mis à jour
                await _combatRepository.UpdateAsync(combat);

                return resultatFuite;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la fuite : {ex.Message}");
                return "Erreur lors de la fuite";
            }
        }

        #endregion

        #region Gestion des Objets et Effets

        /// <summary>
        /// Utilise un objet sur un personnage en appliquant sa logique métier.
        /// </summary>
        public async Task<string> UtiliserObjetAsync(int personnageId, int objetId)
        {
            try
            {
                // 1. Récupérer les entités
                var personnage = await _personnageRepository.GetByIdAsync(personnageId);
                var objet = await _objetRepository.GetByIdAsync(objetId);

                if (personnage == null || objet == null)
                {
                    return "Personnage ou objet introuvable";
                }

                // 2. Utiliser la logique métier d'Objet
                string resultat = objet.AppliquerEffet(personnage);

                // 3. Sauvegarder les modifications du personnage
                await _personnageRepository.UpdateAsync(personnage);

                // 4. Optionnel : retirer l'objet de l'inventaire
                // await _inventaireRepository.RetirerObjetAsync(inventaireId, objetId, 1);

                return resultat;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de l'utilisation de l'objet : {ex.Message}");
                return "Erreur lors de l'utilisation";
            }
        }

        #endregion

        #region Gestion de l'Expérience et Niveaux

        /// <summary>
        /// Fait gagner de l'expérience à un personnage en utilisant sa logique métier.
        /// </summary>
        public async Task<(bool aGagneNiveau, string message)> GagnerExperienceAsync(int personnageId, int montant)
        {
            try
            {
                var personnage = await _personnageRepository.GetByIdAsync(personnageId);
                
                if (personnage == null)
                {
                    return (false, "Personnage introuvable");
                }

                // Utiliser la logique métier de Personnage
                bool aGagneNiveau = personnage.GagnerExperience(montant);
                
                // Sauvegarder les modifications
                await _personnageRepository.UpdateAsync(personnage);

                string message = aGagneNiveau 
                    ? $"{personnage.NomPersonnage} a gagné {montant} XP et un niveau !"
                    : $"{personnage.NomPersonnage} a gagné {montant} XP.";

                return (aGagneNiveau, message);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors du gain d'expérience : {ex.Message}");
                return (false, "Erreur");
            }
        }

        #endregion

        #region Gestion des Inventaires

        /// <summary>
        /// Ajoute un objet à l'inventaire d'un personnage.
        /// </summary>
        public async Task<bool> AjouterObjetInventaireAsync(int inventaireId, int objetId, int quantite = 1)
        {
            try
            {
                return await _inventaireRepository.AjouterObjetAsync(inventaireId, objetId, quantite);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de l'ajout à l'inventaire : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Récupère l'inventaire complet d'un personnage.
        /// </summary>
        public async Task<List<Objet>> GetInventaireCompletAsync(int inventaireId)
        {
            try
            {
                return await _inventaireRepository.GetObjetsByInventaireIdAsync(inventaireId);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la récupération de l'inventaire : {ex.Message}");
                return new List<Objet>();
            }
        }

        #endregion

        #region Gestion des Quêtes

        /// <summary>
        /// Récupère toutes les quêtes actives d'un personnage.
        /// </summary>
        public async Task<List<Quete>> GetQuetesActivesAsync(int personnageId)
        {
            try
            {
                return await _queteRepository.GetByPersonnageIdAsync(personnageId);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la récupération des quêtes : {ex.Message}");
                return new List<Quete>();
            }
        }

        /// <summary>
        /// Met à jour l'état d'une quête.
        /// </summary>
        public async Task<bool> MettreAJourQueteAsync(int queteId, string nouvelEtat)
        {
            try
            {
                var quete = await _queteRepository.GetByIdAsync(queteId);
                if (quete != null)
                {
                    quete.EtatQuete = nouvelEtat;
                    return await _queteRepository.UpdateAsync(quete);
                }
                return false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la mise à jour de la quête : {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Gestion des Sauvegardes

        /// <summary>
        /// Crée une nouvelle sauvegarde pour un joueur.
        /// </summary>
        public async Task<Sauvegarde> CreerSauvegardeAsync(int joueurId, int zoneId, int positionX, int positionY)
        {
            try
            {
                var nouvelleSauvegarde = new Sauvegarde
                {
                    DateSauvegarde = DateTime.Now,
                    PositionXJoueur = positionX,
                    PositionYJoueur = positionY
                };

                return await _sauvegardeRepository.CreateAsync(nouvelleSauvegarde);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la création de la sauvegarde : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Récupère la dernière sauvegarde d'un joueur.
        /// </summary>
        public async Task<Sauvegarde> GetDerniereSauvegardeAsync(int joueurId)
        {
            try
            {
                return await _sauvegardeRepository.GetDerniereSauvegardeByJoueurIdAsync(joueurId);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur lors de la récupération de la sauvegarde : {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Utilitaires

        /// <summary>
        /// Teste la connexion à la base de données en récupérant tous les joueurs.
        /// </summary>
        public async Task<bool> TesterConnexionAsync()
        {
            try
            {
                var joueurs = await _joueurRepository.GetAllAsync();
                GD.Print($"Connexion testée avec succès. {joueurs.Count} joueurs trouvés.");
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Erreur de connexion : {ex.Message}");
                return false;
            }
        }

        #endregion
    }
} 