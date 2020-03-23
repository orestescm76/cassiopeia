using CSCore;
using NVorbis;
using System;
using System.IO;

/*
 * This is the sample from the repository cscore made by filoe
 * For more details: https://github.com/filoe/cscore
 */

namespace aplicacion_musica
{
    public sealed class NVorbisSource : ISampleSource
    {
        private readonly Stream _stream;
        private readonly VorbisReader _vorbisReader;
        private bool _disposed;
        private TimeSpan posAnt;
        private int BitRate;

        public NVorbisSource(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;
            _vorbisReader = new VorbisReader(stream, false);
            WaveFormat = new WaveFormat(_vorbisReader.SampleRate, 16, _vorbisReader.Channels, AudioEncoding.Vorbis1);
            posAnt = TimeSpan.Zero;
            //_vorbisReader.DecodedPosition = 5;
        }

        public bool CanSeek
        {
            get { return _stream.CanSeek; }
        }
        public TimeSpan Duracion { get { return _vorbisReader.TotalTime; } }
        public WaveFormat WaveFormat { get; }

        //got fixed through workitem #17, thanks for reporting @rgodart.
        public long Length
        {
            get 
            { 
                return CanSeek ? (long)(_vorbisReader.TotalTime.TotalSeconds * WaveFormat.SampleRate * WaveFormat.Channels) : 0; 
            }
        }
        public int Bitrate
        {
            get
            {
                return BitRate;
            }
        }
        //got fixed through workitem #17, thanks for reporting @rgodart.
        public long Position
        {
            get
            {
                long pos = (long)(_vorbisReader.DecodedTime.TotalSeconds * _vorbisReader.SampleRate * _vorbisReader.Channels);
                if (pos < 0)
                {
                    return (long)(posAnt.TotalSeconds * _vorbisReader.SampleRate * _vorbisReader.Channels);
                }
                return CanSeek ? pos : 0;
            }
            set
            {
                if (!CanSeek)
                    throw new InvalidOperationException("NVorbisSource is not seekable.");
                if (value < 0 || value > Length)
                    throw new ArgumentOutOfRangeException("value");

                try
                {
                    _vorbisReader.DecodedTime = TimeSpan.FromSeconds((double)value / _vorbisReader.SampleRate / _vorbisReader.Channels);

                }
                catch (Exception)
                {

                    Log.Instance.ImprimirMensaje("Error intentando cambiar el puntero de la canción, poniendo uno de reserva...", TipoMensaje.Error);
                    _vorbisReader.DecodedTime = posAnt;
                }
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            posAnt = _vorbisReader.DecodedTime;
            BitRate = _vorbisReader.Stats[0].InstantBitRate;
            return _vorbisReader.ReadSamples(buffer, offset, count);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _vorbisReader.Dispose();
                _stream.Close();
            }
            else
                throw new ObjectDisposedException("NVorbisSource");
            _disposed = true;
        }
        public string GetTitulo()
        {
            foreach (String meta in _vorbisReader.Comments)
            {
                if(meta.Contains("TITLE=")|| meta.Contains("TITLE=".ToLower()))
                {
                    return meta.Replace("TITLE=", "");
                }
            }
            return null;
        }
        public string GetArtista()
        {
            foreach (String meta in _vorbisReader.Comments)
            {
                if (meta.Contains("ARTIST=")|| meta.Contains("ARTIST=".ToLower()))
                {
                    return meta.Replace("ARTIST=", "");
                }
            }
            return null;
        }
    }
}
