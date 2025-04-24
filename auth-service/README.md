# Service d'Authentification (Auth Service)

Ce microservice g�re l'authentification des utilisateurs pour l'application.

## Fonctionnalit�s

- Inscription utilisateur
- Connexion utilisateur
- V�rification de token JWT
- Gestion des r�les utilisateur

## Endpoints API

### Inscription Utilisateur
```
POST /api/auth/register
```

**Corps de la requ�te**:
```json
{
  "userName": "string",
  "email": "string",
  "password": "string",
  "role": "string" // Optionnel, par d�faut "User"
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

### Connexion Utilisateur
```
POST /api/auth/login
```

**Corps de la requ�te**:
```json
{
  "email": "string",
  "password": "string"
}
```

**R�ponse**:
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

### V�rification de Token
```
GET /api/auth/verify
```

**Headers**:
```
Authorization: Bearer {token}
```

**R�ponse**:
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

## Structure de la Base de Donn�es

Table `Users`:
- `Id` (PK, SERIAL)
- `UserName` (VARCHAR(50), NOT NULL, UNIQUE)
- `Email` (VARCHAR(100), NOT NULL, UNIQUE)
- `Password` (VARCHAR(255), NOT NULL)
- `Role` (VARCHAR(20), DEFAULT 'User')
- `CreationDate` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)

## Technologies Utilis�es

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- JWT Authentication
- BCrypt pour le hachage des mots de passe
- Swagger pour la documentation API

## Configuration

Les variables d'environnement sont d�finies dans le fichier `.env` :
- `DB_HOST`: H�te de la base de donn�es
- `DB_PORT`: Port de la base de donn�es
- `DB_NAME`: Nom de la base de donn�es
- `DB_USER`: Nom d'utilisateur de la base de donn�es
- `DB_PASSWORD`: Mot de passe de la base de donn�es
- `JWT_SECRET`: Cl� secr�te pour la g�n�ration des tokens JWT
- `Jwt_Issuer`: �metteur des tokens JWT
- `Jwt_Audience`: Public cible des tokens JWT