using IncidentsService.Models;

namespace IncidentsService.Services
{
	public interface IIncidentService
	{
		Task<IncidentResponse> CreateIncident(int userId, string userName, CreateIncidentRequest request);
		Task<IncidentResponse> GetIncidentById(int id);
		Task<List<IncidentResponse>> GetNearbyIncidents(NearbyIncidentsRequest request);
		Task<VoteResponse> VoteForIncident(int incidentId, int userId, VoteRequest request);
		Task<StatusUpdateResponse> UpdateIncidentStatus(int incidentId, int userId, StatusUpdateRequest request);
		Task<bool> IsIncidentOwner(int incidentId, int userId);
		Task<bool> CleanupExpiredIncidents();
	}
}