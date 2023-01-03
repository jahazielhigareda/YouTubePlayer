using System;
using System.Threading.Tasks;
using YoutubePlayer.Core.Extensions;
using YoutubePlayer.Core.Interfaces;

namespace YouTubePlayer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCollectionExtensions.Services();

            var logService = ServiceCollectionExtensions.GetService<ILogService>();
            var youtubeService = ServiceCollectionExtensions.GetService<IYoutubeServices>();

            logService.Information("Escriba la cancion que desea:");
            var word = Console.ReadLine();

            //var videos = youtubeService.GetAll(word);

            var videoId = youtubeService.FindAndGetFirst(word);
            youtubeService.StreamAudio($"https://www.youtube.com/watch?v={videoId}");
        }
    }
}