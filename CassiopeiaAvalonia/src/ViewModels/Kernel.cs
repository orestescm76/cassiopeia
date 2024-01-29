using Cassiopeia.src.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cassiopeia.src.VM
{
    internal static class Kernel
    {
        public static readonly string Codename = "Betrayal";
        internal enum CSV_Albums
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
        internal enum CSV_Songs
        {
            Title,
            TotalSeconds,
            IsBonus
        }
        internal enum CSV_PATHS_LYRICS
        {
            Artist,
            SongTitle,
            Album
        }

        internal enum StartType
        {
            Normal,
            PlayerOnly,
            MetadataStream
        }
        internal enum SaveType
        {
            Digital, CD, Vinyl, Cassette_Tape
        }
        public static String[] IDGenres = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", "punk", "postpunk", "folk", "blues" ,"funk", "new wave", "rocksinfonico", "ska", "flamenquito", "jazz fusion", ""};

        public static Genre[] Genres = new Genre[IDGenres.Length];

        public static Task TaskRefreshToken;
        private static CancellationTokenSource RefreshTokenCancellation = new CancellationTokenSource();
        private static CancellationTokenSource CancellationToken = new CancellationTokenSource();
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
        //public static Spotify Spotify;
        public static int NumLanguages;
        public static readonly string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //private static NotifyIcon notifyIconMetadataStream;

        public static uint SongCount = 0;
        private static string SongID = "";
        public static FileInfo HistorialFileInfo;
        public static FileInfo StreamFileInfo;


        public static string SearchSeparator = "/**/";

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
        //public static void ReloadGenres()
        //{
        //    for (int i = 0; i < Genres.Length - 1; i++)
        //    {
        //        //Genres[i].Name = LocalTexts.GetString("genero_" + Genres[i].Id);
        //        Resources.Strings.Strings.ResourceManager.GetObject("genero_" + Genres[i].Id);
        //    }
        //}
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
                    Genres[i].Name = "";/*(string)Resources.Strings.Strings.ResourceManager.GetObject("genero_" + Genres[i].Id);*/
                }
            }
        }





        //public static void Quit()
        //{
        //    if (!MetadataStream)
        //    {
        //        if (Edited)
        //        {
        //            DialogResult save = MessageBox.Show(LocalTexts.GetString("wantSave"), LocalTexts.GetString("titulo_ventana_principal"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //            if (save == DialogResult.Yes)
        //            {
        //                SaveAlbums("discos.csv", SaveType.Digital);
        //                SaveAlbums("cd.json", SaveType.CD);
        //                SaveAlbums("vinyl.json", SaveType.Vinyl);
        //                SaveAlbums("tapes.json", SaveType.Cassette_Tape);
        //                SavePATHS();
        //                SaveLyrics();
        //            }
        //        }

        //        Config.MainFormSize = MainForm.Size;
        //        Log.Instance.PrintMessage("Saving config...", MessageType.Info);
        //        Config.GuardarConfiguracion();

        //        Log.Instance.PrintMessage("Shutting down Player...", MessageType.Info);
        //        Player.Instancia.Apagar();
        //        Player.Instancia.Dispose();

        //        if (File.Exists("./covers/np.jpg"))
        //            File.Delete("./covers/np.jpg");

        //    }
        //    Log.Instance.CloseLog();

        //    if (Console)
        //        FreeConsole();
        //}



        //public static Song searchSong(string keyword)
        //{
        //    //foreach (AlbumData album in Collection.Albums)
        //    foreach (AlbumData album in Collection.Albums.Values)
        //    {
        //        foreach (Song song in album.Songs)
        //        {
        //            if (song.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        //                return song;
        //        }
        //    }
        //    return null;
        //}

        private static void ResetNum_Click(object sender, EventArgs e)
        {
            SongCount = 1;
        }



        //public static void CreateAndAddAlbumFromFolder(string path)
        //{
        //    Log.Instance.PrintMessage("Creating an album from a directory.", MessageType.Info);

        //    AlbumData a = new AlbumData();

        //    //To avoid a random song order, i create an array to store the songs. 150 SHOULD be big enough.
        //    Song[] tempStorage = new Song[150];
        //    int numSongs = 0; //to keep track of how many songs i've addded.
        //    Stopwatch crono = Stopwatch.StartNew();
        //    DirectoryInfo carpeta = new DirectoryInfo(path);
        //    Config.LastOpenedDirectory = carpeta.FullName;
        //    foreach (var filename in carpeta.GetFiles())
        //    {
        //        switch (Path.GetExtension(filename.FullName))
        //        {
        //            case ".mp3":
        //            case ".ogg":
        //            case ".flac":
        //                LocalSong LM = new LocalSong(filename.FullName);
        //                if (a.NeedsMetadata())
        //                {
        //                    a.Title = LM.Album;
        //                    a.Artist = LM.Artist;
        //                    a.Year = (short)LM.Year;
        //                    if (LM.AlbumCover is not null && !File.Exists("cover.jpg"))
        //                    {
        //                        Bitmap cover = new Bitmap(LM.AlbumCover);
        //                        cover.Save(carpeta.FullName + "\\cover.jpg", ImageFormat.Jpeg);
        //                        a.CoverPath = carpeta.FullName + "\\cover.jpg";
        //                    }
        //                }
        //                Song c = new Song(LM.Title, (int)LM.Length.TotalMilliseconds, false);
        //                if (LM.TrackNumber != 0) //A music file with no track number? Can happen. Instead, do the normal process.
        //                {
        //                    tempStorage[LM.TrackNumber - 1] = c;
        //                    numSongs++;
        //                }
        //                else
        //                    a.AddSong(c);
        //                c.SetAlbum(a);
        //                c.Path = filename.FullName;
        //                LM.Dispose();
        //                break;
        //            case ".jpg":
        //                if (filename.Name == "folder.jpg" || filename.Name == "cover.jpg")
        //                    a.CoverPath = filename.FullName;
        //                break;
        //        }
        //        if (numSongs != 0) //The counter has been updated and songs had a track number.
        //        {
        //            //This list goes to the album.
        //            List<Song> songList = new List<Song>();
        //            for (int i = 0; i < numSongs; i++)
        //            {
        //                //Copy the correct song order.
        //                songList.Add(tempStorage[i]);
        //            }
        //            a.Songs = songList;
        //        }
        //    }
        //    a.SoundFilesPath = carpeta.FullName;
        //    a.Type = AlbumType.Studio;
        //    Collection.AddAlbum(ref a);
        //    SetSaveMark();
        //    crono.Stop();
        //    Log.Instance.PrintMessage("Operation completed", MessageType.Correct, crono, TimeType.Milliseconds);
        //    ReloadView();
        //}
        //public static string GetSystemLanguage()
        //{
        //    CultureInfo ci = CultureInfo.CurrentUICulture;
        //    string lan = ci.Name.Split('-')[0];
        //    if (!Languages.Contains(lan))
        //        lan = "en";
        //    return lan;
        //}
        //public static void SetPathsForAlbum(AlbumData a)
        //{
        //    Log.Instance.PrintMessage("Searching songs for " + a.ToString(), MessageType.Info);
        //    Stopwatch crono = Stopwatch.StartNew();
        //    bool correcto = true;
        //    DirectoryInfo directorioCanciones = new DirectoryInfo(a.SoundFilesPath);
        //    foreach (FileInfo file in directorioCanciones.GetFiles())
        //    {
        //        string extension = Path.GetExtension(file.FullName);
        //        if (extension != ".ogg" && extension != ".mp3" && extension != ".flac")
        //            continue;
        //        LocalSong LM = new LocalSong(file.FullName);
        //        foreach (Song c in a.Songs)
        //        {
        //            try
        //            {
        //                if (LM.Evaluable() && !string.IsNullOrEmpty(c.Path))
        //                {
        //                    if ((c.Title.ToLower() == LM.Title.ToLower()) && (c.AlbumFrom.Artist.ToLower() == LM.Artist.ToLower()))
        //                    {
        //                        c.Path = file.FullName;
        //                        Log.Instance.PrintMessage(c.Title + " linked successfully!", MessageType.Correct);
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    if (file.FullName.ToLower().Contains(c.Title.ToLower()))
        //                    {
        //                        c.Path = file.FullName;
        //                        Log.Instance.PrintMessage(c.Title + " linked successfully", MessageType.Correct);
        //                        break;
        //                    }
        //                    correcto = false;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                correcto = false;
        //            }

        //        }
        //    }
        //    crono.Stop();

        //    if (correcto)
        //    {
        //        Log.Instance.PrintMessage("Finished without problems", MessageType.Correct, crono, TimeType.Milliseconds);
        //        MessageBox.Show(LocalTexts.GetString("pathsCorrectos"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }

        //    else
        //    {
        //        foreach (Song cancion in a.Songs)
        //        {
        //            if (string.IsNullOrEmpty(cancion.Path)) //No se ha encontrado
        //            {
        //                Log.Instance.PrintMessage(cancion.Title + " couldn't be linked...", MessageType.Warning);
        //            }
        //        }
        //        MessageBox.Show(LocalTexts.GetString("pathsError"), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //    SavePATHS();
        //}
        //public static void ResetSpotifyLink()
        //{
        //    MainForm.ResetSpotifyLink();
        //}
    }
}
