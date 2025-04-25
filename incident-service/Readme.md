# Service d'Incidents (Incidents Service)

Ce microservice g�re le signalement et la consultation des incidents de circulation pour l'application.

## Fonctionnalit�s

- Signalement d'incidents de circulation (accidents, travaux, etc.)
- Consultation des incidents � proximit�
- Vote et validation des incidents
- Dur�e de vie des incidents
- Filtrage par type d'incident

## Endpoints API

### Signaler un incident
```
POST /api/incidents
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requ�te**:
```json
{
  "latitude": "double",
  "longitude": "double",
  "type": "string", // accident, construction, police, hazard, closure, traffic_jam
  "description": "string",
  "expectedDuration": "integer" // en minutes, optionnel
}
```

**R�ponse**:
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

### R�cup�rer les incidents � proximit�
```
GET /api/incidents/nearby
```

**Headers**:
```
Authorization: Bearer {token}
```

**Param�tres de requ�te**:
```
latitude: double
longitude: double
radius: double (en km, d�faut: 5)
types: string (s�par�s par des virgules, optionnel)
```

**R�ponse**:
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
    "distance": "double", // distance par rapport � la position demand�e (en km)
    "isActive": "boolean"
  }
]
```

### Voter pour un incident
```
POST /api/incidents/{id}/vote
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requ�te**:
```json
{
  "vote": "integer" // 1 pour upvote, -1 pour downvote
}
```

**R�ponse**:
```json
{
  "id": "integer",
  "upvotes": "integer",
  "downvotes": "integer"
}
```

### D�sactiver un incident
```
PUT /api/incidents/{id}/status
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requ�te**:
```json
{
  "isActive": "boolean"
}
```

**R�ponse**:
```json
{
  "id": "integer",
  "isActive": "boolean"
}
```

### R�cup�rer un incident par ID
```
GET /api/incidents/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**R�ponse**:
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

## Structure de la Base de Donn�es

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

## Technologies Utilis�es

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- NetTopologySuite pour les calculs g�ographiques
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont d�finies dans le fichier `.env` :
- `DB_HOST`: H�te de la base de donn�es
- `DB_PORT`: Port de la base de donn�es
- `DB_NAME`: Nom de la base de donn�es
- `DB_USER`: Nom d'utilisateur de la base de donn�es
- `DB_PASSWORD`: Mot de passe de la base de donn�es
- `JWT_SECRET`: Cl� secr�te pour la v�rification des tokens JWT
- `USER_SERVICE_URL`: URL du service utilisateur