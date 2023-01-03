using Discord;
using Discord.Audio;
using Discord.Commands;
using System.Threading.Tasks;
using YoutubePlayer.Core.Interfaces;

namespace YoutubePlayer.Discord.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly IYoutubeServices _youtubeService;

        public MusicModule(IYoutubeServices youtubeService)
        {
            _youtubeService = youtubeService;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayChannel([Remainder] string text = null)
        {
            // Get the audio channel
            var channel = (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();
            await SendAsync(audioClient, text);
        }

        private async Task SendAsync(IAudioClient client, string text)
        {
            var videoId = _youtubeService.FindAndGetFirst(text);
            using (var output = _youtubeService.ConvertURLToPcm($"https://www.youtube.com/watch?v={videoId}"))
            using (var discord = client.CreatePCMStream(AudioApplication.Music))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}
