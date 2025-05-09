# 🚀 Lancement du projet

## 🔧 Prérequis

- [Docker](https://www.docker.com/)
- [Postman](https://www.postman.com/) (recommandé pour tester les routes)

## ▶️ Lancement

Pour démarrer le projet, exécute simplement la commande suivante à la racine du projet :

```bash
docker compose up -d
```

Cela va démarrer tous les services nécessaires en arrière-plan (Docker).

## 📚 Accès aux documentations Swagger

Une fois les conteneurs lancés, les documentations Swagger sont accessibles via les URLs suivantes :

- Auth API : [http://localhost/api/auth/swagger/index.html](http://localhost/api/auth/swagger/index.html)
- User API : [http://localhost/api/user/swagger/index.html](http://localhost/api/user/swagger/index.html)
- Route API : [http://localhost/api/route/swagger/index.html](http://localhost/api/route/swagger/index.html)
- Incident API : [http://localhost/api/incident/swagger/index.html](http://localhost/api/incident/swagger/index.html)

⚠️ **Attention :** Swagger n'est pas configuré pour fonctionner avec Traefik. Il est donc recommandé d'utiliser **Postman** pour tester les routes de l'API.

---
