using Google.Apis.YouTube.v3.Data;
using System.IO;

namespace YoutubePlayer.Core.Interfaces
{
    public interface IYoutubeServices
    {
        string FindAndGetFirst(string word);
        SearchListResponse GetAll(string word);
        void StreamAudio(string videoUrl);
        Stream ConvertURLToPcm(string url);
    }
}
