namespace RoutesService.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
        public string TransportMode { get; set; } = "auto"; // auto, bicycle, pedestrian, etc.
        public bool AvoidTolls { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // On pourrait stocker la réponse de Valhalla (ou des infos essentielles)
        public string RouteData { get; set; } = string.Empty;
    }
}