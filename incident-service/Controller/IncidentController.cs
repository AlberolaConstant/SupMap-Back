using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IncidentsService.Models;
using IncidentsService.Services;
using System.Security.Claims;

namespace IncidentsService.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        private readonly ILogger<IncidentsController> _logger;

        public IncidentsController(IIncidentService incidentService, ILogger<IncidentsController> logger)
        {
            _incidentService = incidentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<IncidentResponse>> CreateIncident([FromBody] CreateIncidentRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Utilisateur";

                if (userId == 0)
                {
                    return Unauthorized("Utilisateur non authentifi�.");
                }

                var incident = await _incidentService.CreateIncident(userId, userName, request);
                return Ok(incident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la cr�ation d'un incident");
                return StatusCode(500, "Une erreur est survenue lors de la cr�ation de l'incident: " + ex.Message);
            }
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<List<IncidentResponse>>> GetNearbyIncidents([FromQuery] NearbyIncidentsRequest request)
        {
            try
            {
                var incidents = await _incidentService.GetNearbyIncidents(request);
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'incidents � proximit�");
                return StatusCode(500, "Une erreur est survenue lors de la recherche d'incidents: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentResponse>> GetIncident(int id)
        {
            try
            {
                var incident = await _incidentService.GetIncidentById(id);
                return Ok(incident);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("L'incident demand� n'a pas �t� trouv�.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la r�cup�ration de l'incident");
                return StatusCode(500, "Une erreur est survenue lors de la r�cup�ration de l'incident: " + ex.Message);
            }
        }

        [HttpPost("{id}/vote")]
        public async Task<ActionResult<VoteResponse>> VoteForIncident(int id, [FromBody] VoteRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized("Utilisateur non authentifié.");
                }

                var result = await _incidentService.VoteForIncident(id, userId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("L'incident demandé n'a pas été trouvé.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du vote pour un incident");
                return StatusCode(500, "Une erreur est survenue lors de l'enregistrement du vote: " + ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<StatusUpdateResponse>> UpdateIncidentStatus(int id, [FromBody] StatusUpdateRequest request)
        {
            try
            {
               var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized("Utilisateur non authentifi�.");
                }

                var isAdmin = User.IsInRole("Admin");
                if (!isAdmin && !(await _incidentService.IsIncidentOwner(id, userId)))
                {
                    return Forbid("Vous n'�tes pas autoris� � modifier cet incident.");
                }

                var result = await _incidentService.UpdateIncidentStatus(id, userId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("L'incident demand� n'a pas �t� trouv�.");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("Vous n'�tes pas autoris� � modifier cet incident.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise � jour du statut de l'incident");
                return StatusCode(500, "Une erreur est survenue lors de la mise � jour du statut: " + ex.Message);
            }
        }
    }
}