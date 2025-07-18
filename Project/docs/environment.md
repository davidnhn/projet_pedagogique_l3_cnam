# Configuration de l'Environnement

## Fichier `.env`

Le projet utilise un fichier `.env` pour stocker les variables d'environnement sensibles comme les mots de passe et configurations spécifiques à l'environnement.

### Création du fichier `.env`

1. Copiez le fichier `.env.example` en `.env` :
   ```bash
   cp .env.example .env
   ```

2. Modifiez les valeurs dans `.env` selon votre environnement. Exemple de contenu :
   ```
   # PostgreSQL
   POSTGRES_USER=postgres
   POSTGRES_PASSWORD=votre_mot_de_passe
   POSTGRES_DB=game_db
   
   # pgAdmin
   PGADMIN_DEFAULT_EMAIL=admin@example.com
   PGADMIN_DEFAULT_PASSWORD=votre_mot_de_passe_pgadmin
   
   ```

### Pourquoi `.env` doit être dans `.gitignore`

Le fichier `.env` contient des informations sensibles comme :
- Mots de passe de base de données
- Informations d'authentification
- Clés d'API ou autres secrets

Pour des raisons de sécurité, ce fichier **ne doit jamais être inclus dans le contrôle de version**. C'est pourquoi il est ajouté au fichier `.gitignore`.

Chaque développeur doit créer son propre fichier `.env` basé sur le modèle `.env.example` fourni dans le dépôt.

## Configuration de Docker

Le projet utilise Docker pour isoler et gérer l'environnement de développement. Assurez-vous que Docker et Docker Compose sont correctement installés sur votre système.

### Vérification de l'installation

```bash
docker --version
docker compose --version
```

### Résolution des problèmes courants

- **Erreur "permission denied"** : Vous pourriez avoir besoin d'exécuter Docker avec des privilèges élevés ou d'ajouter votre utilisateur au groupe docker.
- **Ports déjà utilisés** : Modifiez les ports dans le fichier `.env` si les ports par défaut sont déjà utilisés sur votre système. 