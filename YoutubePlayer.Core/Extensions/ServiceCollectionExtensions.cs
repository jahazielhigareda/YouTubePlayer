using Microsoft.Extensions.DependencyInjection;
using YoutubePlayer.Core.Interfaces;
using YoutubePlayer.Core.Services;

namespace YoutubePlayer.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static ServiceProvider _serviceProvider = null;

        public static void Services()
        {
            _serviceProvider = new ServiceCollection()
                                       .AddTransient<ILogService, LogService>()
                                       .AddTransient<IMediaMusicService, MediaMusicService>()
                                       .AddTransient<IYoutubeServices, YoutubeServices>()
                                       .BuildServiceProvider();
        }

        public static T GetService<T>() 
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
