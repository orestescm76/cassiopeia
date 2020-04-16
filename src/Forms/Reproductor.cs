using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using System.Drawing;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Linq;

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
     * consola y visualizacion UI. <
     */
    public partial class Reproductor : Form
    {
        public ListaReproduccion ListaReproduccion { get; set; }
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
        private int ListaReproduccionPuntero;
        bool SpotifyListo = false;
        bool EsPremium = false;
        DirectoryInfo directorioCanciones;
        PrivateProfile user;
        private Log Log = Log.Instance;
        private static Reproductor ins = new Reproductor();
        private float Volumen;
        private ListaReproduccionUI lrui;
        private ToolTip duracionView;
        private bool GuardarHistorial;
        private FileInfo Historial;
        private uint NumCancion;
        Cancion CancionLocalReproduciendo = null;
        private bool foobar2000 = true;
        Process foobar2kInstance = null;
        string SpotifyID = null;
        //crear una tarea que cada 500ms me cambie la cancion
        public static Reproductor Instancia { get { return ins; } }
        private Reproductor()
        {
            InitializeComponent();
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            Volumen = 1.0f;
            trackBarVolumen.Value = 100;
            duracionView = new ToolTip();
            buttonAgregar.Hide();
            if (!Programa.SpotifyActivado)
                buttonSpotify.Enabled = false;
            Icon = Properties.Resources.iconoReproductor;
            GuardarHistorial = false;
            if(GuardarHistorial)
            {
                DateTime now = DateTime.Now;
                Historial = new FileInfo("Log Musical " + now.Day + "-"+ now.Month + "-"+ now.Year+"-"+ now.Hour + "."+ now.Minute + ".txt");
                NumCancion = 1;
            }
            PonerTextos();
            if (Programa.ModoStream)
            {
                notifyIcon1.Visible = true;
                while (!Programa._spotify.cuentaLista)
                {
                    Thread.Sleep(100);
                }
                ActivarSpotify();
            }
            else notifyIcon1.Visible = false;
            buttonTwit.Enabled = false;
        }
        public void ReproducirLista(ListaReproduccion lr)
        {
            ListaReproduccion = lr;
            ListaReproduccionPuntero = 0;
            Cancion c = lr[ListaReproduccionPuntero];
            lrui = new ListaReproduccionUI(lr);
            ReproducirCancion(c);
        }
        public void RefrescarTextos()
        {
            PonerTextos();
        }
        private void PonerTextos()
        {
            Text = Programa.textosLocal.GetString("reproductor");
            buttonSpotify.Text = Programa.textosLocal.GetString("cambiarSpotify");
            notifyIcon1.Text = Programa.textosLocal.GetString("cerrarModoStream");
            buttoncrearLR.Text = Programa.textosLocal.GetString("crearLR");
            buttonAgregar.Text = Programa.textosLocal.GetString("agregarBD");
            buttonTwit.Text = Programa.textosLocal.GetString("twittearCancion");
        }
        public void SetPATH(Cancion c)
        {
            directorioCanciones = new DirectoryInfo(c.album.DirectorioSonido);
            foreach (FileInfo file in directorioCanciones.GetFiles())
            {
                if (CancionLocalReproduciendo == null || file.FullName == CancionLocalReproduciendo.PATH)
                    continue;
                try
                {
                    LectorMetadatos LM = new LectorMetadatos(file.FullName);
                    if (LM.Evaluable() && c.titulo.ToLower() == LM.Titulo.ToLower() && c.album.artista.ToLower() == LM.Artista.ToLower())
                    {
                        c.PATH = file.FullName;
                        break;
                    }
                    else
                    {
                        if (file.FullName.ToLower().Contains(c.titulo.ToLower()))
                        {
                            c.PATH = file.FullName;
                            Text = c.ToString();
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
        public void ReproducirCancion(Cancion c)
        {
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;

            estadoReproductor = EstadoReproductor.Detenido;
            string s = "";
            try
            {
                SetPATH(c);
            }
            catch (DirectoryNotFoundException)
            {
                Log.ImprimirMensaje("No se encuentra el directorio", TipoMensaje.Error);
                return;
            }
            CancionLocalReproduciendo = c;
            if (string.IsNullOrEmpty(c.PATH))
            {   
                MessageBox.Show(c.titulo + " " +c.album.nombre + Environment.NewLine + "ERROR_CANCION");
                return;
            }
            else
                s = c.PATH;
            nucleo.Apagar();
            try
            {
                nucleo.CargarCancion(CancionLocalReproduciendo.PATH);
                nucleo.Reproducir();
            }
            catch (Exception)
            {
                MessageBox.Show(Programa.textosLocal.GetString("errorReproduccion"));
                return;
            }
            if(GuardarHistorial)
            {
                using (StreamWriter escritor = new StreamWriter(Historial.FullName, true))
                {
                    escritor.WriteLine(NumCancion + " - " + c.album.artista + " - " + c.titulo);
                    NumCancion++;
                }
            }
            PrepararReproductor();
            if (c.album.caratula != null)
            {
                if(c.album.caratula!="")
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.caratula);
                else
                {
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.DirectorioSonido + "\\folder.jpg");
                }
            }
            timerCancion.Enabled = true;
            timerMetadatos.Enabled = true;
            buttonTwit.Enabled = true;
        }
        private void PrepararReproductor()
        {
            nucleo.SetVolumen(Volumen);
            dur = nucleo.Duracion();
            pos = TimeSpan.Zero;
            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
            timerCancion.Enabled = true;
            labelDuracion.Text = (int)dur.TotalMinutes + ":" + dur.Seconds;
            Text = nucleo.CancionReproduciendose();
            labelDatosCancion.Text = nucleo.GetDatos();
            estadoReproductor = EstadoReproductor.Reproduciendo;
            buttonReproducirPausar.Text = GetTextoReproductor(estadoReproductor);
        }
        private bool FicheroLeible(string s)
        {
            if (Path.GetExtension(s) == ".mp3")
            {
                timerMetadatos.Enabled = false;
                return true;
            }
            else if (Path.GetExtension(s) == ".ogg")
            {
                timerMetadatos.Enabled = true;
                return true;
            }
            else if (Path.GetExtension(s) == ".flac")
            {
                timerMetadatos.Enabled = false;
                return true;
            }
            else return false;
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PlaybackContext PC = (PlaybackContext)e.Result;
            if(PC != null && PC.Item != null)
            {
                dur = new TimeSpan(0, 0, 0, 0, PC.Item.DurationMs);
                pos = new TimeSpan(0, 0, 0, 0, PC.ProgressMs);
                SpotifyID = PC.Item.Id;
                if(!Programa.ModoStream)
                {
                    trackBarPosicion.Value = (int)pos.TotalSeconds;
                    if (PC.Item.Id != cancionReproduciendo.Id || pictureBoxCaratula.Image == null)
                    {
                        //using (StreamWriter escritor = new StreamWriter(Historial.FullName, true))
                        //{
                        //    escritor.WriteLine(NumCancion + " - " + PC.Item.Artists[0].Name + " - " + PC.Item.Name);
                        //    NumCancion++;
                        //}
                        if (!string.IsNullOrEmpty(PC.Item.Id))
                        {
                            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                            try
                            {
                                DescargarPortada(PC.Item.Album);
                                pictureBoxCaratula.Image = System.Drawing.Image.FromFile("./covers/np.jpg");
                            }
                            catch (Exception)
                            {
                                pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
                            }

                        }
                        else
                        {
                            Log.ImprimirMensaje("Se ha detectado una canción local.", TipoMensaje.Info);
                            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                            pictureBoxCaratula.Image.Dispose();
                            pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
                        }
                    }
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
                    if (PC.ShuffleState)
                        checkBoxAleatorio.Checked = true;
                    else
                        checkBoxAleatorio.Checked = false;
                    cancionReproduciendo = PC.Item;
                    Text = PC.Item.Artists[0].Name + " - " + cancionReproduciendo.Name;
                    trackBarVolumen.Value = PC.Device.VolumePercent;
                    if (string.IsNullOrEmpty(PC.Item.Id))
                        buttonAgregar.Enabled = false;
                    else
                        buttonAgregar.Enabled = true;
                }
                using(StreamWriter salida = new StreamWriter("np.txt"))
                {
                    TimeSpan np = TimeSpan.FromMilliseconds(PC.ProgressMs);
                    salida.WriteLine(PC.Item.Artists[0].Name + " - " + PC.Item.Name);
                    salida.Write(np.ToString(@"mm\:ss") + " / ");
                    salida.Write(dur.ToString(@"mm\:ss"));
                }

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
                Log.ImprimirMensaje("Token caducado!", TipoMensaje.Advertencia);
                while(Programa._spotify.TokenExpirado())
                {
                    Thread.Sleep(100);
                }
            }
        }
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
                    Log.ImprimirMensaje("Error descargando la imagen", TipoMensaje.Advertencia);
                    File.Delete("./covers/np.jpg");
                }
                catch(IOException)
                {
                    Log.ImprimirMensaje("Error descargando la imagen, no es posible reemplazar el fichero...", TipoMensaje.Error);
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            DuracionSeleccionada.SetToolTip(trackBarPosicion, new TimeSpan(0, 0, trackBarPosicion.Value).ToString());
        }
        private void PrepararSpotify()
        {
            Spotify = true;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            cancionReproduciendo = new FullTrack();
            _spotify = Programa._spotify._spotify;
            user = _spotify.GetPrivateProfile();
            EsPremium = (user.Product == "premium") ? true : false;
            Log.ImprimirMensaje("Iniciando el Reproductor en modo Spotify, con cuenta " + user.Email, TipoMensaje.Info);
            SpotifyListo = true;
            timerSpotify.Enabled = true;
            toolStripStatusLabelCorreoUsuario.Text = "Conectado como " + user.DisplayName;
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
            Log.ImprimirMensaje("Iniciando el Reproductor en modo local",TipoMensaje.Info);
            try
            {
                foobar2kInstance = Process.GetProcessesByName("foobar2000")[0];
                Log.ImprimirMensaje("Se ha encontrado foobar2000", TipoMensaje.Correcto);
            }
            catch (IndexOutOfRangeException)
            {

                Log.ImprimirMensaje("No se ha encontrado foobar2000", TipoMensaje.Info);
                foobar2kInstance = null;
                checkBoxFoobar.Enabled = false;
            }
        }
        private void timerCancion_Tick(object sender, EventArgs e)
        {
            if (estadoReproductor == EstadoReproductor.Detenido)
                trackBarPosicion.Enabled = false;
            else
                trackBarPosicion.Enabled = true;
            if (!Spotify && timerCancion.Enabled && nucleo.ComprobarSonido())
            {
                pos = nucleo.Posicion();
                using (StreamWriter salida = new StreamWriter("np.txt"))
                {
                    if (CancionLocalReproduciendo == null)
                        salida.WriteLine(Text);
                    else
                        salida.WriteLine(CancionLocalReproduciendo.ToString());
                    salida.Write(pos.ToString(@"mm\:ss") + " / ");
                    salida.Write(dur.ToString(@"mm\:ss"));
                }
            }
            labelPosicion.Text = pos.ToString(@"mm\:ss");
            if (pos > dur)
                dur = pos;
            if(TiempoRestante)
            {
                TimeSpan tRes = dur - pos;
                labelDuracion.Text = "-" + tRes.ToString(@"mm\:ss");
                //int secsRestantes = (int)((dur.TotalSeconds - pos.TotalSeconds) % 60);
                //int minsRestantes = (int)((dur.TotalSeconds - pos.TotalSeconds) / 60);
                //if(secsRestantes < 10)
                //    labelDuracion.Text = "-" + minsRestantes + ":0" + secsRestantes; 
                //else
                //    labelDuracion.Text = "-" + minsRestantes + ":" + secsRestantes; 
            }
            else
            {
                labelDuracion.Text = dur.ToString(@"mm\:ss");
            }
            if(nucleo.ComprobarSonido())
            {
                double val = pos.TotalMilliseconds / dur.TotalMilliseconds * trackBarPosicion.Maximum;
                trackBarPosicion.Value = (int)val;
            }

            if (pos == dur)
            {
                estadoReproductor = EstadoReproductor.Detenido;
                if(ListaReproduccion != null)
                {
                    ListaReproduccionPuntero++;
                    if (!ListaReproduccion.Final(ListaReproduccionPuntero))
                        ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                    else
                        nucleo.Detener();
                }
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Programa.ModoReproductor || !Programa.ModoStream)
            {
                Hide();
                e.Cancel = true;
            }
            else
            {
                if (nucleo != null)
                    nucleo.Apagar();
                Dispose();
                Application.Exit();
            }

        }
        public void Apagar()
        {
            if(backgroundWorker != null)
                backgroundWorker.CancelAsync();
            if(pictureBoxCaratula.Image != null)
                pictureBoxCaratula.Image.Dispose();
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            nucleo.Apagar();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string fich = null;
            openFileDialog1.Filter = "*.mp3, *.flac, *.ogg|*.mp3;*.flac;*.ogg";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                timerCancion.Enabled = false;
                nucleo.Apagar();
                estadoReproductor = EstadoReproductor.Detenido;
                fich = openFileDialog1.FileName;
                this.fich = fich;
                try
                {
                    nucleo.CargarCancion(fich);
                    FicheroLeible(fich);
                    nucleo.Reproducir();
                    PrepararReproductor();
                }
                catch (Exception ex)
                {
                    Log.ImprimirMensaje("Error intentando cargar la canción", TipoMensaje.Error);
                    Log.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                    nucleo.Apagar();
                    return;
                }
                try
                {
                    System.Drawing.Image caratula = nucleo.GetCaratula();
                    if(caratula != null) 
                        pictureBoxCaratula.Image = nucleo.GetCaratula();
                    else
                    {
                        FileInfo fi = new FileInfo(openFileDialog1.FileName);
                        DirectoryInfo info = new DirectoryInfo(fi.DirectoryName);
                        foreach (FileInfo item in info.GetFiles())
                        {
                            if (item.Name == "cover.jpg" || item.Name == "folder.jpg")
                                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(item.FullName);
                        }
                    }

                }
                catch (NullReferenceException)
                {
                    Log.ImprimirMensaje("No hay carátula, usando por defecto", TipoMensaje.Advertencia);
                    pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
                }
            }
        }

        private void buttonReproducirPausar_Click(object sender, EventArgs e)
        {
            switch (estadoReproductor)
            {
                case EstadoReproductor.Reproduciendo:
                    if (!Spotify)
                        nucleo.Pausar();
                    else if (Spotify && EsPremium)
                    {
                        ErrorResponse err = _spotify.PausePlayback();
                        if (err.Error != null && err.Error.Message != null)
                        {
                            Log.ImprimirMensaje(err.Error.Message, TipoMensaje.Error);
                            MessageBox.Show(err.Error.Message);
                        }
                        break;
                    }
                    estadoReproductor = EstadoReproductor.Pausado;
                    buttonReproducirPausar.Text = "▶";
                    break;

                case EstadoReproductor.Pausado:
                    if (!Spotify)
                        nucleo.Reproducir();
                    else if (Spotify && EsPremium)
                    {
                        ErrorResponse err = _spotify.ResumePlayback("", "", null, "", 0);
                        if(err.Error != null && err.Error.Message != null)
                        {
                            Log.ImprimirMensaje(err.Error.Message, TipoMensaje.Error);
                            MessageBox.Show(err.Error.Message);
                        }
                        break;
                    }
                    estadoReproductor = EstadoReproductor.Reproduciendo;
                    buttonReproducirPausar.Text = "❚❚";
                    break;
                case EstadoReproductor.Detenido:
                    if(!Spotify)
                        nucleo.Reproducir();
                    else if (Spotify && EsPremium)
                    {
                        ErrorResponse err = _spotify.ResumePlayback("", "", null, "", 0);
                        if (err.Error != null && err.Error.Message != null)
                        {
                            Log.ImprimirMensaje(err.Error.Message, TipoMensaje.Error);
                            MessageBox.Show(err.Error.Message);
                        }
                        break;
                    }
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
            timerMetadatos.Enabled = false;
            trackBarPosicion.Value = (int)((e.X * dur.TotalSeconds) / Size.Width);
        }

        private void trackBarPosicion_MouseUp(object sender, MouseEventArgs e)
        {

            if (!Spotify)
            {
                timerCancion.Enabled = true;
                timerMetadatos.Enabled = true;
                nucleo.Saltar(new TimeSpan(0, 0, trackBarPosicion.Value));
            }
            else if (Spotify && EsPremium)
            {
                _spotify.SeekPlayback(trackBarPosicion.Value * 1000);
                timerSpotify.Enabled = true;
            }
            else
                timerSpotify.Enabled = true;
        }
        private void trackBarPosicion_Scroll(object sender, EventArgs e)
        {

            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            pos = new TimeSpan(0, 0, trackBarPosicion.Value);
            duracionView.SetToolTip(trackBarPosicion, pos.ToString());
            timerCancion_Tick(null, null);
        }
        private void trackBarVolumen_Scroll(object sender, EventArgs e)
        {
            Volumen = (float)trackBarVolumen.Value / 100;
            if (!Spotify && (nucleo.ComprobarSonido()))
                nucleo.SetVolumen(Volumen);
            else if (EsPremium && Spotify)
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
            if(EsPremium && Spotify)
                _spotify.SetShuffle(checkBoxAleatorio.Checked);
            else
            {
                try
                {
                    ListaReproduccion.Mezclar();//cambiar func
                    lrui.Refrescar();
                }
                catch (NullReferenceException)
                {
                    Log.ImprimirMensaje("No hay lista de reproducción", TipoMensaje.Advertencia);
                }
            }
        }

        private void buttonSaltarAdelante_Click(object sender, EventArgs e)
        {
            if (EsPremium && Spotify)
                _spotify.SkipPlaybackToNext();
            else
            {
                if (ListaReproduccion != null)
                {
                    if (ListaReproduccion.Final(ListaReproduccionPuntero))
                    {
                        nucleo.Detener();
                        buttonReproducirPausar.Text = GetTextoReproductor(EstadoReproductor.Detenido);
                    }
                    else
                    {
                        //try
                        {
                            ListaReproduccionPuntero++;
                            lrui.SetActivo((int)ListaReproduccionPuntero);
                            ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                        }
                        //catch (Exception)
                        {

                            return;
                        }

                    }

                }
            }
        }

        private void buttonSaltarAtras_Click(object sender, EventArgs e)
        {
            if (Spotify && EsPremium)
                _spotify.SkipPlaybackToPrevious();
            else
            {
                if (ListaReproduccion != null && !ListaReproduccion.Inicio(ListaReproduccionPuntero))
                {
                    ListaReproduccionPuntero--;
                    lrui.SetActivo((int)ListaReproduccionPuntero);
                    ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                }
            }
        }

        private void Reproductor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F9 && !Spotify && lrui != null)
                lrui.Show();
            if (e.Control && e.KeyData == Keys.Right)
                buttonSaltarAdelante_Click(null, null);
            if (e.Control && e.KeyData == Keys.Left)
                buttonSaltarAtras_Click(null, null);
            if (e.KeyData == Keys.Space)
                buttonReproducirPausar_Click(null, null);

        }
        private String GetTextoReproductor(EstadoReproductor er)
        {
            switch (er)
            {
                case EstadoReproductor.Reproduciendo:
                    return "❚❚";
                case EstadoReproductor.Pausado:
                case EstadoReproductor.Detenido:
                    return "▶";
            }
            return "";
        }

        private void timerMetadatos_Tick(object sender, EventArgs e)
        {
            labelDatosCancion.Text = nucleo.GetDatos();
        }
        private void ApagarSpotify()
        {
            backgroundWorker.CancelAsync();
            buttoncrearLR.Show();
            buttonSpotify.Text = Programa.textosLocal.GetString("cambiarSpotify");
            timerSpotify.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            Spotify = false;
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
            button2.Enabled = true;
            trackBarPosicion.Value = 0;
            dur = new TimeSpan(0);
            pos = new TimeSpan(0);
            labelDuracion.Text = "-";
            labelPosicion.Text = "0:00";
            Volumen = 1.0f;
            Text = "";
            toolStripStatusLabelCorreoUsuario.Text = "";
            labelDatosCancion.Text = "";
            Icon = Properties.Resources.iconoReproductor;
            checkBoxFoobar.Visible = true;
            buttonAgregar.Hide();
        }
        public void ActivarSpotify()
        {
            try
            {
                timerMetadatos.Enabled = false;
                timerCancion.Enabled = false;
                checkBoxFoobar.Visible = false;
                nucleo.Apagar();
            }
            catch (Exception)
            {
            }
            if (Programa.SpotifyActivado)
            {
                if (!SpotifyListo || Programa.ModoStream)
                {
                    PrepararSpotify();
                }
                try
                {
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile("./covers/np.jpg");
                }
                catch (Exception)
                {
                    Log.ImprimirMensaje("No hay fichero de np.jpg", TipoMensaje.Advertencia);
                }
                buttonAgregar.Show();
                Icon = Properties.Resources.spotifyico;
                timerSpotify.Enabled = true;
                buttonSpotify.Text = Programa.textosLocal.GetString("cambiarLocal");
                button2.Enabled = false;
                Spotify = true;
                toolStripStatusLabelCorreoUsuario.Text = Programa.textosLocal.GetString("conectadoComo")+ " " + user.DisplayName;
                if (!EsPremium)
                {
                    toolStripStatusLabelCorreoUsuario.Text += " - NO PREMIUM";
                }
                buttonTwit.Enabled = true;
                buttoncrearLR.Hide();
            }
            else
                return;

        }
        private void buttonSpotify_Click(object sender, EventArgs e)
        {
            if (Spotify)
                ApagarSpotify();
            else
                ActivarSpotify();
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            Programa._spotify.insertarAlbumFromURI(cancionReproduciendo.Album.Id);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxFoobar.Checked)
            {
                nucleo.Apagar();
                timerCancion.Enabled = false;
                timerFoobar.Enabled = true;
                foobar2000 = true;
                buttonReproducirPausar.Enabled = false;
            }
            else
            {
                timerFoobar.Enabled = false;
                foobar2000 = false;
                buttonReproducirPausar.Enabled = true;
            }
        }

        private void timerFoobar_Tick(object sender, EventArgs e)
        {
            foobar2kInstance = Process.GetProcessById(foobar2kInstance.Id);
            using (StreamWriter salida  = new StreamWriter("np.txt", false))
            {
                salida.WriteLine(foobar2kInstance.MainWindowTitle);
            }
        }

        private void buttonTwit_Click(object sender, EventArgs e)
        {
            string test;
            string link = "https://twitter.com/intent/tweet?text=";
            if (Spotify)
            {
                if (!string.IsNullOrEmpty(cancionReproduciendo.Id))
                    test = Programa.textosLocal.GetString("compartirTwitter1").Replace(" ", "%20") + "%20https://open.spotify.com/track/" + cancionReproduciendo.Id + "%0a" +
                        Programa.textosLocal.GetString("compartirTwitter2").Replace(" ", "%20") + "%20" + Programa.textosLocal.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Programa.version + "%20" + Programa.CodeName;
                else
                    test = Programa.textosLocal.GetString("compartirTwitter1").Replace(" ", "%20") + "%20" +
                        cancionReproduciendo.Name + "%20" +
                        cancionReproduciendo.Artists[0].Name + "%20"+ "%0a" +
                        Programa.textosLocal.GetString("compartirTwitter2").Replace(" ", "%20") + "%20" + Programa.textosLocal.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Programa.version + "%20" + Programa.CodeName;
            }
            else
                test = Programa.textosLocal.GetString("compartirLocal1").Replace(" ", "%20") + "%20" + 
                    CancionLocalReproduciendo.titulo + "%20" + 
                    Programa.textosLocal.GetString("compartirLocal2").Replace(" ", "%20") + "%20" +
                    CancionLocalReproduciendo.album.artista + "%20" +
                    Programa.textosLocal.GetString("compartirLocal3").Replace(" ", "%20") + "%20" + 
                    Programa.textosLocal.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + 
                    Programa.version + "%20" + Programa.CodeName;
            link += test;
            Process.Start(link);
        }
        private void Reproductor_DragDrop(object sender, DragEventArgs e)
        {
            Cancion c = null;
            if((c = (Cancion)e.Data.GetData(typeof(Cancion))) != null)
            {
                if (!string.IsNullOrEmpty(c.PATH))
                    ReproducirCancion(c);
            }
        }

        private void Reproductor_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListaReproduccion lr = new ListaReproduccion("");
            ListaReproduccion = lr;
            lrui = new ListaReproduccionUI(ListaReproduccion);
            ListaReproduccionPuntero = 0;
        }
    }
}
