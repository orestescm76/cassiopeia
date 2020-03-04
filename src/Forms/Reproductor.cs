using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;

namespace aplicacion_musica
{
    public enum EstadoReproductor
    {
        Reproduciendo,
        Pausado,
        Detenido
    }
    public partial class Reproductor : Form
    {
        private readonly ReproductorNucleo nucleo = new ReproductorNucleo();
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
        private string fich;
        public EstadoReproductor estadoReproductor;
        public Reproductor()
        {
            InitializeComponent();
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
        }
        public Reproductor(Cancion c)
        {
            
        }
        public void test()
        {
            //nucleo.CargarCancion("S:/Música/doctor.mp3", _devices[0]);
            //trackBarPosicion.Maximum = (int)nucleo.Duracion().TotalSeconds; 
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }


        private void Reproductor_Load(object sender, EventArgs e)
        {
            using (var enumerador = new MMDeviceEnumerator())
            {
                using (var mmColeccion = enumerador.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var item in mmColeccion)
                    {
                        _devices.Add(item);
                    }
                }
            }
            test();
        }

        private void timerCancion_Tick(object sender, EventArgs e)
        {

            TimeSpan dur = nucleo.Duracion();
            TimeSpan pos = nucleo.Posicion();
            if(pos.Seconds < 10)
                labelPosicion.Text = (int)pos.TotalMinutes + ":0" + (int)pos.Seconds;
            else
                labelPosicion.Text = (int)pos.TotalMinutes + ":" + (int)pos.Seconds;
            if (pos > dur)
                dur = pos;
            double val = pos.TotalMilliseconds / dur.TotalMilliseconds * trackBarPosicion.Maximum;
            trackBarPosicion.Value = (int)val;
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            nucleo.Apagar();
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fich = null;
            openFileDialog1.Filter = "*.mp3|*.mp3";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                fich = openFileDialog1.FileName;
                nucleo.Apagar();
                this.fich = fich;
                nucleo.CargarCancion(fich, _devices[0]);
                trackBarPosicion.Maximum = (int)nucleo.Duracion().TotalSeconds;
                timerCancion.Enabled = true;
                labelDuracion.Text = (int)nucleo.Duracion().TotalMinutes + ":" + nucleo.Duracion().Seconds;
            }
        }

        private void buttonReproducirPausar_Click(object sender, EventArgs e)
        {
            switch (estadoReproductor)
            {
                case EstadoReproductor.Reproduciendo:
                    nucleo.Pausar();
                    estadoReproductor = EstadoReproductor.Pausado;
                    buttonReproducirPausar.Text = "▶";
                    break;
                case EstadoReproductor.Pausado:
                    nucleo.Reproducir();
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    break;
                case EstadoReproductor.Detenido:
                    nucleo.Reproducir();
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    Text = fich;
                    break;
                default:
                    break;
            }

            
        }
    }
}
