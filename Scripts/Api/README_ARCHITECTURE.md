# 🎮 API Jeu Vidéo - Architecture Complète

## 📁 Structure finale

```
Scripts/
├── Api/                           ← API Layer (nouveau)
│   ├── Controllers/
│   │   ├── JoueurController.cs    ← Routes HTTP pour joueurs
│   │   ├── CombatController.cs    ← Routes HTTP pour combats
│   │   └── ObjetController.cs     ← Routes HTTP pour objets
│   ├── Program.cs                 ← Point d'entrée de l'API
│   ├── Startup.cs                 ← Configuration DI + CORS + Swagger
│   └── appsettings.json           ← Configuration DB + Logging
├── Data/
│   ├── Database/
│   │   ├── BaseRepository.cs      ← Repository pattern de base
│   │   ├── JoueurRepository.cs    ← CRUD Joueurs
│   │   ├── PersonnageRepository.cs ← CRUD Personnages
│   │   ├── ObjetRepository.cs     ← CRUD Objets
│   │   ├── [...autres repositories]
│   │   └── GameApiService.cs      ← Service d'orchestration
│   ├── Combat.cs                  ← Service métier (existant)
│   ├── Objet.cs                   ← Service métier (existant)
│   ├── Personnage.cs              ← Service métier (existant)
│   └── [autres modèles métier]
└── Core/                          ← Interfaces et patterns (existant)
```

## 🏗️ Architecture en couches

```
┌─────────────────────────────────────────────────────────┐
│                   Frontend Godot                        │ ← GDScript
│                  (HTTP Client)                          │
└─────────────────────┬───────────────────────────────────┘
                      │ HTTP/REST
┌─────────────────────▼───────────────────────────────────┐
│                 API Controllers                         │ ← ASP.NET Core
│          (JoueurController, CombatController)           │
└─────────────────────┬───────────────────────────────────┘
                      │ Injection de dépendances
┌─────────────────────▼───────────────────────────────────┐
│                GameApiService                           │ ← Orchestration
│              (Validation + Coordination)                │
└─────────────────────┬───────────────────────────────────┘
                      │
        ┌─────────────▼──────────────┬────────────────────┐
        │                           │                    │
┌───────▼────────┐        ┌─────────▼─────────┐ ┌────────▼────────┐
│ Services Métier │        │ Services de       │ │ Infrastructure  │
│                │        │ Persistance       │ │                 │
│ • Combat.cs    │        │                   │ │ • DatabaseConn  │
│ • Objet.cs     │        │ • CombatRepo      │ │ • Logging       │
│ • Personnage.cs│        │ • ObjetRepo       │ │ • Configuration │
│ • Zone.cs      │        │ • PersonnageRepo  │ │                 │
│ • etc...       │        │ • etc...          │ │                 │
└────────────────┘        └───────────────────┘ └─────────────────┘
                                   │
                          ┌────────▼────────┐
                          │   PostgreSQL    │
                          │   Database      │
                          └─────────────────┘
```

## 🚀 Routes API disponibles

### 👤 Joueurs (`/api/joueur`)
- `GET /api/joueur` - Liste tous les joueurs
- `GET /api/joueur/{id}` - Détails d'un joueur + ses personnages
- `POST /api/joueur` - Crée un nouveau joueur
- `PUT /api/joueur/{id}` - Met à jour un joueur
- `DELETE /api/joueur/{id}` - Supprime un joueur

### ⚔️ Combats (`/api/combat`)
- `POST /api/combat/initier` - Initie un nouveau combat
- `GET /api/combat/{id}` - Détails d'un combat
- `POST /api/combat/{id}/tour` - Gère un tour de combat *(utilise Combat.cs)*
- `POST /api/combat/{id}/fuir` - Tentative de fuite *(utilise Combat.cs)*
- `GET /api/combat/zone/{zoneId}` - Combats d'une zone
- `GET /api/combat/{id}/historique` - Historique complet *(utilise Combat.cs)*

### 🎒 Objets (`/api/objet`)
- `GET /api/objet` - Liste tous les objets
- `GET /api/objet/{id}` - Détails d'un objet
- `POST /api/objet/{objetId}/utiliser/{personnageId}` - Utilise un objet *(utilise Objet.cs)*
- `GET /api/objet/recherche/effet/{effet}` - Recherche par effet
- `POST /api/objet` - Crée un nouvel objet

## 🔧 Configuration et démarrage

### 1. Configuration base de données
Modifiez `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=jeuvideo;Username=postgres;Password=VOTRE_PASSWORD"
  }
}
```

### 2. Démarrage de l'API
```bash
cd Scripts/Api
dotnet run
```

L'API sera disponible sur :
- **HTTP** : http://localhost:5000
- **HTTPS** : https://localhost:5001
- **Swagger** : http://localhost:5000 (documentation interactive)

## 🧪 Test de l'API

### Tester la santé de l'API
```bash
curl http://localhost:5000/health
# Réponse : "API is running!"
```

### Créer un joueur
```bash
curl -X POST http://localhost:5000/api/joueur \
  -H "Content-Type: application/json" \
  -d '{
    "pseudoJoueur": "TestPlayer",
    "nomPersonnage": "Hero",
    "typePersonnageId": 1
  }'
```

### Initier un combat
```bash
curl -X POST http://localhost:5000/api/combat/initier \
  -H "Content-Type: application/json" \
  -d '{
    "personnageId": 1,
    "botId": 1,
    "zoneId": 1
  }'
```

### Gérer un tour de combat
```bash
curl -X POST http://localhost:5000/api/combat/1/tour
```

## 🎮 Intégration avec Godot

Dans Godot, utilisez HTTPRequest :

```gdscript
# Exemple GDScript pour Godot
extends Node

func _ready():
    var http_request = HTTPRequest.new()
    add_child(http_request)
    http_request.request_completed.connect(_on_request_completed)
    
    # Récupérer les joueurs
    http_request.request("http://localhost:5000/api/joueur")

func _on_request_completed(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray):
    if response_code == 200:
        var json = JSON.new()
        var parse_result = json.parse(body.get_string_from_utf8())
        print("Joueurs récupérés: ", parse_result.data)
```

## ✅ Points forts de cette architecture

1. **✅ Injection de dépendances** - Testable et maintenable
2. **✅ Séparation des responsabilités** - Logique métier préservée
3. **✅ CORS configuré** - Compatible avec Godot
4. **✅ Swagger** - Documentation auto-générée
5. **✅ Gestion d'erreurs** - Réponses HTTP appropriées
6. **✅ Utilisation des services métier existants** - Combat.cs, Objet.cs, etc.
7. **✅ Pattern Repository** - Accès données uniforme

## 🔮 Prochaines étapes possibles

1. **Authentification JWT** pour sécuriser l'API
2. **Validation des DTOs** avec FluentValidation
3. **Tests unitaires** pour chaque contrôleur
4. **Rate limiting** pour éviter le spam
5. **Caching** avec Redis pour les performances
6. **Logging structuré** avec Serilog

Votre API est maintenant **prête pour la production** ! 🎉 