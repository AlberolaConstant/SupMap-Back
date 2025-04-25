namespace RoutesService.Service
{
    using RoutesService.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRouteService
    {
        Task<string> GetRouteFromValhalla(double startLat, double startLon, double endLat, double endLon, string transportMode, bool avoidTolls);
        List<Road> GetUserRecentRoutes(int userId, int limit);
        Road GetRouteById(int id);
        Road SaveRoute(Road route);
        bool DeleteRoute(int id);
    }
}
