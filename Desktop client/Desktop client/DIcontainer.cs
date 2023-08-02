using Desktop_client.ViewModels;
using Desktop_client.Services.Interfaces;
using Desktop_client.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Desktop_client.Models;

namespace Desktop_client
{
    public class DIcontainer
    {
        private static ServiceProvider serviceProvider;

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainViewModel>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IUserManager, UserManager>();

            serviceProvider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                serviceProvider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => serviceProvider.GetRequiredService<MainViewModel>();
    }
}
