# Configuration de Godot et .NET

Ce projet utilise la version .NET de Godot Engine, qui combine la puissance de Godot avec les fonctionnalités du framework .NET.

## Installation de Godot .NET

1. Téléchargez la version .NET (C#) de Godot Engine depuis le [site officiel de Godot](https://godotengine.org/download)
   - Assurez-vous de choisir la version **4.x** avec le support .NET/C#
   - Sélectionnez la version correspondant à votre système d'exploitation (Windows, macOS, Linux)

2. Installez Godot en extrayant l'archive téléchargée vers un emplacement de votre choix
   - Godot ne nécessite pas d'installation traditionnelle, il suffit d'extraire et d'exécuter

## Installation du SDK .NET

Le SDK .NET est nécessaire pour compiler les scripts C# utilisés dans le projet.

1. Téléchargez et installez le [.NET SDK](https://dotnet.microsoft.com/download) (version 7.0 ou supérieure recommandée)

2. Vérifiez l'installation en exécutant :
   ```bash
   dotnet --version
   ```

3. L'installation du .NET SDK inclut :
   - Le runtime .NET
   - Les outils de développement .NET
   - Les bibliothèques de base .NET

## Configuration du projet Godot

1. Lancez Godot Engine

2. À l'écran du gestionnaire de projets, cliquez sur "Importer"

3. Naviguez jusqu'au dossier du projet cloné et sélectionnez le fichier `project.godot`

4. Une fois le projet ouvert, vérifiez que .NET est correctement configuré :
   - Ouvrez le menu "Projet" > "Outils de projet" > "Configuration C#"
   - Vérifiez que le SDK .NET est détecté correctement

## Structure des scripts C#

Dans un projet Godot .NET, les scripts C# sont organisés de la manière suivante :

- Les scripts C# se trouvent dans le dossier `src/`
- Chaque script C# est associé à un nœud Godot
- Les noms de classes doivent correspondre aux noms de fichiers

Exemple de script C# pour un nœud Personnage :

```csharp
using Godot;
using System;

public partial class Personnage : CharacterBody2D
{
    [Export]
    public int Vie { get; set; } = 100;
    
    public override void _Ready()
    {
        // Code d'initialisation
    }
    
    public override void _Process(double delta)
    {
        // Code exécuté à chaque frame
    }
}
```

## Communication avec la base de données

Le projet utilise Npgsql pour la communication avec PostgreSQL depuis C#. Assurez-vous que les références appropriées sont incluses dans le fichier `.csproj`.

Exemple de connexion à la base de données :

```csharp
using Npgsql;
using System;

public class DatabaseManager
{
    private static string ConnectionString = "Host=localhost;Port=5432;Database=game_db;Username=postgres;Password=votre_mot_de_passe";
    
    public static void ExecuteQuery(string sql)
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();
        
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }
}
```

## Construction du projet

Pour construire le projet Godot .NET :

1. Dans l'éditeur Godot, allez dans le menu "Projet" > "Outils" > "Construire C#"

2. Alternativement, vous pouvez construire depuis la ligne de commande :
   ```bash
   cd chemin/vers/le/projet
   dotnet build
   ```

## Résolution des problèmes courants

- **Erreur de référence de .NET** : Vérifiez que la version du SDK .NET est compatible avec celle requise par Godot
- **Scripts non détectés** : Assurez-vous que les noms de classes correspondent aux noms de fichiers
- **Erreurs de compilation** : Consultez le panneau de sortie pour les détails des erreurs 