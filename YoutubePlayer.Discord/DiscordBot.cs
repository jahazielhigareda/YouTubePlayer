using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using YoutubePlayer.Discord.Extensions;
using YoutubePlayer.Discord.Services;

namespace YoutubePlayer.Discord
{
    public class DiscordBot
    {
        private readonly string _token = "MTA1OTU0NzI5MTgxNzYwNzI1OQ.GMt2yS.Jr9V9P_uM-0ZmhqCNBrgrKFd_GIGLo7wbYJfJc";

        private ServiceProvider _services;
        private DiscordSocketClient _discord;
        private CommandService _commands;

        public async Task Run()
        {
            _services = DiscordBotExtension.ConfigureServices();

            _discord = _services.GetRequiredService<DiscordSocketClient>();

            _commands = new CommandService();

            _discord.Log += DiscordBotExtension.Log;

            await _discord.LoginAsync(TokenType.Bot, _token);
            await _discord.StartAsync();

            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            _discord.Ready += DiscordBotExtension.Client_Ready;

            // Block this program until it is closed.
            await Task.Delay(-1);
        }

    }
}
