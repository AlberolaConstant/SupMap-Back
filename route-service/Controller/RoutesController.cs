using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutesService.Models;
using RoutesService.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutesService.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<Road>> CalculateRoute([FromBody] RouteRequest request)
        {
            try
            {
                // Récupérer l'ID de l'utilisateur à partir du token JWT
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized("Utilisateur non authentifié.");
                }

                // Calculer l'itinéraire avec Valhalla
                var routeData = await _routeService.GetRouteFromValhalla(
                    request.StartLatitude,
                    request.StartLongitude,
                    request.EndLatitude,
                    request.EndLongitude,
                    request.TransportMode ?? "auto",
                    request.AvoidTolls
                );

                // Créer et sauvegarder l'itinéraire
                var route = new Road
                {
                    UserId = userId,
                    StartLatitude = request.StartLatitude,
                    StartLongitude = request.StartLongitude,
                    EndLatitude = request.EndLatitude,
                    EndLongitude = request.EndLongitude,
                    TransportMode = request.TransportMode ?? "auto",
                    AvoidTolls = request.AvoidTolls,
                    CreatedAt = DateTime.Now,
                    RouteData = routeData
                };

                _routeService.SaveRoute(route);
                
                return Ok(route);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de l'itinéraire");
                return StatusCode(500, "Une erreur est survenue lors du calcul de l'itinéraire.");
            }
        }

        [HttpGet("user/{userId}/recent")]
        public ActionResult<IEnumerable<Route>> GetUserRecentRoutes(int userId, [FromQuery] int limit = 10)
        {
            try
            {
                // Vérifier que l'utilisateur peut accéder à ces données
                var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid("Vous n'êtes pas autorisé à accéder aux itinéraires de cet utilisateur.");
                }

                var routes = _routeService.GetUserRecentRoutes(userId, limit);
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des itinéraires récents");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des itinéraires récents.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Route> GetRoute(int id)
        {
            try
            {
                var route = _routeService.GetRouteById(id);
                
                // Vérifier que l'utilisateur peut accéder à ces données
                var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (route.UserId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid("Vous n'êtes pas autorisé à accéder à cet itinéraire.");
                }
                
                return Ok(route);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("L'itinéraire demandé n'a pas été trouvé.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'itinéraire");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'itinéraire.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRoute(int id)
        {
            try
            {
                var route = _routeService.GetRouteById(id);
                
                // Vérifier que l'utilisateur peut supprimer cet itinéraire
                var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (route.UserId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid("Vous n'êtes pas autorisé à supprimer cet itinéraire.");
                }
                
                var result = _routeService.DeleteRoute(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("L'itinéraire demandé n'a pas été trouvé.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("L'itinéraire demandé n'a pas été trouvé.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'itinéraire");
                return StatusCode(500, "Une erreur est survenue lors de la suppression de l'itinéraire.");
            }
        }
    }

    public class RouteRequest
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public string? TransportMode { get; set; } = "auto";
        public bool AvoidTolls { get; set; } = false;
    }
}