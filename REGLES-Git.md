
# 🎉 Bienvenue dans le guide Git de l'équipe !

👋 **Salut l’équipe,**

🎁 J’ai le plaisir (et la peur un peu aussi) de vous partager **notre guide Git officiel**, aussi appelé :

> _"Git ou double : le manuel de survie collaboratif"_

Dans ce fichier Markdown, vous trouverez :
- Les bases pour les débutants (comme moi 😅),
- Les bonnes pratiques pour les experts (oui, vous là, les merge warriors 💪),
- Et un exemple ultra détaillé pour éviter de faire exploser `master` sans le vouloir 🔥

📘 ➜ Ce fichier, c’est notre parachute commun. Lisez-le, améliorez-le, utilisez-le.
Spoiler : personne n'a été blessé lors de la rédaction de ce document. Pas encore.

📢 N’hésitez pas à me faire vos retours, à corriger, compléter ou simplement à rire un bon coup avant de faire une PR !

---

# 📘 Règles d’utilisation de Git en équipe

## 🎯 Objectif

Assurer un travail collaboratif fluide, organisé et traçable avec Git, en utilisant des branches par fonctionnalité et en respectant les bonnes pratiques.

---

## 🧱 Structure des branches

| Branche         | Rôle                                               |
|----------------|----------------------------------------------------|
| `master`          | Code stable et prêt pour la production             |
| `develop`       | Code validé mais non encore en production          |
| `feature/*`     | Fonctionnalités en cours de développement          |
| `bugfix/*`      | Correctifs mineurs                                 |
| `hotfix/*`      | Urgences en production                             |
| `release/*`     | Préparation d'une nouvelle version stable          |

---

## ✅ Règles générales

- Ne pas travailler directement sur `master` ou `develop`
- Une tâche = une branche = une Pull Request
- Toujours mettre à jour votre branche avant de la pousser
- Écrire des **commits clairs et concis**
- Supprimer les branches une fois fusionnées

---

## ✍️ Format des messages de commit

```
<type>(<scope>): <message>
```

Exemples :
- `feat(auth): ajout de l’authentification par token`
- `fix(login): correction du bug de validation`
- `docs(readme): mise à jour de la documentation`

Types : `feat`, `fix`, `docs`, `refactor`, `test`, `chore`

---

## 🔁 Exemple détaillé : ajout de la page de contact

### 🎬 Étapes complètes

#### 1. Se placer sur `develop`
```bash
git checkout develop
git pull origin develop
```

#### 2. Créer une branche pour la fonctionnalité
```bash
git checkout -b feature/contact-page
```

#### 3. Développer la fonctionnalité
Modifie tes fichiers (ex: `contact.html`, `styles.css`, etc.)

#### 4. Vérifier les modifications
```bash
git status
git diff
```

#### 5. Ajouter les fichiers modifiés
```bash
git add contact.html styles.css
```

#### 6. Faire un commit explicite
```bash
git commit -m "feat(contact): ajout de la page de contact avec formulaire"
```

#### 7. Synchroniser avec `develop` s’il a changé (optionnel mais recommandé)
```bash
git fetch origin
git rebase origin/develop
```

#### 8. Pousser la branche sur le serveur
```bash
git push -u origin feature/contact-page
```

---

## 🔀 Faire une Pull Request (PR)

Une Pull Request permet de proposer l'intégration de ton travail dans une branche cible (souvent `develop`).

### Étapes sur GitHub ou GitLab :
1. Aller sur le dépôt distant (GitHub/GitLab)
2. Cliquer sur **"Compare & Pull Request"** ou **"Merge Request"**
3. Choisir :
   - la branche source (ex: `feature/contact-page`)
   - la branche cible (ex: `develop`)
4. Donner un **titre clair** et une **description de ce que tu as fait**
5. Assigner un ou plusieurs **relecteurs**
6. Cliquer sur **"Create Pull Request"**

Une fois la PR validée et fusionnée :
- Supprime la branche **depuis l’interface**
- Ou fais-le en ligne de commande :

```bash
git branch -d feature/contact-page                    # localement
git push origin --delete feature/contact-page         # à distance
```

---

## 🔄 Cas particulier : mise à jour de sa branche locale

Avant de commencer à travailler :
```bash
git checkout develop
git pull origin develop
```

Pour mettre à jour une branche en cours :
```bash
git checkout feature/ma-branche
git fetch origin
git rebase origin/develop  # ou git merge origin/develop
```

---

## 🛠️ Astuces utiles

| Action                                 | Commande                                        |
|----------------------------------------|-------------------------------------------------|
| Voir l’historique                      | `git log --oneline --graph --all`              |
| Voir les branches locales              | `git branch`                                   |
| Voir les branches distantes            | `git branch -r`                                |
| Nettoyer les branches distantes mortes | `git fetch --prune`                            |
| Sauvegarder temporairement             | `git stash` / `git stash pop`                  |

---

## 📌 À retenir

- Travailler **toujours** dans des branches dédiées
- Faire des commits **réguliers et lisibles**
- Relire le code dans les Pull Requests
- **Ne jamais forcer un push** sauf si absolument nécessaire

---


