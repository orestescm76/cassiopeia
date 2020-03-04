using System;
using System.IO;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using NVorbis.Ogg;

namespace aplicacion_musica
{
    class ReproductorNucleo
    {
        private DirectSoundOut _salida;
        private IWaveSource _sonido;
        private MemoryStream BufferCancion;
        private CSCore.Codecs.MP3.Mp3MediafoundationDecoder _decodificadorMP3;
        private CSCore.Tags.ID3.ID3v2QuickInfo tags;
        private CSCore.Codecs.FLAC.FlacFile _ficheroFLAC;
        private CSCore.Codecs.FLAC.FlacMetadata _metadatosFLAC;
        bool isFLAC = false;
        public void CargarCancion(string cual, MMDevice dispositivo)
        {
            if(cual.EndsWith(".ogg"))
            {
                ContainerReader ogg = new ContainerReader(cual);
            }
            else if(cual.EndsWith(".mp3"))
            {
                _decodificadorMP3 = new CSCore.Codecs.MP3.Mp3MediafoundationDecoder(cual);
                _sonido = _decodificadorMP3.ToStereo();
                CSCore.Tags.ID3.ID3v2 mp3tag = CSCore.Tags.ID3.ID3v2.FromFile(cual);
                tags = new CSCore.Tags.ID3.ID3v2QuickInfo(mp3tag);
                _salida = new DirectSoundOut();
                _salida.Initialize(_sonido);
            }
            else if(cual.EndsWith(".flac"))
            {
                isFLAC = true;
                _ficheroFLAC = new CSCore.Codecs.FLAC.FlacFile(cual);
                _sonido = _ficheroFLAC.ToStereo();
                _salida = new DirectSoundOut();
                _salida.Initialize(_sonido);
            }
            else
            {

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
            if(_salida != null)
            {
                _salida.Dispose();
                _salida = null;
            }
            if(_sonido != null)
            {
                _sonido.Dispose();
                _sonido = null;
            }
        }
        public void Apagar() { Limpiar(); }
        public String CancionReproduciendose()
        {
            return isFLAC ? "" : tags.LeadPerformers + " - " + tags.Title;
        }
        public System.Drawing.Image GetCaratula() { return tags.Image == null ? null:tags.Image; }
    }
    
}
