using RoutesService.Models;
using RoutesService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RoutesService.Service
{
    public class RouteService : IRouteService
    {
        private readonly RoutesDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RouteService> _logger;

        public RouteService(RoutesDbContext context, HttpClient httpClient, ILogger<RouteService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetRouteFromValhalla(double startLat, double startLon, double endLat, double endLon, string transportMode, bool avoidTolls)
        {
            try
            {
                _logger.LogDebug("Calculating route from ({0}, {1}) to ({2}, {3}) with mode: {4}, avoidTolls: {5}", 
                    startLat, startLon, endLat, endLon, transportMode, avoidTolls);

                // Exemple d'une requête vers un service Valhalla (à adapter selon votre implémentation)
                var request = new
                {
                    locations = new[]
                    {
                        new { lat = startLat, lon = startLon },
                        new { lat = endLat, lon = endLon }
                    },
                    costing = transportMode,
                    costing_options = new
                    {
                        auto = new
                        {
                            use_tolls = !avoidTolls ? 1.0 : 0.0
                        }
                    }
                };

                // Dans un environnement réel, vous auriez un service Valhalla configuré
                // Pour ce projet de démonstration, nous allons simuler une réponse
                
                // Simulation d'une réponse (à remplacer par un appel réel à Valhalla)
                var mockResponse = new
                {
                    trip = new
                    {
                        locations = new[]
                        {
                            new { lat = startLat, lon = startLon },
                            new { lat = endLat, lon = endLon }
                        },
                        legs = new[]
                        {
                            new
                            {
                                distance = 10.5, // en km
                                time = 600, // en secondes
                                shape = "simulated_polyline_data" // Données polyline encodées
                            }
                        },
                        summary = new
                        {
                            distance = 10.5,
                            time = 600
                        }
                    }
                };

                return JsonSerializer.Serialize(mockResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de l'itinéraire");
                throw new Exception("Impossible de calculer l'itinéraire demandé. Veuillez réessayer plus tard.", ex);
            }
        }

        public List<Route> GetUserRecentRoutes(int userId, int limit)
        {
            _logger.LogDebug("Récupération des {0} itinéraires récents pour l'utilisateur {1}", limit, userId);
            return _context.Routes
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(limit)
                .ToList();
        }

        public Route GetRouteById(int id)
        {
            _logger.LogDebug("Récupération de l'itinéraire avec l'ID {0}", id);
            var route = _context.Routes.Find(id);
            if (route == null)
            {
                throw new KeyNotFoundException("L'itinéraire demandé n'a pas été trouvé.");
            }
            return route;
        }

        public Route SaveRoute(Route route)
        {
            _logger.LogDebug("Sauvegarde d'un nouvel itinéraire pour l'utilisateur {0}", route.UserId);
            _context.Routes.Add(route);
            _context.SaveChanges();
            return route;
        }

        public bool DeleteRoute(int id)
        {
            _logger.LogDebug("Suppression de l'itinéraire avec l'ID {0}", id);
            var route = _context.Routes.Find(id);
            if (route == null)
            {
                return false;
            }
            
            _context.Routes.Remove(route);
            _context.SaveChanges();
            return true;
        }
    }
}