using IncidentsService.Services;

namespace IncidentsService.Services
{
    public class IncidentCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IncidentCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);

        public IncidentCleanupService(IServiceProvider serviceProvider, ILogger<IncidentCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service de nettoyage des incidents d�marr�");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredIncidents();
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du nettoyage des incidents expir�s");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("Service de nettoyage des incidents arr�t�");
        }

        private async Task CleanupExpiredIncidents()
        {
            using var scope = _serviceProvider.CreateScope();
            var incidentService = scope.ServiceProvider.GetRequiredService<IIncidentService>();

            bool success = await incidentService.CleanupExpiredIncidents();
            if (success)
            {
                _logger.LogInformation("Nettoyage des incidents expir�s effectu� avec succ�s");
            }
            else
            {
                _logger.LogWarning("Le nettoyage des incidents expir�s a �chou�");
            }
        }
    }
}