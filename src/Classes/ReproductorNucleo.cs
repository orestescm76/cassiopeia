using System;
using System.IO;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using NVorbis;
using JAudioTags;
using SpotifyAPI.Web;

namespace aplicacion_musica
{
    public enum FormatoSonido
    {
        MP3,
        FLAC,
        OGG
    }
    class ReproductorNucleo
    {
        private ISoundOut _salida;
        private FormatoSonido FormatoSonido;
        private IWaveSource _sonido;
        private VorbisReader _vorbisReader;
        private CSCore.Tags.ID3.ID3v2QuickInfo tags;
        private FLACFile _ficheroFLAC;
        private SingleBlockNotificationStream notificationStream;
        private NVorbisSource NVorbis;
        private String artista;
        private String titulo;
        long tamFich;
        public void CargarCancion(string cual)
        {
            switch (Path.GetExtension(cual))
            {
                case ".mp3":
                    CSCore.Tags.ID3.ID3v2 mp3tag = CSCore.Tags.ID3.ID3v2.FromFile(cual);
                    tags = new CSCore.Tags.ID3.ID3v2QuickInfo(mp3tag);
                    FormatoSonido = FormatoSonido.MP3;
                    break;
                case ".flac":
                    _ficheroFLAC = new FLACFile(cual, true);
                    FormatoSonido = FormatoSonido.FLAC;
                    break;
                case ".ogg":
                    FormatoSonido = FormatoSonido.OGG;
                    break;
                default:
                    break;
            }
            try
            {
                Log.Instance.ImprimirMensaje("Intentando cargar " + cual, TipoMensaje.Info);
                if (Path.GetExtension(cual) == ".ogg")
                {
                    FileStream stream = new FileStream(cual, FileMode.Open);
                    NVorbis = new NVorbisSource(stream);
                    _sonido = NVorbis.ToWaveSource(16);
                }
                else
                {
                    _sonido = CSCore.Codecs.CodecFactory.Instance.GetCodec(cual).ToSampleSource().ToStereo().ToWaveSource(16);
                    notificationStream = new SingleBlockNotificationStream(_sonido.ToSampleSource());
                    //_salida.Initialize(notificationStream.ToWaveSource(16));
                    FileInfo info = new FileInfo(cual);
                    tamFich = info.Length;
                }
                _salida = new WasapiOut(false, AudioClientShareMode.Shared, 50);
                _salida.Initialize(_sonido);
            }
            catch (Exception)
            {
                Log.Instance.ImprimirMensaje("No se encuentra el fichero", TipoMensaje.Advertencia);
                throw;
            }

        }
        public void Reproducir()
        {
            if (_salida != null)
                _salida.Play();
        }
        public void Pausar()
        {
            if (_salida != null)
                _salida.Pause();
        }
        public void Saltar(TimeSpan a)
        {
            _salida.WaveSource.SetPosition(a);
        }
        public TimeSpan Duracion()
        {
            return _sonido.GetLength();
        }
        public TimeSpan Posicion()
        {
            return _sonido.GetPosition();
        }
        private void Limpiar()
        {
            if (_salida != null)
            {
                _salida.Dispose();
                _salida = null;
            }
            if (_sonido != null)
            {
                _sonido.Position = 1;
                _sonido.Dispose();
                _sonido = null;
            }
        }
        public void Apagar() { Limpiar(); }
        public String CancionReproduciendose()
        {
            switch (FormatoSonido)
            {
                case FormatoSonido.MP3:
                    if (tags != null)
                        return tags.LeadPerformers + " - " + tags.Title;
                    else return null;
                case FormatoSonido.FLAC:
                    return _ficheroFLAC.ARTIST + " - " + _ficheroFLAC.TITLE;
                case FormatoSonido.OGG:
                    try
                    {
                        artista = NVorbis.GetArtista();
                        titulo = NVorbis.GetTitulo();
                        return artista + " - " + titulo;
                    }
                    catch (NullReferenceException)
                    {
                        return null;
                    }
                default:
                    return null;
            }
            return null;
        }
        public System.Drawing.Image GetCaratula()
        {
            switch (FormatoSonido)
            {
                case FormatoSonido.MP3:
                    return tags.Image;
                case FormatoSonido.FLAC:
                    return null;
                case FormatoSonido.OGG:
                    return null;
                default:
                    return null;
            }
        }
        public void ConfigurarOGG()
        {
            CSCore.Codecs.CodecFactory.Instance.Register("ogg-vorbis", new CSCore.Codecs.CodecFactoryEntry(s => new NVorbisSource(s).ToWaveSource(), ".ogg"));
        }
        public String GetDatos()
        {
            if(FormatoSonido != FormatoSonido.OGG)
            {
                int kbps = (int)((tamFich / _sonido.GetLength().TotalSeconds) / 128);
                return _sonido.WaveFormat.SampleRate / 1000 + "kHz - " + kbps + "kbps medio";
            }
            else
                return _sonido.WaveFormat.SampleRate / 1000 + "kHz - " + NVorbis.Bitrate/1024 + "kbps";

        }
        public void SetVolumen(float v)
        {
            _salida.Volume = v;
        }
        public void Detener() //detiene una canción
        {
            _salida.Pause();
            _sonido.SetPosition(TimeSpan.Zero);
        }
    }
}
