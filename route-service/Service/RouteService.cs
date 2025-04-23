namespace RoutesService.Service
{
    using RoutesService.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RouteService : IRouteService
    {
        private readonly List<Route> _routes = new(); // Tu devrais remplacer cela par un acc�s � une base de donn�es

        public async Task<string> GetRouteFromValhalla(double startLat, double startLon, double endLat, double endLon, string transportMode, bool avoidTolls)
        {
            // Logique pour interagir avec Valhalla API et obtenir les donn�es de route
            // Tu peux utiliser HttpClient pour envoyer une requ�te HTTP � l'API Valhalla et obtenir les informations n�cessaires.
            // Pour l'instant, je retourne une cha�ne simul�e.

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
            route.Id = _routes.Count + 1; // Simple logique pour g�n�rer un ID unique
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
