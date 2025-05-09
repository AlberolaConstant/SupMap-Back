# Service d'Itinéraires (Route Service)

Ce microservice gère le calcul et le stockage des itinéraires pour l'application SupMap.

## 🚗 Fonctionnalités

- Calcul d'itinéraires entre deux points géographiques
- Personnalisation du mode de transport (auto, bicycle, pedestrian)
- Option pour éviter les péages
- Stockage des itinéraires récents des utilisateurs
- Récupération de l'historique des itinéraires

## 🛣️ Endpoints API

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

## 🛠️ Technologies utilisées

- **.NET 8.0** : Framework de développement
- **Entity Framework Core 8.0** : ORM pour l'accès aux données
- **PostgreSQL** : Stockage des itinéraires
- **JWT Bearer** : Authentification par tokens
- **Valhalla** : Moteur de calcul d'itinéraires
- **Swagger/OpenAPI** : Documentation d'API automatisée

## 🗺️ Intégration avec Valhalla

Le service utilise Valhalla comme moteur de calcul d'itinéraires. Valhalla est un calculateur d'itinéraires open-source qui prend en charge différents modes de transport et options comme :

- Calcul d'itinéraires optimisés pour différents modes de transport
- Évitement des péages
- Considération du trafic (simulée dans cette implémentation)
- Retour de chemins polyline encodés pour l'affichage sur une carte

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

Table `Routes` :
- `Id` (SERIAL, PK)
- `UserId` (INTEGER, NOT NULL)
- `StartLatitude` (DOUBLE PRECISION, NOT NULL)
- `StartLongitude` (DOUBLE PRECISION, NOT NULL)
- `EndLatitude` (DOUBLE PRECISION, NOT NULL)
- `EndLongitude` (DOUBLE PRECISION, NOT NULL)
- `TransportMode` (VARCHAR(20), DEFAULT 'auto')
- `AvoidTolls` (BOOLEAN, DEFAULT FALSE)
- `CreatedAt` (TIMESTAMP, DEFAULT CURRENT_TIMESTAMP)
- `RouteData` (TEXT, DEFAULT '')

## 🔒 Sécurité

- Tous les endpoints nécessitent une authentification
- Validation des autorisations utilisateur pour accéder aux itinéraires
- Vérification que les utilisateurs ne peuvent accéder qu'à leurs propres itinéraires (sauf pour les administrateurs)

## 🧪 Test de l'API

Utilisez Postman ou cURL pour tester les endpoints :

```bash
# Calculer un itinéraire
curl -X POST http://localhost/api/route/calculate \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"startLatitude":45.764043,"startLongitude":4.835659,"endLatitude":45.757814,"endLongitude":4.832011,"transportMode":"auto","avoidTolls":false}'

# Récupérer les itinéraires récents
curl -X GET http://localhost/api/route/user/1/recent?limit=5 \
  -H "Authorization: Bearer {token}"
```

## 🔌 Intégration avec d'autres services

- **Auth Service** : Validation des tokens JWT et des autorisations
- **User Service** : Récupération des informations utilisateur
- **Incident Service** : Peut être intégré pour afficher les incidents sur les itinéraires