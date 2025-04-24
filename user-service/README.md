# Service Utilisateur (User Service)

Ce microservice g�re les op�rations CRUD pour les utilisateurs de l'application.

## Fonctionnalit�s

- R�cup�ration des informations utilisateur
- Mise � jour des profils utilisateur
- Suppression de compte utilisateur
- Recherche d'utilisateurs
- Gestion des pr�f�rences utilisateur

## Endpoints API

### R�cup�rer tous les utilisateurs
```
GET /api/user
```

**Headers**:
```
Authorization: Bearer {token}
```

**R�ponse**:
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

### R�cup�rer un utilisateur par ID
```
GET /api/user/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**R�ponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string"
}
```

### R�cup�rer le profil de l'utilisateur connect�
```
GET /api/user/profile
```

**Headers**:
```
Authorization: Bearer {token}
```

**R�ponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string"
}
```

### Mettre � jour un utilisateur
```
PUT /api/user/{id}
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requ�te**:
```json
{
  "userName": "string",
  "email": "string"
}
```

**R�ponse**:
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

**R�ponse**:
```
204 No Content
```

### Mettre � jour les pr�f�rences utilisateur
```
PATCH /api/user/{id}/preferences
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requ�te**:
```json
{
  "defaultTransportMode": "string",
  "avoidTolls": "boolean",
  "avoidHighways": "boolean",
  "distanceUnit": "string"
}
```

**R�ponse**:
```json
{
  "id": "integer",
  "defaultTransportMode": "string",
  "avoidTolls": "boolean",
  "avoidHighways": "boolean",
  "distanceUnit": "string"
}
```

## Structure de la Base de Donn�es

Table `Users`:
- `Id` (PK, SERIAL)
- `UserName` (VARCHAR(50), NOT NULL, UNIQUE)
- `Email` (VARCHAR(100), NOT NULL, UNIQUE)
- `Password` (VARCHAR(255), NOT NULL)
- `Role` (VARCHAR(20), DEFAULT 'User')
- `CreationDate` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)

Table `UserPreferences` (� impl�menter):
- `UserId` (PK, FK -> Users.Id)
- `DefaultTransportMode` (VARCHAR(20), DEFAULT 'auto')
- `AvoidTolls` (BOOLEAN, DEFAULT FALSE)
- `AvoidHighways` (BOOLEAN, DEFAULT FALSE)
- `DistanceUnit` (VARCHAR(10), DEFAULT 'km')

## Technologies Utilis�es

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont d�finies dans le fichier `.env` :
- `DB_HOST`: H�te de la base de donn�es
- `DB_PORT`: Port de la base de donn�es
- `DB_NAME`: Nom de la base de donn�es
- `DB_USER`: Nom d'utilisateur de la base de donn�es
- `DB_PASSWORD`: Mot de passe de la base de donn�es
- `JWT_SECRET`: Cl� secr�te pour la v�rification des tokens JWT