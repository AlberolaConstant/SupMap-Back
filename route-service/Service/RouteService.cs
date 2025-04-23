namespace RoutesService.Service
{
    using RoutesService.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RouteService : IRouteService
    {
        private readonly List<Route> _routes = new(); // Tu devrais remplacer cela par un accès à une base de données

        public async Task<string> GetRouteFromValhalla(double startLat, double startLon, double endLat, double endLon, string transportMode, bool avoidTolls)
        {
            // Logique pour interagir avec Valhalla API et obtenir les données de route
            // Tu peux utiliser HttpClient pour envoyer une requête HTTP à l'API Valhalla et obtenir les informations nécessaires.
            // Pour l'instant, je retourne une chaîne simulée.

            await Task.Delay(500); // Simuler l'appel asynchrone

            return $"Route from {startLat},{startLon} to {endLat},{endLon} by {transportMode} with avoidTolls={avoidTolls}";
        }

        public List<Route> GetUserRecentRoutes(int userId, int limit)
        {
            return _routes.Where(r => r.UserId == userId)
                          .OrderByDescending(r => r.CreatedAt)
                          .Take(limit)
                          .ToList();
        }

        public Route GetRouteById(int id)
        {
            return _routes.FirstOrDefault(r => r.Id == id);
        }

        public Route SaveRoute(Route route)
        {
            route.Id = _routes.Count + 1; // Simple logique pour générer un ID unique
            _routes.Add(route);
            return route;
        }

        public bool DeleteRoute(int id)
        {
            var route = _routes.FirstOrDefault(r => r.Id == id);
            if (route == null) return false;
            _routes.Remove(route);
            return true;
        }
    }
}
