namespace RouteService.Controller
{
    using RouteService.Model;
    using RouteService.Service;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<Route>> GetUserRecentRoutes(int userId, [FromQuery] int limit = 5)
        {
            return _routeService.GetUserRecentRoutes(userId, limit);
        }

        [HttpGet("{id}")]
        public ActionResult<Route> GetRouteById(int id)
        {
            var route = _routeService.GetRouteById(id);
            if (route == null)
                return NotFound();

            return route;
        }

        [HttpPost]
        public ActionResult<Route> SaveRoute(Route route)
        {
            // Validation des données
            if (!IsValidTransportMode(route.TransportMode))
            {
                return BadRequest("Mode de transport invalide. Les modes valides sont : 'auto', 'bicycle', 'pedestrian', 'motor_scooter', 'bus', 'motorcycle'");
            }

            var savedRoute = _routeService.SaveRoute(route);
            return CreatedAtAction(nameof(GetRouteById), new { id = savedRoute.Id }, savedRoute);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoute(int id)
        {
            var result = _routeService.DeleteRoute(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("calculate")]
        public async Task<ActionResult<string>> CalculateRoute(
            [FromQuery] double startLat,
            [FromQuery] double startLon,
            [FromQuery] double endLat,
            [FromQuery] double endLon,
            [FromQuery] string transportMode = "auto",
            [FromQuery] bool avoidTolls = false)
        {
            if (!IsValidTransportMode(transportMode))
            {
                return BadRequest("Mode de transport invalide. Les modes valides sont : 'auto', 'bicycle', 'pedestrian', 'motor_scooter', 'bus', 'motorcycle'");
            }

            try
            {
                var routeData = await _routeService.GetRouteFromValhalla(
                    startLat, startLon, endLat, endLon, transportMode, avoidTolls);

                return Ok(routeData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération de l'itinéraire: {ex.Message}");
            }
        }

        private bool IsValidTransportMode(string transportMode)
        {
            var validModes = new[] { "auto", "bicycle", "pedestrian", "motor_scooter", "bus", "motorcycle" };
            return validModes.Contains(transportMode.ToLower());
        }
    }
}