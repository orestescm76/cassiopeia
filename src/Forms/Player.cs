using Cassiopeia.Properties;
using Cassiopeia.src.Classes;
using CSCore.CoreAudioAPI;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public enum EstadoReproductor
    {
        Playing,
        Paused,
        Stop
    }
    public partial class Player : Form
    {
        public Playlist Playlist { get; set; }
        private readonly PlayerKernel nucleo = new PlayerKernel();
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
        private string fich;
        public EstadoReproductor estadoReproductor;
        private bool TiempoRestante = false;
        ToolTip DuracionSeleccionada;
        ToolTip VolumenSeleccionado;
        TimeSpan dur;
        TimeSpan pos;
        bool SpotifySync;
        FullTrack SpotifyPlayingSong;
        private BackgroundWorker backgroundWorker;
        public int ListaReproduccionPuntero { get; set; }
        bool SpotifyListo = false;
        bool EsPremium = false;
        DirectoryInfo directorioCanciones;
        PrivateUser user;
        private Log Log = Log.Instance;
        private float Volumen;
        private PlaylistIU PlaylistUI;
        private ToolTip duracionView;
        string Artist, Title = null;
        private bool foobar2000 = true;
        Process foobar2kInstance = null;
        string SpotifyID = null;
        bool Reproduciendo = false;
        bool ShuffleState = false;
        string PreviousSpotifyID = "";
        bool VolumeHold = false, PositonHold = false, ShuffleHold = false;
        private Song NowPlaying;
        private AlbumData NowPlayingAlbum;
        Random Random { get; }
        public bool ModoCD { get; private set; }
        private static Player instance = null;
        public static Player Instancia { get => instance; }
        public static void Init()
        {
            instance = new Player();
        }

        public Player()
        {
            InitializeComponent();

            Activated += (object sender, EventArgs e) => { timerSpotify.Interval = 150; };
            Deactivate += (object sender, EventArgs e) => { timerSpotify.Interval = 1000; };
            SetPlayerButtons(false);
            timerCancion.Enabled = false;
            estadoReproductor = EstadoReproductor.Stop;
            trackBarPosicion.Enabled = false;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            Volumen = 1.0f;
            trackBarVolumen.Value = 100;
            duracionView = new ToolTip();
            buttonAgregar.Hide();
            if (Kernel.Spotify is null)
                buttonSpotify.Enabled = false;
            else
            {
                buttonSpotify.Enabled = true;
                if (!Kernel.Spotify.AccountReady)
                    buttonSpotify.Enabled = false;
            }
            Icon = Resources.iconoReproductor;
            if (Kernel.MetadataStream) //inicia el programa con solo la imperesión
            {
                notifyIconStream.Visible = true;
                while (!Kernel.Spotify.AccountReady)
                {
                    Thread.Sleep(100);
                }
                try
                {
                    ActivarSpotify();
                }
                catch (APIException ex)
                {
                    Log.Instance.PrintMessage("Could not load Spotify!", MessageType.Error);
                    MessageBox.Show(ex.Message);
                }

            }

            else notifyIconStream.Visible = false;
            buttonTwit.Enabled = false;
            ModoCD = false;
            Random = new Random();
        }

        private void Player_Activated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConfigurarTimers(bool val) //Configura los timers cancion y metadatos.
        {
            timerCancion.Enabled = val;
            timerMetadatos.Enabled = val;
        }

        public void ActivarPorLista() //Prepara para reproducir una lista de reproducción
        {
            SetPlayerButtons(true);
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
            SetWindowTitle(Kernel.LocalTexts.GetString("reproductor"));
            labelDatosCancion.Text = "";
            notifyIconReproduciendo.Visible = false;
            if (!(PlaylistUI is null))
                PlaylistUI.Stop(); //Update the UI if it's not null.
        }

        public void PlayCD(char disp)
        {
            //reproduce un cd
            SetPlayerButtons(true);
            nucleo.ReproducirCD(disp);
            Reproduciendo = true;
            if (nucleo.PistasCD == null) //fail?
                return;
            ModoCD = true;
            PrepararReproductor();
            SetWindowTitle("CD - Pista 1");
            CreatePlaylist("CD-A");
            for (int i = 0; i < nucleo.PistasCD.Length; i++)
            {
                Song c = new Song("Pista " + (i + 1), (int)nucleo.PistasCD[i].Duracion.TotalMilliseconds, false);
                Playlist.AddSong(c);
            }
            PlaylistUI.Refresh();
        }
        public void SpotifyEncendido()
        {
            buttonSpotify.Enabled = true;
        }
        private void PrepararSpotify()
        {
            try
            {
                user = Kernel.Spotify.GetPrivateUser();
                Log.PrintMessage("Starting player with Spotify mode, e-mail: " + user.Email, MessageType.Info);
                EsPremium = Kernel.Spotify.UserIsPremium();
            }
            catch (APIException ex)
            {
                SpotifyListo = false;
                timerSpotify.Enabled = false;
                Log.PrintMessage(ex.Message, MessageType.Error);
                throw ex;
            }

            SpotifySync = true;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            SpotifyPlayingSong = new FullTrack();
            PreviousSpotifyID = SpotifyPlayingSong.Id;

            SpotifyListo = true;
            timerSpotify.Enabled = true;
            toolStripStatusLabelCorreoUsuario.Text = Kernel.LocalTexts.GetString("conectadoComo") + " " + user.DisplayName;
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
            Log.Instance.PrintMessage("Shutting down Spotify", MessageType.Info);
            backgroundWorker.CancelAsync();
            buttoncrearLR.Show();
            buttonSpotify.Text = Kernel.LocalTexts.GetString("cambiarSpotify");
            timerSpotify.Enabled = false;
            estadoReproductor = EstadoReproductor.Stop;
            SpotifySync = false;
            timerCancion.Enabled = false;
            timerMetadatos.Enabled = false;
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            buttonAbrir.Enabled = true;
            trackBarPosicion.Value = 0;
            trackBarPosicion.Maximum = 0;
            dur = new TimeSpan(0);
            pos = new TimeSpan(0);
            labelPosicion.Text = "0:00";
            labelDuracion.Text = "XX:XX";
            Volumen = 1.0f;
            SetWindowTitle("");
            toolStripStatusLabelCorreoUsuario.Text = "";
            labelDatosCancion.Text = "";
            Icon = Resources.iconoReproductor;
            checkBoxFoobar.Visible = true;
            SetPlayerButtons(false);
            buttonDetener.Enabled = true;
            File.Delete("./covers/np.jpg");
            buttonAgregar.Hide();
        }
        public void ActivarSpotify()
        {
            Log.Instance.PrintMessage("Changing to Spotify", MessageType.Info);
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
            if (Kernel.Spotify.AccountLinked)
            {
                if (!SpotifyListo || Kernel.MetadataStream)
                {
                    try
                    {
                        PrepararSpotify();
                    }
                    catch (APIException ex)
                    {

                        throw ex;
                    }
                }
                buttonAgregar.Show();
                Icon = Properties.Resources.spotifyico;
                timerSpotify.Enabled = true;
                buttonSpotify.Text = Kernel.LocalTexts.GetString("cambiarLocal");
                buttonAbrir.Enabled = false;
                SpotifySync = true;
                toolStripStatusLabelCorreoUsuario.Text = Kernel.LocalTexts.GetString("conectadoComo") + " " + user.DisplayName;
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
        private void DownloadCoverAndSet(SimpleAlbum album, bool firstTry)
        {
            if (album is null)
                return;
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    if (File.Exists("./covers/np.jpg") && pictureBoxCaratula.Image != null)
                    {
                        File.Delete("./covers/np.jpg");
                    }

                    cliente.DownloadFileAsync(new Uri(album.Images[1].Url), Environment.CurrentDirectory + "/covers/np.jpg");
                    cliente.DownloadFileCompleted += (s, e) =>
                    {
                        //Doing this will allow me to replace album cover and not locking the file
                        System.Drawing.Image cover;
                        using (var temp = new Bitmap("./covers/np.jpg"))
                            cover = new Bitmap(temp);
                        pictureBoxCaratula.Image = cover;
                    };
                }
                catch (System.Net.WebException ex)
                {
                    if (firstTry)
                    {
                        Log.PrintMessage("Couldn't download the album cover, retrying...", MessageType.Warning);
                        DownloadCoverAndSet(album, false);
                    }
                    else
                    {
                        Log.PrintMessage("Second try failed.", MessageType.Error);
                        Log.PrintMessage(ex.Status.ToString(), MessageType.Warning);

                    }
                    pictureBoxCaratula.Image = Resources.albumdesconocido;
                    File.Delete("./covers/np.jpg");
                }
                catch (IOException)
                {
                    Log.PrintMessage("Couldn't download the album cover, cannot replace...", MessageType.Error);
                }
            }
        }
        private string GetTextButtonPlayer(EstadoReproductor er)
        {
            switch (er)
            {
                case EstadoReproductor.Playing: //return pause
                    return ";";
                case EstadoReproductor.Paused: //return play
                case EstadoReproductor.Stop:
                    return "4";
            }
            return "";
        }
        //Plays the playlist from the start. UI shouldn't be null and should be refreshed here.
        public void ReproducirLista()
        {
            ListaReproduccionPuntero = 0;
            PlaylistUI.RefreshView();
            Song c = Playlist[ListaReproduccionPuntero];
            PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
            PlaySong(c);
        }
        public void RefrescarTextos()
        {
            PonerTextos();
        }
        private void PonerTextos()
        {
            if (!Reproduciendo)
                SetWindowTitle(Kernel.LocalTexts.GetString("reproductor"));
            buttonSpotify.Text = Kernel.LocalTexts.GetString("cambiarSpotify");
            notifyIconStream.Text = Kernel.LocalTexts.GetString("cerrarModoStream");
            buttoncrearLR.Text = Kernel.LocalTexts.GetString("crearLR");
            buttonAgregar.Text = Kernel.LocalTexts.GetString("agregarBD");
            buttonTwit.Text = Kernel.LocalTexts.GetString("twittearCancion");
            buttonAbrir.Text = Kernel.LocalTexts.GetString("abrir_cancion");
            notifyIconReproduciendo.Text = Kernel.LocalTexts.GetString("click_reproductor");
        }

        //public void SetPATH(Song c) //probablemente deprecated pero configura los paths
        //{
        //    directorioCanciones = new DirectoryInfo(c.AlbumFrom.SoundFilesPath);
        //    foreach (FileInfo file in directorioCanciones.GetFiles())
        //    {
        //        try
        //        {
        //            MetadataSong LM = new MetadataSong(file.FullName);
        //            if (LM.Evaluable() && c.Title.ToLower() == LM.Title.ToLower() && c.AlbumFrom.Artist.ToLower() == LM.Artist.ToLower())
        //            {
        //                c.Path = file.FullName;
        //                break;
        //            }
        //            else
        //            {
        //                if (file.FullName.ToLower().Contains(c.Title.ToLower()))
        //                {
        //                    c.Path = file.FullName;
        //                    SetWindowTitle(c.ToString());
        //                    break;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

        //    }
        //}

        public void PlaySong(string path) //reproduce una cancion por path
        {
            SetPlayerButtons(true);
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Stop;
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
            MetadataSong Lector = new MetadataSong(path);
            if (Lector.Cover != null)
                pictureBoxCaratula.Image = Lector.Cover;
            SetWindowTitle(Lector.GetSongID());
            nucleo.Apagar();
            try
            {
                nucleo.CargarCancion(path);
                PrepararReproductor();
                nucleo.Reproducir();
            }
            catch (Exception e)
            {
                Log.PrintMessage("A problem happened playing the song", MessageType.Error);
                Log.PrintMessage(e.Message, MessageType.Error);
                MessageBox.Show(Kernel.LocalTexts.GetString("errorReproduccion"));
                return;
            }

            if (Config.HistoryEnabled)
            {
                using (StreamWriter escritor = new StreamWriter(Kernel.HistorialFileInfo.FullName, true))
                {
                    AlbumData album = new AlbumData(Kernel.Genres[^1], "", Lector.Artist, (short)Lector.Year);
                    Song song = new Song(Lector.Title, Lector.Length, ref album);
                    NowPlaying = song;
                    NowPlayingAlbum = album;
                    escritor.WriteLine(Utils.GetHistoryString(song, Kernel.SongCount));
                    Kernel.SongCount++;
                }
            }
        }
        public void PlaySong(Song c) //reproduce una cancion
        {
            if (c.AlbumFrom is null) //Puede darse el caso de que sea una canción local suelta, usamos el otro método.
            {
                PlaySong(c.Path);
                return;
            }
            SetPlayerButtons(true);
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Stop;
            pictureBoxCaratula.Image.Dispose();
            pictureBoxCaratula.Image = null;

            MetadataSong LM = new MetadataSong(c.Path);
            ConfigSong(LM.Artist, LM.Title);

            if (string.IsNullOrEmpty(c.AlbumFrom.CoverPath))
                pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.AlbumFrom.CoverPath);
            else
            {
                if (File.Exists(c.AlbumFrom.SoundFilesPath + "\\cover.jpg"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.AlbumFrom.SoundFilesPath + "\\cover.jpg");
                else if (File.Exists(c.AlbumFrom.SoundFilesPath + "\\cover.png"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.AlbumFrom.SoundFilesPath + "\\cover.png");
                else if (File.Exists(c.AlbumFrom.SoundFilesPath + "\\folder.jpg"))
                    pictureBoxCaratula.Image = System.Drawing.Image.FromFile(c.AlbumFrom.SoundFilesPath + "\\folder.jpg");
            }
            if (LM.Cover != null && pictureBoxCaratula.Image is null)
                pictureBoxCaratula.Image = LM.Cover;

            SetWindowTitle(LM.GetSongID());
            nucleo.Apagar();
            try
            {
                nucleo.CargarCancion(c.Path);
                PrepararReproductor();
                nucleo.Reproducir();

                NowPlaying = c;
                NowPlayingAlbum = c.AlbumFrom;
            }
            catch (Exception)
            {
                Log.PrintMessage("Cannot play", MessageType.Error);
                return;
            }
            if (Config.HistoryEnabled)
            {
                using (StreamWriter escritor = new StreamWriter(Kernel.HistorialFileInfo.FullName, true))
                {
                    escritor.WriteLine(Utils.GetHistoryString(c, Kernel.SongCount));
                    Kernel.SongCount++;
                }
            }
        }
        public void PlaySong(LongSong c)
        {
            SetPlayerButtons(true);
            ConfigurarTimers(false);
            estadoReproductor = EstadoReproductor.Stop;

            if (Playlist is null)
                CreatePlaylist(c.Title);
            foreach (Song song in c.Parts)
            {
                Playlist.AddSong(song);
            }
            ReproducirLista();
        }
        public void PlaySong(int Pista)
        {
            ConfigurarTimers(false);
            ListaReproduccionPuntero = Pista;
            estadoReproductor = EstadoReproductor.Stop;
            if (ModoCD)
                nucleo.SaltarCancionCD(Pista);
            else
                PlaySong(Playlist.Songs[Pista]);
            PrepararReproductor();
            PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
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
            if (ModoCD)
            {
                dur = nucleo.PistasCD[ListaReproduccionPuntero].Duracion;
                SetWindowTitle("CD - Pista " + (ListaReproduccionPuntero + 1));
            }
            labelDatosCancion.Text = nucleo.GetDatos();
            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
            labelDuracion.Text = (int)dur.TotalMinutes + ":" + dur.Seconds;
            estadoReproductor = EstadoReproductor.Playing;
            buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
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
        //configs the song that's about to play...
        private void ConfigSong(string art, string tit)
        {
            Artist = art;
            Title = tit;
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

        //Creates a playlist and sets it as the active one, overriding the previous one.
        public void CreatePlaylist(string Title)
        {
            buttoncrearLR.Text = Kernel.LocalTexts.GetString("verLR");
            Playlist lr = new Playlist(Title);
            Playlist = lr;
            ListaReproduccionPuntero = 0;
            if (PlaylistUI is null)
                CreatePlaylistUI();
            PlaylistUI.Playlist = lr;
        }
        private void CreatePlaylistUI()
        {
            PlaylistUI = new PlaylistIU(Playlist);

        }
        public void SetPlaylist(Playlist playlist) //Sets a loaded playlist from a file.
        {
            Playlist = playlist;
            //Not needed to change the text, because we have a empty playlist after opening the form.
        }

        private void SaltarAtras()
        {
            if (SpotifySync && EsPremium)
                Kernel.Spotify.SkipPrevious();
            else
            {
                if (Playlist != null && !Playlist.IsFirstSong(ListaReproduccionPuntero))
                {
                    ListaReproduccionPuntero--;
                    PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
                    if (!ModoCD)
                        PlaySong(Playlist.GetSong(ListaReproduccionPuntero));
                    else
                        PlaySong(ListaReproduccionPuntero);
                }
            }
        }

        private void SaltarAdelante()
        {
            if (EsPremium && SpotifySync)
                Kernel.Spotify.SkipNext();
            else
            {
                if (Playlist != null)
                {
                    if (Playlist.IsOutside(ListaReproduccionPuntero))
                    {
                        nucleo.Stop();
                        buttonReproducirPausar.Text = GetTextButtonPlayer(EstadoReproductor.Stop);
                    }
                    else
                    {
                        try
                        {
                            if (!ShuffleState)
                                ListaReproduccionPuntero++;
                            else
                                ListaReproduccionPuntero = Random.Next(Playlist.Songs.Count);

                            PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
                            if (!ModoCD)
                                PlaySong(Playlist.GetSong(ListaReproduccionPuntero));
                            else
                                PlaySong(ListaReproduccionPuntero);
                        }
                        catch (Exception)
                        {
                            Log.PrintMessage("Out of bounds!", MessageType.Error);
                            MessageBox.Show("Out of bounds!", "Player error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }

                }
            }
        }

        private void PausaReproducir()
        {
            switch (estadoReproductor)
            {
                case EstadoReproductor.Playing: //Si está reproduciendo pausa.
                    if (!SpotifySync)
                        nucleo.Pausar();
                    else if (SpotifySync && EsPremium)
                    {
                        try
                        {
                            Kernel.Spotify.PlayResume();
                        }
                        catch (APIException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    estadoReproductor = EstadoReproductor.Paused;
                    buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
                    break;

                case EstadoReproductor.Paused:
                    if (!SpotifySync)
                        nucleo.Reproducir();
                    else if (SpotifySync && EsPremium)
                    {
                        try
                        {
                            Kernel.Spotify.PlayResume();
                        }
                        catch (APIException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    estadoReproductor = EstadoReproductor.Playing;
                    buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
                    break;
                case EstadoReproductor.Stop:
                    if (!SpotifySync)
                        nucleo.Reproducir();
                    else if (SpotifySync && EsPremium)
                    {
                        try
                        {
                            Kernel.Spotify.PlayResume();
                        }
                        catch (APIException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    estadoReproductor = EstadoReproductor.Playing;
                    buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
                    break;
                default:
                    break;
            }
        }

        private string GetSongTime(TimeSpan time)
        {
            if (time.TotalMinutes >= 60)
                return time.ToString(@"hh\:mm\:ss");
            else
                return time.ToString(@"mm\:ss");
        }

        #region Events
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CurrentlyPlayingContext PC = (CurrentlyPlayingContext)e.Result; //datos de spotify

            if (PC != null && PC.Item != null) //si son válidos
            {
                if (PC.Item.Type == ItemType.Track)
                {
                    SpotifyPlayingSong = (FullTrack)PC.Item;
                    dur = new TimeSpan(0, 0, 0, 0, SpotifyPlayingSong.DurationMs);
                    SpotifyID = SpotifyPlayingSong.Id;
                    trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                }
                else SpotifyPlayingSong = null;
                pos = new TimeSpan(0, 0, 0, 0, PC.ProgressMs);
                if (!Kernel.MetadataStream)
                {
                    //update trackbar
                    trackBarPosicion.Value = (int)pos.TotalSeconds;
                    //if we don't have an image or we have the same one
                    if (SpotifyID != PreviousSpotifyID || !File.Exists("./covers/np.jpg"))
                    {
                        //Update shuffle state not too often, when changin songs
                        if (PC.ShuffleState)
                            checkBoxAleatorio.Checked = true;
                        else
                            checkBoxAleatorio.Checked = false;

                        PreviousSpotifyID = SpotifyID;
                        if (Config.HistoryEnabled)
                        {
                            using (StreamWriter escritor = new StreamWriter(Kernel.HistorialFileInfo.FullName, true))
                            {
                                if (SpotifyPlayingSong is not null)
                                {
                                    escritor.WriteLine(Utils.GetHistoryString(SpotifyPlayingSong, Kernel.SongCount));
                                    Kernel.SongCount++;
                                }
                            }
                        }
                        //Check if local song
                        if (!string.IsNullOrEmpty(SpotifyID))
                        {
                            try
                            {
                                DownloadCoverAndSet(SpotifyPlayingSong.Album, true);

                            }
                            catch (Exception ex)
                            {
                                pictureBoxCaratula.Image = Resources.albumdesconocido;
                                Log.Instance.PrintMessage("Couldn't set the new cover!" + ex.Message, MessageType.Warning);
                            }
                        }
                        else
                        {
                            Log.PrintMessage("Local song detected.", MessageType.Info);
                            trackBarPosicion.Maximum = (int)dur.TotalSeconds;
                            pictureBoxCaratula.Image.Dispose();
                            pictureBoxCaratula.Image = Resources.albumdesconocido;
                        }
                    }
                    if (PC.IsPlaying)
                    {
                        estadoReproductor = EstadoReproductor.Playing;
                        buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
                        timerCancion.Enabled = true;
                    }
                    else
                    {
                        estadoReproductor = EstadoReproductor.Paused;
                        buttonReproducirPausar.Text = GetTextButtonPlayer(estadoReproductor);
                        timerCancion.Enabled = false;
                    }

                    SpotifyPlayingSong = (FullTrack)PC.Item;
                    SetWindowTitle(SpotifyPlayingSong.Artists[0].Name + " - " + SpotifyPlayingSong.Name);
                    if (!VolumeHold)
                        trackBarVolumen.Value = (int)PC.Device.VolumePercent;
                    if (string.IsNullOrEmpty(SpotifyPlayingSong.Id))
                        buttonAgregar.Enabled = false;
                    else
                        buttonAgregar.Enabled = true;
                }
                if (Config.StreamEnabled)
                {
                    using (StreamWriter salida = new StreamWriter(Kernel.StreamFileInfo.FullName))
                    {
                        TimeSpan pos = TimeSpan.FromMilliseconds(PC.ProgressMs);
                        salida.WriteLine(Utils.GetStreamString(SpotifyPlayingSong, Kernel.SongCount, pos));
                    }
                }
            }
            else
            {
                SetWindowTitle("No Spotify context");
                //reset the player but once.
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) //tarea asíncrona que comprueba si el token ha caducado y espera a la tarea que lo refresque
        {
            //Throws exception but it's catched and returns null
            CurrentlyPlayingContext PC = Kernel.Spotify.GetPlayingContext();
            if (PC is not null)
                e.Result = PC;
            else //we have a problem, just wait.
                Thread.Sleep(100);
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
            Log.PrintMessage("Starting player in local mode", MessageType.Info);
            try
            {
                foobar2kInstance = Process.GetProcessesByName("foobar2000")[0];
                Log.PrintMessage("foobar2000 has been found!", MessageType.Correct);
            }
            catch (IndexOutOfRangeException)
            {

                Log.PrintMessage("foobar2000 hasn't been found on the system", MessageType.Info);
                foobar2kInstance = null;
                checkBoxFoobar.Enabled = false;
            }
        }
        private void timerCancion_Tick(object sender, EventArgs e)
        {
            if (estadoReproductor == EstadoReproductor.Stop)
                trackBarPosicion.Enabled = false;
            else
                trackBarPosicion.Enabled = true;
            if (!SpotifySync && timerCancion.Enabled && nucleo.ComprobarSonido())
            {
                pos = nucleo.Posicion();
                //using (StreamWriter salida = new StreamWriter("np.txt"))
                //{
                //    /*
                //    if (CancionLocalReproduciendo == null)
                //        salida.WriteLine(Text);
                //    else
                //        salida.WriteLine(CancionLocalReproduciendo.ToString());
                //    salida.Write(pos.ToString(@"mm\:ss") + " / ");
                //    salida.Write(dur.ToString(@"mm\:ss"));
                //    */
                //}
            }
            labelPosicion.Text = GetSongTime(pos);
            if (pos > dur)
                dur = pos;
            if (TiempoRestante)
            {
                TimeSpan tRes = dur - pos;
                labelDuracion.Text = "-" + GetSongTime(tRes);
            }
            else
            {
                labelDuracion.Text = GetSongTime(dur);
            }
            if (nucleo.ComprobarSonido())
            {
                double val = pos.TotalMilliseconds / dur.TotalMilliseconds * trackBarPosicion.Maximum;
                trackBarPosicion.Value = (int)val;
            }

            if (pos.Minutes == dur.Minutes && pos.Seconds == dur.Seconds)
            {
                estadoReproductor = EstadoReproductor.Stop;
                if (Playlist != null)
                {
                    ListaReproduccionPuntero++;
                    //Check if we are out
                    if (!Playlist.IsOutside(ListaReproduccionPuntero))
                    {
                        PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
                        if (!ModoCD)
                            PlaySong(Playlist.GetSong(ListaReproduccionPuntero));
                        else
                            PlaySong(ListaReproduccionPuntero);
                    }

                    else
                        nucleo.Stop();
                }
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Kernel.PlayerMode || !Kernel.MetadataStream)
            {
                Hide();
                if (Reproduciendo || SpotifySync)
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
            string songPath = null;

            openFileDialog1.Filter = "*.mp3, *.flac, *.ogg|*.mp3;*.flac;*.ogg";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                nucleo.Apagar();
                estadoReproductor = EstadoReproductor.Stop;
                songPath = openFileDialog1.FileName;

                this.fich = songPath;

                try
                {
                    if (FicheroLeible(songPath))
                    {
                        Song c = new Song();
                        c.Path = songPath;

                        if (Playlist == null)
                            Playlist = new Playlist("Selección");
                        Playlist.AddSong(c);
                        ReproducirLista();
                    }
                }
                catch (Exception ex)
                {
                    Log.PrintMessage("Cannot load the song. " + songPath, MessageType.Error);
                    Log.PrintMessage(ex.Message, MessageType.Error);
                    nucleo.Apagar();
                    return;
                }
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

            if (!SpotifySync)
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
            else if (SpotifySync && EsPremium)
            {
                long pos = trackBarPosicion.Value * 1000;
                Kernel.Spotify.SeekTo(pos);
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
            if (!SpotifySync && (nucleo.ComprobarSonido()))
                nucleo.SetVolumen(Volumen);
        }

        private void trackBarVolumen_MouseDown(object sender, MouseEventArgs e)
        {
            VolumeHold = true;
            Volumen = (float)trackBarVolumen.Value / 100;
        }

        private void trackBarVolumen_MouseHover(object sender, EventArgs e)
        {
            VolumenSeleccionado.SetToolTip(trackBarVolumen, trackBarVolumen.Value + "%");
        }

        private void timerSpotify_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
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
            SetShuffle();
        }
        private void SetShuffle()
        {
            ShuffleState = checkBoxAleatorio.Checked;
            if (EsPremium && SpotifySync)
            {
                Kernel.Spotify.SetShuffle(ShuffleState);
            }

        }
        private void buttonSaltarAdelante_Click(object sender, EventArgs e)
        {
            SaltarAdelante();
        }

        private void buttonSaltarAtras_Click(object sender, EventArgs e)
        {
            SaltarAtras();
        }

        private void Reproductor_KeyDown(object sender, KeyEventArgs e)
        {
            //Condiciones especiales
            if (e.KeyData == Keys.F9 && !SpotifySync && PlaylistUI != null)
                PlaylistUI.Show();
            if (e.Control && e.KeyData == Keys.Right)
                buttonSaltarAdelante_Click(null, null);
            if (e.Control && e.KeyData == Keys.Left)
                buttonSaltarAtras_Click(null, null);
            switch (e.KeyData)
            {
                case Keys.Space:
                case Keys.MediaPlayPause:
                    PausaReproducir();
                    break;
                case Keys.MediaStop:
                    Detener();
                    break;
                case Keys.MediaNextTrack:
                    if (!SpotifySync)
                        SaltarAdelante();
                    break;
                case Keys.MediaPreviousTrack:
                    if (!SpotifySync)
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
            try
            {
                if (SpotifySync)
                    ApagarSpotify();
                else
                    ActivarSpotify();
            }
            catch (APIException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                return;
            }

        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            bool res = Kernel.Spotify.InsertAlbumFromURI(SpotifyPlayingSong.Album.Id);
            if (res) //Añadido correctamente...
                MessageBox.Show(Kernel.LocalTexts.GetString("album_agregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Kernel.LocalTexts.GetString("album_noagregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFoobar.Checked)
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
            using (StreamWriter salida = new StreamWriter(Kernel.StreamFileInfo.FullName, false))
            {
                salida.WriteLine(foobar2kInstance.MainWindowTitle);
            }
        }
        //Fixed in 1.7.0.16 !!!
        private void buttonTwit_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Writing a twit...", MessageType.Info);
            string test;
            string link = "https://twitter.com/intent/tweet?text=";
            if (SpotifySync && !string.IsNullOrEmpty(SpotifyPlayingSong.Id))
            {
                test = Kernel.LocalTexts.GetString("compartirTwitter1").Replace(" ", "%20") + "%20https://open.spotify.com/track/" + SpotifyPlayingSong.Id + "%0a" +
                    Kernel.LocalTexts.GetString("compartirTwitter2").Replace(" ", "%20") + "%20" + Kernel.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Kernel.Version + "%20" + Kernel.Codename;
            }
            else if (!ModoCD && Reproduciendo)
                test = Kernel.LocalTexts.GetString("compartirLocal1").Replace(" ", "%20") + "%20" +
                    NowPlaying.Title.Replace(" ", "%20") + "%20" +
                    Kernel.LocalTexts.GetString("compartirLocal2").Replace(" ", "%20") + "%20" +
                    NowPlayingAlbum.Artist.Replace(" ", "%20") + "%20" +
                    Kernel.LocalTexts.GetString("compartirLocal3").Replace(" ", "%20") + "%20" +
                    Kernel.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" +
                    Kernel.Version + "%20" + Kernel.Codename;
            else
                test = "Escuchando un CD con " + Kernel.LocalTexts.GetString("titulo_ventana_principal").Replace(" ", "%20") + "%20" +
                    Kernel.Version + "%20" + Kernel.Codename;
            link += test;
            Process.Start(new ProcessStartInfo("cmd", $"/c start {link}") { CreateNoWindow = true });
        }
        private void Reproductor_DragDrop(object sender, DragEventArgs e)
        {
            Log.PrintMessage("Drag & Drop detected", MessageType.Info);
            Song c = null;
            String[] canciones = null;
            if ((c = (Song)e.Data.GetData(typeof(Song))) != null)
            {
                if (!string.IsNullOrEmpty(c.Path))
                {
                    Log.PrintMessage("Success processing the input: " + c.Path, MessageType.Correct);
                    ModoCD = false;
                    PlaySong(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null)
            {
                Log.PrintMessage("Processing " + canciones.Length + " files", MessageType.Info);
                List<string> input = new List<string>();
                for (int i = 0; i < canciones.Length; i++)
                {
                    if (FicheroLeible(canciones[i]))
                        input.Add(canciones[i]);
                }
                if (input.Count == 0)
                {
                    Log.Instance.PrintMessage("No valid songs are on the input, maybe wrong file extensions?", MessageType.Info);
                    return;
                }
                Log.PrintMessage("Creating playlist with " + input.Count + " songs.", MessageType.Info);
                if (Playlist is null)
                {
                    CreatePlaylist(Kernel.LocalTexts.GetString("seleccion"));
                    foreach (string cancion in input)
                    {
                        if (cancion is not null)
                        {
                            Song clr = new Song();
                            clr.Path = cancion;
                            Playlist.AddSong(clr);
                        }
                    }
                    ReproducirLista();
                }
                else
                {
                    foreach (string songfile in input)
                    {
                        Song clr = new Song();
                        clr.Path = songfile;
                        Playlist.AddSong(clr);
                    }
                    PlaylistUI.RefreshView();
                }
            }
            else
                Log.PrintMessage("Cannot process the data. Wrong input?", MessageType.Warning);
        }

        private void Reproductor_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void buttonLR_Click(object sender, EventArgs e)
        {
            if (Playlist is null) //If we don't have a playlist, create one. If we don't have the UI, create it.
            {
                CreatePlaylist("");
                if (PlaylistUI is null)
                    CreatePlaylistUI();
                PlaylistUI.Show();
            }
            else
            {
                PlaylistUI.RefreshView();
                PlaylistUI.SetActiveSong(ListaReproduccionPuntero);
                PlaylistUI.Show();
            }
        }
        private void buttonDetener_Click(object sender, EventArgs e)
        {
            Detener();
        }

        private void trackBarVolumen_MouseUp(object sender, MouseEventArgs e)
        {
            int vol = trackBarVolumen.Value;
            if (EsPremium && SpotifySync)
                Kernel.Spotify.SetVolume(vol);
            VolumeHold = false;
        }

        private void notifyIconReproduciendo_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
        }
        #endregion
    }
}
