# Projet Pédagogique L3 CNAM

Ce projet est un jeu développé avec Godot et utilisant une base de données PostgreSQL.

## Prérequis

- [Godot Engine](https://godotengine.org/download) version .NET (4.x)
- [.NET SDK](https://dotnet.microsoft.com/download) 
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/) (généralement inclus avec Docker Desktop)

## Installation

1. Clonez ce dépôt :
   ```bash
   git clone <url-du-dépôt>
   cd projet_pedagogique_l3_cnam
   ```

2. Configurez les variables d'environnement :
   ```bash
   cp .env.example .env
   ```
   
   Modifiez le fichier `.env` avec vos propres valeurs (mots de passe, etc.)

3. Lancez les conteneurs Docker :
   ```bash
   docker compose up -d
   ```

4. Ouvrez le projet dans Godot :
   - Lancez Godot Engine
   - Cliquez sur "Importer"
   - Naviguez jusqu'au dossier du projet et sélectionnez le fichier `project.godot`



Pour plus de détails, consultez la [documentation complète](./docs/README.md). 