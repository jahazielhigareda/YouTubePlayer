using System.IO;
using System.Threading;

namespace YoutubePlayer.Core.Interfaces
{
    public interface IMediaMusicService
    {
        void PlayFromStremUrl(string streamUrl, CancellationTokenSource cancel);
        void PlayPCM(Stream pcm);
    }
}
