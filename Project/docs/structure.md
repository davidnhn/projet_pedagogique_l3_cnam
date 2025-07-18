# Structure du Projet

Ce document décrit la structure des dossiers et fichiers du projet pour faciliter la navigation et la compréhension de l'organisation globale.

## Organisation des dossiers

```
projet_pedagogique_l3_cnam/
├── .env.example             # Exemple de fichier de variables d'environnement
├── .gitignore               # Fichiers et dossiers ignorés par Git
├── docker-compose.yml       # Configuration Docker Compose
├── README.md                # Documentation principale
├── Docker/                  # Configuration Docker
│   ├── postgres/            # Configuration PostgreSQL
│   └── SQL/                 # Scripts SQL
│       ├── 00_schema.sql    # Création des tables
│       └── 01_seed.sql      # Données initiales
├── docs/                    # Documentation détaillée
└── src/                     # Code source du jeu (Godot)
    ├── assets/              # Ressources graphiques et audio
    ├── scenes/              # Scènes Godot
    ├── scripts/             # Scripts C#
    ├── .godot/              # Configuration Godot (généré)
    ├── project.godot        # Fichier de projet Godot
    └── projet.csproj        # Projet .NET pour C#
```

## Composants clés

### Configuration Docker

Le projet utilise Docker pour encapsuler la base de données PostgreSQL et pgAdmin, facilitant ainsi le développement et le déploiement.

- `docker-compose.yml` - Définit les services, réseaux et volumes
- `Docker/postgres/` - Configuration spécifique à PostgreSQL
- `Docker/SQL/` - Scripts SQL pour initialiser la base de données

### Base de données

La base de données est configurée via deux scripts principaux :

- `00_schema.sql` - Définit le schéma de la base de données (tables et relations)
- `01_seed.sql` - Fournit des données de test initiales

### Application Godot

Le jeu est développé avec Godot Engine (version .NET) et est structuré comme suit :

- `scenes/` - Contient les scènes Godot (.tscn)
- `scripts/` - Contient les scripts C# (.cs)
- `assets/` - Contient les ressources graphiques, sons, etc.

## Conventions de nommage

Le projet suit les conventions de nommage suivantes :

- **Classes C#** : PascalCase (ex: `PlayerController.cs`)
- **Scènes Godot** : PascalCase (ex: `MainMenu.tscn`)
- **Variables** : camelCase pour les variables locales, _camelCase pour les champs privés
- **Constantes** : SNAKE_CASE_MAJUSCULE

## Flux de données

Le flux de données dans l'application suit généralement ce schéma :

1. L'interface utilisateur (UI) dans Godot capture les actions du joueur
2. Les scripts C# traitent ces actions et mettent à jour l'état du jeu
3. Les modifications d'état importantes sont persistées dans la base de données PostgreSQL
4. Les données sont chargées depuis la base de données lors de l'initialisation du jeu ou des chargements de sauvegarde

## Pattern d'architecture

Le projet implémente principalement les patterns suivants :

- **Memento** pour la fonctionnalité de sauvegarde/chargement
- **MVC (Model-View-Controller)** pour séparer la logique du jeu, l'affichage et le contrôle
- **Singleton** pour les gestionnaires globaux (ex: gestionnaire de base de données)

## Documentation du code

Le code C# est documenté selon les standards XML de .NET pour faciliter l'utilisation d'IntelliSense et la génération de documentation.

Exemple :
```csharp
/// <summary>
/// Représente un personnage jouable dans le jeu.
/// </summary>
public class Personnage
{
    /// <summary>
    /// Obtient ou définit les points de vie du personnage.
    /// </summary>
    public int PointsDeVie { get; set; }
    
    /// <summary>
    /// Applique des dégâts au personnage.
    /// </summary>
    /// <param name="montant">Le montant de dégâts à appliquer.</param>
    /// <returns>True si le personnage est encore vivant, false sinon.</returns>
    public bool AppliquerDegats(int montant)
    {
        // Implémentation
    }
}
``` 