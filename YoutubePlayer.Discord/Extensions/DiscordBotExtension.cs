using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubePlayer.Core.Interfaces;
using YoutubePlayer.Core.Services;
using YoutubePlayer.Discord.Services;

namespace YoutubePlayer.Discord.Extensions
{
    public static class DiscordBotExtension
    {
        public static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public static async Task Client_Ready()
        {
            Console.WriteLine("Client Ready!");
        }

        public static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<PictureService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<IYoutubeServices, YoutubeServices>()
                .AddSingleton<ILogService, LogService>()
                .AddSingleton<IMediaMusicService, MediaMusicService>()
                .BuildServiceProvider();
        }
    }
}
