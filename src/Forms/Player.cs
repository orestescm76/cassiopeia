using Cassiopeia.Properties;
using Cassiopeia.src.Classes;
using Cassiopeia.src.Player;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{

    public partial class Player : Form
    {
        private bool RemainingTime = false;
        ToolTip DuracionSeleccionada;
        ToolTip VolumenSeleccionado;
        TimeSpan dur;
        TimeSpan pos;
        private BackgroundWorker backgroundWorker;
        DirectoryInfo directorioCanciones;
        private Log Log = Log.Instance;
        private float Volumen;
        private PlaylistIU PlaylistUI;
        private ToolTip duracionView;
        private bool foobar2000 = true;
        Process foobar2kInstance = null;
        bool Reproduciendo = false;
        bool ShuffleState = false;
        string PreviousSpotifyID = "";
        bool VolumeHold = false, PositonHold = false, ShuffleHold = false;
        private Song NowPlaying;
        private AlbumData NowPlayingAlbum;
        private IPlayer PlayerImplementation;
        Random Random { get; }
        public bool ModoCD { get; private set; }
        private static Player instance = null;
        public static Player Instancia { get => instance; }
        public bool SpotifyListo;
        public bool SpotifySync;

        private CancellationTokenSource RefreshTaskTokenSource;
        private CancellationToken RefreshTaskCancellationToken;
        private Task RefreshSpotifyTask;
        public static void Init()
        {
            instance = new Player();
        }

        public Player()
        {
            InitializeComponent();
            PlayerImplementation = new LocalPlayer();
            Activated += (object sender, EventArgs e) => { timerUIRefresh.Interval = 150; };
            Deactivate += (object sender, EventArgs e) => { timerUIRefresh.Interval = 1000; };
            SetPlayerButtons(false);
            timerUIRefresh.Enabled = false;
            trackBarPosition.Enabled = false;
            DuracionSeleccionada = new ToolTip();
            VolumenSeleccionado = new ToolTip();
            Volumen = 1.0f;
            trackBarVolumen.Value = 100;
            duracionView = new ToolTip();
            buttonAdd.Hide();
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
                    ActivateSpotifySync();
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

        private void ConfigTimers(bool val) //Configura los timers cancion y metadatos.
        {
            timerUIRefresh.Enabled = val;
            timerMetadataRefresh.Enabled = val;
        }

        public void ActivarPorLista() //Prepara para reproducir una lista de reproducción
        {
            SetPlayerButtons(true);
        }

        private void Stop()
        {
            PlayerImplementation.Stop();
            ConfigTimers(false);
            SetPlayerButtons(false);
            trackBarPosition.Enabled = false;
            trackBarPosition.Value = 0;
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            labelDuracion.Text = "XX:XX";
            labelPosicion.Text = "0:00";
            labelPorcentaje.Text = "0%";
            SetWindowTitle(Kernel.GetText("reproductor"));
            labelDatosCancion.Text = "";
            notifyIconReproduciendo.Visible = false;
            if (PlaylistUI is not null)
                PlaylistUI.Stop(); //Update the UI if it's not null.
        }

        public void PlayCD(char disp)
        {
            /*
            //reproduce un cd
            SetPlayerButtons(true);
            nucleo.ReproducirCD(disp);
            Reproduciendo = true;
            if (nucleo.PistasCD is null) //fail?
                return;
            ModoCD = true;
            PlayerImplementation.PlaylistPointer = 0;
            PrepararReproductor();
            SetWindowTitle("CD - Pista 1");
            CreatePlayerImplementation.Playlist("CD-A");
            for (int i = 0; i < nucleo.PistasCD.Length; i++)
            {
                Song c = new Song("Pista " + (i + 1), (int)nucleo.PistasCD[i].Duracion.TotalMilliseconds, false);
                PlayerImplementation.Playlist.AddSong(c);
            }
            PlayerImplementation.PlaylistUI.SetActiveSong(PlayerImplementation.PlaylistPointer);
            PlayerImplementation.PlaylistUI.RefreshView();
            Show();
            */
        }
        public void SpotifyEncendido()
        {
            buttonSpotify.Enabled = true;
        }
        private void PrepararSpotify(SpotifyPlayer spotifyPlayer)
        {
            SpotifySync = true;
            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.DoWork += BackgroundWorker_DoWork;
            //backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            //backgroundWorker.WorkerSupportsCancellation = true;
            //Prepare async task
            RefreshTaskTokenSource = new CancellationTokenSource();
            RefreshTaskCancellationToken = RefreshTaskTokenSource.Token;
            RefreshSpotifyTask = Task.Run(() =>
            {
                while(!RefreshTaskCancellationToken.IsCancellationRequested)
                {
                    //get playing context async
                    SpotifyPlayer spotifyPlayer = PlayerImplementation as SpotifyPlayer;
                    spotifyPlayer.RefreshPlayingContext();
                    RefreshSpotifyUI();
                }
            }
            );

            toolStripStatusLabelCorreoUsuario.Text = Kernel.GetText("conectadoComo") + " " + spotifyPlayer.User.DisplayName;
        }
        public void Apagar()
        {
            if (backgroundWorker != null)
                backgroundWorker.CancelAsync();
            if (pictureBoxCaratula.Image != null)
                pictureBoxCaratula.Image.Dispose();
            timerUIRefresh.Enabled = false;
            timerMetadataRefresh.Enabled = false;
            PlayerImplementation.Stop();
        }
        private void ApagarSpotify()
        {
            Log.Instance.PrintMessage("Shutting down Spotify", MessageType.Info);
            backgroundWorker.CancelAsync();
            buttoncrearLR.Show();
            buttonSpotify.Text = Kernel.GetText("cambiarSpotify");
            PlayerImplementation.State = PlayingState.Stop;
            SpotifySync = false;
            timerUIRefresh.Enabled = false;
            timerMetadataRefresh.Enabled = false;
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            buttonAbrir.Enabled = true;
            trackBarPosition.Value = 0;
            trackBarPosition.Maximum = 0;
            dur = new TimeSpan(0);
            pos = new TimeSpan(0);
            labelPosicion.Text = "0:00";
            labelDuracion.Text = "XX:XX";
            Volumen = 1.0f;
            SetWindowTitle(Kernel.GetText("reproductor"));
            toolStripStatusLabelCorreoUsuario.Text = "";
            labelDatosCancion.Text = "";
            Icon = Resources.iconoReproductor;
            //checkBoxFoobar.Visible = true;
            SetPlayerButtons(false);
            buttonDetener.Enabled = true;
            if (File.Exists("./covers/np.jpg"))
                File.Delete("./covers/np.jpg");
            buttonAdd.Hide();
        }
        public void ActivateSpotifySync()
        {
            Log.Instance.PrintMessage("Changing to Spotify", MessageType.Info);
            PlayerImplementation.Dispose();
            //Create Spotify player implementation
            PlayerImplementation = new SpotifyPlayer();
            SpotifyPlayer spotifyPlayer = (SpotifyPlayer)PlayerImplementation;
            try
            {
                timerMetadataRefresh.Enabled = false;
                timerUIRefresh.Enabled = false;
                checkBoxFoobar.Visible = false;
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
                try
                {
                    PrepararSpotify(spotifyPlayer);
                }
                catch (APIException ex)
                {
                    Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                    throw ex;
                }
                buttonAdd.Show();
                Icon = Resources.spotifyico;
                buttonSpotify.Text = Kernel.GetText("cambiarLocal");
                buttonAbrir.Enabled = false;
                SpotifySync = true;
                toolStripStatusLabelCorreoUsuario.Text = Kernel.GetText("conectadoComo") + " " + spotifyPlayer.User.DisplayName;
                SetPlayerButtons(true);
                if (!spotifyPlayer.UserIsPremium)
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

        private string GetTextButtonPlayer(PlayingState er)
        {
            switch (er)
            {
                case PlayingState.Playing: //return pause
                    return ";";
                case PlayingState.Paused: //return play
                case PlayingState.Stop:
                    return "4";
            }
            return "";
        }
        //Plays the Playlist from the start. UI shouldn't be null and should be refreshed here.
        public void PlayPlaylist()
        {
            PlayerImplementation.PlaylistPointer = 0;
            PlaylistUI.RefreshView();
            Song c = PlayerImplementation.Playlist[PlayerImplementation.PlaylistPointer];
            PlaylistUI.SetActiveSong(PlayerImplementation.PlaylistPointer);
            PlaySong(c);
        }
        public void RefrescarTextos()
        {
            PonerTextos();
        }
        private void PonerTextos()
        {
            if (!Reproduciendo)
                SetWindowTitle(Kernel.GetText("reproductor"));
            buttonSpotify.Text = Kernel.GetText("cambiarSpotify");
            notifyIconStream.Text = Kernel.GetText("cerrarModoStream");
            buttoncrearLR.Text = Kernel.GetText("crearLR");
            buttonAdd.Text = Kernel.GetText("agregarBD");
            buttonTwit.Text = Kernel.GetText("twittearCancion");
            buttonAbrir.Text = Kernel.GetText("abrir_cancion");
            notifyIconReproduciendo.Text = Kernel.GetText("click_reproductor");
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
            pictureBoxCaratula.Image = Resources.albumdesconocido;
            ConfigTimers(false);
            PlayerImplementation.State = PlayingState.Stop;
            PlayerImplementation.PlaySong(path);
            SetPlayerUI();
            //if (Config.HistoryEnabled)
            //{
            //    using (StreamWriter escritor = new StreamWriter(Kernel.HistorialFileInfo.FullName, true))
            //    {
            //        AlbumData album = new AlbumData(Kernel.Genres[^1], "", Lector.Artist, (short)Lector.Year);
            //        Song song = new Song(Lector.Title, Lector.Length, ref album);
            //        NowPlaying = song;
            //        NowPlayingAlbum = album;
            //        escritor.WriteLine(Utils.GetHistoryString(song, Kernel.SongCount));
            //        Kernel.SongCount++;
            //    }
            //}
        }
        public void PlaySong(Song c) //reproduce una cancion llamando al reproductor local
        {
            var local = (LocalPlayer)PlayerImplementation;
            local.PlaySong(c);
            SetPlayerButtons(true);
            ConfigTimers(false);

            pictureBoxCaratula.Image.Dispose();
            pictureBoxCaratula.Image = local.GetCover();

            SetWindowTitle(PlayerImplementation.GetSongPlaying());
            SetPlayerUI();
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
            //SetPlayerButtons(true);
            //ConfigTimers(false);
            //PlayerImplementation.State = PlayingState.Stop;

            //if (PlayerImplementation.Playlist is null)
            //    CreatePlayerImplementation.Playlist(c.Title);
            //foreach (Song song in c.Parts)
            //{
            //    PlayerImplementation.Playlist.AddSong(song);
            //}
            //PlayerImplementation.Playlist();
        }
        public void PlaySong(int Pista)
        {
            ConfigTimers(false);
            LocalPlayer localPlayer = (LocalPlayer)PlayerImplementation;
            localPlayer.PlaySong(localPlayer.Playlist.Songs[Pista]);
            SetPlayerUI();
            PlaylistUI.SetActiveSong(localPlayer.PlaylistPointer);
            timerUIRefresh.Enabled = true;
            timerMetadataRefresh.Enabled = false;
            buttonTwit.Enabled = false;
        }
        private void SetWindowTitle(string text)
        {
            Text = text;
        }
        private void SetPlayerUI()
        {
            trackBarPosition.Value = 0; //reseteo
            dur = PlayerImplementation.Duration;
            pos = TimeSpan.Zero;
            //if (ModoCD)
            //{
            //    dur = nucleo.PistasCD[PlayerImplementation.PlaylistPointer].Duracion;
            //    SetWindowTitle("CD - Pista " + (PlayerImplementation.PlaylistPointer + 1));
            //}
            labelDatosCancion.Text = PlayerImplementation.GetSongPlaying();
            labelDuracion.Text = (int)dur.TotalMinutes + ":" + dur.Seconds;
            buttonReproducirPausar.Text = GetTextButtonPlayer(PlayerImplementation.State);
            buttonTwit.Enabled = true;
            Reproduciendo = true;
            ConfigTimers(true);
            SetPlayerButtons(true);
        }

        //Controls the player buttons
        private void SetPlayerButtons(bool encendido)
        {
            buttonReproducirPausar.Enabled = encendido;
            buttonSaltarAdelante.Enabled = encendido;
            buttonSaltarAtras.Enabled = encendido;
        }

        //Creates a Playlist and sets it as the active one, overriding the previous one.
        public void CreatePlaylist(string Title)
        {
            buttoncrearLR.Text = Kernel.GetText("verLR");
            Playlist lr = new Playlist(Title);
            PlayerImplementation.Playlist = lr;
            PlayerImplementation.PlaylistPointer = -1;
            if (PlaylistUI is null)
                CreatePlaylistUI();
            PlaylistUI.Playlist = lr;
        }
        private void CreatePlaylistUI()
        {
            PlaylistUI = new PlaylistIU(PlayerImplementation.Playlist);

        }
        public void Playlist(Playlist Playlist) //Sets a loaded PlayerImplementation.Playlist from a file.
        {
            PlayerImplementation.Playlist = PlayerImplementation.Playlist;
            //Not needed to change the text, because we have a empty PlayerImplementation.Playlist after opening the form.
        }

        private void SaltarAtras()
        {
            PlayerImplementation.Previous();
            PlaylistUI?.SetActiveSong(PlayerImplementation.PlaylistPointer);
        }

        private void SaltarAdelante()
        {
            PlayerImplementation.Next();
            PlaylistUI?.SetActiveSong(PlayerImplementation.PlaylistPointer);
        }

        private void PausaReproducir()
        {
            switch (PlayerImplementation.State)
            {
                case PlayingState.Playing: //Si está reproduciendo pausa.
                    PlayerImplementation.Pause();
                    buttonReproducirPausar.Text = GetTextButtonPlayer(PlayerImplementation.State);
                    break;

                case PlayingState.Paused:
                    PlayerImplementation.Play();
                    buttonReproducirPausar.Text = GetTextButtonPlayer(PlayerImplementation.State);
                    break;
                case PlayingState.Stop:
                    PlayerImplementation.Play();
                    buttonReproducirPausar.Text = GetTextButtonPlayer(PlayerImplementation.State);
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

        //Sets the Spotify Switch button
        public void SetSpotify(bool flag)
        {
            buttonSpotify.Enabled = flag;
        }

        #region Events
        private void RefreshSpotifyUI()
        {
            //spotify info is refreshed
            var SpotifyPlayer = PlayerImplementation as SpotifyPlayer;
            pos = SpotifyPlayer.Position;
            trackBarPosition.Maximum = (int)SpotifyPlayer.Duration.TotalSeconds;
            trackBarPosition.Value = (int)pos.TotalSeconds;
            trackBarVolumen.Value = (int)SpotifyPlayer.Volume;
            checkBoxAleatorio.Checked = SpotifyPlayer.Shuffle;
            buttonReproducirPausar.Text = GetTextButtonPlayer(PlayerImplementation.State);
            SetWindowTitle(PlayerImplementation.GetSongPlaying());
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

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
            DuracionSeleccionada.SetToolTip(trackBarPosition, new TimeSpan(0, 0, trackBarPosition.Value).ToString());
        }

        private void Reproductor_Load(object sender, EventArgs e)
        {

            //try
            //{
            //    foobar2kInstance = Process.GetProcessesByName("foobar2000")[0];
            //    Log.PrintMessage("foobar2000 has been found!", MessageType.Correct);
            //}
            //catch (IndexOutOfRangeException)
            //{

            //    Log.PrintMessage("foobar2000 hasn't been found on the system", MessageType.Info);
            //    foobar2kInstance = null;
            //    checkBoxFoobar.Enabled = false;
            //}
        }
        private void timerUIRefresh_Tick(object sender, EventArgs e)
        {
            if (PlayerImplementation.State == PlayingState.Stop)
                trackBarPosition.Enabled = false;
            else
                trackBarPosition.Enabled = true;
            pos = PlayerImplementation.Position;
            labelPosicion.Text = GetSongTime(pos);
            if (pos > dur)
                dur = pos;
            if (RemainingTime)
            {
                TimeSpan tRes = dur - pos;
                labelDuracion.Text = "-" + GetSongTime(tRes);
            }
            else
            {
                labelDuracion.Text = GetSongTime(dur);
            }
            double val = pos.TotalMilliseconds / dur.TotalMilliseconds * trackBarPosition.Maximum;
            trackBarPosition.Value = (int)val;

            if (pos.Minutes == dur.Minutes && pos.Seconds == dur.Seconds)
            {
                PlayerImplementation.State = PlayingState.Stop;
                if (PlayerImplementation.Playlist is not null)
                {
                    PlayerImplementation.PlaylistPointer++;
                    //Check if we are out
                    if (!PlayerImplementation.Playlist.IsOutside(PlayerImplementation.PlaylistPointer))
                    {
                        PlaylistUI.SetActiveSong(PlayerImplementation.PlaylistPointer);
                        if (!ModoCD)
                            PlaySong(PlayerImplementation.Playlist.GetSong(PlayerImplementation.PlaylistPointer));
                        else
                            PlaySong(PlayerImplementation.PlaylistPointer);
                    }

                    else
                        PlayerImplementation.Stop();
                }
            }
        }

        private void Reproductor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!Kernel.PlayerMode || !Kernel.MetadataStream)
            //{
            //    Hide();
            //    if (Reproduciendo || SpotifySync)
            //        notifyIconReproduciendo.Visible = true;
            //    if (e.CloseReason != CloseReason.ApplicationExitCall)
            //        e.Cancel = true;
            //    else
            //        e.Cancel = false;
            //}
            //else
            //{
            //    if (nucleo != null)
            //        nucleo.Apagar();
            //    Dispose();
            //    Application.Exit();
            //}

        }

        private void buttonAbrir_Click(object sender, EventArgs e)
        {
            string songPath = null;

            openFileDialog1.Filter = "*.mp3, *.flac, *.ogg|*.mp3;*.flac;*.ogg";
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.Cancel)
            {
                //load the song
                PlayerImplementation.Stop();
                songPath = openFileDialog1.FileName;
                try
                {
                    //if no playlist
                    //todo change name
                    if (PlayerImplementation.Playlist is null)
                        CreatePlaylist("Selección");
                    LocalSong song = new LocalSong(songPath);
                    PlayerImplementation.Playlist.AddSong(song);
                    PlayPlaylist();
                }
                catch (Exception ex)
                {
                    Log.PrintMessage("Cannot load the song. " + songPath, MessageType.Error);
                    Log.PrintMessage(ex.Message, MessageType.Error);
                    PlayerImplementation.Stop();
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
            if (RemainingTime)
                RemainingTime = false;
            else RemainingTime = true;
        }

        private void trackBarPosicion_MouseDown(object sender, MouseEventArgs e)
        {
            timerUIRefresh.Enabled = false;
            timerMetadataRefresh.Enabled = false;
            trackBarPosition.Value = (int)((e.X * dur.TotalSeconds) / Size.Width);
        }

        private void trackBarPosicion_MouseUp(object sender, MouseEventArgs e)
        {

            //if (!SpotifySync)
            //{
            //    timerUIRefresh.Enabled = true;
            //    timerMetadataRefresh.Enabled = true;
            //    int value = trackBarPosition.Value;
            //    if (!ModoCD)
            //        nucleo.Saltar(TimeSpan.FromSeconds(trackBarPosition.Value));
            //    else
            //        nucleo.SaltarCD((int)nucleo.TimeSpanASectores(TimeSpan.FromSeconds(trackBarPosition.Value)));
            //    trackBarPosition.Value = value;
            //}
            //else if (SpotifySync && EsPremium)
            //{
            //    long pos = trackBarPosition.Value * 1000;
            //    Kernel.Spotify.SeekTo(pos);
            //    timerSpotify.Enabled = true;
            //}
            //else
            //    timerSpotify.Enabled = true;
        }
        private void trackBarPosicion_Scroll(object sender, EventArgs e)
        {
            ConfigTimers(false);
            pos = new TimeSpan(0, 0, trackBarPosition.Value);
            duracionView.SetToolTip(trackBarPosition, pos.ToString());
            timerUIRefresh_Tick(null, null);
        }
        private void trackBarVolumen_Scroll(object sender, EventArgs e)
        {
            //Volumen = (float)trackBarVolumen.Value / 100;
            //if (!SpotifySync && !(nucleo.ComprobarSonido()))
            //    nucleo.SetVolumen(Volumen);
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

        private void trackBarVolumen_ValueChanged(object sender, EventArgs e)
        {
            labelVolumen.Text = trackBarVolumen.Value.ToString() + "%";
        }

        private void trackBarPosicion_ValueChanged(object sender, EventArgs e)
        {
            labelPorcentaje.Text = trackBarPosition.Value * 100 / trackBarPosition.Maximum + "%";
        }

        private void checkBoxAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            SetShuffle();
        }
        private void SetShuffle()
        {
            //ShuffleState = checkBoxAleatorio.Checked;
            //if (EsPremium && SpotifySync)
            //{
            //    Kernel.Spotify.SetShuffle(ShuffleState);
            //}

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
            if (e.KeyData == Keys.F9 && !SpotifySync && PlaylistUI is not null)
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
                    Stop();
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
            labelDatosCancion.Text = PlayerImplementation.GetSongPlaying();
        }

        private void buttonSpotify_Click(object sender, EventArgs e)
        {
            try
            {
                if (SpotifySync)
                    ApagarSpotify();
                else
                    ActivateSpotifySync();
            }
            catch (APIException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                return;
            }

        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            var spotifyPlayer = (SpotifyPlayer)PlayerImplementation;
            bool res = Kernel.Spotify.InsertAlbumFromURI(spotifyPlayer.PlayingSong.Album.Id);
            if (res) //Añadido correctamente...
                MessageBox.Show(Kernel.GetText("album_agregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Kernel.GetText("album_noagregado"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void checkBoxFoobar_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBoxFoobar.Checked)
            //{
            //    nucleo.Apagar();
            //    timerUIRefresh.Enabled = false;
            //    timerFoobar.Enabled = true;
            //    foobar2000 = true;
            //    buttonReproducirPausar.Enabled = false;
            //}
            //else
            //{
            //    timerFoobar.Enabled = false;
            //    foobar2000 = false;
            //    buttonReproducirPausar.Enabled = true;
            //}
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
            var spotifyPlayer = (SpotifyPlayer)PlayerImplementation;
            string test;
            string link = "https://twitter.com/intent/tweet?text=";
            if (SpotifySync && !string.IsNullOrEmpty(spotifyPlayer.PlayingSong.Id))
            {
                test = Kernel.GetText("compartirTwitter1").Replace(" ", "%20") + "%20https://open.spotify.com/track/" + spotifyPlayer.PlayingSong.Id + "%0a" +
                    Kernel.GetText("compartirTwitter2").Replace(" ", "%20") + "%20" + Kernel.GetText("titulo_ventana_principal").Replace(" ", "%20") + "%20" + Kernel.Version + "%20" + Kernel.Codename;
            }
            else if (!ModoCD && Reproduciendo)
                test = Kernel.GetText("compartirLocal1").Replace(" ", "%20") + "%20" +
                    NowPlaying.Title.Replace(" ", "%20") + "%20" +
                    Kernel.GetText("compartirLocal2").Replace(" ", "%20") + "%20" +
                    NowPlayingAlbum.Artist.Replace(" ", "%20") + "%20" +
                    Kernel.GetText("compartirLocal3").Replace(" ", "%20") + "%20" +
                    Kernel.GetText("titulo_ventana_principal").Replace(" ", "%20") + "%20" +
                    Kernel.Version + "%20" + Kernel.Codename;
            else
                test = "Escuchando un CD con " + Kernel.GetText("titulo_ventana_principal").Replace(" ", "%20") + "%20" +
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
                    input.Add(canciones[i]);
                }
                if (input.Count == 0)
                {
                    Log.Instance.PrintMessage("No valid songs are on the input, maybe wrong file extensions?", MessageType.Info);
                    return;
                }
                Log.PrintMessage("Creating PlayerImplementation.Playlist with " + input.Count + " songs.", MessageType.Info);
                if (PlayerImplementation.Playlist is null)
                {
                    //CreatePlaylist(Kernel.GetText("seleccion"));
                    foreach (string cancion in input)
                    {
                        if (cancion is not null)
                        {
                            Song clr = new Song();
                            clr.Path = cancion;
                            PlayerImplementation.Playlist.AddSong(clr);
                        }
                    }
                    PlayPlaylist();
                }
                else
                {
                    foreach (string songfile in input)
                    {
                        Song clr = new Song();
                        clr.Path = songfile;
                        PlayerImplementation.Playlist.AddSong(clr);
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
            if (PlayerImplementation.Playlist is null) //If we don't have a PlayerImplementation.Playlist, create one. If we don't have the UI, create it.
            {
                CreatePlaylist("");
                if (PlaylistUI is null)
                    CreatePlaylistUI();
                PlaylistUI.Show();
            }
            else
            {
                PlaylistUI.RefreshView();
                PlaylistUI.SetActiveSong(PlayerImplementation.PlaylistPointer);
                PlaylistUI.Show();
            }
        }
        private void buttonDetener_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void trackBarVolumen_MouseUp(object sender, MouseEventArgs e)
        {
            //int vol = trackBarVolumen.Value;
            //if (EsPremium && SpotifySync)
            //    Kernel.Spotify.SetVolume(vol);
            //VolumeHold = false;
        }

        private void notifyIconReproduciendo_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
        }
        #endregion
    }
}
