using System;
using YoutubePlayer.Core.Interfaces;

namespace YoutubePlayer.Core.Services
{
    public class LogService : ILogService
    {
        public void Information(string arg)
        {
            Console.WriteLine(arg);
        }
    }
}
