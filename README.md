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

2. Configurez les variables d'environnement presente dans Project/BaseDeDonnees/.env (Par defaut-> user/passwd = postgres/example_password):

   ```bash
   nano Project/Base_De_Donnees/.env
   ```
   
   Modifiez le fichier `.env` avec vos propres valeurs (mots de passe, etc.)


3. Se placer dans le repertoire BaseDeDonnees et lancez les conteneurs Docker :

   ```bash
   docker compose up -d
   ```
   pour verifier que les conteneurs sont bien lances:
   ```bash
   docker ps
   ```
   pour arreter les conteneurs:
   ```bash
   docker compose down -v
   ```

4. Ouvrez le projet dans Godot :
   - Lancez Godot Engine
   - Cliquez sur "Importer"
   - Naviguez jusqu'au dossier du projet et sélectionnez le fichier `Project/project.godot`

5. Pour se connecter a la base de donnees en commandline plutot que pgadmin: 
   - Installer psql
   - ```bash
	 psql postgresql://postgres:example_password@localhost:5432/game_db
	 ```
	 ici c'est le format://POSTGRES_USER:POSTGRES_PASSWORD@adresseDB:port/POSTGRES_DB


Pour plus de détails, consultez la [documentation complète](./docs/README.md). 

6. Pour se connecter avec pgadmin, se connecter avec le navigateur a l'adresse : http://localhost:5050/browser/
user: admin@example.com
password: admin_password

## Let's connect to the server

| Champ                 | Valeur               |
|-----------------------|----------------------|
| Existing Server       | `db` *(optionnel)*   |
| Server Name           | `db`                 |
| Host name/address     | `jeuvideo_postgres`  |
| Port                  | `5432`               |
| Database              | `game_db`            |
| User                  | `postgres`           |
| Password              | example_password|
| Role                  | *(vide)*             |
| Service               | *(vide)*             |

### Connection Parameters

| Name                | Keyword         | Value     |
|---------------------|------------------|-----------|
| SSL mode            | `sslmode`        | `prefer`  |
| Connection timeout  | `connect_timeout`| `10`      |

[Connect & Open Query Tool] (bouton)

