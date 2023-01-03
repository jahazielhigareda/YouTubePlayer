using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YoutubePlayer.Core.Interfaces;

namespace YoutubePlayer.Core.Services
{
    public class YoutubeServices : IYoutubeServices
    {
        private readonly ILogService _logger;
        private readonly IMediaMusicService _mediaMusicService;

        private YouTubeService _youTubeService;
        private readonly string _apiKey = "AIzaSyALR_Em44_v-M95L8XwT3SOU_x4g02u9MU";
        private readonly string _baseUrl = "https://www.youtube.com/watch?v=";

        private readonly CancellationTokenSource _cancel;

        public YoutubeServices(ILogService logger, IMediaMusicService mediaMusicService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediaMusicService = mediaMusicService ?? throw new ArgumentNullException(nameof(mediaMusicService));

            _youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey
            });

            _cancel = new CancellationTokenSource();
        }

        public string FindAndGetFirst(string word)
        {
            _logger.Information("Buscando en youtube...");
            // Obtén una clave de API de YouTube en https://console.developers.google.com
           
            // Obtén el video que deseas reproducir utilizando la API de YouTube
            var searchListRequest = _youTubeService.Search.List("snippet");
            searchListRequest.Q = word; // La búsqueda del video
            searchListRequest.MaxResults = 1; // Solo obtén un resultado
            var searchListResponse = searchListRequest.Execute();

            // Obtén el enlace al video
            string videoId = searchListResponse.Items[0].Id.VideoId;
            string videoUrl = $"{_baseUrl}{videoId}";
            return videoId;
        }

        public SearchListResponse GetAll(string word)
        {
            _logger.Information("Buscando en youtube...");

            // Obtén el video que deseas reproducir utilizando la API de YouTube
            var searchListRequest = _youTubeService.Search.List("snippet");
            searchListRequest.Q = word; // La búsqueda del video
            //searchListRequest.MaxResults = 1; // Solo obtén un resultado
            var searchListResponse = searchListRequest.Execute();

            // Obtén el enlace al video
            return searchListResponse;
        }

        public void StreamAudio(string videoUrl)
        {
            _logger.Information("Cancion en reproducion...");
           
            // fetch audio stream url
            YoutubeDL ytdl = new YoutubeDL();
            var options = new OptionSet() { Format = "m4a", GetUrl = true };
            var task = ytdl.RunWithOptions(
                new[] { videoUrl },
                options,
                _cancel.Token
            );

            var streamUrl = task.Result.Data[0];

            // play audio from url
            _mediaMusicService.PlayFromStremUrl(streamUrl, _cancel);
        }

        public Stream ConvertURLToPcm(string url)
        {
            string args = $"/C youtube-dl --ignore-errors -o - {url} | ffmpeg -err_detect ignore_err -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1";
            var ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            var pcm = ffmpeg.StandardOutput.BaseStream;
            //_mediaMusicService.PlayPCM(pcm);
            return pcm;
        }


    }
}
