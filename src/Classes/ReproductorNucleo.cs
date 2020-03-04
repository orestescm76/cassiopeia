using System;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using NVorbis.Ogg;

namespace aplicacion_musica
{
    class ReproductorNucleo
    {
        private ISoundOut _salida;
        private IWaveSource _sonido;
        public void CargarCancion(string cual, MMDevice dispositivo)
        {
            if(cual.EndsWith(".ogg"))
            {
                ContainerReader ogg = new ContainerReader(cual);
            }
            else
            {
                _sonido = CodecFactory.Instance.GetCodec(cual).ToSampleSource().ToStereo().ToWaveSource();
                _salida = new WasapiOut() { Latency = 100, Device = dispositivo };
                _salida.Initialize(_sonido);
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
    }
    
}
