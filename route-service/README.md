# Service d'Itinéraires (Route Service)

Ce microservice gère le calcul et le stockage des itinéraires pour l'application.

## Fonctionnalités

- Calcul d'itinéraires entre deux points
- Stockage des itinéraires récents des utilisateurs
- Récupération des itinéraires par utilisateur
- Gestion des préférences d'itinéraire (mode de transport, éviter les péages, etc.)

## Endpoints API

### Calculer un itinéraire
```
POST /api/route/calculate
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "startLatitude": "double",
  "startLongitude": "double",
  "endLatitude": "double",
  "endLongitude": "double",
  "transportMode": "string", // auto, bicycle, pedestrian
  "avoidTolls": "boolean"
}
```

**Réponse**:
```json
{
  "id": "integer",
  "userId": "integer",
  "startLatitude": "double",
  "startLongitude": "double",
  "endLatitude": "double",
  "endLongitude": "double",
  "transportMode": "string",
  "avoidTolls": "boolean",
  "createdAt": "datetime",
  "routeData": "string" // JSON contenant les données d'itinéraire
}
```

### Récupérer les itinéraires récents de l'utilisateur
```
GET /api/route/user/{userId}/recent
```

**Headers**:
```
Authorization: Bearer {token}
```

**Paramètres de requête**:
```
limit: integer (défaut: 10)
```

**Réponse**:
```json
[
  {
    "id": "integer",
    "userId": "integer",
    "startLatitude": "double",
    "startLongitude": "double",
    "endLatitude": "double",
    "endLongitude": "double",
    "transportMode": "string",
    "avoidTolls": "boolean",
    "createdAt": "datetime"
  }
]
```

### Récupérer un itinéraire par ID
```
GET /api/route/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
{
  "id": "integer",
  "userId": "integer",
  "startLatitude": "double",
  "startLongitude": "double",
  "endLatitude": "double",
  "endLongitude": "double",
  "transportMode": "string",
  "avoidTolls": "boolean",
  "createdAt": "datetime",
  "routeData": "string" // JSON contenant les données d'itinéraire
}
```

### Supprimer un itinéraire
```
DELETE /api/route/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```
204 No Content
```

## Structure de la Base de Données

Table `Routes`:
- `Id` (PK, SERIAL)
- `UserId` (INTEGER, NOT NULL)
- `StartLatitude` (DOUBLE PRECISION, NOT NULL)
- `StartLongitude` (DOUBLE PRECISION, NOT NULL)
- `EndLatitude` (DOUBLE PRECISION, NOT NULL)
- `EndLongitude` (DOUBLE PRECISION, NOT NULL)
- `TransportMode` (VARCHAR(20), DEFAULT 'auto')
- `AvoidTolls` (BOOLEAN, DEFAULT FALSE)
- `CreatedAt` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)
- `RouteData` (TEXT, DEFAULT '')

## Technologies Utilisées

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication pour la sécurité des API
- Intégration avec Valhalla pour le calcul d'itinéraires
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont définies dans le fichier `.env` :
- `DB_HOST`: Hôte de la base de données
- `DB_PORT`: Port de la base de données
- `DB_NAME`: Nom de la base de données
- `DB_USER`: Nom d'utilisateur de la base de données
- `DB_PASSWORD`: Mot de passe de la base de données
- `JWT_SECRET`: Clé secrète pour la vérification des tokens JWT