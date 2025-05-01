using IncidentsService.Data;
using IncidentsService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncidentsService.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IncidentsDbContext _context;
        private readonly ILogger<IncidentService> _logger;

        public IncidentService(IncidentsDbContext context, ILogger<IncidentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IncidentResponse> CreateIncident(int userId, string userName, CreateIncidentRequest request)
        {
            try
            {
                _logger.LogDebug("Cr�ation d'un incident par l'utilisateur {UserId}", userId);
                // Calculer la date d'expiration
                int durationMinutes = request.ExpectedDuration ?? 60; // Par d�faut, 60 minutes

                // Ajuster la dur�e en fonction du type d'incident
                durationMinutes = request.Type switch
                {
                    "accident" => Math.Max(durationMinutes, 120), // Au moins 2 heures
                    "construction" => Math.Max(durationMinutes, 1440), // Au moins 24 heures
                    "police" => Math.Max(durationMinutes, 60), // Au moins 1 heure
                    "hazard" => Math.Max(durationMinutes, 120), // Au moins 2 heures
                    "closure" => Math.Max(durationMinutes, 240), // Au moins 4 heures
                    "traffic_jam" => Math.Max(durationMinutes, 30), // Au moins 30 minutes
                    _ => 60 // Valeur par d�faut
                };

                var incident = new Incident
                {
                    UserId = userId,
                    UserName = userName,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Type = request.Type,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(durationMinutes),
                    IsActive = true
                };

                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();

                return new IncidentResponse
                {
                    Id = incident.Id,
                    UserId = incident.UserId,
                    UserName = incident.UserName,
                    Latitude = incident.Latitude,
                    Longitude = incident.Longitude,
                    Type = incident.Type,
                    Description = incident.Description,
                    CreatedAt = incident.CreatedAt,
                    ExpiresAt = incident.ExpiresAt,
                    Upvotes = 0,
                    Downvotes = 0,
                    IsActive = incident.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la cr�ation d'un incident");
                throw new Exception("Impossible de cr�er l'incident. Veuillez r�essayer plus tard.", ex);
            }
        }

        public async Task<IncidentResponse> GetIncidentById(int id)
        {
            try
            {
                _logger.LogDebug("R�cup�ration de l'incident avec l'ID {Id}", id);

                var incident = await _context.Incidents
                    .Include(i => i.Votes)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (incident == null)
                {
                    throw new KeyNotFoundException("L'incident demand� n'a pas �t� trouv�.");
                }

                int upvotes = incident.Votes.Count(v => v.Vote > 0);
                int downvotes = incident.Votes.Count(v => v.Vote < 0);

                return new IncidentResponse
                {
                    Id = incident.Id,
                    UserId = incident.UserId,
                    UserName = incident.UserName,
                    Latitude = incident.Latitude,
                    Longitude = incident.Longitude,
                    Type = incident.Type,
                    Description = incident.Description,
                    CreatedAt = incident.CreatedAt,
                    ExpiresAt = incident.ExpiresAt,
                    Upvotes = upvotes,
                    Downvotes = downvotes,
                    IsActive = incident.IsActive
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la r�cup�ration de l'incident");
                throw new Exception("Impossible de r�cup�rer l'incident. Veuillez r�essayer plus tard.", ex);
            }
        }

        public async Task<List<IncidentResponse>> GetNearbyIncidents(NearbyIncidentsRequest request)
        {
            try
            {
                _logger.LogDebug("Recherche d'incidents � proximit� de ({Lat}, {Lon}) dans un rayon de {Radius} km",
                    request.Latitude, request.Longitude, request.Radius);

                // Filtrer les types d'incidents si sp�cifi�s
                var typesFilter = new List<string>();
                if (!string.IsNullOrEmpty(request.Types))
                {
                    typesFilter = request.Types.Split(',').Select(t => t.Trim()).ToList();
                }

                // R�cup�rer tous les incidents actifs qui ne sont pas expir�s
                var query = _context.Incidents
                    .Include(i => i.Votes)
                    .Where(i => i.IsActive && i.ExpiresAt > DateTime.UtcNow);

                // Appliquer le filtre par type si n�cessaire
                if (typesFilter.Any())
                {
                    query = query.Where(i => typesFilter.Contains(i.Type));
                }

                // R�cup�rer les incidents
                var incidents = await query.ToListAsync();

                // Calculer la distance pour chaque incident et filtrer par rayon
                // Note: Dans une impl�mentation r�elle, vous utiliseriez NetTopologySuite pour des calculs g�ographiques pr�cis
                var result = incidents
                    .Select(i => {
                        double distance = CalculateDistance(
                            request.Latitude, request.Longitude,
                            i.Latitude, i.Longitude
                        );

                        int upvotes = i.Votes.Count(v => v.Vote > 0);
                        int downvotes = i.Votes.Count(v => v.Vote < 0);

                        return new IncidentResponse
                        {
                            Id = i.Id,
                            UserId = i.UserId,
                            UserName = i.UserName,
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                            Type = i.Type,
                            Description = i.Description,
                            CreatedAt = i.CreatedAt,
                            ExpiresAt = i.ExpiresAt,
                            Upvotes = upvotes,
                            Downvotes = downvotes,
                            Distance = distance,
                            IsActive = i.IsActive
                        };
                    })
                    .Where(r => r.Distance <= request.Radius)
                    .OrderBy(r => r.Distance)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'incidents � proximit�");
                throw new Exception("Impossible de rechercher les incidents � proximit�. Veuillez r�essayer plus tard.", ex);
            }
        }

        public async Task<VoteResponse> VoteForIncident(int incidentId, int userId, VoteRequest request)
        {
            try
            {
                _logger.LogDebug("Vote de l'utilisateur {UserId} pour l'incident {IncidentId}: {Vote}",
                    userId, incidentId, request.Vote);

                // V�rifier que l'incident existe et est actif
                var incident = await _context.Incidents
                    .Include(i => i.Votes)
                    .FirstOrDefaultAsync(i => i.Id == incidentId);

                if (incident == null)
                {
                    throw new KeyNotFoundException("L'incident demand� n'a pas �t� trouv�.");
                }

                if (!incident.IsActive || incident.ExpiresAt < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Impossible de voter pour un incident inactif ou expir�.");
                }

                // V�rifier si l'utilisateur a d�j� vot�
                var existingVote = await _context.IncidentVotes
                    .FirstOrDefaultAsync(v => v.IncidentId == incidentId && v.UserId == userId);

                if (existingVote != null)
                {
                    // Mettre � jour le vote existant
                    existingVote.Vote = request.Vote;
                }
                else
                {
                    // Cr�er un nouveau vote
                    var vote = new IncidentVote
                    {
                        IncidentId = incidentId,
                        UserId = userId,
                        Vote = request.Vote,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.IncidentVotes.Add(vote);
                }

                await _context.SaveChangesAsync();

                // Recharger l'incident pour obtenir les votes mis � jour
                incident = await _context.Incidents
                    .Include(i => i.Votes)
                    .FirstOrDefaultAsync(i => i.Id == incidentId);

                int upvotes = incident.Votes.Count(v => v.Vote > 0);
                int downvotes = incident.Votes.Count(v => v.Vote < 0);

                return new VoteResponse
                {
                    Id = incidentId,
                    Upvotes = upvotes,
                    Downvotes = downvotes
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du vote pour un incident");
                throw new Exception("Impossible d'enregistrer votre vote. Veuillez r�essayer plus tard.", ex);
            }
        }

        public async Task<StatusUpdateResponse> UpdateIncidentStatus(int incidentId, int userId, StatusUpdateRequest request)
        {
            try
            {
                _logger.LogDebug("Mise � jour du statut de l'incident {IncidentId} par l'utilisateur {UserId}: {Status}",
                    incidentId, userId, request.IsActive);

                var incident = await _context.Incidents.FindAsync(incidentId);
                if (incident == null)
                {
                    throw new KeyNotFoundException("L'incident demand� n'a pas �t� trouv�.");
                }

                // V�rifier que seul le propri�taire ou un admin peut modifier le statut
                if (incident.UserId != userId)
                {
                    throw new UnauthorizedAccessException("Vous n'�tes pas autoris� � modifier cet incident.");
                }

                incident.IsActive = request.IsActive;
                await _context.SaveChangesAsync();

                return new StatusUpdateResponse
                {
                    Id = incidentId,
                    IsActive = request.IsActive
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise � jour du statut de l'incident");
                throw new Exception("Impossible de mettre � jour le statut de l'incident. Veuillez r�essayer plus tard.", ex);
            }
        }

        public async Task<bool> IsIncidentOwner(int incidentId, int userId)
        {
            var incident = await _context.Incidents.FindAsync(incidentId);
            return incident != null && incident.UserId == userId;
        }

        public async Task<bool> CleanupExpiredIncidents()
        {
            try
            {
                _logger.LogInformation("Nettoyage des incidents expir�s");

                var now = DateTime.UtcNow;
                var expiredIncidents = await _context.Incidents
                    .Where(i => i.IsActive && i.ExpiresAt < now)
                    .ToListAsync();

                foreach (var incident in expiredIncidents)
                {
                    incident.IsActive = false;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Nettoyage termin�: {Count} incidents d�sactiv�s", expiredIncidents.Count);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du nettoyage des incidents expir�s");
                return false;
            }
        }

        // M�thode auxiliaire pour calculer la distance entre deux coordonn�es (formule de Haversine)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Rayon de la Terre en kilom�tres

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance en kilom�tres
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}