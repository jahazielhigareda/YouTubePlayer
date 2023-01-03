namespace YoutubePlayer.Discord
{
    class Program
    {
        static void Main(string[] args)
        {
            new DiscordBot().Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
