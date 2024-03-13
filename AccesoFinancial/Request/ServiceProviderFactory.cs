
using Microsoft.Extensions.DependencyInjection;

namespace AccesoFinancial.Request
{ 
    public static class ServiceProviderFactory
    {
        private static ServiceProvider _serviceProvider;

        public static void SetServiceProvider(ServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public static T GetService<T>() where T : class
            => _serviceProvider.GetRequiredService<T>();
    }

}
