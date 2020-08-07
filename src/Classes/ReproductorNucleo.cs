using System;
using System.IO;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using NVorbis;
using JAudioTags;
using SpotifyAPI.Web;
using aplicacion_musica.CD;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public enum FormatoSonido
    {
        MP3,
        FLAC,
        OGG,
        CDA
    }
    class ReproductorNucleo
    {
        private ISoundOut _salida;
        public FormatoSonido FormatoSonido { get; private set; }
        private IWaveSource _sonido;
        private CSCore.Tags.ID3.ID3v2QuickInfo tags;
        private FLACFile _ficheroFLAC;
        private SingleBlockNotificationStream notificationStream;
        private NVorbisSource NVorbis;
        private String artista;
        private String titulo;
        private CDDrive Disquetera;
        public PistaCD[] PistasCD { private set; get; }
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
                    CSCore.Codecs.FLAC.FlacFile ff = new CSCore.Codecs.FLAC.FlacFile(cual);
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
                _salida = new WasapiOut(false, AudioClientShareMode.Shared, 100);
                _sonido.Position = 0;
                _salida.Initialize(_sonido);
                Log.Instance.ImprimirMensaje("Cargado correctamente" + cual, TipoMensaje.Correcto);
            }
            catch (IOException)
            {
                Log.Instance.ImprimirMensaje("No se puede reproducir el fichero porque está siendo utilizado por otro proceso", TipoMensaje.Error);
                throw;
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
            if (FormatoSonido != FormatoSonido.OGG)
                return _sonido.GetLength();
            else if (FormatoSonido == FormatoSonido.CDA)
                return SectoresATimeSpan(_sonido.Length);
            else
                return NVorbis.Duracion;
        }
        public TimeSpan Posicion()
        {
            if (FormatoSonido != FormatoSonido.CDA)
                return _sonido.GetPosition();
            else
                return SectoresATimeSpan(_sonido.Position);
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
                _sonido.Dispose();
                _sonido = null;
            }
        }
        public bool ComprobarSonido()
        {
            if (_sonido == null || _salida == null)
                return false;
            else return true;
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
                        Log.Instance.ImprimirMensaje("No hay metadatos", TipoMensaje.Advertencia);
                        return null;
                    }
                default:
                    return null;
            }
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
        public String GetDatos()
        {
            switch (FormatoSonido)
            {
                case FormatoSonido.MP3:
                case FormatoSonido.FLAC:
                    return _sonido.WaveFormat.SampleRate / 1000 + "kHz - " + NVorbis.Bitrate / 1024 + "kbps";
                case FormatoSonido.OGG:
                    int kbps = (int)((tamFich / _sonido.GetLength().TotalSeconds) / 128);
                    return _sonido.WaveFormat.SampleRate / 1000 + "kHz - " + kbps + "kbps medio";
                case FormatoSonido.CDA:
                    return "44.1 kHz - 16 bits. CD-A. / 176000 bytes/s";
                default:
                    return string.Empty;
            }
        }
        public void SetVolumen(float v)
        {
            _salida.Volume = v;
        }
        public void Detener() //detiene una canción
        {
            _salida.Stop();
            _sonido.SetPosition(TimeSpan.Zero);
        }
        public TimeSpan SectoresATimeSpan(long sec)
        {
            TimeSpan dur;
            double secs = sec / 75.0;
            dur = TimeSpan.FromSeconds(secs);
            return dur;
        }
        public long TimeSpanASectores(TimeSpan dur)
        {
            return (long)Math.Floor(dur.TotalSeconds * 75);
        }
        //Lee un cd de audio segun los ficheros CDA que genera Windows
        public PistaCD[] LeerCD(char Disco)
        {
            Log.Instance.ImprimirMensaje("Leyendo CD", TipoMensaje.Info);
            DirectoryInfo DiscoD = null;
            FileInfo[] Ficheros = null;
            try
            {
                DiscoD = new DirectoryInfo(Disco + ":\\");
                Ficheros = DiscoD.GetFiles();
            }
            catch (IOException)
            {
                Log.Instance.ImprimirMensaje("No se puede leer el CD. El dispositivo no está preparado...", TipoMensaje.Error);
                MessageBox.Show(Programa.textosLocal.GetString("errorCD"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            PistaCD[] Pistas = new PistaCD[Ficheros.Length];
            //Lectura .CDA
            string ID = "";
            for (int i = 0; i < Ficheros.Length; i++)
            {
                using (BinaryReader br = new BinaryReader(new FileStream(Ficheros[i].FullName, FileMode.Open, FileAccess.Read)))
                {
                    br.ReadBytes(24);
                    byte[] idR = br.ReadBytes(4);
                    byte[] id = new byte[4];
                    int c = 0;
                    for (int j = 3; j >= 0; j--)
                    {
                        id[c] = idR[j];
                        c++;
                    }
                    ID = BitConverter.ToString(id);
                    ID = ID.Replace("-", "");
                    uint sectorInicial = br.ReadUInt32();
                    uint duracionSectores = br.ReadUInt32();
                    Pistas[i] = new PistaCD(sectorInicial, sectorInicial + duracionSectores, ID);
                }
            }
            return Pistas;
        }
        public void ReproducirCD(char disp)
        {
            Disquetera = CDDrive.Open(disp);
            if (Disquetera == null)
            {
                Log.Instance.ImprimirMensaje("No se puede leer el CD. El dispositivo no está preparado...", TipoMensaje.Error);
                throw new IOException();
            }
            FormatoSonido = FormatoSonido.CDA;
            PistaCD[] Pistas = LeerCD(disp);
            if (Pistas == null)
                return;
            PistasCD = Pistas;
            _salida = new WasapiOut(false, AudioClientShareMode.Shared, 100);
            _sonido = Disquetera.ReadTrack(Pistas[0]);
            _salida.Initialize(_sonido);
            _salida.Play();
            Log.Instance.ImprimirMensaje("Se ha cargado correctamente el CD", TipoMensaje.Correcto);
        }
        public void SaltarCancionCD(int cual) //sobre 0
        {
            _salida.Stop();
            _sonido = Disquetera.ReadTrack(PistasCD[cual]);
            _salida.Initialize(_sonido);
            _salida.Play();
        }
        public void SaltarCD(int sector)
        {
            _sonido.Position = sector;
        }
    }
}
