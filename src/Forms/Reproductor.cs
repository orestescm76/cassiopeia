using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using System.Drawing;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.IO;

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
        private bool TiempoRestante = false;
        ToolTip DuracionSeleccionada;
        ToolTip VolumenSeleccionado;
        TimeSpan dur;
        TimeSpan pos;
        bool Spotify;
        SpotifyWebAPI _spotify;
        //todo: crear una tarea que cada 1s me cambie la cancion, todo
        public Reproductor()
        {
            InitializeComponent();
            nucleo.ConfigurarOGG();
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            if (Programa._spotify.cuentaVinculada)
            {
                Spotify = true;
                _spotify = Programa._spotify._spotify;
                PlaybackContext PC = _spotify.GetPlayback("ES");
                Text = PC.Item.Artists[0].Name + " - " + PC.Item.Name;
                DescargarPortada(PC.Item.Album);
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile("./covers/np.jpg");
            }

        }
        private void DescargarPortada(SimpleAlbum album)
        {
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    if(File.Exists("./covers/np.jpg") && pictureBoxCaratula.Image != null)
                        pictureBoxCaratula.Image.Dispose();
                    cliente.DownloadFile(new Uri(album.Images[1].Url), Environment.CurrentDirectory + "/covers/np.jpg");
                }
                catch (System.Net.WebException)
                {
                    Console.WriteLine("Excepción capturada System.Net.WebException");
                    MessageBox.Show("");
                }

            }
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
            DuracionSeleccionada.SetToolTip(trackBarPosicion, new TimeSpan(0, 0, trackBarPosicion.Value).ToString());
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

            dur =  nucleo.Duracion();
            pos = nucleo.Posicion();
            if (pos.Seconds < 10)
                labelPosicion.Text = (int)pos.TotalMinutes + ":0" + (int)pos.Seconds;
            else
                labelPosicion.Text = (int)pos.TotalMinutes + ":" + (int)pos.Seconds;
            if (pos > dur)
                dur = pos;
            if(TiempoRestante)
            {
                int secsRestantes = (int)((dur.TotalSeconds - pos.TotalSeconds) % 60);
                int minsRestantes = (int)((dur.TotalSeconds - pos.TotalSeconds) / 60);
                if(secsRestantes < 10)
                    labelDuracion.Text = "-" + minsRestantes + ":0" + secsRestantes; //??
                else
                    labelDuracion.Text = "-" + minsRestantes + ":" + secsRestantes; //??
            }
            else
            {
                if (dur.Seconds < 10)
                    labelDuracion.Text = (int)dur.TotalMinutes + ":0" + (int)dur.Seconds;
                else
                    labelDuracion.Text = (int)dur.TotalMinutes + ":" + (int)dur.Seconds;
            }
            double val = pos.TotalMilliseconds / dur.TotalMilliseconds * trackBarPosicion.Maximum;
            trackBarPosicion.Value = (int)val;
            if (pos == dur)
            {
                estadoReproductor = EstadoReproductor.Detenido;
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            nucleo.Apagar();
            pictureBoxCaratula.Image.Dispose();
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fich = null;
            openFileDialog1.Filter = "*.mp3, *.flac, *.ogg|*.mp3;*.flac;*.ogg";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                fich = openFileDialog1.FileName;
                nucleo.Apagar();
                estadoReproductor = EstadoReproductor.Detenido;
                buttonReproducirPausar.Text = "▶";
                this.fich = fich;
                nucleo.CargarCancion(fich);
                trackBarPosicion.Maximum = (int)nucleo.Duracion().TotalSeconds;
                timerCancion.Enabled = true;
                labelDuracion.Text = (int)nucleo.Duracion().TotalMinutes + ":" + nucleo.Duracion().Seconds;
                Text = nucleo.CancionReproduciendose();
                labelDatosCancion.Text = nucleo.GetDatos();
                trackBarVolumen.Value = 100;
                try
                {
                    pictureBoxCaratula.Image = nucleo.GetCaratula();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void buttonReproducirPausar_Click(object sender, EventArgs e)
        {
            switch (estadoReproductor)
            {
                case EstadoReproductor.Reproduciendo:
                    estadoReproductor = EstadoReproductor.Pausado;
                    buttonReproducirPausar.Text = "▶";
                    if (!Spotify)
                        nucleo.Pausar();
                    else
                        _spotify.PausePlayback();
                    break;
                case EstadoReproductor.Pausado:
                    if (!Spotify)
                        nucleo.Reproducir();
                    else
                        _spotify.ResumePlayback("", "", null, "");
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    break;
                case EstadoReproductor.Detenido:
                    if(!Spotify)
                        nucleo.Reproducir();
                    else
                        _spotify.ResumePlayback("", "", null, "");
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    break;
                default:
                    break;
            }
        }

        private void labelDuracion_Click(object sender, EventArgs e)
        {
            if (TiempoRestante)
                TiempoRestante = false;
            else TiempoRestante = true;
        }

        private void trackBarPosicion_MouseDown(object sender, MouseEventArgs e)
        {
            timerCancion.Enabled = false;
            trackBarPosicion.Value = (int)((e.X * dur.TotalSeconds) / Size.Width);
        }

        private void trackBarPosicion_MouseUp(object sender, MouseEventArgs e)
        {
            nucleo.Saltar(new TimeSpan(0,0,trackBarPosicion.Value));
            timerCancion.Enabled = true;
        }

        private void trackBarVolumen_Scroll(object sender, EventArgs e)
        {
            float vol = trackBarVolumen.Value;
            nucleo.SetVolumen(vol / 100);
        }

        private void trackBarVolumen_MouseDown(object sender, MouseEventArgs e)
        {
            VolumenSeleccionado.SetToolTip(trackBarVolumen, trackBarVolumen.Value + "%");
            float vol = trackBarVolumen.Value;
            nucleo.SetVolumen(vol / 100);
        }

        private void trackBarVolumen_MouseHover(object sender, EventArgs e)
        {
            VolumenSeleccionado.SetToolTip(trackBarVolumen, trackBarVolumen.Value + "%");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
