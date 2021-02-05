using Microsoft.Extensions.DependencyInjection;

namespace JokeGenerator
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddHttpClient();

            services.AddSingleton<IPrinter, ConsolePrinter>();

            services.AddTransient<IJsonFeed, JsonFeed>();

            services.AddSingleton<EntryPoint>();

            return services;
        }
    }
}