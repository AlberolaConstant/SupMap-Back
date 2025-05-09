# Service d'Authentification (Auth Service)

Ce microservice gère l'authentification et l'autorisation des utilisateurs pour l'application SupMap.

## ✨ Fonctionnalités

- Inscription des utilisateurs
- Authentification par email/mot de passe
- Génération et validation de tokens JWT
- Gestion des rôles utilisateurs (User/Admin)
- Stockage sécurisé des mots de passe avec BCrypt

## 🔑 Endpoints API

### Inscription d'un nouvel utilisateur
```
POST /api/auth/register
```

**Corps de la requête**:
```json
{
  "userName": "string",
  "email": "string",
  "password": "string",
  "role": "string" // Optionnel, "User" par défaut
}
```

**Réponse**:
```json
{
  "id": "integer",
  "userName": "string",
  "email": "string",
  "role": "string",
  "creationDate": "datetime"
}
```

### Connexion
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
  "userId": "integer",
  "userName": "string",
  "email": "string",
  "role": "string",
  "expiresIn": "integer"
}
```

### Vérification de token
```
POST /api/auth/verify
```

**Headers**:
```
Authorization: Bearer {token}
```

**Réponse**:
```json
{
  "isValid": "boolean",
  "userId": "integer",
  "userName": "string",
  "role": "string"
}
```

## 🛠️ Technologies utilisées

- **.NET 8.0** : Framework de développement
- **Entity Framework Core 8.0** : ORM pour l'accès aux données
- **PostgreSQL** : Stockage des données utilisateurs
- **JWT Bearer** : Authentification par tokens
- **BCrypt.Net-Next** : Hachage et vérification des mots de passe
- **Swagger/OpenAPI** : Documentation d'API automatisée

## ⚙️ Configuration

Les variables d'environnement sont définies dans le fichier `.env` :

- `DB_HOST` : Hôte de la base de données
- `DB_PORT` : Port de la base de données
- `DB_NAME` : Nom de la base de données
- `DB_USER` : Nom d'utilisateur de la base de données
- `DB_PASSWORD` : Mot de passe de la base de données
- `JWT_SECRET` : Clé secrète pour la signature des tokens JWT
- `JWT_ISSUER` : Émetteur des tokens JWT
- `JWT_AUDIENCE` : Public cible des tokens JWT

## 📊 Schéma de la base de données

Table `Users` :
- `Id` (SERIAL, PK)
- `UserName` (VARCHAR(50), UNIQUE)
- `Email` (VARCHAR(100), UNIQUE)
- `Password` (VARCHAR(255))
- `Role` (VARCHAR(20), DEFAULT 'User')
- `CreationDate` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)

## 🔄 Flux d'authentification

1. L'utilisateur s'inscrit via le endpoint `/register`
2. L'utilisateur se connecte via le endpoint `/login` et reçoit un token JWT
3. Pour les requêtes nécessitant une authentification, le token JWT est envoyé dans l'en-tête `Authorization`
4. Les autres services utilisent le endpoint `/verify` pour valider les tokens

## 🔒 Sécurité

- Les mots de passe sont hachés avec BCrypt avant stockage
- Les tokens JWT intègrent les claims standard plus des claims personnalisés (`userId`, `role`)
- Les tokens ont une durée de validité limitée
- Validation complète des requêtes avec vérification des modèles (models)

## 🧪 Test de l'API

Utilisez Postman ou cURL pour tester les endpoints :

```bash
# Inscription
curl -X POST http://localhost/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"userName":"test","email":"test@example.com","password":"password123"}'

# Connexion
curl -X POST http://localhost/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}'
```