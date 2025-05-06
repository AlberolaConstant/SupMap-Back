# Service d'Incidents (Incident Service)

Ce microservice gère le signalement et la consultation des incidents de circulation pour l'application.

## Fonctionnalités

- Signalement d'incidents de circulation (accidents, travaux, etc.)
- Consultation des incidents à proximité
- Vote et validation des incidents
- Durée de vie des incidents
- Filtrage par type d'incident

## Endpoints API

### Signaler un incident
```
POST /api/incident
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "latitude": "double",
  "longitude": "double",
  "type": "string", // accident, construction, police, hazard, closure, traffic_jam
  "description": "string",
  "expectedDuration": "integer" // en minutes, optionnel
}
```

**Réponse**:
```json
{
  "id": "integer",
  "userId": "integer",
  "userName": "string",
  "latitude": "double",
  "longitude": "double",
  "type": "string",
  "description": "string",
  "createdAt": "datetime",
  "expiresAt": "datetime",
  "upvotes": 0,
  "downvotes": 0,
  "isActive": true
}
```

### Récupérer les incidents à proximité
```
GET /api/incident/nearby
```

**Headers**:
```
Authorization: Bearer {token}
```

**Paramètres de requête**:
```
latitude: double
longitude: double
radius: double (en km, défaut: 5)
types: string (séparés par des virgules, optionnel)
```

**Réponse**:
```json
[
  {
    "id": "integer",
    "userId": "integer",
    "userName": "string",
    "latitude": "double",
    "longitude": "double",
    "type": "string",
    "description": "string",
    "createdAt": "datetime",
    "expiresAt": "datetime",
    "upvotes": "integer",
    "downvotes": "integer",
    "distance": "double", // distance par rapport à la position demandée (en km)
    "isActive": "boolean"
  }
]
```

### Voter pour un incident
```
POST /api/incident/{id}/vote
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "vote": "integer" // 1 pour upvote, -1 pour downvote
}
```

**Réponse**:
```json
{
  "id": "integer",
  "upvotes": "integer",
  "downvotes": "integer"
}
```

### Désactiver un incident
```
PUT /api/incident/{id}/status
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "isActive": "boolean"
}
```

**Réponse**:
```json
{
  "id": "integer",
  "isActive": "boolean"
}
```

### Récupérer un incident par ID
```
GET /api/incident/{id}
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
  "userName": "string",
  "latitude": "double",
  "longitude": "double",
  "type": "string",
  "description": "string",
  "createdAt": "datetime",
  "expiresAt": "datetime",
  "upvotes": "integer",
  "downvotes": "integer",
  "isActive": "boolean"
}
```

## Structure de la Base de Données

Table `Incidents`:
- `Id` (SERIAL, PK)
- `UserId` (INTEGER, NOT NULL)
- `UserName` (VARCHAR(50), NOT NULL)
- `Latitude` (DOUBLE PRECISION, NOT NULL)
- `Longitude` (DOUBLE PRECISION, NOT NULL)
- `Type` (VARCHAR(20), NOT NULL) // accident, construction, police, hazard, closure, traffic_jam
- `Description` (TEXT, NOT NULL)
- `CreatedAt` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)
- `ExpiresAt` (TIMESTAMP, NOT NULL)
- `IsActive` (BOOLEAN, DEFAULT TRUE)

Table `IncidentVotes`:
- `Id` (SERIAL, PK)
- `IncidentId` (INTEGER, NOT NULL, FK -> Incidents.Id)
- `UserId` (INTEGER, NOT NULL)
- `Vote` (INTEGER, NOT NULL) // 1 pour upvote, -1 pour downvote
- `CreatedAt` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)

## Technologies Utilisées

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- NetTopologySuite pour les calculs géographiques
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont définies dans le fichier `.env` :
- `DB_HOST`: Hôte de la base de données
- `DB_PORT`: Port de la base de données
- `DB_NAME`: Nom de la base de données
- `DB_USER`: Nom d'utilisateur de la base de données
- `DB_PASSWORD`: Mot de passe de la base de données
- `JWT_SECRET`: Clé secrète pour la vérification des tokens JWT
- `USER_SERVICE_URL`: URL du service utilisateur
