# Service Utilisateur (User Service)

Ce microservice gère les profils utilisateurs et leurs informations personnelles pour l'application SupMap.

## ✨ Fonctionnalités

- Récupération des informations utilisateur
- Mise à jour du profil utilisateur
- Suppression de compte
- Autorisation basée sur les tokens JWT
- Gestion des rôles utilisateur

## 👤 Endpoints API

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
    "role": "string",
    "creationDate": "datetime"
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
  "userName": "string"
}
```

### Récupérer le profil de l'utilisateur connecté
```
GET /api/user/me
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
  "role": "string",
  "creationDate": "datetime"
}
```

### Créer un nouvel utilisateur
```
POST /api/user/create
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "userName": "string",
  "email": "string",
  "password": "string",
  "role": "string"
}
```

### Mettre à jour le profil de l'utilisateur connecté
```
PUT /api/user/update/me
```

**Headers**:
```
Authorization: Bearer {token}
```

**Corps de la requête**:
```json
{
  "userName": "string", // optionnel
  "email": "string", // optionnel
  "password": "string" // optionnel
}
```

### Supprimer le compte de l'utilisateur connecté
```
DELETE /api/user/delete/me
```

**Headers**:
```
Authorization: Bearer {token}
```

## 🛠️ Technologies utilisées

- **.NET 8.0** : Framework de développement
- **Entity Framework Core 8.0** : ORM pour l'accès aux données
- **PostgreSQL** : Stockage des données utilisateurs
- **JWT Bearer** : Authentification basée sur les tokens
- **BCrypt.Net-Next** : Gestion sécurisée des mots de passe
- **Swagger/OpenAPI** : Documentation d'API automatisée

## ⚙️ Configuration

Les variables d'environnement sont définies dans le fichier `.env` :

- `DB_HOST` : Hôte de la base de données
- `DB_PORT` : Port de la base de données
- `DB_NAME` : Nom de la base de données
- `DB_USER` : Nom d'utilisateur de la base de données
- `DB_PASSWORD` : Mot de passe de la base de données
- `JWT_SECRET` : Clé secrète pour la validation des tokens JWT
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

## 🔒 Sécurité

- Tous les endpoints nécessitent une authentification
- Vérification de la validité du token JWT
- Vérification de l'identité de l'utilisateur pour les opérations sensibles
- Hachage des mots de passe avec BCrypt lors de leur mise à jour

## 🧪 Test de l'API

Utilisez Postman ou cURL pour tester les endpoints :

```bash
# Récupérer le profil de l'utilisateur connecté
curl -X GET http://localhost/api/user/me \
  -H "Authorization: Bearer {token}"

# Mettre à jour le profil
curl -X PUT http://localhost/api/user/update/me \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"userName":"nouveau_nom"}'
```

## 🔌 Intégration avec d'autres services

- **Auth Service** : Fournit l'authentification et les tokens JWT
- **Route Service** : Utilise l'identité de l'utilisateur pour les itinéraires
- **Incident Service** : Associe les incidents aux utilisateurs