using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Resources;
using System.Windows.Forms;
using Cassiopeia.src.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Cassiopeia
{
    static class Kernel
    {
        public static readonly string CodeName = "Storm";
        private enum CSV_Albums
        {
            Title,
            Artist,
            Year,
            NumSongs,
            Genre,
            Cover_PATH,
            SpotifyID,
            SongFiles_DIR
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

        public static ResXResourceSet LocalTexts;
        public static MainForm MainForm;
        public static Collection Collection;
#if DEBUG
        public static bool CheckUpdates = false;
#else
        public static bool CheckUpdates = true;
#endif
        public static bool Console = false;
        public static bool SpotifyEnabled = true;
        public static bool StartPlayer = false;
        public static bool MetadataStream = false;
        public static bool PlayerMode = false;
        public static bool SpotifyReady = true;
        public static bool JSON = false;

        public static string[] Languages;
        public static Spotify Spotify;
        public static int NumLanguages;
        public static readonly string Version = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static void RefreshSpotifyToken(CancellationToken cancellationToken)
        {
            while (true)
            {
                var cancelar = cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(15));
                if (cancelar && cancellationToken.IsCancellationRequested)
                    return;
                if (Spotify.IsTokenExpired())
                {
                    Spotify.RefreshToken();
                }
            }
        }
        public static void ChangeLanguage(String lang)
        {
            Log.Instance.PrintMessage("Changing language to " + lang, MessageType.Info);
            LocalTexts = new ResXResourceSet(@"./idiomas/" + "original." + lang + ".resx");
            Config.Language = lang;
            ReloadGenres();
            ReloadView();
            Reproductor.Instancia.RefrescarTextos();
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
            MainForm.EnableInternet(i);
        }

        public static void ActivarReproduccionSpotify()
        {
            MainForm.ActivarReproduccionSpotify();
        }

        public static void ReloadView()
        {
            MainForm.Refrescar();
        }
        public static void LoadConfig()
        {
            Config.CargarConfiguracion();
            LocalTexts = new ResXResourceSet(@"./idiomas/" + "original." + Config.Language + ".resx");
        }
        public static void ParseArgs(String[] args)
        {
            foreach (string Arg in args)
            {
                switch (Arg)
                {
                    case "-consola":
                    case "-console":
                        Console = true;
                        break;
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
            foreach (var idioma in cod.GetFiles())
            {
                string id = idioma.Name.Replace(".resx", "");
                id = id.Replace("original.", "");
                Languages[j] = id;
                j++;
            }
            NumLanguages = Languages.Length;
        }
        static bool GetUpdate(out string newVer)
        {
            HttpWebRequest GithubRequest = WebRequest.CreateHttp("https://api.github.com/repos/orestescm76/cassiopeia/releases");
            string contenido = string.Empty;
            GithubRequest.Accept = "text/html,application/vnd.github.v3+json";
            GithubRequest.UserAgent = ".NET Framework Test Agent"; //Si no lo pongo, 403.
            try
            {
                using (HttpWebResponse respuesta = (HttpWebResponse)GithubRequest.GetResponse())
                using (Stream flujo = respuesta.GetResponseStream())
                using (StreamReader lector = new StreamReader(flujo))
                {
                    while (!lector.EndOfStream)
                        contenido += lector.ReadLine();
                }
            }
            catch (WebException e)
            {
                Log.Instance.PrintMessage("There was a problem when trying to fetch updates...", MessageType.Error);
                Log.Instance.PrintMessage("Server response: " + e.Response, MessageType.Info);
                newVer = string.Empty;
                return false;
            }

            int indexVersion = contenido.IndexOf("tag_name");
            newVer = contenido.Substring(indexVersion, 40);
            newVer = newVer.Split('\"')[2];
            int newVerInt = 0, oldVerInt = 0;

            string[] oldVerArr = Version.Split('.');
            string[] newVerArr = newVer.Split('.');
            oldVerArr[0] = oldVerArr[0].Remove(0, 1);
            newVerArr[0] = newVerArr[0].Remove(0, 1);

            for (int i = 0; i < oldVerArr.Length; i++)
            {
                if (Convert.ToInt32(newVerArr[i]) > Convert.ToInt32(oldVerArr[i]))
                   return true; //if one of the versions is higher, clearly there is an update.
            }
            return false; //same version.
        }
        public static void CheckForUpdates()
        {
            string newVersion;
            if (GetUpdate(out newVersion))
            {
                Log.Instance.PrintMessage("A new update is avaliable: " + newVersion, MessageType.Info);
                DialogResult act = MessageBox.Show(LocalTexts.GetString("actualizacion1") + Environment.NewLine + newVersion + Environment.NewLine + LocalTexts.GetString("actualizacion2"), "", MessageBoxButtons.YesNo);
                if (act == DialogResult.Yes)
                    Process.Start("https://github.com/orestescm76/aplicacion-gestormusica/releases");
            }
        }
        public static void CreateProgram()
        {
            Collection = new Collection();
            SpotifyReady = false;
            MainForm = new MainForm();
        }
        public static void InitSpotify()
        {
            if (SpotifyEnabled)
            {
                if (!Config.LinkedWithSpotify)
                    Spotify = new Spotify(false);
                else
                {
                    Spotify = new Spotify(true);
                    SpotifyReady = true;
                    MainForm.DesactivarVinculacion();
                }

            }
            else
            {
                SpotifyReady = false;
                Log.Instance.PrintMessage("Cassiopeia has been launched with the -noSpotify option, there will not be any Spotify integration", MessageType.Info);
                Spotify = null;
                MainForm.EnableInternet(false);
            }
        }
        public static void InitTask()
        {
            TaskRefreshToken = Task.Factory.StartNew(
                () => { RefreshSpotifyToken(RefreshTokenCancellation.Token); },
                RefreshTokenCancellation.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
                );
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
            Reproductor.Init();
            Reproductor.Instancia.RefrescarTextos();
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
                }
                else
                {
                    Log.Instance.PrintMessage("discos.csv does not exist, a new database will be created...", MessageType.Warning);
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
                    Application.Run(MainForm);
                    break;
                case StartType.PlayerOnly:
                    Application.Run(Reproductor.Instancia);
                    break;
                case StartType.MetadataStream:
                    Application.Run();
                    break;
                default:
                    break;
            }
        }

        public static void Quit()
        {
            if(TaskRefreshToken is not null)
            {
                RefreshTokenCancellation.Cancel();
                try
                {
                    TaskRefreshToken.Wait();
                }
                catch (Exception)
                {
                    Log.Instance.PrintMessage("TaskRefreshToken is terminated", MessageType.Correct);
                }
            }
            SaveAlbums("discos.csv", SaveType.Digital);
            SaveAlbums("cd.json", SaveType.CD);
            SavePATHS();
            SaveLyrics();

            Config.GuardarConfiguracion();

            Log.Instance.PrintMessage("Shutting down Player", MessageType.Info);
            Reproductor.Instancia.Apagar();
            Reproductor.Instancia.Dispose();

            if (File.Exists("./covers/np.jpg"))
                File.Delete("./covers/np.jpg");
            if (Console)
                FreeConsole();
            Log.Instance.CloseLog();
            Environment.Exit(0);
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
                    if (datos.Length != 8)
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
                        a = new AlbumData(g, datos[(int)CSV_Albums.Title], datos[(int)CSV_Albums.Artist], Convert.ToInt16(datos[(int)CSV_Albums.Year]), datos[(int)CSV_Albums.Cover_PATH]);
                    }
                    catch (FormatException)
                    {
                        SendErrorLoading(lineaC, file);
                        Environment.Exit(-1);
                    }
                    if (!string.IsNullOrEmpty(datos[(int)CSV_Albums.SpotifyID]))
                        a.IdSpotify = datos[(int)CSV_Albums.SpotifyID];
                    if (!string.IsNullOrEmpty(datos[(int)CSV_Albums.SongFiles_DIR]))
                        a.SoundFilesPath = datos[(int)CSV_Albums.SongFiles_DIR];
                    bool exito = false;
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
                    if (Collection.IsInCollection(a))
                    {
                        exito = false; //pues ya está repetido.
                        Log.Instance.PrintMessage("Repeated album -> " + a.Artist + " - " + a.Title, MessageType.Warning);
                    }

                    if (exito)
                        Collection.AddAlbum(ref a);


                    a.CanBeRemoved = true;
                    lineaC++;
                }
            }
            crono.Stop();
            Log.Instance.PrintMessage("Cargados " + Collection.Albums.Count + " álbumes correctamente", MessageType.Correct, crono, TimeType.Milliseconds);
            ReloadView();
        }
        public static void LoadCD(string fichero = "cd.json")
        {
            if (!File.Exists(fichero))
                return;
            Log.Instance.PrintMessage("Loading CDS...",MessageType.Info);
            using (StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while (!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    CompactDisc cd = JsonConvert.DeserializeObject<CompactDisc>(linea);

                    cd.InstallAlbum();
                    Collection.AddCD(ref cd);
                    cd.AlbumData.CanBeRemoved = false;
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
                foreach (AlbumData album in Collection.Albums)
                {
                    if (string.IsNullOrEmpty(album.SoundFilesPath))
                        continue;
                    foreach (Song cancion in album.Songs)
                    {
                        if (!string.IsNullOrEmpty(cancion.Path))
                        {
                            salida.Write(cancion.SavePath());
                        }
                    }
                }
                salida.Flush();
                pathsInfo.Refresh();
                crono.Stop();
            }
            Log.Instance.PrintMessage("Saved songs PATH", MessageType.Correct, crono, TimeType.Milliseconds);
            Log.Instance.PrintMessage("Filesize: " + pathsInfo.Length / 1024.0 + " kb", MessageType.Info);
        }
        public static void SaveAlbums(string path, SaveType tipoGuardado, bool json = false)
        {

            Stopwatch crono = Stopwatch.StartNew();
            FileInfo fich = new FileInfo(path);
            if (json)
            {
                using (StreamWriter salida = fich.CreateText())
                {
                    switch (tipoGuardado)
                    {
                        case SaveType.Digital:
                            Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the album data... (" + Collection.Albums.Count + " albums)", MessageType.Info);
                            Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                            foreach (AlbumData a in Collection.Albums)
                            {
                                JsonSerializer s = new JsonSerializer();
                                s.TypeNameHandling = TypeNameHandling.All;
                                salida.WriteLine(JsonConvert.SerializeObject(a));
                            }
                            break;
                        case SaveType.CD:
                            Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the album data... (" + Collection.CDS.Count + " cds)", MessageType.Info);
                            Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                            foreach (CompactDisc compacto in Collection.CDS)
                            {
                                salida.WriteLine(JsonConvert.SerializeObject(compacto));
                            }
                            break;
                        default:
                            break;
                    }
                    salida.Flush();
                }
            }
            else
            {
                using (StreamWriter salida = fich.CreateText())
                {
                    switch (tipoGuardado)
                    {
                        case SaveType.Digital:
                            Log.Instance.PrintMessage(nameof(SaveAlbums) + " - Saving the album data... (" + Collection.Albums.Count + " albums)", MessageType.Info);
                            Log.Instance.PrintMessage("Filename: " + path, MessageType.Info);
                            foreach (AlbumData a in Collection.Albums)
                            {
                                if (a.Songs[0] is not null) //no puede ser un album con 0 canciones
                                {
                                    salida.WriteLine(a.Title + ";" + a.Artist + ";" + a.Year + ";" + a.NumberOfSongs + ";" + a.Genre.Id + ";" + a.CoverPath + ";" + a.IdSpotify + ";" + a.SoundFilesPath);
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
                            break;
                        case SaveType.Vinilo:
                            break;
                        default:
                            break;
                    }
                    salida.Flush();
                }
            }
            crono.Stop();
            fich.Refresh();
            crono.Stop();
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
                foreach (AlbumData album in Collection.Albums)
                {
                    foreach (Song cancion in album.Songs)
                    {
                        if (cancion.Lyrics != null && cancion.Lyrics.Length != 0)
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
            Log.Instance.PrintMessage("Saved lyrics", MessageType.Correct, crono, TimeType.Milliseconds);
            FileInfo lyrics = new FileInfo("lyrics.txt");
            Log.Instance.PrintMessage("Filesize: " + lyrics.Length / 1024.0 + " kb", MessageType.Info);
        }
        public static void ShowError(string msg)
        {
            MessageBox.Show(LocalTexts.GetString("error"), msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static int AllocConsole()
        {
            return WinAPI.AllocConsole();
        }
        private static int FreeConsole()
        {
            System.Console.WriteLine("Cassiopeia has finished, please press enter to exit...");
            System.Console.ReadLine();
            return WinAPI.FreeConsole();
        }
    }
}
