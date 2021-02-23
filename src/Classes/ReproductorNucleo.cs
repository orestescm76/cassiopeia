using System;
using System.IO;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using Cassiopea.CD;
using System.Windows.Forms;

namespace Cassiopea
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
        private SingleBlockNotificationStream notificationStream;
        private NVorbisSource NVorbis;
        private CDDrive Disquetera;
        public PistaCD[] PistasCD { private set; get; }
        long tamFich;
        public void CargarCancion(string cual)
        {
            switch (Path.GetExtension(cual))
            {
                case ".mp3":
                    FormatoSonido = FormatoSonido.MP3;
                    break;
                case ".flac":
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
                Log.Instance.PrintMessage("Intentando cargar " + cual, MessageType.Info);
                if (Path.GetExtension(cual) == ".ogg")
                {
                    FileStream stream = new FileStream(cual, FileMode.Open, FileAccess.Read);
                    NVorbis = new NVorbisSource(stream);
                    _sonido = NVorbis.ToWaveSource(16);
                }
                else
                {
                    _sonido = CSCore.Codecs.CodecFactory.Instance.GetCodec(cual).ToSampleSource().ToStereo().ToWaveSource(16);
                    notificationStream = new SingleBlockNotificationStream(_sonido.ToSampleSource());
                    FileInfo info = new FileInfo(cual);
                    tamFich = info.Length;
                }
                
                _salida = new WasapiOut(false, AudioClientShareMode.Shared, 100);
                //_sonido.Position = 0;
                _salida.Initialize(_sonido);
                Log.Instance.PrintMessage("Cargado correctamente" + cual, MessageType.Correct);
            }
            catch (IOException ex)
            {
                Log.Instance.PrintMessage("Error de IO", MessageType.Error);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                _salida = null;
                _sonido = null;
                throw;
            }
            catch (Exception ex)
            {
                Log.Instance.PrintMessage("Hubo un problema...", MessageType.Error);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                _salida = null;
                _sonido = null;
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
            {
                try
                {
                    return NVorbis.Length_Timespan;
                }
                catch (Exception)
                {
                }
                return TimeSpan.Zero;
            }
                
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
        public String GetDatos()
        {
            switch (FormatoSonido)
            {
                case FormatoSonido.OGG:
                    return _sonido.WaveFormat.SampleRate / 1000 + "kHz - " + NVorbis.Bitrate / 1024 + "kbps";
                case FormatoSonido.MP3:
                case FormatoSonido.FLAC:
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
            if(!(_salida is null))
                _salida.Volume = v;
        }
        public void Detener() //detiene una canción
        {
            _salida.Stop();
            _sonido.SetPosition(TimeSpan.Zero);
        }
        public TimeSpan SectoresATimeSpan(long sector)
        {
            double secs = sector / 75.0;
            return TimeSpan.FromSeconds(secs);
        }
        public long TimeSpanASectores(TimeSpan dur)
        {
            return (long)Math.Floor(dur.TotalSeconds * 75);
        }
        //Lee un cd de audio segun los ficheros CDA que genera Windows
        public PistaCD[] LeerCD(char Disco)
        {
            Log.Instance.PrintMessage("Leyendo CD", MessageType.Info);
            DirectoryInfo DiscoD = null;
            FileInfo[] Ficheros = null;
            try
            {
                DiscoD = new DirectoryInfo(Disco + ":\\");
                Ficheros = DiscoD.GetFiles();
            }
            catch (IOException)
            {
                Log.Instance.PrintMessage("No se puede leer el CD. El dispositivo no está preparado...", MessageType.Error);
                MessageBox.Show(Program.LocalTexts.GetString("errorCD"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        id[c++] = idR[j];
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
                Log.Instance.PrintMessage("No se puede leer el CD. El dispositivo no está preparado...", MessageType.Error);
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
            Log.Instance.PrintMessage("Se ha cargado correctamente el CD", MessageType.Correct);
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
