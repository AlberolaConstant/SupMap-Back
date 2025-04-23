namespace RouteService.Service
{
    using RouteService.Model; 
    using Microsoft.Extensions.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    public class RouteManager : IRouteService
    {
        private readonly List<Route> _routes = new();
        private int _nextId = 1;
        private readonly HttpClient _httpClient;
        private readonly string _valhallaApiUrl;

        public RouteManager(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _valhallaApiUrl = configuration["ValhallaApi:Url"] ?? "http://localhost:8002/route";
        }

        public List<Route> GetUserRecentRoutes(int userId, int limit = 5)
        {
            return _routes
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(limit)
                .ToList();
        }

        public Route? GetRouteById(int id)
        {
            return _routes.FirstOrDefault(r => r.Id == id);
        }

        public Route SaveRoute(Route route)
        {
            route.Id = _nextId++;
            route.CreatedAt = DateTime.UtcNow;
            _routes.Add(route);

            // Maintenir seulement les 5 itinéraires les plus récents par utilisateur
            var userRoutes = _routes.Where(r => r.UserId == route.UserId).ToList();
            if (userRoutes.Count > 5)
            {
                var oldestRoutes = userRoutes
                    .OrderByDescending(r => r.CreatedAt)
                    .Skip(5)
                    .ToList();

                foreach (var oldRoute in oldestRoutes)
                {
                    _routes.Remove(oldRoute);
                }
            }

            return route;
        }

        public bool DeleteRoute(int id)
        {
            var route = _routes.FirstOrDefault(r => r.Id == id);
            if (route == null)
                return false;

            _routes.Remove(route);
            return true;
        }

        public async Task<string> GetRouteFromValhalla(
            double startLat, double startLon,
            double endLat, double endLon,
            string transportMode = "auto",
            bool avoidTolls = false)
        {
            // Construire la requête pour Valhalla
            var requestObj = new
            {
                locations = new[]
                {
                    new { lat = startLat, lon = startLon },
                    new { lat = endLat, lon = endLon }
                },
                costing = transportMode,
                costing_options = new Dictionary<string, object>()
            };

            // Ajouter l'option pour éviter les péages si nécessaire
            if (avoidTolls && transportMode == "auto")
            {
                requestObj.costing_options.Add("auto", new { avoid = new[] { "toll" } });
            }
            else
            {
                // Ajouter une option vide pour respecter le format requis
                requestObj.costing_options.Add(transportMode, new { });
            }

            var jsonContent = JsonSerializer.Serialize(requestObj);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_valhallaApiUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}