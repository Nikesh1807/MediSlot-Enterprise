using Microsoft.EntityFrameworkCore;
using HealthcareApi.Data;

namespace HealthcareApi.Services
{
    public class ExpiredLockCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredLockCleanupService> _logger;

        public ExpiredLockCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredLockCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Expired Lock Cleanup Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        // Find slots where lock has expired but not booked
                        var expiredSlots = await dbContext.TimeSlots
                            .Where(s => s.IsLocked && !s.IsBooked && s.LockExpiresAt < DateTime.UtcNow)
                            .ToListAsync(stoppingToken);

                        if (expiredSlots.Any())
                        {
                            foreach (var slot in expiredSlots)
                            {
                                slot.IsLocked = false;
                                slot.LockExpiresAt = null;
                            }

                            await dbContext.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation($"Released {expiredSlots.Count} expired locked slots.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up expired locks.");
                }

                // Run every 30 seconds
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}