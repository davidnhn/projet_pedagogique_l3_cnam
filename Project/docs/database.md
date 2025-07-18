# Base de Données PostgreSQL

Le projet utilise PostgreSQL comme système de gestion de base de données relationnelle pour stocker les données du jeu.

## Structure de la base de données

La base de données contient plusieurs tables pour stocker les informations du jeu :
- Joueur
- Personnage
- TypePersonnage
- Bot
- Objet
- Inventaire
- Quete
- Zone
- Combat
- Action
- Sauvegarde
- ... et d'autres tables de relation

Le schéma complet est défini dans le fichier `Docker/SQL/00_schema.sql`.

## Données initiales

Les données initiales (seed) sont définies dans `Docker/SQL/01_seed.sql`. Ces données comprennent :
- Des joueurs de test
- Des personnages prédéfinis
- Des objets et inventaires
- Des zones et des quêtes
- Des bots et des actions

## Accès à la base de données avec pgAdmin

pgAdmin est inclus dans la configuration Docker pour faciliter la gestion de la base de données.

### Connexion à pgAdmin

1. Après avoir lancé les conteneurs Docker (`docker compose up -d`), accédez à pgAdmin via votre navigateur :
   ```
   http://localhost:5050
   ```

2. Connectez-vous avec les identifiants définis dans votre fichier `.env` :
   - Email : valeur de `PGADMIN_DEFAULT_EMAIL`
   - Mot de passe : valeur de `PGADMIN_DEFAULT_PASSWORD`

### Configuration d'une connexion à la base de données

1. Cliquez sur "Add New Server" dans le Dashboard pgAdmin
2. Dans l'onglet "General", donnez un nom à votre connexion (par exemple "Game Database")
3. Dans l'onglet "Connection", entrez les informations suivantes :
   - Host name/address: `postgres` (nom du service dans docker-compose)
   - Port: `5432`
   - Maintenance database: valeur de `POSTGRES_DB` (par défaut: `game_db`)
   - Username: valeur de `POSTGRES_USER` (par défaut: `postgres`)
   - Password: valeur de `POSTGRES_PASSWORD`

4. Cliquez sur "Save"

### Exploration des tables

Vous pouvez maintenant explorer les tables en naviguant dans :
Servers > Game Database > Databases > game_db > Schemas > public > Tables

### Exécution de requêtes SQL

pgAdmin permet d'exécuter des requêtes SQL via l'outil Query Tool :
1. Cliquez avec le bouton droit sur la base de données
2. Sélectionnez "Query Tool"
3. Écrivez et exécutez vos requêtes SQL

## Réinitialisation de la base de données

Si vous souhaitez réinitialiser complètement la base de données :

```bash
# Arrêtez les conteneurs et supprimez les volumes
docker compose down -v

# Redémarrez les conteneurs
docker compose up -d
```

Cette opération recréera entièrement le schéma et les données initiales. 