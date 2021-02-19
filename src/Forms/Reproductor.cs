﻿using System;
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
using aplicacion_musica.Properties;

namespace aplicacion_musica.src.Forms
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
        private int ListaReproduccionPuntero = 0;
        bool SpotifyListo = false;
        bool EsPremium = false;
        DirectoryInfo directorioCanciones;
        PrivateProfile user;
        private Log Log = Log.Instance;
        private float Volumen;
        private ListaReproduccionUI lrui;
        private ToolTip duracionView;
        private bool GuardarHistorial;
        private FileInfo Historial;
        private uint NumCancion;
        Song CancionLocalReproduciendo = null;
        private bool foobar2000 = true;
        Process foobar2kInstance = null;
        string SpotifyID = null;
        bool Reproduciendo = false;
        public bool ModoCD { get; private set; }
        public static Reproductor Instancia { get; set; }
        public Reproductor()
        {
            InitializeComponent();
            SetPlayerButtons(false);
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            trackBarPosicion.Enabled = false;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            Volumen = 1.0f;
            trackBarVolumen.Value = 100;
            duracionView = new ToolTip();
            buttonAgregar.Hide();
            if (!Program.SpotifyActivado)
                buttonSpotify.Enabled = false;
            Icon = Resources.iconoReproductor;
            GuardarHistorial = false;
            if(GuardarHistorial) //sin uso
            {
                DateTime now = DateTime.Now;
                Historial = new FileInfo("Log Musical " + now.Day + "-"+ now.Month + "-"+ now.Year+"-"+ now.Hour + "."+ now.Minute + ".txt");
                NumCancion = 1;
            }
            if (Program.ModoStream) //inicia el programa con solo la imperesión
            {
                notifyIcon1.Visible = true;
                while (!Program._spotify.cuentaLista)
                {
                    Thread.Sleep(100);
                }
                ActivarSpotify();
            }

            else notifyIcon1.Visible = false;
            buttonTwit.Enabled = false;
            ModoCD = false;
        }
        private void ConfigurarTimers(bool val) //Configura los timers cancion y metadatos.
        {
            timerCancion.Enabled = val;
            timerMetadatos.Enabled = val;
        }
        public void PlayCD(char disp)
        {
            //reproduce un cd
            SetPlayerButtons(true);
            nucleo.ReproducirCD(disp);
            Reproduciendo = true;
            if (nucleo.PistasCD == null)
                return;
            ModoCD = true;
            PrepararReproductor();
            Text = "CD - Pista 1";
            CreatePlaylist("CD-A");
            for (int i = 0; i < nucleo.PistasCD.Length; i++)
            {
                Song c = new Song("Pista " + (i + 1), (int)nucleo.PistasCD[i].Duracion.TotalMilliseconds, false);
                ListaReproduccion.AgregarCancion(c);
            }
        }
        public void SpotifyEncendido()
        {
            buttonSpotify.Enabled = true;
        }
        private void PrepararSpotify()
        {
            _spotify = Program._spotify._spotify;
            user = _spotify.GetPrivateProfile();
            Log.ImprimirMensaje("Iniciando el Reproductor en modo Spotify, con cuenta " + user.Email, TipoMensaje.Info);
            Spotify = true;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            cancionReproduciendo = new FullTrack();
            EsPremium = (user.Product == "premium") ? true : false;
            SpotifyListo = true;
            timerSpotify.Enabled = true;
            toolStripStatusLabelCorreoUsuario.Text = "Conectado como " + user.DisplayName;
        }
        public void Apagar()
        {
            if (backgroundWorker != null)
                backgroundWorker.CancelAsync();
            if (pictureBoxCaratula.Image != null)
                pictureBoxCaratula.Image.Dispose();
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            nucleo.Apagar();
        }
        private void ApagarSpotify()
        {
            Log.Instance.ImprimirMensaje("Apagando Spotify", TipoMensaje.Info);
            backgroundWorker.CancelAsync();
            buttoncrearLR.Show();
            buttonSpotify.Text = Program.LocalTexts.GetString("cambiarSpotify");
            timerSpotify.Enabled = false;
            estadoReproductor = EstadoReproductor.Detenido;
            Spotify = false;
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
            buttonAbrir.Enabled = true;
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
            SetPlayerButtons(false);
            buttonDetener.Enabled = true;
            buttonAgregar.Hide();
        }
        public void ActivarSpotify()
        {
            Log.Instance.ImprimirMensaje("Activando Spotify", TipoMensaje.Info);
            try
            {
                timerMetadatos.Enabled = false;
                timerCancion.Enabled = false;
                checkBoxFoobar.Visible = false;
                nucleo.Apagar();
                SetPlayerButtons(false);
                buttonDetener.Enabled = false;
            }
            catch (Exception)
            {
                SetPlayerButtons(true);
                buttonDetener.Enabled = true;
            }
            if (Program._spotify.cuentaVinculada)
            {
                if (!SpotifyListo || Program.ModoStream)
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
                buttonSpotify.Text = Program.LocalTexts.GetString("cambiarLocal");
                buttonAbrir.Enabled = false;
                Spotify = true;
                toolStripStatusLabelCorreoUsuario.Text = Program.LocalTexts.GetString("conectadoComo") + " " + user.DisplayName;
                SetPlayerButtons(true);
                if (!EsPremium)
                {
                    toolStripStatusLabelCorreoUsuario.Text += " - NO PREMIUM";
                    SetPlayerButtons(false);
                }
                buttonTwit.Enabled = true;
                buttoncrearLR.Hide();
                
            }
            else
                return;

        }
        private void DescargarPortada(SimpleAlbum album)
        {
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    if (File.Exists("./covers/np.jpg") && pictureBoxCaratula.Image != null)
                        pictureBoxCaratula.Image.Dispose();
                    cliente.DownloadFile(new Uri(album.Images[1].Url), Environment.CurrentDirectory + "/covers/np.jpg");
                }
                catch (System.Net.WebException)
                {
                    Log.ImprimirMensaje("Error descargando la imagen", TipoMensaje.Advertencia);
                    File.Delete("./covers/np.jpg");
                }
                catch (IOException)
                {
                    Log.ImprimirMensaje("Error descargando la imagen, no es posible reemplazar el fichero...", TipoMensaje.Error);
                }
            }
        }
        private string GetTextoReproductor(EstadoReproductor er)
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

        public void ReproducirLista(ListaReproduccion lr)
        {
            Song c = lr[ListaReproduccionPuntero];
            ReproducirCancion(c);
        }
        public void RefrescarTextos()
        {
            PonerTextos();
        }
        private void PonerTextos()
        {
            if(!Reproduciendo)
                Text = Program.LocalTexts.GetString("reproductor");
            buttonSpotify.Text = Program.LocalTexts.GetString("cambiarSpotify");
            notifyIcon1.Text = Program.LocalTexts.GetString("cerrarModoStream");
            buttoncrearLR.Text = Program.LocalTexts.GetString("crearLR");
            buttonAgregar.Text = Program.LocalTexts.GetString("agregarBD");
            buttonTwit.Text = Program.LocalTexts.GetString("twittearCancion");
            buttonAbrir.Text = Program.LocalTexts.GetString("abrir_cancion");
            notifyIconReproduciendo.Text = Program.LocalTexts.GetString("click_reproductor");
        }
        public void SetPATH(Song c) //probablemente deprecated pero configura los paths
        {
            directorioCanciones = new DirectoryInfo(c.album.SoundFilesPath);
            foreach (FileInfo file in directorioCanciones.GetFiles())
            {
                if (CancionLocalReproduciendo == null || file.FullName == CancionLocalReproduciendo.PATH)
                    continue;
                try
                {
                    LectorMetadatos LM = new LectorMetadatos(file.FullName);
                    if (LM.Evaluable() && c.titulo.ToLower() == LM.Titulo.ToLower() && c.album.Artist.ToLower() == LM.Artista.ToLower())
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
        public void ReproducirCancion(string path) //reproduce una cancion por path
        {
            pictureBoxCaratula.Image = Properties.Resources.albumdesconocido;
            SetPlayerButtons(true);
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Detenido;
            DirectoryInfo dir = new DirectoryInfo(path);
            dir = dir.Parent;
            //Intento sacar la portada mediante un fichero primero.
            if (File.Exists(dir.FullName + "\\folder.jpg"))
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\folder.jpg");
            else if (File.Exists(dir.FullName + "\\cover.jpg"))
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\cover.jpg");
            else if (File.Exists(dir.FullName + "\\cover.png"))
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\cover.png");
            //Si esto no fuera posible, hay que extraerla de la cancion.
            LectorMetadatos Lector = new LectorMetadatos(path);
            if (Lector.Cover != null)
                pictureBoxCaratula.Image = Lector.Cover;
            Text = Lector.GetSongTitle();
            nucleo.Apagar();
            try
            {
                nucleo.CargarCancion(path);
                PrepararReproductor();
                nucleo.Reproducir();
            }
            catch (Exception)
            {
                Log.ImprimirMensaje("Hubo un problema", TipoMensaje.Error);
                MessageBox.Show(Program.LocalTexts.GetString("errorReproduccion"));
                return;
            }

            if (GuardarHistorial)
            {
                using (StreamWriter escritor = new StreamWriter(Historial.FullName, true))
                {
                    escritor.WriteLine(NumCancion + " - " + Lector.Artista + " - " + Lector.Titulo);
                    NumCancion++;
                }
            }

        }
        public void ReproducirCancion(Song c) //reproduce una cancion
        {
            SetPlayerButtons(true);
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Detenido;
            if(c.album is null) //Puede darse el caso de que sea una canción local suelta, intentamos poner la carátula primero por fichero.
            {
                DirectoryInfo dir = new DirectoryInfo(c.PATH);
                dir = dir.Parent;
                if(File.Exists(dir.FullName + "\\folder.jpg"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\folder.jpg");
                else if(File.Exists(dir.FullName + "\\cover.jpg"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\cover.jpg");
                else if (File.Exists(dir.FullName + "\\cover.png"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(dir.FullName + "\\cover.png");

            }
            //try
            //{
            //    if(c.album != null)
            //        SetPATH(c);
            //}
            //catch (DirectoryNotFoundException)
            //{
            //    Log.ImprimirMensaje("No se encuentra el directorio", TipoMensaje.Error);
            //    return;
            //}

            CancionLocalReproduciendo = c;
            //if (string.IsNullOrEmpty(c.PATH))
            //{   
            //    MessageBox.Show(c.titulo + " " +c.album.Title + Environment.NewLine + "ERROR_CANCION");
            //    return;
            //}
            //else
            //    s = c.PATH;
            LectorMetadatos LM = new LectorMetadatos(c.PATH);
            if (!(c.album is null) && c.album.CoverPath != null)
            {
                if (c.album.CoverPath != "")
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.CoverPath);
                else
                {
                    if (File.Exists(c.album.SoundFilesPath + "\\cover.jpg"))
                        pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.SoundFilesPath + "\\cover.jpg");
                    else if (File.Exists(c.album.SoundFilesPath + "\\cover.png"))
                        pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.SoundFilesPath + "\\cover.png");
                    else if (File.Exists(c.album.SoundFilesPath + "\\folder.jpg"))
                        pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.album.SoundFilesPath + "\\folder.jpg");
                }
            }
            else
            {
                if (LM.Cover != null)
                    pictureBoxCaratula.Image = LM.Cover;
            }
            Text = LM.GetSongTitle();
            nucleo.Apagar();
            try
            {
                nucleo.CargarCancion(CancionLocalReproduciendo.PATH);
                nucleo.Reproducir();
            }
            catch (Exception)
            {
                MessageBox.Show(Program.LocalTexts.GetString("errorReproduccion"));
                return;
            }
            if(GuardarHistorial)
            {
                using (StreamWriter escritor = new StreamWriter(Historial.FullName, true))
                {
                    escritor.WriteLine(NumCancion + " - " + c.album.Artist + " - " + c.titulo);
                    NumCancion++;
                }
            }
            PrepararReproductor();

        }
        public void ReproducirCancion(int Pista)
        {
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Detenido;
            if (ModoCD)
                nucleo.SaltarCancionCD(Pista);
            else
                ReproducirCancion(ListaReproduccion.Canciones[Pista]);
            PrepararReproductor();
            ListaReproduccionPuntero = Pista;
            lrui.SetActivo(ListaReproduccionPuntero);
            timerCancion.Enabled = true;
            timerMetadatos.Enabled = false;
            buttonTwit.Enabled = false;
        }
        private void SetWindowTitle(string text)
        {
            Text = text;
        }
        private void PrepararReproductor()
        {
            trackBarPosicion.Value = 0; //reseteo
            nucleo.SetVolumen(Volumen);
            dur = nucleo.Duracion();
            pos = TimeSpan.Zero;
            if (!ModoCD)
            {
                buttoncrearLR.Show();
            }
            else
            {
                dur = nucleo.PistasCD[ListaReproduccionPuntero].Duracion;
                Text = "CD - Pista " + (ListaReproduccionPuntero + 1);
                buttoncrearLR.Hide();
            }
            labelDatosCancion.Text = nucleo.GetDatos();
            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
            labelDuracion.Text = (int)dur.TotalMinutes + ":" + dur.Seconds;
            estadoReproductor = EstadoReproductor.Reproduciendo;
            buttonReproducirPausar.Text = GetTextoReproductor(estadoReproductor);
            buttonTwit.Enabled = true;
            Reproduciendo = true;
            ConfigurarTimers(true);
            

        }
        private bool FicheroLeible(string s)
        {
            string Ext = Path.GetExtension(s);
            switch (Ext)
            {
                case ".mp3":
                    timerMetadatos.Enabled = false;
                    return true;
                case ".ogg":
                    timerMetadatos.Enabled = true;
                    return true;
                case ".flac":
                    timerMetadatos.Enabled = false;
                    return true;
                default:
                    return false;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PlaybackContext PC = (PlaybackContext)e.Result; //datos de spotify
            if(PC != null && PC.Item != null) //si son válidos
            {

                dur = new TimeSpan(0, 0, 0, 0, PC.Item.DurationMs);
                trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                pos = new TimeSpan(0, 0, 0, 0, PC.ProgressMs);
                SpotifyID = PC.Item.Id;
                if(!Program.ModoStream)
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
                using(StreamWriter salida = new StreamWriter("np.txt")) //se debería poder personalizar con filtros pero otro día
                {
                    TimeSpan np = TimeSpan.FromMilliseconds(PC.ProgressMs);
                    salida.WriteLine(PC.Item.Artists[0].Name + " - " + PC.Item.Name);
                    salida.Write(np.ToString(@"mm\:ss") + " / ");
                    salida.Write(dur.ToString(@"mm\:ss"));
                }

            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) //tarea asíncrona que comprueba si el token ha caducado y espera a la tarea que lo refresque
        {
            if(!Program._spotify.TokenExpirado())
            {
                PlaybackContext PC = _spotify.GetPlayback();
                e.Result = PC;
            }
            else
            {
                Log.ImprimirMensaje("Token caducado!", TipoMensaje.Advertencia);
                while(Program._spotify.TokenExpirado())
                {
                    Thread.Sleep(100);
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

            if (pos.Minutes == dur.Minutes && pos.Seconds == dur.Seconds)
            {
                estadoReproductor = EstadoReproductor.Detenido;
                if(ListaReproduccion != null)
                {
                    ListaReproduccionPuntero++;
                    lrui.SetActivo(ListaReproduccionPuntero);
                    if (!ListaReproduccion.Final(ListaReproduccionPuntero))
                    {
                        if (!ModoCD)
                            ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                        else
                            ReproducirCancion(ListaReproduccionPuntero);
                    }
                        
                    else
                        nucleo.Detener();
                }
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.ModoReproductor || !Program.ModoStream)
            {
                Hide();
                if(Reproduciendo)
                    notifyIconReproduciendo.Visible = true;
                if (e.CloseReason != CloseReason.ApplicationExitCall)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
            else
            {
                if (nucleo != null)
                    nucleo.Apagar();
                Dispose();
                Application.Exit();
            }

        }

        private void buttonAbrir_Click(object sender, EventArgs e)
        {
            string fich = null;
            openFileDialog1.Filter = "*.mp3, *.flac, *.ogg|*.mp3;*.flac;*.ogg";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                nucleo.Apagar();
                estadoReproductor = EstadoReproductor.Detenido;
                fich = openFileDialog1.FileName;
               
                this.fich = fich;
                try
                {
                    if (FicheroLeible(fich))
                    {
                        Song c = new Song(fich);
                        if (ListaReproduccion == null)
                            ListaReproduccion = new ListaReproduccion("Selección");
                        ListaReproduccion.AgregarCancion(c);
                        ReproducirLista(ListaReproduccion);
                    }
                }
                catch (Exception ex)
                {
                    Log.ImprimirMensaje("Error intentando cargar la canción", TipoMensaje.Error);
                    Log.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                    nucleo.Apagar();
                    return;
                }
            }
        }
        private void PausaReproducir()
        {
            switch (estadoReproductor)
            {
                case EstadoReproductor.Reproduciendo: //Si está reproduciendo pausa.
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
                case EstadoReproductor.Detenido:
                    if (!Spotify)
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
        private void buttonReproducirPausar_Click(object sender, EventArgs e)
        {
            PausaReproducir();
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
                int value = trackBarPosicion.Value;
                if (!ModoCD)
                    nucleo.Saltar(TimeSpan.FromSeconds(trackBarPosicion.Value));
                else
                    nucleo.SaltarCD((int)nucleo.TimeSpanASectores(TimeSpan.FromSeconds(trackBarPosicion.Value)));
                trackBarPosicion.Value = value;
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
            ConfigurarTimers(false);
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
            SaltarAdelante();
        }
        private void SaltarAdelante()
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
                        try
                        {
                            ListaReproduccionPuntero++;
                            lrui.SetActivo(ListaReproduccionPuntero);
                            if (!ModoCD)
                                ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                            else
                                ReproducirCancion(ListaReproduccionPuntero);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("No puedes");
                            return;
                        }

                    }

                }
            }
        }
        private void SaltarAtras()
        {
            if (Spotify && EsPremium)
                _spotify.SkipPlaybackToPrevious();
            else
            {
                if (ListaReproduccion != null && !ListaReproduccion.Inicio(ListaReproduccionPuntero))
                {
                    ListaReproduccionPuntero--;
                    lrui.SetActivo(ListaReproduccionPuntero);
                    if (!ModoCD)
                        ReproducirCancion(ListaReproduccion.GetCancion(ListaReproduccionPuntero));
                    else
                        ReproducirCancion(ListaReproduccionPuntero);
                }
            }
        }
        private void buttonSaltarAtras_Click(object sender, EventArgs e)
        {
            SaltarAtras();
        }

        private void Reproductor_KeyDown(object sender, KeyEventArgs e)
        {
            //Condiciones especiales
            if (e.KeyData == Keys.F9 && !Spotify && lrui != null)
                lrui.Show();
            if (e.Control && e.KeyData == Keys.Right)
                buttonSaltarAdelante_Click(null, null);
            if (e.Control && e.KeyData == Keys.Left)
                buttonSaltarAtras_Click(null, null);
            switch(e.KeyData)
            {
                case Keys.Space:
                case Keys.MediaPlayPause:
                    PausaReproducir();
                    break;
                case Keys.MediaStop:
                    Detener();
                    break;
                case Keys.MediaNextTrack:
                    SaltarAdelante();
                    break;
                case Keys.MediaPreviousTrack:
                    SaltarAtras();
                    break;
            }
        }

        private void timerMetadatos_Tick(object sender, EventArgs e)
        {
            labelDatosCancion.Text = nucleo.GetDatos();
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
            bool res = Program._spotify.InsertarAlbumFromURI(cancionReproduciendo.Album.Id);
            if(res) //Añadido correctamente...
                MessageBox.Show(Program.LocalTexts.GetString("album_agregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Program.LocalTexts.GetString("album_noagregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
        //Esto está rotísimo... ;(
        private void buttonTwit_Click(object sender, EventArgs e)
        {
            string test;
            string link = "https://twitter.com/intent/tweet?text=";
            if (Spotify)
            {
                if (!string.IsNullOrEmpty(cancionReproduciendo.Id))
                    test = Program.LocalTexts.GetString("compartirTwitter1").Replace(" ", "%20") + "%20https://open.spotify.com/track/" + cancionReproduciendo.Id + "%0a" +
                        Program.LocalTexts.GetString("compartirTwitter2").Replace(" ", "%20") + "%20" + Program.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Program.Version + "%20" + Program.CodeName;
                else
                    test = Program.LocalTexts.GetString("compartirTwitter1").Replace(" ", "%20") + "%20" +
                        cancionReproduciendo.Name + "%20" +
                        cancionReproduciendo.Artists[0].Name + "%20"+ "%0a" +
                        Program.LocalTexts.GetString("compartirTwitter2").Replace(" ", "%20") + "%20" + Program.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Program.Version + "%20" + Program.CodeName;
            }
            else if(!ModoCD)
                test = Program.LocalTexts.GetString("compartirLocal1").Replace(" ", "%20") + "%20" + 
                    CancionLocalReproduciendo.titulo + "%20" + 
                    Program.LocalTexts.GetString("compartirLocal2").Replace(" ", "%20") + "%20" +
                    CancionLocalReproduciendo.album.Artist + "%20" +
                    Program.LocalTexts.GetString("compartirLocal3").Replace(" ", "%20") + "%20" + 
                    Program.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + 
                    Program.Version + "%20" + Program.CodeName;
            else
                test = "Escuchando un CD con " + Program.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" +
                    Program.Version + "%20" + Program.CodeName;
            link += test;
            Process.Start(link);
        }
        private void Reproductor_DragDrop(object sender, DragEventArgs e)
        {
            Log.ImprimirMensaje("Detectado Drag & Drop", TipoMensaje.Info);
            Song c = null;
            String[] canciones = null;
            if((c = (Song)e.Data.GetData(typeof(Song))) != null)
            {
                if (!string.IsNullOrEmpty(c.PATH))
                {
                    ModoCD = false;
                    ReproducirCancion(c);
                }
            }
            else if((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null)
            {
                Log.ImprimirMensaje("Creando playlist con "+canciones.Length + " canciones.", TipoMensaje.Info);
                if(ListaReproduccion is null)
                {
                    CreatePlaylist(Program.LocalTexts.GetString("seleccion"));
                    foreach (string cancion in canciones)
                    {
                        Song clr = new Song();
                        clr.PATH = cancion;
                        ListaReproduccion.AgregarCancion(clr);
                    }
                    ReproducirLista(ListaReproduccion);
                }
                else
                {
                    foreach (string songfile in canciones)
                    {
                        Song clr = new Song();
                        clr.PATH = songfile;
                        ListaReproduccion.AgregarCancion(clr);
                    }
                }
                lrui.Refrescar();
            }
            else
                Log.ImprimirMensaje("No se ha podido determinar la canción", TipoMensaje.Advertencia);
        }

        private void Reproductor_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        //Creates a playlist and sets it as the active one, overriding the previous one.
        private void CreatePlaylist(string Title)
        {
            buttoncrearLR.Text = Program.LocalTexts.GetString("verLR");
            ListaReproduccion lr = new ListaReproduccion(Title);
            ListaReproduccion = lr;
            ListaReproduccionPuntero = 0;
            if (lrui is null)
                CreatePlaylistUI();
            lrui.ListaReproduccion = lr;
        }
        private void CreatePlaylistUI()
        {
            lrui = new ListaReproduccionUI(ListaReproduccion);
            
        }
        public void SetPlaylist(ListaReproduccion playlist) //Sets a loaded playlist from a file.
        {
            ListaReproduccion = playlist;
            //Not needed to change the text, because we have a empty playlist after opening the form.
        }
        private void buttonLR_Click(object sender, EventArgs e)
        {
            if(ListaReproduccion is null) //If we don't have a playlist, create one. If we don't have the UI, create it.
            {
                CreatePlaylist("");
                if (lrui is null)
                    CreatePlaylistUI();
                lrui.Show();
            }
            else
            {
                lrui.Show();
            }
        }
        public TimeSpan getDuracionFromFile(string path)
        {
            nucleo.CargarCancion(path);
            TimeSpan dur = nucleo.Duracion();
            nucleo.Apagar();
            return dur;
        }
        //Controls the player buttons
        private void SetPlayerButtons(bool encendido)
        {
            buttonReproducirPausar.Enabled = encendido;
            buttonSaltarAdelante.Enabled = encendido;
            buttonSaltarAtras.Enabled = encendido;
        }
        private void Detener()
        {
            nucleo.Apagar();
            Reproduciendo = false;
            ConfigurarTimers(false);
            SetPlayerButtons(false);
            trackBarPosicion.Enabled = false;
            trackBarPosicion.Value = 0;
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            labelDuracion.Text = "XX:XX";
            labelPosicion.Text = "0:00";
            labelPorcentaje.Text = "0%";
            Text = "";
            labelDatosCancion.Text = "";
            notifyIconReproduciendo.Visible = false;
        }
        public void ActivarPorLista() //Prepara para reproducir una lista de reproducción
        {
            SetPlayerButtons(true);
        }
        private void buttonDetener_Click(object sender, EventArgs e)
        {
            Detener();
        }

        private void notifyIconReproduciendo_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
        }
    }
}
