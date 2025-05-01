using System;
using System.ComponentModel.DataAnnotations;

namespace IncidentsService.Models
{
	public class Incident
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; } = "";
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Type { get; set; } = ""; // accident, construction, police, hazard, closure, traffic_jam
		public string Description { get; set; } = "";
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime ExpiresAt { get; set; }
		public bool IsActive { get; set; } = true;

		// Propri�t�s de navigation
		public ICollection<IncidentVote> Votes { get; set; } = new List<IncidentVote>();
	}

	public class IncidentVote
	{
		[Key]
		public int Id { get; set; }
		public int IncidentId { get; set; }
		public int UserId { get; set; }
		public int Vote { get; set; } // 1 pour upvote, -1 pour downvote
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Propri�t� de navigation
		public Incident? Incident { get; set; }
	}

	// DTOs
	public class CreateIncidentRequest
	{
		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }

		[Required]
		[RegularExpression("accident|construction|police|hazard|closure|traffic_jam")]
		public string Type { get; set; } = "";

		[Required]
		public string Description { get; set; } = "";

		public int? ExpectedDuration { get; set; } // En minutes
	}

	public class IncidentResponse
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; } = "";
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Type { get; set; } = "";
		public string Description { get; set; } = "";
		public DateTime CreatedAt { get; set; }
		public DateTime ExpiresAt { get; set; }
		public int Upvotes { get; set; }
		public int Downvotes { get; set; }
		public bool IsActive { get; set; }
		public double? Distance { get; set; } // En km, optionnel
	}

	public class NearbyIncidentsRequest
	{
		[Required]
		public double Latitude { get; set; }

		[Required]
		public double Longitude { get; set; }

		public double Radius { get; set; } = 5.0; // Rayon en km, par d�faut 5 km

		public string? Types { get; set; } // Types d'incidents s�par�s par des virgules
	}

	public class VoteRequest
	{
		[Required]
		[Range(-1, 1)]
		public int Vote { get; set; } // 1 pour upvote, -1 pour downvote
	}

	public class VoteResponse
	{
		public int Id { get; set; }
		public int Upvotes { get; set; }
		public int Downvotes { get; set; }
	}

	public class StatusUpdateRequest
	{
		[Required]
		public bool IsActive { get; set; }
	}

	public class StatusUpdateResponse
	{
		public int Id { get; set; }
		public bool IsActive { get; set; }
	}
}