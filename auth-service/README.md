# Service d'Authentification (Auth Service)

Ce microservice gère l'authentification des utilisateurs pour l'application.

## Fonctionnalités

- Inscription utilisateur
- Connexion utilisateur
- Vérification de token JWT
- Gestion des rôles utilisateur

## Endpoints API

### Inscription Utilisateur
```
POST /api/auth/register
```

**Corps de la requête**:
```json
{
  "userName": "string",
  "email": "string",
  "password": "string",
  "role": "string" // Optionnel, par défaut "User"
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

### Connexion Utilisateur
```
POST /api/auth/login
```

**Corps de la requête**:
```json
{
  "email": "string",
  "password": "string"
}
```

**Réponse**:
```json
{
  "token": "string",
  "user": {
    "id": "integer",
    "userName": "string",
    "email": "string",
    "role": "string"
  }
}
```

### Vérification de Token
```
GET /api/auth/verify
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
{
  "isValid": "boolean",
  "userData": {
    "id": "integer",
    "userName": "string",
    "email": "string",
    "role": "string"
  }
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

## Technologies Utilisées

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- BCrypt pour le hachage des mots de passe
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont définies dans le fichier `.env` :
- `DB_HOST`: Hôte de la base de données
- `DB_PORT`: Port de la base de données
- `DB_NAME`: Nom de la base de données
- `DB_USER`: Nom d'utilisateur de la base de données
- `DB_PASSWORD`: Mot de passe de la base de données
- `JWT_SECRET`: Clé secrète pour la génération des tokens JWT
- `Jwt_Issuer`: Émetteur des tokens JWT
- `Jwt_Audience`: Public cible des tokens JWT