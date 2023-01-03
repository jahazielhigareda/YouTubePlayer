using NAudio.Wave;
using System;
using System.IO;
using System.Threading;
using YoutubePlayer.Core.Constants;
using YoutubePlayer.Core.Interfaces;

namespace YoutubePlayer.Core.Services
{
    public class MediaMusicService : IMediaMusicService
    {
        private readonly ILogService _logger;

        private MediaFoundationReader _mediaFoundationReader;
        private WaveOutEvent _waveOutEvent;

        private float _volumeAux = 0;
        private CancellationTokenSource _cancel;

        public MediaMusicService(ILogService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void PlayFromStremUrl(string streamUrl, CancellationTokenSource cancel)
        {
            _cancel = cancel ?? throw new ArgumentNullException(nameof(cancel));

            // play audio from url
            using (_mediaFoundationReader = new MediaFoundationReader(streamUrl))
            using (_waveOutEvent = new WaveOutEvent())
            {
                _waveOutEvent.Init(_mediaFoundationReader);
                _waveOutEvent.Play();

                do
                {
                    MenuOptions();
                } while ((MediaMusicOption)Console.ReadKey(true).Key != MediaMusicOption.StopAndReturn);
            }
        }
        public void PlayPCM(Stream pcm)
        {

        }

        public void SendAudio(string pathOrUrl)
        {

        }


        public void SendAudio2(string filePath)
        {

        }

        private void MenuOptions()
        {
            switch ((MediaMusicOption)Console.ReadKey(true).Key)
            {
                case MediaMusicOption.Stop:
                    Stop();
                    break;
                case MediaMusicOption.Pause:
                    Pause();
                    break;
                case MediaMusicOption.Play:
                    Play();
                    break;
                case MediaMusicOption.IncrementVolume:
                    IncrementVolume();
                    break;
                case MediaMusicOption.DescrementVolume:
                    DescrementVolume();
                    break;
                case MediaMusicOption.MuteVolume:
                    MuteVolume();
                    break;
            }
        }
        private void Stop()
        {
            _logger.Information("Se ha detenido la canción.");
            _waveOutEvent.Stop();
            _cancel.Cancel();
        }
        private void Pause()
        {
            _logger.Information("Se ha pausado la canción.");
            _waveOutEvent.Pause();
        }
        private void Play()
        {
            _logger.Information("Cancion en reprodución...");
            _waveOutEvent.Play();
        }
        private void IncrementVolume()
        {
            if ((_waveOutEvent.Volume + (float)0.1) > 1)
            {
                _logger.Information($"No se puede subir mas el volumen...");
                return;
            }
            _logger.Information($"El volumen está en {_waveOutEvent.Volume} y subio a {_waveOutEvent.Volume + (float)0.1}");
            _waveOutEvent.Volume += (float)0.1;
        }
        private void DescrementVolume()
        {
            if ((_waveOutEvent.Volume - (float)0.1) < 0)
            {
                _logger.Information($"No se puede bajar mas el volumen...");
                return;
            }
            _logger.Information($"El volumen está en {_waveOutEvent.Volume} y bajo a {_waveOutEvent.Volume - (float)0.1}");
            _waveOutEvent.Volume -= (float)0.1;
        }
        private void MuteVolume()
        {
            var muteStr = _volumeAux == 0 ? "mute" : "unmute";
            if (_volumeAux == 0)
            {
                _volumeAux = _waveOutEvent.Volume;
                _waveOutEvent.Volume = 0;
            }
            else
            {
                _waveOutEvent.Volume = _volumeAux;
                _volumeAux = 0;
            }
            _logger.Information($"El volumen está en {muteStr}");
        }
    }
}