using Cassiopeia.src.Classes;
using Cassiopeia.src.VM;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Cassiopeia.src.VM.Kernel;

namespace Cassiopeia.VM
{
    public partial class MainVM : ObservableObject
    {
        private bool Edited = false;
        public static FileInfo HistorialFileInfo;
        public static FileInfo StreamFileInfo;
        public Collection Collection { get; set; }
        public MainVM()
        {
            InitApp();
        }

        private void InitApp()
        {
            Log.InitLog();
            InitGenres();
            Collection = new Collection();
            DateTime now = DateTime.Now;
            HistorialFileInfo = new FileInfo("Musical log " + now.Day + "-" + now.Month + "-" + now.Year + ".txt");
            StreamFileInfo = new FileInfo("np.txt");
            LoadFiles();

        }
        public void LoadFiles()
        {
            //if (JSON)
            //    LoadAlbums("discos.json");
            //else
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
                }
            }
            if (File.Exists("paths.txt"))
                LoadPATHS();
            if (File.Exists("lyrics.txt"))
                LoadLyrics();
        }
        //Methods for loading and saving...
        public void LoadAlbums(string fichero)
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
        }
        private void SendErrorLoading(int line, string file)
        {
            Log.Instance.PrintMessage("The album data file has mistakes. Check line " + line + " of " + file, MessageType.Error);
            //MessageBox.Show(LocalTexts.GetString("errorLoadingAlbums1") + line + " " + LocalTexts.GetString("errorLoadingAlbums2") + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void LoadCSVAlbums(string file)
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
        }
        public void LoadCD(string fichero = "cd.json")
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
        public void LoadVinyl(string fichero = "vinyl.json")
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
        public void LoadTapes(string fichero = "tapes.json")
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
        public void LoadPATHS()
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
        public void SavePATHS()
        {
            Log.Instance.PrintMessage("Saving PATHS", MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            FileInfo pathsInfo = new FileInfo("paths.txt");
            using (StreamWriter salida = pathsInfo.CreateText())
            {
                foreach (var pair in Collection.Albums)
                {
                    if (string.IsNullOrEmpty(pair.SoundFilesPath))
                        continue;
                    foreach (Song cancion in pair.Songs)
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
        private void SaveAlbums(string path, SaveType tipoGuardado)
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
                    foreach (var a in Collection.Albums)
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
        public void LoadLyrics()
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
                    AlbumData albumData = Collection.SearchAlbum(datos[(int)Kernel.CSV_PATHS_LYRICS.Album])[0];
                    Song song = albumData.GetSong(datos[(int)Kernel.CSV_PATHS_LYRICS.SongTitle]);
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
        public void SaveLyrics()
        {
            Log.Instance.PrintMessage("Saving lyrics", MessageType.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamWriter salida = new FileInfo("lyrics.txt").CreateText()) //change?
            {
                //foreach (AlbumData album in Collection.Albums)
                foreach (var album in Collection.Albums)
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
        public void ShowError(string msg)
        {
            //App.MainPage.DisplayAlert("error", "", "jaja");
            //MessageBox.Show(msg, LocalTexts.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void CloseApplication(object obj)
        {
            //App.Quit();
        }
    }
}
