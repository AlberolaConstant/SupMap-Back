# Service d'Incidents (Incident Service)

Ce microservice gère le signalement et la consultation des incidents de circulation pour l'application SupMap.

## 🚨 Fonctionnalités

- Signalement d'incidents routiers (accidents, travaux, police, obstacles, etc.)
- Recherche d'incidents à proximité d'une position
- Système de votes (upvote/downvote) pour valider les incidents
- Gestion de la durée de vie des incidents
- Nettoyage automatique des incidents expirés

## 🚦 Types d'incidents supportés

- **accident** : Accidents de circulation
- **construction** : Travaux routiers
- **police** : Contrôles policiers
- **hazard** : Obstacles ou dangers sur la route
- **closure** : Routes fermées
- **traffic_jam** : Embouteillages

## 📡 Endpoints API

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

### Mettre à jour le statut d'un incident
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

## ⏰ Gestion de la durée de vie

Les incidents ont une durée de vie automatique qui dépend de leur type :
- **accident** : Au moins 2 heures
- **construction** : Au moins 24 heures
- **police** : Au moins 1 heure
- **hazard** : Au moins 2 heures
- **closure** : Au moins 4 heures
- **traffic_jam** : Au moins 30 minutes

Un service de nettoyage automatique désactive les incidents expirés.

## 🛠️ Technologies utilisées

- **.NET 8.0** : Framework de développement
- **Entity Framework Core 8.0** : ORM pour l'accès aux données
- **PostgreSQL** : Stockage des incidents
- **JWT Bearer** : Authentification par tokens
- **Formule de Haversine** : Calcul des distances géographiques
- **Service hébergé** : Nettoyage automatique des incidents expirés
- **Swagger/OpenAPI** : Documentation d'API automatisée

## ⚙️ Configuration

Les variables d'environnement sont définies dans le fichier `.env` :

- `DB_HOST` : Hôte de la base de données
- `DB_PORT` : Port de la base de données
- `DB_NAME` : Nom de la base de données
- `DB_USER` : Nom d'utilisateur de la base de données
- `DB_PASSWORD` : Mot de passe de la base de données
- `CONNECTION_STRING` : Chaîne de connexion PostgreSQL complète
- `JWT_SECRET` : Clé secrète pour la validation des tokens JWT
- `JWT_ISSUER` : Émetteur des tokens JWT
- `JWT_AUDIENCE` : Public cible des tokens JWT

## 📊 Schéma de la base de données

Table `Incidents` :
- `Id` (SERIAL, PK)
- `UserId` (INTEGER, NOT NULL)
- `UserName` (VARCHAR(255), NOT NULL)
- `Latitude` (DOUBLE PRECISION, NOT NULL)
- `Longitude` (DOUBLE PRECISION, NOT NULL)
- `Type` (VARCHAR(50), NOT NULL)
- `Description` (TEXT, NOT NULL)
- `CreatedAt` (TIMESTAMP, NOT NULL, DEFAULT NOW())
- `ExpiresAt` (TIMESTAMP, NOT NULL)
- `IsActive` (BOOLEAN, NOT NULL, DEFAULT TRUE)

Table `IncidentVotes` :
- `Id` (SERIAL, PK)
- `IncidentId` (INTEGER, NOT NULL, FK -> Incidents.Id)
- `UserId` (INTEGER, NOT NULL)
- `Vote` (INTEGER, NOT NULL) // 1 pour upvote, -1 pour downvote
- `CreatedAt` (TIMESTAMP, NOT NULL, DEFAULT NOW())

## 🔍 Algorithme de recherche géographique

Le service utilise la formule de Haversine pour calculer la distance entre deux points géographiques. Cette formule prend en compte la courbure de la Terre pour déterminer précisément la distance au sol entre deux paires de coordonnées (latitude, longitude).

## 🔒 Sécurité

- Tous les endpoints nécessitent une authentification
- Vérification que seul le créateur d'un incident peut modifier son statut
- Limitation à un seul vote par utilisateur et par incident

## 🧪 Test de l'API

Utilisez Postman ou cURL pour tester les endpoints :

```bash
# Signaler un accident
curl -X POST http://localhost/api/incident \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"latitude":45.762089,"longitude":4.830909,"type":"accident","description":"Collision entre deux véhicules"}'

# Trouver les incidents à proximité
curl -X GET "http://localhost/api/incident/nearby?latitude=45.762089&longitude=4.830909&radius=