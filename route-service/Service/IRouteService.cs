namespace RouteService.Service
{
    using RouteService.Model;

    public interface IRouteService
    {
        List<Route> GetUserRecentRoutes(int userId, int limit = 5);
        Route? GetRouteById(int id);
        Route SaveRoute(Route route);
        bool DeleteRoute(int id);

        // Cette méthode pourrait appeler l'API Valhalla
        Task<string> GetRouteFromValhalla(
            double startLat, double startLon,
            double endLat, double endLon,
            string transportMode = "auto",
            bool avoidTolls = false);
    }
}