using Cassiopeia.CD;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cassiopeia.src.Classes
{
    public enum FormatoSonido
    {
        MP3,
        FLAC,
        OGG,
        CDA
    }
    class PlayerKernel
    {
        private ISoundOut _output;
        public FormatoSonido FormatoSonido { get; private set; }
        private IWaveSource _sound;
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
                    _sound = NVorbis.ToWaveSource(16);
                }
                else
                {
                    _sound = CSCore.Codecs.CodecFactory.Instance.GetCodec(cual).ToSampleSource().ToStereo().ToWaveSource(16);
                    notificationStream = new SingleBlockNotificationStream(_sound.ToSampleSource());
                    FileInfo info = new FileInfo(cual);
                    tamFich = info.Length;
                }

                _output = new WasapiOut(false, AudioClientShareMode.Shared, 100);
                //_sonido.Position = 0;
                _output.Initialize(_sound);
                Log.Instance.PrintMessage("Loaded " + cual, MessageType.Correct);
            }
            catch (IOException ex)
            {
                Log.Instance.PrintMessage("Error de IO", MessageType.Error);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                Kernel.ShowError(Kernel.LocalTexts.GetString("errorReproduccion"));
                _output = null;
                _sound = null;
                throw;
            }
            catch (Exception ex)
            {
                Log.Instance.PrintMessage("Hubo un problema...", MessageType.Error);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                Kernel.ShowError(ex.Message);
                _output = null;
                _sound = null;
                throw;
            }

        }
        public void Reproducir()
        {
            if (_output != null)
                _output.Play();
        }
        public void Pausar()
        {
            if (_output != null)
                _output.Pause();
        }
        public void Saltar(TimeSpan a)
        {
            _output.WaveSource.SetPosition(a);
        }
        public TimeSpan Duracion()
        {
            if (FormatoSonido != FormatoSonido.OGG)
                return _sound.GetLength();
            else if (FormatoSonido == FormatoSonido.CDA)
                return SectoresATimeSpan(_sound.Length);
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
                return _sound.GetPosition();
            else
                return SectoresATimeSpan(_sound.Position);
        }
        private void Limpiar()
        {
            if (_output != null)
            {
                _output.Dispose();
                _output = null;
            }
            if (_sound != null)
            {
                _sound.Dispose();
                _sound = null;
            }
        }
        public bool ComprobarSonido()
        {
            return _sound == null || _output == null;
        }
        public void Apagar() { Limpiar(); }
        public String GetDatos()
        {
            switch (FormatoSonido)
            {
                case FormatoSonido.OGG:
                    return _sound.WaveFormat.SampleRate / 1000 + "kHz - " + NVorbis.Bitrate / 1024 + "kbps";
                case FormatoSonido.MP3:
                case FormatoSonido.FLAC:
                    int kbps = (int)((tamFich / _sound.GetLength().TotalSeconds) / 128);
                    return _sound.WaveFormat.SampleRate / 1000 + "kHz - " + kbps + "kbps medio";
                case FormatoSonido.CDA:
                    return "44.1 kHz - 16 bits. CD-A. / 176000 bytes/s";
                default:
                    return string.Empty;
            }
        }
        public void SetVolumen(float v)
        {
            if (!(_output is null))
                _output.Volume = v;
        }
        public void Stop() //detiene una canción
        {
            _output.Stop();
            _sound.SetPosition(TimeSpan.Zero);
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
            Log.Instance.PrintMessage("Reading CD", MessageType.Info);
            FileInfo[] Ficheros;
            try
            {
                DirectoryInfo DiscoD = new DirectoryInfo(Disco + ":\\");
                Ficheros = DiscoD.GetFiles();
            }
            catch (IOException)
            {
                Log.Instance.PrintMessage("Cannot read the CD Drive. Is the device ready?", MessageType.Error);
                Kernel.ShowError(Kernel.LocalTexts.GetString("errorCD"));
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
                Log.Instance.PrintMessage("Cannot read the CD Drive, the device is not ready.", MessageType.Error);
                throw new IOException();
            }
            FormatoSonido = FormatoSonido.CDA;
            PistaCD[] Pistas = LeerCD(disp);
            if (Pistas == null)
                return;
            PistasCD = Pistas;
            _output = new WasapiOut(false, AudioClientShareMode.Shared, 100);
            _sound = Disquetera.ReadTrack(Pistas[0]);
            _output.Initialize(_sound);
            _output.Play();
            Log.Instance.PrintMessage("CD has loaded correctly", MessageType.Correct);
        }
        public void SaltarCancionCD(int cual) //sobre 0
        {
            _output.Stop();
            _sound = Disquetera.ReadTrack(PistasCD[cual]);
            _output.Initialize(_sound);
            _output.Play();
        }
        public void SaltarCD(int sector)
        {
            _sound.Position = sector;
        }
    }
}
