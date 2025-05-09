# SupMap - Microservices pour applications de navigation

SupMap est une application de navigation avancée composée de plusieurs microservices interconnectés qui fournissent des fonctionnalités de gestion d'utilisateurs, d'authentification, de calcul d'itinéraires et de signalement d'incidents routiers.

## 🌟 Fonctionnalités principales

- **Gestion des utilisateurs** : Inscription, connexion, et gestion de profils
- **Calcul d'itinéraires** : Génération d'itinéraires optimisés avec options de transport
- **Signalement d'incidents** : Signalement et consultation d'incidents routiers en temps réel
- **Système de votes** : Validation communautaire des incidents signalés

## 🏗️ Architecture

L'application est construite sur une architecture de microservices avec les composants suivants :

- **Auth Service** : Service d'authentification et gestion des JWT
- **User Service** : Gestion des informations utilisateurs
- **Route Service** : Calcul et sauvegarde des itinéraires
- **Incident Service** : Gestion des incidents routiers
- **Valhalla** : Moteur de calcul d'itinéraires
- **Traefik** : Proxy inverse et équilibreur de charge
- **PostgreSQL** : Bases de données dédiées pour chaque service

## 🔧 Prérequis

- [Docker](https://www.docker.com/) et Docker Compose
- [Postman](https://www.postman.com/) (recommandé pour tester les APIs)

## 🚀 Démarrage rapide

1. Clonez ce dépôt :
   ```bash
   git clone https://github.com/votre-utilisateur/supmap.git
   cd supmap
   ```

2. Lancez l'ensemble des services avec Docker Compose :
   ```bash
   docker compose up -d
   ```

3. L'application est maintenant accessible sur http://localhost

## 📡 Endpoints API

Les API sont accessibles via les URL suivantes :

- **Auth API** : `http://localhost/api/auth`
- **User API** : `http://localhost/api/user`
- **Route API** : `http://localhost/api/route`
- **Incident API** : `http://localhost/api/incident`
- **Navigation API** (Valhalla) : `http://localhost/api/navigation`

## 📚 Documentation Swagger

Les documentations Swagger sont disponibles pour chaque service :

- Auth API : [http://localhost/api/auth/swagger](http://localhost/api/auth/swagger)
- User API : [http://localhost/api/user/swagger](http://localhost/api/user/swagger)
- Route API : [http://localhost/api/route/swagger](http://localhost/api/route/swagger)
- Incident API : [http://localhost/api/incident/swagger](http://localhost/api/incident/swagger)

## 🔐 Variables d'environnement

Le projet utilise plusieurs variables d'environnement définies dans les fichiers `.env` :

- Bases de données : Paramètres de connexion PostgreSQL
- JWT : Secret, émetteur et audience pour les tokens
- Autres configurations spécifiques aux services

## 🔄 Flux d'utilisation typique

1. Créer un compte utilisateur via l'Auth Service
2. S'authentifier pour obtenir un token JWT
3. Utiliser le Route Service pour calculer un itinéraire
4. Consulter les incidents à proximité via l'Incident Service
5. Signaler un nouvel incident ou voter sur des incidents existants

## 📂 Structure du projet

- `/auth-service` : Service d'authentification
- `/user-service` : Service de gestion des utilisateurs
- `/route-service` : Service de calcul d'itinéraires
- `/incident-service` : Service de gestion des incidents
- `/init-scripts` : Scripts d'initialisation des bases de données
- `/custom_files` : Fichiers de configuration personnalisés
- `docker-compose.yml` : Orchestration des conteneurs

## 📖 Détails des services

Chaque service dispose de son propre README détaillé :

- [Auth Service](./auth-service/README.md)
- [User Service](./user-service/README.md)
- [Route Service](./route-service/README.md)
- [Incident Service](./incident-service/README.md)

## 🤝 Contribution

1. Forkez le projet
2. Créez votre branche de fonctionnalité (`git checkout -b feature/amazing-feature`)
3. Commitez vos changements (`git commit -m 'Add some amazing feature'`)
4. Poussez vers la branche (`git push origin feature/amazing-feature`)
5. Ouvrez une Pull Request

## 📝 Licence

Ce projet est sous licence [MIT](LICENSE).