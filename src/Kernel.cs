﻿/*
 * CASSIOPEIA 2.0.247.30
 * PROGRAM KERNEL. CORE FUNCTIONS, LOAD, SAVE, QUIT. METADATA STREAM
 * CODENAME Θάλασσα
 * MADE BY ORESTESCM76
 */

using Cassiopeia.src.Classes;
using Cassiopeia.src.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopeia
{
    public static class Kernel
    {
        public static readonly string Codename = "Betrayal";
        private enum CSV_Albums
        {
            Title,
            Artist,
            Year,
            NumSongs,
            Genre,
            Cover_PATH,
            SpotifyID,
            SongFiles_DIR,
            Type
        }
        private enum CSV_Songs
        {
            Title,
            TotalSeconds,
            IsBonus
        }
        private enum CSV_PATHS_LYRICS
        {
            Artist,
            SongTitle,
            Album
        }

        public enum StartType
        {
            Normal,
            PlayerOnly,
            MetadataStream
        }

        public static String[] IDGenres = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", "punk", "postpunk", "folk", "blues" ,"funk", "new wave", "rocksinfonico", "ska", "flamenquito", "jazz fusion", ""};

        public static Genre[] Genres = new Genre[IDGenres.Length];

        public static Task TaskRefreshToken;
        private static CancellationTokenSource RefreshTokenCancellation = new CancellationTokenSource();
        private static CancellationTokenSource CancellationToken = new CancellationTokenSource();
        public static ResXResourceSet LocalTexts;
        public static MainForm MainForm { get; private set; }
        public static Collection Collection;
#if DEBUG
        public static bool Console = true;
        public static bool CheckUpdates = false;
#else
        public static bool Console = false;
        public static bool CheckUpdates = true;
#endif
        public static bool SpotifyEnabled = true;
        public static bool StartPlayer = false;
        public static bool MetadataStream = false;
        public static bool PlayerMode = false;
        public static bool SpotifyReady = true;
        public static bool JSON = false;

        public static string[] Languages;
        public static Spotify Spotify;
        public static int NumLanguages;
        public static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static NotifyIcon notifyIconMetadataStream;

        public static uint SongCount = 0;
        private static string SongID = "";
        public static FileInfo HistorialFileInfo;
        public static FileInfo StreamFileInfo;

        private static bool Edited = false;

        public static string SearchSeparator = "/**/";

        public static string GetText(string key)
        {
            return LocalTexts.GetString(key);
        }
        public async static void MetadataStreamTask()
        {
            SongCount = 1;
            Stopwatch songTimer = new();
            DateTime now = DateTime.Now;
            HistorialFileInfo = new FileInfo("Musical log " + now.Day + "-" + now.Month + "-" + now.Year + ".txt");
            while (true)
            {
                //Get context
                SpotifyAPI.Web.CurrentlyPlayingContext PC = null;
                try
                {
                    PC = await Spotify.GetPlayingContextAsync();

                }
                catch (Exception ex)
                {
                    Log.Instance.PrintMessage("Couldn't get the Playing Context", MessageType.Error);
                    Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                }
                //Write to the file, but check if it is null, if it is, we had a problem
                if (PC is not null && PC.Item is not null)
                {
                    SpotifyAPI.Web.FullTrack track = (SpotifyAPI.Web.FullTrack)PC.Item;
                    if (track.Id != SongID)
                    {
                        if(songTimer.Elapsed.TotalSeconds > 60)
                            SongCount++;
                        songTimer.Restart();
                        SongID = track.Id;
                        //Write once every song change.
                        if (Config.HistoryEnabled)
                        {
                            using StreamWriter streamWriter = new StreamWriter(HistorialFileInfo.FullName, true);
                            streamWriter.WriteLine(Utils.GetHistoryString(track, SongCount));
                        }
                    }
                    try
                    {
                        using (StreamWriter salida = new StreamWriter("np.txt"))
                        {
                            TimeSpan pos = TimeSpan.FromMilliseconds(PC.ProgressMs);
                            salida.WriteLine(Utils.GetStreamString(track, SongCount, pos));
                            //output is the first one
                            notifyIconMetadataStream.ContextMenuStrip.Items[0].Text = Utils.GetStreamString(track, SongCount, pos);
                        }


                    }
                    catch (Exception ex)
                    {
                        Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                    }

                }
                await Task.Delay(1000);
            }
        }
        public static void ChangeLanguage(String lang)
        {
            Log.Instance.PrintMessage("Changing language to " + lang, MessageType.Info);
            LocalTexts = new ResXResourceSet(@"./idiomas/" + "original." + lang + ".resx");
            Config.Language = lang;
            ReloadGenres();
            ReloadView();
            Player.Instancia.RefrescarTextos();
        }
        public static int FindGenre(string g)
        {
            for (int i = 0; i < IDGenres.Length; i++)
            {
                if (g == IDGenres[i])
                    return i;
            }
            return -1;
        }
        public static int FindTranslatedGenre(string g)
        {
            for (int i = 0; i < Genres.Length; i++)
            {
                if (g == Genres[i].Name)
                    return i;
            }
            return -1;
        }
        public static void ReloadGenres()
        {
            for (int i = 0; i < Genres.Length - 1; i++)
            {
                Genres[i].Name = LocalTexts.GetString("genero_" + Genres[i].Id);
            }
        }
        public static void InternetAvaliable(bool i)
        {
            if (!MetadataStream)
            {
                MainForm.EnableInternet(i);
                if (Player.Instancia is not null && Spotify.AccountReady)
                    Player.Instancia.SetSpotify(i);
            }
        }
        public static void ActivarReproduccionSpotify()
        {
            if (!MetadataStream)
                MainForm.ActivarReproduccionSpotify();
        }
        public static void ReloadView()
        {
            MainForm.ReloadView();
        }
        public static void LoadConfig()
        {
            Config.CargarConfiguracion();
            try
            {
                LocalTexts = new ResXResourceSet(@"./idiomas/" + "original." + Config.Language + ".resx");
            }
            catch (Exception)
            {
                Log.Instance.PrintMessage("Unable to find languages folder, quitting...", MessageType.Error);
                Environment.Exit(-1);
            }
        }
        public static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-noSpotify":
                        SpotifyEnabled = false;
                        break;
                    case "-modoStream":
                    case "-streamMode":
                        MetadataStream = true;
                        break;
                    case "-noActualizar":
                    case "-noUpdates":
                        CheckUpdates = false;
                        break;
                    case "-reproductor":
                    case "-player":
                        StartPlayer = true;
                        break;
                    case "-json":
                        JSON = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public static void LoadLanguages()
        {
            DirectoryInfo cod = new DirectoryInfo("./idiomas");
            Languages = new String[cod.GetFiles().Length];
            int j = 0;
            try
            {
                foreach (var idioma in cod.GetFiles())
                {
                    string id = idioma.Name.Replace(".resx", "");
                    id = id.Replace("original.", "");
                    Languages[j] = id;
                    j++;
                }
                NumLanguages = Languages.Length;
            }
            catch (DirectoryNotFoundException)
            {
                Log.Instance.PrintMessage("Couldn't find languages folder, cannot load", MessageType.Error);
                Environment.Exit(-1);
            }

        }
        static async Task<bool> GetUpdate()
        {
            string contenido = string.Empty;
            using (HttpClient httpClient = new())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Cassiopeia 2.0");
                var gitHubResponse = await httpClient.GetAsync("https://api.github.com/repos/orestescm76/cassiopeia/releases");
                try
                {
                    gitHubResponse.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Log.Instance.PrintMessage("There was a problem when trying to fetch updates...", MessageType.Error);
                    Log.Instance.PrintMessage("Server response: " + ex.StatusCode, MessageType.Info);
                    return false;
                }
                var responseStream = gitHubResponse.Content.ReadAsStream();
                using (StreamReader lector = new(responseStream))
                {
                    while (!lector.EndOfStream)
                        contenido += lector.ReadLine();
                }
            }

            //HttpWebRequest GithubRequest = WebRequest.CreateHttp("https://api.github.com/repos/orestescm76/cassiopeia/releases");
            ////string contenido = string.Empty;
            //GithubRequest.Accept = "text/html,application/vnd.github.v3+json";
            //GithubRequest.UserAgent = ".NET Framework Test Agent"; //Si no lo pongo, 403.
            //try
            //{
            //    using (HttpWebResponse respuesta = (HttpWebResponse)GithubRequest.GetResponse())
            //    using (Stream flujo = respuesta.GetResponseStream())
            //    //using (StreamReader lector = new StreamReader(flujo))
            //    //{
            //    //    while (!lector.EndOfStream)
            //    //        contenido += lector.ReadLine();
            //    //}
            //}
            //catch (WebException e)
            //{
            //    Log.Instance.PrintMessage("There was a problem when trying to fetch updates...", MessageType.Error);
            //    Log.Instance.PrintMessage("Server response: " + e.Response, MessageType.Info);
            //    newVer = string.Empty;
            //    return false;
            //}

            int indexVersion = contenido.IndexOf("tag_name");
            string newVer;
            newVer = contenido.Substring(indexVersion, 40);
            newVer = newVer.Split('\"')[2];
            int newVerInt = 0, oldVerInt = 0;

            string[] oldVerArr = Version.Split('.');
            string[] newVerArr = newVer.Split('.');
            newVerInt = Convert.ToInt32(newVerArr[2]);
            oldVerInt = Convert.ToInt32(oldVerArr[2]);
            if (newVerArr[0].Length == 2)
                newVerArr[0] = newVerArr[0].Remove(0, 1);
            if (Convert.ToInt32(newVerArr[0]) > Convert.ToInt32(oldVerArr[0])) //2>1?
                return true;
            else if (newVerInt > oldVerInt) //214 > 200?
                return true;
            else if (Convert.ToInt32(newVerArr[3]) > Convert.ToInt32(oldVerArr[3]) && newVerInt == oldVerInt) //2.0.x.20 > 2.0.x.10
                return true;

            return false; //same version.
        }
        public static async void CheckForUpdates()
        {
            if (await GetUpdate())
            {
                Log.Instance.PrintMessage("A new update is avaliable", MessageType.Info);
                DialogResult act = MessageBox.Show(LocalTexts.GetString("actualizacion1") + Environment.NewLine + /*newVersion + */ Environment.NewLine + LocalTexts.GetString("actualizacion2"), "", MessageBoxButtons.YesNo);
                if (act == DialogResult.Yes)
                    Process.Start("https://github.com/orestescm76/aplicacion-gestormusica/releases");
            }
            else
                Log.Instance.PrintMessage("No updates avaliable", MessageType.Info);
        }
        public static void InitProgram()
        {
            Collection = new Collection();
            SpotifyReady = false;
            MainForm = new MainForm();
            if (!SpotifyEnabled)
                MainForm.EnableInternet(false);
            DateTime now = DateTime.Now;
            HistorialFileInfo = new FileInfo("Musical log " + now.Day + "-" + now.Month + "-" + now.Year + ".txt");
            StreamFileInfo = new FileInfo("np.txt");
        }
        public static async void InitSpotify()
        {
            if (SpotifyEnabled)
            {
                Spotify = new();
                if (!Config.LinkedWithSpotify)
                    Spotify.InitNormalMode();
                else
                {
                    await Spotify.InitStreamMode();

                    SpotifyReady = true;
                }
            }
            else
            {
                SpotifyReady = false;
                Log.Instance.PrintMessage("Cassiopeia has been launched with the -noSpotify option, there will not be any Spotify integration", MessageType.Info);
                Spotify = null;

            }
        }
        public static void InitGenres()
        {
            Log.Instance.PrintMessage("Creating genres...", MessageType.Info);
            for (int i = 0; i < IDGenres.Length; i++)
            {
                if (IDGenres[i] == "")
                {
                    Genres[i] = new Genre(IDGenres[i]);
                    Genres[i].Name = "-";
                }
                else
                {
                    Genres[i] = new Genre(IDGenres[i]);
                    Genres[i].Name = LocalTexts.GetString("genero_" + Genres[i].Id);
                }
            }
        }


        public static void InitPlayer()
        {
            Player.Init();
            Player.Instancia.RefrescarTextos();
        }

        public static void LoadFiles()
        {
            if (JSON)
                LoadAlbums("discos.json");
            else
            {
                if (File.Exists("discos.csv"))
                {
                    LoadCSVAlbums("discos.csv");
                    LoadCD();
                    LoadVinyl();
                    LoadTapes();
                }
                else
                {
                    Log.Instance.PrintMessage("discos.csv does not exist, a new database will be created...", MessageType.Warning);
                    ReloadView();
                }
            }
            if (File.Exists("paths.txt"))
                LoadPATHS();
            if (File.Exists("lyrics.txt"))
                LoadLyrics();
        }
        public static void StartApplication()
        {
            if (MetadataStream) //Start stream mode
                Start(StartType.MetadataStream);
            else if (!PlayerMode) //Normal start
                Start(StartType.Normal);
            else
                Start(StartType.PlayerOnly); //Player start
        }
        private static void Start(StartType start)
        {
            switch (start)
            {
                case StartType.Normal:
                    Log.Instance.PrintMessage("Running main form", MessageType.Info);
                    CleanSaveMark();
                    Application.Run(MainForm);
                    break;
                case StartType.PlayerOnly:
                    Application.Run(Player.Instancia);
                    break;
                case StartType.MetadataStream:
                    InitMetadataStream();
                    Application.Run();
                    break;
                default:
                    break;
            }
        }

        public static void Quit()
        {
            if (!MetadataStream)
            {
                if (Edited)
                {
                    DialogResult save = MessageBox.Show(LocalTexts.GetString("wantSave"), LocalTexts.GetString("titulo_ventana_principal"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (save == DialogResult.Yes)
                    {
                        SaveAlbums("discos.csv", SaveType.Digital);
                        SaveAlbums("cd.json", SaveType.CD);
                        SaveAlbums("vinyl.json", SaveType.Vinyl);
                        SaveAlbums("tapes.json", SaveType.Cassette_Tape);
                        SavePATHS();
                        SaveLyrics();
                    }
                }

                Config.MainFormSize = MainForm.Size;
                Log.Instance.PrintMessage("Saving config...", MessageType.Info);
                Config.GuardarConfiguracion();

                Log.Instance.PrintMessage("Shutting down Player...", MessageType.Info);
                Player.Instancia.Apagar();
                Player.Instancia.Dispose();

                if (File.Exists("./covers/np.jpg"))
                    File.Delete("./covers/np.jpg");

            }
            Log.Instance.CloseLog();

            if (Console)
                FreeConsole();
        }

        //Methods for loading and saving...
        public static void LoadAlbums(string fichero)
        {
            Log.Instance.PrintMessage("Loading albums stored at " + fichero, MessageType.Info, "cargarAlbumes(string)");
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamReader lector = new StreamReader(fichero))
            {
                string LineaJson = "";
                while (!lector.EndOfStream)
                {
                    LineaJson = lector.ReadLine();
                    AlbumData a = JsonConvert.DeserializeObject<AlbumData>(LineaJson);
                    a.Genre = Genres[FindGenre(a.Genre.Id)];
                    Collection.AddAlbum(ref a);
                    a.CanBeRemoved = true;
                }
            }
            crono.Stop();
            Log.Instance.PrintMessage("Loaded " + Collection.Albums.Count + " albums without problems", MessageType.Correct, crono, TimeType.Milliseconds);
            ReloadView();
        }
        private static void SendErrorLoading(int line, string file)
        {
            Log.Instance.PrintMessage("The album data file has mistakes. Check line " + line + " of " + file, MessageType.Error);
            MessageBox.Show(LocalTexts.GetString("errorLoadingAlbums1") + line + " " + LocalTexts.GetString("errorLoadingAlbums2") + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void LoadCSVAlbums(string file)
        {
            Log.Instance.PrintMessage("Loading CSV albums stored at " + file, MessageType.Info, "cargarAlbumesLegacy(string)");
            Stopwatch crono = Stopwatch.StartNew();
            //cargando CSV a lo bestia
            int lineaC = 1;
            using (StreamReader lector = new StreamReader(file))
            {
                string linea;
                while (!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    while (linea == "")
                    {
                        linea = lector.ReadLine();
                        lineaC++;
                    }

                    if (linea == null) continue; //si no hay nada tu sigue, que hemos llegado al final del fichero, después del nulo porque siempre al terminar un disco pongo línea nueva.
                    string[] datos = linea.Split(';');
                    if (datos.Length == 8) //need to convert
                    {
                        Log.Instance.PrintMessage("Adding Studio type to album", MessageType.Info);
                    }
                    else if (datos.Length != 9)
                    {
                        SendErrorLoading(lineaC, file);
                        Environment.Exit(-1);
                    }
                    short nC = 0;
                    int gen = FindGenre(datos[(int)CSV_Albums.Genre]);
                    Genre g = Genres[gen];
                    if (string.IsNullOrEmpty(datos[(int)CSV_Albums.Cover_PATH])) datos[(int)CSV_Albums.Cover_PATH] = string.Empty;
                    AlbumData a = null;
                    try
                    {
                        nC = Convert.ToInt16(datos[(int)CSV_Albums.NumSongs]);
                        string CoverPath = String.Empty;
                        if (!string.IsNullOrEmpty(datos[(int)CSV_Albums.Cover_PATH]))
                            CoverPath = Environment.CurrentDirectory + "\\" + datos[(int)CSV_Albums.Cover_PATH];
                        a = new AlbumData(g, datos[(int)CSV_Albums.Title], datos[(int)CSV_Albums.Artist], Convert.ToInt16(datos[(int)CSV_Albums.Year]), CoverPath);
                        a.Type = (AlbumType)Convert.ToInt32(datos[(int)CSV_Albums.Type]);
                    }
                    catch (FormatException)
                    {
                        SendErrorLoading(lineaC, file);
                        Environment.Exit(-1);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        a.Type = AlbumType.Studio;
                    }
                    if (!string.IsNullOrEmpty(datos[(int)CSV_Albums.SpotifyID]))
                        a.IdSpotify = datos[(int)CSV_Albums.SpotifyID];
                    if (!string.IsNullOrEmpty(datos[(int)CSV_Albums.SongFiles_DIR]))
                        a.SoundFilesPath = datos[(int)CSV_Albums.SongFiles_DIR];
                    bool exito = true;
                    for (int i = 0; i < nC; i++)
                    {
                        exito = false;
                        linea = lector.ReadLine();
                        lineaC++;
                        if (string.IsNullOrEmpty(linea))
                        {
                            /*System.Windows.Forms.MessageBox.Show("mensajeError"+Environment.NewLine
                                + a.nombre + " - " + a.nombre + Environment.NewLine
                                + "saltarAlSiguiente", "error", System.Windows.Forms.MessageBoxButtons.OK);*/
                            break; //no sigue cargando el álbum
                        }
                        else
                        {
                            try
                            {
                                exito = true;
                                string[] datosCancion = linea.Split(';');
                                if (datosCancion.Length == 3)
                                {
                                    byte bonus = Convert.ToByte(datosCancion[(int)CSV_Songs.IsBonus]);
                                    Song c = new Song(datosCancion[(int)CSV_Songs.Title], TimeSpan.FromSeconds(Convert.ToInt32(datosCancion[(int)CSV_Songs.TotalSeconds])), ref a, Convert.ToBoolean(bonus));
                                    a.AddSong(c, i);
                                }
                                else
                                {
                                    LongSong cl = new LongSong(datosCancion[(int)CSV_Songs.Title], a);
                                    int np = Convert.ToInt32(datosCancion[(int)CSV_Songs.TotalSeconds]);
                                    for (int j = 0; j < np; j++)
                                    {
                                        linea = lector.ReadLine();
                                        lineaC++;
                                        datosCancion = linea.Split(';');
                                        Song c = new Song(datosCancion[0], TimeSpan.FromSeconds(Convert.ToInt32(datosCancion[(int)CSV_Songs.TotalSeconds])), ref a);
                                        cl.AddPart(c);
                                    }
                                    a.AddSong(cl, i);
                                }
                            }
                            catch (FormatException)
                            {
                                SendErrorLoading(lineaC, file);
                                Environment.Exit(-1);
                            }
                        }
                    }

                    if (exito)
                        Collection.AddAlbum(ref a);
                    else
                        Log.Instance.PrintMessage("Couldn't add the album " + a, MessageType.Error);

                    a.CanBeRemoved = true;
                    lineaC++;
                }
            }
            crono.Stop();
            Log.Instance.PrintMessage("Loaded " + Collection.Albums.Count + " albums", MessageType.Correct, crono, TimeType.Milliseconds);
            ReloadView();
        }
        public static void LoadCD(string fichero = "cd.json")
        {
            if (!File.Exists(fichero))
                return;
            Log.Instance.PrintMessage("Loading CDS...", MessageType.Info);
            using (StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while (!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    CompactDisc cd = JsonConvert.DeserializeObject<CompactDisc>(linea);

                    cd.InstallAlbum();
                    Collection.AddCD(ref cd);
                    cd.Album.CanBeRemoved = false;
                }
            }
        }
        public static void LoadVinyl(string fichero = "vinyl.json")
        {
            if (!File.Exists(fichero))
                return;
            Log.Instance.PrintMessage("Loading vinyls...", MessageType.Info);
            using (StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while (!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    VinylAlbum vinyl = JsonConvert.DeserializeObject<VinylAlbum>(linea);

                    vinyl.InstallAlbum();
                    Collection.AddVinyl(ref vinyl);
                    vinyl.Album.CanBeRemoved = false;
                }
            }
        }
        public static void LoadTapes(string fichero = "tapes.json")
        {
            if (!File.Exists(fichero))
                return;
            Log.Instance.PrintMessage("Loading tapes...", MessageType.Info);
            using (StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while (!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    CassetteTape tape = JsonConvert.DeserializeObject<CassetteTape>(linea);

                    tape.InstallAlbum();
                    Collection.AddTape(ref tape);
                    tape.Album.CanBeRemoved = false;
                }
            }
        }
        public static void LoadPATHS()
        {
            Log.Instance.PrintMessage("Loading PATHS", MessageType.Info);
            using (StreamReader entrada = new FileInfo("paths.txt").OpenText())
            {
                string linea = null;
                while (!entrada.EndOfStream)
                {
                    linea = entrada.ReadLine();
                    string[] datos = linea.Split(';');
                    List<AlbumData> listaAlbumes = Collection.SearchAlbum(datos[(int)CSV_PATHS_LYRICS.Album]);
                    if (listaAlbumes.Count != 0)
                    {
                        foreach (AlbumData album in listaAlbumes)
                        {
                            if (album.Artist == datos[(int)CSV_PATHS_LYRICS.Artist] && album.Title == datos[(int)CSV_PATHS_LYRICS.Album])
                            {
                                Song c = album.GetSong(datos[(int)CSV_PATHS_LYRICS.SongTitle]);
                                linea = entrada.ReadLine();
                                c.Path = linea;
                            }
                        }
                    }
                    else
                    {
                        linea = entrada.ReadLine();
                        continue;
                    }
                }
            }
        }
        public static void SavePATHS()
        {
            Log.Instance.PrintMessage("Saving PATHS", MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            FileInfo pathsInfo = new FileInfo("paths.txt");
            using (StreamWriter salida = pathsInfo.CreateText())
            {
                foreach (var pair in Collection.Albums)
                {
                    if (string.IsNullOrEmpty(pair.Value.SoundFilesPath))
                        continue;
                    foreach (Song cancion in pair.Value.Songs)
                    {
                        if (!string.IsNullOrEmpty(cancion.Path))
                        {
                            salida.Write(cancion.SavePath());
                        }
                    }
                }
                //foreach (AlbumData album in Collection.Albums)
                //{
                //    if (string.IsNullOrEmpty(album.SoundFilesPath))
                //        continue;
                //    foreach (Song cancion in album.Songs)
                //    {
                //        if (!string.IsNullOrEmpty(cancion.Path))
                //        {
                //            salida.Write(cancion.SavePath());
                //        }
                //    }
                //}
                salida.Flush();
                pathsInfo.Refresh();
                crono.Stop();
            }
            Edited = false;
            Log.Instance.PrintMessage("Saved songs PATH", MessageType.Correct, crono, TimeType.Milliseconds);
            Log.Instance.PrintMessage("Filesize: " + pathsInfo.Length / 1024.0 + " kb", MessageType.Info);
        }
        public static void SaveAlbums(string path, SaveType tipoGuardado)
        {
            Stopwatch crono = Stopwatch.StartNew();
            FileInfo fich = new FileInfo(path);
            using StreamWriter salida = fich.CreateText();
            switch (tipoGuardado)
            {
                case SaveType.Digital:
                    if (Collection.Albums.Count == 0)
                        break;
                    Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the album data... (" + Collection.Albums.Count + " albums)", MessageType.Info);
                    Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                    //foreach (AlbumData a in Collection.Albums)
                    foreach (var a in Collection.Albums.Values)
                    {
                        if (a.Songs[0] is not null) //no puede ser un album con 0 canciones
                        {
                            string CoverRelativePath = String.Empty;
                            if (!string.IsNullOrEmpty(a.CoverPath))
                                CoverRelativePath = Path.GetRelativePath(Environment.CurrentDirectory, a.CoverPath);
                            salida.WriteLine(a.Title + ";" + a.Artist + ";" + a.Year + ";" + a.NumberOfSongs + ";" + a.Genre.Id + ";" + CoverRelativePath + ";" + a.IdSpotify + ";" + a.SoundFilesPath + ";" + (int)a.Type);
                            for (int i = 0; i < a.NumberOfSongs; i++)
                            {
                                if (a.Songs[i] is LongSong longSong)
                                {
                                    salida.WriteLine(longSong.Title + ";" + longSong.Parts.Count);//no tiene duracion y son 2 datos a guardar
                                    foreach (Song parte in longSong.Parts)
                                    {
                                        salida.WriteLine(parte.Title + ";" + (int)(parte.Length.TotalSeconds));
                                    }

                                }
                                else //titulo;400;0
                                    salida.WriteLine(a.Songs[i].Title + ";" + (int)a.Songs[i].Length.TotalSeconds + ";" + Convert.ToInt32(a.Songs[i].IsBonus));
                            }
                        }
                        salida.WriteLine();
                    }
                    break;
                case SaveType.CD:
                    if (Collection.CDS.Count == 0)
                        break;
                    Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the CD data... (" + Collection.CDS.Count + " cds)", MessageType.Info);
                    Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                    foreach (CompactDisc compacto in Collection.CDS)
                    {
                        salida.WriteLine(JsonConvert.SerializeObject(compacto));
                    }
                    break;
                case SaveType.Vinyl:
                    if (Collection.Vinyls.Count == 0)
                        break;
                    Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the Vinyl data... (" + Collection.Vinyls.Count + " vinyls)", MessageType.Info);
                    Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                    foreach (VinylAlbum vinyl in Collection.Vinyls)
                    {
                        salida.WriteLine(JsonConvert.SerializeObject(vinyl));
                    }
                    break;
                case SaveType.Cassette_Tape:
                    if (Collection.Tapes.Count == 0)
                        break;
                    Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the Tapes data... (" + Collection.Tapes.Count + " vinyls)", MessageType.Info);
                    Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                    foreach (CassetteTape tape in Collection.Tapes)
                    {
                        salida.WriteLine(JsonConvert.SerializeObject(tape));
                    }
                    break;
                default:
                    break;
            }
            salida.Flush();
            fich.Refresh();
            crono.Stop();
            Edited = false;
            Log.Instance.PrintMessage(nameof(SaveAlbums) + "- Saved", MessageType.Correct, crono, TimeType.Milliseconds);
            Log.Instance.PrintMessage("Filesize: " + fich.Length / 1024.0 + " kb", MessageType.Info);
        }
        public static void LoadLyrics()
        {
            Log.Instance.PrintMessage("Loading lyrics", MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamReader entrada = new FileInfo("lyrics.txt").OpenText())
            {
                string linea = null;
                while (!entrada.EndOfStream)
                {
                    linea = entrada.ReadLine();
                    string[] datos = linea.Split(';');
                    AlbumData albumData = Collection.SearchAlbum(datos[(int)CSV_PATHS_LYRICS.Album])[0];
                    Song song = albumData.GetSong(datos[(int)CSV_PATHS_LYRICS.SongTitle]);
                    List<string> lyrics = new List<string>();
                    do
                    {
                        linea = entrada.ReadLine();
                        lyrics.Add(linea);
                    } while (linea != "---");
                    lyrics.Remove("---");
                    song.Lyrics = lyrics.ToArray();
                }
            }
            crono.Stop();
            Log.Instance.PrintMessage("Lyrics loaded", MessageType.Correct, crono, TimeType.Milliseconds);
        }
        public static void SaveLyrics()
        {
            Log.Instance.PrintMessage("Saving lyrics", MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamWriter salida = new FileInfo("lyrics.txt").CreateText()) //change?
            {
                //foreach (AlbumData album in Collection.Albums)
                foreach (AlbumData album in Collection.Albums.Values)
                {
                    foreach (Song cancion in album.Songs)
                    {
                        if (cancion.Lyrics is not null && cancion.Lyrics.Length != 0)
                        {
                            salida.WriteLine(album.Artist + ";" + cancion.Title + ";" + album.Title);
                            foreach (string line in cancion.Lyrics)
                            {
                                salida.WriteLine(line);
                            }
                            salida.WriteLine("---");
                        }
                    }
                }
            }
            crono.Stop();
            Edited = false;
            Log.Instance.PrintMessage("Saved lyrics", MessageType.Correct, crono, TimeType.Milliseconds);
            FileInfo lyrics = new FileInfo("lyrics.txt");
            Log.Instance.PrintMessage("Filesize: " + lyrics.Length / 1024.0 + " kb", MessageType.Info);
        }
        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, LocalTexts.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static int AllocConsole()
        {
            return WinAPI.AllocConsole();
        }
        private static int FreeConsole()
        {
            return WinAPI.FreeConsole();
        }
        public static void BringMainFormFront()
        {
            if (!MetadataStream)
                MainForm.Activate();
        }
        public static DialogResult Warn(string reason)
        {
            return MessageBox.Show(reason, "Warn", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }
        public static Song searchSong(string keyword)
        {
            //foreach (AlbumData album in Collection.Albums)
            foreach (AlbumData album in Collection.Albums.Values)
            {
                foreach (Song song in album.Songs)
                {
                    if (song.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        return song;
                }
            }
            return null;
        }
        private static async void InitMetadataStream()
        {
            Log.Instance.PrintMessage("Starting stream mode...", MessageType.Info);
            notifyIconMetadataStream = new NotifyIcon();
            notifyIconMetadataStream.Text = LocalTexts.GetString("cerrarModoStream");
            notifyIconMetadataStream.Icon = Properties.Resources.spotifyico;
            notifyIconMetadataStream.DoubleClick += NotifyIconMetadataStream_DoubleClick;
            notifyIconMetadataStream.Visible = true;
            //Create the menu strip
            ContextMenuStrip notifyIconMetadataStreamContextMenu = new();
            ToolStripMenuItem output = new ToolStripMenuItem();
            output.Enabled = false;
            output.Text = "";
            notifyIconMetadataStreamContextMenu.Items.Add(output);
            //---
            ToolStripMenuItem openConfig = new ToolStripMenuItem();
            openConfig.Text = GetText("configuracion");
            openConfig.Click += OpenConfig_Click;
            notifyIconMetadataStreamContextMenu.Items.Add(openConfig);
            //---
            ToolStripMenuItem resetNum = new ToolStripMenuItem();
            resetNum.Text = GetText("resetNum");
            resetNum.Click += ResetNum_Click;
            notifyIconMetadataStreamContextMenu.Items.Add(resetNum);
            //---
            ToolStripMenuItem quit = new ToolStripMenuItem();
            quit.Text = GetText("salir");
            quit.Click += NotifyIconMetadataStream_DoubleClick;
            notifyIconMetadataStreamContextMenu.Items.Add(quit);
            notifyIconMetadataStream.ContextMenuStrip = notifyIconMetadataStreamContextMenu;
            await Task.Run(() => InitSpotify());
            while (!Spotify.AccountReady)
            {
                Thread.Sleep(1);
            }
            Log.Instance.PrintMessage("Starting task...", MessageType.Info);
            MetadataStreamTask();
        }

        private static void ResetNum_Click(object sender, EventArgs e)
        {
            SongCount = 1;
        }

        private static void OpenConfig_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new(true)
            {
                ActiveConfig = ActiveConfig.Stream
            };
            configForm.ShowDialog();
        }

        private static void NotifyIconMetadataStream_DoubleClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public static void SetSaveMark()
        {
            Edited = true;
            MainForm.SetSaveMark();
        }
        private static void CleanSaveMark()
        {
            Edited = false;
            MainForm.CleanSaveMark();
        }

        public static void CreateAndAddAlbumFromFolder(string path)
        {
            Log.Instance.PrintMessage("Creating an album from a directory.", MessageType.Info);

            AlbumData a = new AlbumData();

            //To avoid a random song order, i create an array to store the songs. 150 SHOULD be big enough.
            Song[] tempStorage = new Song[150];
            int numSongs = 0; //to keep track of how many songs i've addded.
            Stopwatch crono = Stopwatch.StartNew();
            DirectoryInfo carpeta = new DirectoryInfo(path);
            Config.LastOpenedDirectory = carpeta.FullName;
            foreach (var filename in carpeta.GetFiles())
            {
                switch (Path.GetExtension(filename.FullName))
                {
                    case ".mp3":
                    case ".ogg":
                    case ".flac":
                        LocalSong LM = new LocalSong(filename.FullName);
                        if (a.NeedsMetadata())
                        {
                            a.Title = LM.Album;
                            a.Artist = LM.Artist;
                            a.Year = (short)LM.Year;
                            if (LM.AlbumCover is not null && !File.Exists("cover.jpg"))
                            {
                                Bitmap cover = new Bitmap(LM.AlbumCover);
                                cover.Save(carpeta.FullName + "\\cover.jpg", ImageFormat.Jpeg);
                                a.CoverPath = carpeta.FullName + "\\cover.jpg";
                            }
                        }
                        Song c = new Song(LM.Title, (int)LM.Length.TotalMilliseconds, false);
                        if (LM.TrackNumber != 0) //A music file with no track number? Can happen. Instead, do the normal process.
                        {
                            tempStorage[LM.TrackNumber - 1] = c;
                            numSongs++;
                        }
                        else
                            a.AddSong(c);
                        c.SetAlbum(a);
                        c.Path = filename.FullName;
                        LM.Dispose();
                        break;
                    case ".jpg":
                        if (filename.Name == "folder.jpg" || filename.Name == "cover.jpg")
                            a.CoverPath = filename.FullName;
                        break;
                }
                if (numSongs != 0) //The counter has been updated and songs had a track number.
                {
                    //This list goes to the album.
                    List<Song> songList = new List<Song>();
                    for (int i = 0; i < numSongs; i++)
                    {
                        //Copy the correct song order.
                        songList.Add(tempStorage[i]);
                    }
                    a.Songs = songList;
                }
            }
            a.SoundFilesPath = carpeta.FullName;
            a.Type = AlbumType.Studio;
            Collection.AddAlbum(ref a);
            SetSaveMark();
            crono.Stop();
            Log.Instance.PrintMessage("Operation completed", MessageType.Correct, crono, TimeType.Milliseconds);
            ReloadView();
        }
        public static string GetSystemLanguage()
        {
            CultureInfo ci = CultureInfo.CurrentUICulture;
            string lan = ci.Name.Split('-')[0];
            if (!Languages.Contains(lan))
                lan = "en";
            return lan;
        }
        public static void SetPathsForAlbum(AlbumData a)
        {
            Log.Instance.PrintMessage("Searching songs for " + a.ToString(), MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            bool correcto = true;
            DirectoryInfo directorioCanciones = new DirectoryInfo(a.SoundFilesPath);
            foreach (FileInfo file in directorioCanciones.GetFiles())
            {
                string extension = Path.GetExtension(file.FullName);
                if (extension != ".ogg" && extension != ".mp3" && extension != ".flac")
                    continue;
                LocalSong LM = new LocalSong(file.FullName);
                foreach (Song c in a.Songs)
                {
                    try
                    {
                        if (LM.Evaluable() && !string.IsNullOrEmpty(c.Path))
                        {
                            if ((c.Title.ToLower() == LM.Title.ToLower()) && (c.AlbumFrom.Artist.ToLower() == LM.Artist.ToLower()))
                            {
                                c.Path = file.FullName;
                                Log.Instance.PrintMessage(c.Title + " linked successfully!", MessageType.Correct);
                                break;
                            }
                        }
                        else
                        {
                            if (file.FullName.ToLower().Contains(c.Title.ToLower()))
                            {
                                c.Path = file.FullName;
                                Log.Instance.PrintMessage(c.Title + " linked successfully", MessageType.Correct);
                                break;
                            }
                            correcto = false;
                        }
                    }
                    catch (Exception)
                    {
                        correcto = false;
                    }

                }
            }
            crono.Stop();

            if (correcto)
            {
                Log.Instance.PrintMessage("Finished without problems", MessageType.Correct, crono, TimeType.Milliseconds);
                MessageBox.Show(LocalTexts.GetString("pathsCorrectos"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                foreach (Song cancion in a.Songs)
                {
                    if (string.IsNullOrEmpty(cancion.Path)) //No se ha encontrado
                    {
                        Log.Instance.PrintMessage(cancion.Title + " couldn't be linked...", MessageType.Warning);
                    }
                }
                MessageBox.Show(LocalTexts.GetString("pathsError"), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SavePATHS();
        }
        public static void ResetSpotifyLink()
        {
            MainForm.ResetSpotifyLink();
        }
    }
}
