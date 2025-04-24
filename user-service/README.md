# Service Utilisateur (User Service)

Ce microservice gère les opérations CRUD pour les utilisateurs de l'application.

## Fonctionnalités

- Récupération des informations utilisateur
- Mise à jour des profils utilisateur
- Suppression de compte utilisateur
- Recherche d'utilisateurs
- Gestion des préférences utilisateur

## Endpoints API

### Récupérer tous les utilisateurs
```
GET /api/user
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
[
  {
    "id": "integer",
    "userName": "string",
    "email": "string",
    "role": "string"
  }
]
```

### Récupérer un utilisateur par ID
```
GET /api/user/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string"
}
```

### Récupérer le profil de l'utilisateur connecté
```
GET /api/user/profile
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string"
}
```

### Mettre à jour un utilisateur
```
PUT /api/user/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "userName": "string",
  "email": "string"
}
```

**Réponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string"
}
```

### Supprimer un utilisateur
```
DELETE /api/user/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```
204 No Content
```

### Mettre à jour les préférences utilisateur
```
PATCH /api/user/{id}/preferences
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "defaultTransportMode": "string",
  "avoidTolls": "boolean",
  "avoidHighways": "boolean",
  "distanceUnit": "string"
}
```

**Réponse**:
```json
{
  "id": "integer",
  "defaultTransportMode": "string",
  "avoidTolls": "boolean",
  "avoidHighways": "boolean",
  "distanceUnit": "string"
}
```

## Structure de la Base de Données

Table `Users`:
- `Id` (PK, SERIAL)
- `UserName` (VARCHAR(50), NOT NULL, UNIQUE)
- `Email` (VARCHAR(100), NOT NULL, UNIQUE)
- `Password` (VARCHAR(255), NOT NULL)
- `Role` (VARCHAR(20), DEFAULT 'User')
- `CreationDate` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)

Table `UserPreferences` (à implémenter):
- `UserId` (PK, FK -> Users.Id)
- `DefaultTransportMode` (VARCHAR(20), DEFAULT 'auto')
- `AvoidTolls` (BOOLEAN, DEFAULT FALSE)
- `AvoidHighways` (BOOLEAN, DEFAULT FALSE)
- `DistanceUnit` (VARCHAR(10), DEFAULT 'km')

## Technologies Utilisées

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont définies dans le fichier `.env` :
- `DB_HOST`: Hôte de la base de données
- `DB_PORT`: Port de la base de données
- `DB_NAME`: Nom de la base de données
- `DB_USER`: Nom d'utilisateur de la base de données
- `DB_PASSWORD`: Mot de passe de la base de données
- `JWT_SECRET`: Clé secrète pour la vérification des tokens JWT