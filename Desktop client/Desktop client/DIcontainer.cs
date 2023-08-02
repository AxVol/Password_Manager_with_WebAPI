using Desktop_client.ViewModels;
using Desktop_client.Services.Interfaces;
using Desktop_client.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop_client
{
    public class DIcontainer
    {
        private static ServiceProvider serviceProvider;

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IUserManager, UserManager>();

            services.AddHttpClient();

            serviceProvider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                if (!item.ServiceType.ContainsGenericParameters)
                {
                    serviceProvider.GetRequiredService(item.ServiceType);
                }
            }
        }

        public MainViewModel MainViewModel => serviceProvider.GetRequiredService<MainViewModel>();
        public LoginViewModel LoginViewModel => serviceProvider.GetRequiredService<LoginViewModel>();
        public RegisterViewModel RegisterViewModel => serviceProvider.GetRequiredService<RegisterViewModel>();
    }
}
