using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using System.Drawing;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.IO;
using System.ComponentModel;

namespace aplicacion_musica
{
    public enum EstadoReproductor
    {
        Reproduciendo,
        Pausado,
        Detenido
    }
    /*
     * τοδο:
     * consola y visualizacion UI.
     */
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
        FullTrack cancionReproduciendo;
        private BackgroundWorker backgroundWorker;
        public ListaReproduccion ListaReproduccion { get; set; }
        private uint ListaReproduccionPuntero;
        bool SpotifyListo = false;
        bool Aleatorio = false;
        bool EsPremium;
        DirectoryInfo directorioCanciones;
        private Log Log = Log.Instance;
        private static Reproductor ins = new Reproductor();
        private float Volumen;
        //crear una tarea que cada 500ms me cambie la cancion
        public static Reproductor Instancia { get { return ins; } }
        private Reproductor()
        {
            InitializeComponent();
            nucleo.ConfigurarOGG();
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            Volumen = 1.0f;
            trackBarVolumen.Value = 100;
            //button1.Hide();
            //button2.Hide();
        }
        public void ReproducirLista(ListaReproduccion lr)
        {
            ListaReproduccion = lr;
            ListaReproduccionPuntero = 0;
            Cancion c = lr.GetCancion(ListaReproduccionPuntero);
            ReproducirCancion(c);
        }
        private void ReproducirCancion(Cancion c)
        {
            estadoReproductor = EstadoReproductor.Detenido;
            directorioCanciones = new DirectoryInfo(c.album.DirectorioSonido);
            string s = "";
            var fichs = directorioCanciones.GetFiles();
            foreach (FileInfo file in directorioCanciones.GetFiles())
            {
                if (file.FullName.Contains(c.titulo) && FicheroLeible(file.FullName))
                {
                    s = file.FullName;
                    break;
                }
            }
            nucleo.Apagar();
            buttonReproducirPausar.Text = "▶";
            nucleo.CargarCancion(s);
            trackBarPosicion.Maximum = (int)nucleo.Duracion().TotalSeconds;
            timerCancion.Enabled = true;
            labelDuracion.Text = (int)nucleo.Duracion().TotalMinutes + ":" + nucleo.Duracion().Seconds;
            Text = nucleo.CancionReproduciendose();
            labelDatosCancion.Text = nucleo.GetDatos();
            dur = nucleo.Duracion();
            nucleo.Reproducir();
            estadoReproductor = EstadoReproductor.Reproduciendo;
            if (c.album.caratula != null)
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.caratula);
        }
        private bool FicheroLeible(string s)
        {
            if (Path.GetExtension(s) == ".mp3")
                return true;
            else if (Path.GetExtension(s) == ".ogg")
                return true;
            else if (Path.GetExtension(s) == ".flac")
                return true;
            else return false;
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PlaybackContext PC = (PlaybackContext)e.Result;
            if(PC != null && PC.Item != null)
            {
                dur = new TimeSpan(0, 0, 0, 0, PC.Item.DurationMs);

                pos = new TimeSpan(0, 0, 0, 0, PC.ProgressMs);
                if (PC.IsPlaying)
                {
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    timerCancion.Enabled = true;
                }
                else
                {
                    estadoReproductor = EstadoReproductor.Pausado;
                    buttonReproducirPausar.Text = "▶";
                    timerCancion.Enabled = false;
                }
                if (PC.Item.Id != cancionReproduciendo.Id || pictureBoxCaratula.Image == null)
                {
                    trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                    DescargarPortada(PC.Item.Album);
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile("./covers/np.jpg");
                    labelDatosCancion.Text = "BPM: " + _spotify.GetAudioFeatures(PC.Item.Id).Tempo + "bpm";
                }
                if (PC.ShuffleState)
                    checkBoxAleatorio.Checked = true;
                else
                    checkBoxAleatorio.Checked = false;
                cancionReproduciendo = PC.Item;
                Text = PC.Item.Artists[0].Name + " - " + cancionReproduciendo.Name;
                trackBarVolumen.Value = PC.Device.VolumePercent;

            }
            else
            {
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(!Programa._spotify.TokenExpirado())
            {
                PlaybackContext PC = _spotify.GetPlayback();
                e.Result = PC;
            }
            else
            {
                Programa._spotify.RefrescarToken();
            }
        }
        /*
        public Reproductor (bool S = false)
        {
            //SIN SPOTIFY
            InitializeComponent();
            nucleo.ConfigurarOGG();
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            if (Programa._spotify.cuentaVinculada && S)
            {
                Spotify = true;
                _spotify = Programa._spotify._spotify;
                PlaybackContext PC = _spotify.GetPlayback("ES");
                if (_spotify.GetPrivateProfile().Product != "premium")
                    EsPremium = false;
                else
                    EsPremium = true;
                SetInfoSpotify(PC);

                DescargarPortada(PC.Item.Album);
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile("./covers/np.jpg");
            }
        }*/
        private void DescargarPortada(SimpleAlbum album)
        {
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
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
            if (Programa._spotify != null && Programa._spotify.cuentaVinculada)
            {
                Spotify = true;
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += BackgroundWorker_DoWork;
                backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                cancionReproduciendo = new FullTrack();
                _spotify = Programa._spotify._spotify;
                SpotifyListo = true;
                timerSpotify.Enabled = true;
                toolStripStatusLabelCorreoUsuario.Text = "Conectado como " + _spotify.GetPrivateProfile().DisplayName;
            }
        }

        private void timerCancion_Tick(object sender, EventArgs e)
        {
            if (estadoReproductor == EstadoReproductor.Detenido)
                trackBarPosicion.Enabled = false;
            else
                trackBarPosicion.Enabled = true;
            if (!Spotify && timerCancion.Enabled)
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
                    labelDuracion.Text = "-" + minsRestantes + ":0" + secsRestantes; 
                else
                    labelDuracion.Text = "-" + minsRestantes + ":" + secsRestantes; 
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
                if(ListaReproduccion != null)
                {
                    ListaReproduccionPuntero++;
                    ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                }
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            nucleo.Apagar();
            if(pictureBoxCaratula.Image != null)
                pictureBoxCaratula.Image.Dispose();
            Dispose();*/
            Hide();
            e.Cancel = true;
        }
        public void Apagar()
        {
            nucleo.Apagar();
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
                dur = nucleo.Duracion();
                try
                {
                    pictureBoxCaratula.Image = nucleo.GetCaratula();
                }
                catch (Exception)
                {

                    pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
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
                    {
                        ErrorResponse err = _spotify.ResumePlayback("", "", null, "", 0);
                        if(err.Error != null)
                            Console.WriteLine(err.Error.Message);
                    }

                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    break;
                case EstadoReproductor.Detenido:
                    if(!Spotify)
                        nucleo.Reproducir();
                    else
                        _spotify.ResumePlayback("", "", null, 0);
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
            timerSpotify.Enabled = false;
            trackBarPosicion.Value = (int)((e.X * dur.TotalSeconds) / Size.Width);
        }

        private void trackBarPosicion_MouseUp(object sender, MouseEventArgs e)
        {

            if (!Spotify)
            {
                nucleo.Saltar(new TimeSpan(0, 0, trackBarPosicion.Value));
                timerCancion.Enabled = true;
            }
            else
            {
                _spotify.SeekPlayback(trackBarPosicion.Value * 1000);
                timerSpotify.Enabled = true;
            }
        }
        private void trackBarPosicion_Scroll(object sender, EventArgs e)
        {
            timerCancion.Enabled = false;
            pos = new TimeSpan(0, 0, trackBarPosicion.Value);
            timerCancion_Tick(null, null);
        }
        private void trackBarVolumen_Scroll(object sender, EventArgs e)
        {
            Volumen = (float)trackBarVolumen.Value / 100;
            if (!Spotify)
                nucleo.SetVolumen(Volumen);
            else
                _spotify.SetVolume(trackBarVolumen.Value);
        }

        private void trackBarVolumen_MouseDown(object sender, MouseEventArgs e)
        {
            Volumen = (float)trackBarVolumen.Value / 100;
        }

        private void trackBarVolumen_MouseHover(object sender, EventArgs e)
        {
            VolumenSeleccionado.SetToolTip(trackBarVolumen, trackBarVolumen.Value + "%");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            estadoReproductor = EstadoReproductor.Detenido;
            Spotify = false;
            timerSpotify.Enabled = false;
            timerCancion.Enabled = false;
            pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
            dur = new TimeSpan(0);
            pos = new TimeSpan(0);
            _spotify = null;
        }

        private void SetInfoSpotify(PlaybackContext PC)
        {
            cancionReproduciendo = PC.Item;
            if (PC.IsPlaying)
            {
                timerCancion.Enabled = true;
                estadoReproductor = EstadoReproductor.Reproduciendo;
            }
            Text = PC.Item.Artists[0].Name + " - " + PC.Item.Name;
        }

        private void timerSpotify_Tick(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        private void trackBarVolumen_ValueChanged(object sender, EventArgs e)
        {
            labelVolumen.Text = trackBarVolumen.Value.ToString() + "%";
            
        }

        private void trackBarPosicion_ValueChanged(object sender, EventArgs e)
        {
            labelPorcentaje.Text = trackBarPosicion.Value * 100 / trackBarPosicion.Maximum + "%";
        }

        private void checkBoxAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            if(SpotifyListo && Spotify)
                _spotify.SetShuffle(checkBoxAleatorio.Checked);
        }

        private void buttonSaltarAdelante_Click(object sender, EventArgs e)
        {
            if (SpotifyListo && Spotify)
                _spotify.SkipPlaybackToNext();
            else
            {
                if (ListaReproduccion != null)
                {
                    ListaReproduccionPuntero++;
                    ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                }
            }
        }

        private void buttonSaltarAtras_Click(object sender, EventArgs e)
        {
            if (SpotifyListo && Spotify)
                _spotify.SkipPlaybackToPrevious();
            else
            {
                if (ListaReproduccion != null)
                {
                    ListaReproduccionPuntero--;
                    ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                }
            }
        }
    }
}
