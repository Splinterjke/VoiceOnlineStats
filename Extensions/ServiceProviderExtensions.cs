using Microsoft.Extensions.DependencyInjection;
using VoiceOnlineStats.Services;

namespace VoiceOnlineStats.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddLiteDBService(this IServiceCollection services)
        {
            services.AddSingleton<IDBService, LiteDBService>();
        }
    }
}
