namespace RoutesService.Service
{
    using RoutesService.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRouteService
    {
        Task<string> GetRouteFromValhalla(double startLat, double startLon, double endLat, double endLon, string transportMode, bool avoidTolls);
        List<Route> GetUserRecentRoutes(int userId, int limit);
        Route GetRouteById(int id);
        Route SaveRoute(Route route);
        bool DeleteRoute(int id);
    }
}
