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
        public NVorbisSource(String path)
        {
            _vorbisReader = new VorbisReader(path);
            WaveFormat = new WaveFormat(_vorbisReader.SampleRate, 16, _vorbisReader.Channels, AudioEncoding.Vorbis1);
            posAnt = TimeSpan.Zero;
            _vorbisReader.SamplePosition = 0;
        }
        public NVorbisSource(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;
            _vorbisReader = new VorbisReader(stream,false);
            WaveFormat = new WaveFormat(_vorbisReader.SampleRate, 16, _vorbisReader.Channels, AudioEncoding.IeeeFloat);
            posAnt = TimeSpan.Zero;
            _vorbisReader.SamplePosition = 0;
        }

        public bool CanSeek
        {
            get { return _stream.CanSeek; }
        }
        public TimeSpan Length_Timespan { get { return _vorbisReader.TotalTime; } }
        public WaveFormat WaveFormat { get; }
        //got fixed through workitem #17, thanks for reporting @rgodart.
        public long Length
        {
            get 
            {
                return (long)_vorbisReader.TotalTime.TotalSeconds * WaveFormat.SampleRate * _vorbisReader.Channels;
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
                return (long)_vorbisReader.TimePosition.TotalSeconds * WaveFormat.SampleRate * _vorbisReader.Channels;
            }
            set
            {
                if (!CanSeek)
                    throw new InvalidOperationException("NVorbisSource is not seekable.");
                if (value < 0 || value > Length)
                    throw new ArgumentOutOfRangeException("value");

                try
                {
                    _vorbisReader.TimePosition = TimeSpan.FromSeconds((double)value / _vorbisReader.SampleRate / _vorbisReader.Channels);

                }
                catch (ArgumentOutOfRangeException)
                {

                    Log.Instance.PrintMessage("Error intentando cambiar el puntero de la canción, poniendo uno de reserva...", MessageType.Error);
                    _vorbisReader.TimePosition = posAnt;
                }
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            posAnt = _vorbisReader.TimePosition;
            BitRate = _vorbisReader.Streams[0].Stats.InstantBitRate;
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
        public string GetTitle()
        {
            return _vorbisReader.Tags.Title;
        }
        public string GetArtist()
        {
            return _vorbisReader.Tags.Artist;
        }
    }
}
