# API Repositories - Guide d'utilisation

## Vue d'ensemble

Cette API fournit un ensemble complet de repositories pour interagir avec la base de données PostgreSQL de votre jeu vidéo. Chaque repository implémente le pattern Repository avec les opérations CRUD de base.

## Repositories disponibles

### 1. **JoueurRepository** ✅
- `GetByIdAsync(int id)` : Récupère un joueur par son ID
- `GetAllAsync()` : Récupère tous les joueurs
- `CreateAsync(Joueur joueur)` : Crée un nouveau joueur
- `UpdateAsync(Joueur joueur)` : Met à jour un joueur existant
- `DeleteAsync(int id)` : Supprime un joueur

### 2. **PersonnageRepository** ✅
- Opérations CRUD standard
- `GetByJoueurIdAsync(int joueurId)` : Récupère tous les personnages d'un joueur

### 3. **ObjetRepository** ✅
- Opérations CRUD standard
- `GetByEffetAsync(string effet)` : Recherche des objets par effet

### 4. **ZoneRepository** ✅
- Opérations CRUD standard
- `GetByNomAsync(string nom)` : Recherche des zones par nom

### 5. **QueteRepository** ✅
- Opérations CRUD standard
- `GetByEtatAsync(string etat)` : Récupère les quêtes par état
- `GetByPersonnageIdAsync(int personnageId)` : Récupère les quêtes d'un personnage

### 6. **BotRepository** ✅
- Opérations CRUD standard
- `GetByNiveauAsync(string niveau)` : Récupère les bots par niveau
- `GetAntagonistesAsync()` : Récupère tous les bots antagonistes

### 7. **CombatRepository** ✅
- Opérations CRUD standard
- `GetByZoneIdAsync(int zoneId)` : Récupère les combats d'une zone
- `GetByResultatAsync(string resultat)` : Récupère les combats par résultat

### 8. **TypePersonnageRepository** ✅
- Opérations CRUD standard
- `GetByNomAsync(string nom)` : Récupère un type par nom

### 9. **InventaireRepository** ✅
- Opérations CRUD standard
- `GetObjetsByInventaireIdAsync(int inventaireId)` : Récupère les objets d'un inventaire
- `AjouterObjetAsync(int inventaireId, int objetId, int quantite)` : Ajoute un objet
- `RetirerObjetAsync(int inventaireId, int objetId, int quantite)` : Retire un objet

### 10. **SauvegardeRepository** ✅
- Opérations CRUD standard
- `GetByJoueurIdAsync(int joueurId)` : Récupère les sauvegardes d'un joueur
- `GetDerniereSauvegardeByJoueurIdAsync(int joueurId)` : Récupère la dernière sauvegarde

## Exemple d'utilisation

```csharp
// Utilisation d'un repository
var joueurRepository = new JoueurRepository();

// Créer un nouveau joueur
var nouveauJoueur = new Joueur { PseudoJoueur = "PlayerOne" };
var joueurCree = await joueurRepository.CreateAsync(nouveauJoueur);

// Récupérer un joueur
var joueur = await joueurRepository.GetByIdAsync(1);

// Récupérer les personnages d'un joueur
var personnageRepository = new PersonnageRepository();
var personnages = await personnageRepository.GetByJoueurIdAsync(joueur.IdJoueur);
```

## Notes importantes

1. **Connexion à la base de données** : Tous les repositories utilisent `DatabaseConnection.Instance` pour se connecter à PostgreSQL.

2. **Relations** : Les objets complexes avec des relations (comme Personnage.TypePersonnage) nécessitent des requêtes séparées pour charger les données complètes.

3. **Valeurs par défaut** : Certains repositories utilisent des valeurs par défaut (comme `idJoueur = 1`) qui doivent être ajustées selon vos besoins métier.

4. **Gestion des erreurs** : Implémentez une gestion d'erreurs appropriée dans votre couche service.

## Architecture recommandée

```
Frontend Godot (GDScript)
        ↓ (HTTP/REST)
    API Controller (C#)
        ↓
    Service Layer (C#)
        ↓
    Repository Layer (C#) ← Nous sommes ici
        ↓
    Database (PostgreSQL)
```

## Prochaines étapes

1. **Créer des contrôleurs d'API** : Utilisez ASP.NET Core ou un autre framework web
2. **Implémenter la validation** : Valider les données avant les opérations CRUD
3. **Ajouter la gestion d'erreurs** : Try-catch et logging appropriés
4. **Sérialisation JSON** : Pour les réponses API
5. **Authentification** : JWT ou autres mécanismes de sécurité 