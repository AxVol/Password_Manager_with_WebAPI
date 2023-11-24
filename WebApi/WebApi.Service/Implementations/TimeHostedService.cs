using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.DAL.Interfaces;
using WebApi.Domain.Entity;

namespace WebApi.Service.Implementations
{
    public class TimeHostedService : IHostedService, IDisposable
    {
        private readonly IRepository<BlockedUser> repository;
        private readonly ILogger<TimeHostedService> logger;
        private readonly int refreshInterval = 5;

        private Timer? timer = null;

        public TimeHostedService(IServiceScopeFactory factory, ILogger<TimeHostedService> log)
        {
            repository = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<BlockedUser>>();
            logger = log;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(refreshInterval));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            logger.LogInformation("Очищаю базу с заблокированными пользователями от истекших");
            CheckBlockUsers();
            logger.LogInformation("Очистил");
        }

        private void CheckBlockUsers()
        {
            List<BlockedUser> blockedUsers = repository.GetAll().ToList();

            foreach (BlockedUser blockedUser in blockedUsers)
            {
                if (blockedUser.UnbanDate <= DateTime.Now)
                {
                    repository.Delete(blockedUser);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
