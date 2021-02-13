using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Net;

/* VERSION 1.6.xx CODENAME COCKROACH
* Reproductor:
*  Soporte CD Audio
*  Rework del sistema de playlists
* Gestor:
*  Ahora se puede redimensonar la ventana principal
*  Nuevo botón, abrir una disquetera para reproducir un CD
*  Visor de lyrics
*  Visor de log
* Misc:
*  Argumentos de lanzamiento en inglés
*/
namespace aplicacion_musica
{
    static class Programa
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        public static ResXResourceSet textosLocal;
        public static String[] idGeneros = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", "punk", "postpunk", "folk", "blues" ,"funk", "new wave", "rocksinfonico", "ska", "flamenquito", "jazz fusion", ""};
        public static Coleccion miColeccion;
        public static Genre[] genres = new Genre[idGeneros.Length];
        private static Version ver = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string version = ver.ToString()+ " ";
        public static string[] idiomas;
        public static Spotify _spotify;
        private static principal principal = null;
        public static bool ModoOscuro = false;
        public static readonly string CodeName = "Cockroach";
        public static bool SpotifyActivado = true;
        public static bool ModoReproductor = false;
        public static Thread tareaRefrescoToken;
        public static bool ModoStream = false;
        public static int NumIdiomas = 0;
        private static bool ComprobarActualizaciones = true;
        private static bool Spotify = true;
        private static bool InicioReproductor = false;
        private static bool Consola = false;

        public static void Refresco()
        {
            while(true)
            {
                if (_spotify.TokenExpirado())
                {
                    _spotify.RefrescarToken();
                }

                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }

        //método mierdoso por temas de privado público
        public static void HayInternet(bool i)
        {
            principal.HayInternet(i);
        }
        public static void CambiarIdioma(String idioma)
        {
            Log.Instance.ImprimirMensaje("Cambiando idioma al " + idioma, TipoMensaje.Info);
            textosLocal = new ResXResourceSet(@"./idiomas/" + "original." + idioma + ".resx");
            Config.Idioma = idioma;
            RefrescarGeneros();
            RefrescarVista();
            src.Forms.Reproductor.Instancia.RefrescarTextos();
        }
        public static void ActivarReproduccionSpotify()
        {
            principal.ActivarReproduccionSpotify();
        }
        public static void RefrescarVista()
        {
            principal.Refrescar();
        }
        public static int FindGenero(string g)
        {
            for (int i = 0; i < idGeneros.Length; i++)
            {
                if (g == idGeneros[i])
                    return i;
            }
            return -1;
        }
        public static int FindGeneroTraducido(string g)
        {
            for (int i = 0; i < genres.Length; i++)
            {
                if (g == genres[i].Name)
                    return i;
            }
            return -1;
        }
        public static void RefrescarGeneros()
        {
            for (int i = 0; i < genres.Length-1; i++)
            {
                genres[i].Name = textosLocal.GetString("genero_" + genres[i].Id);
            }
        }

        public static void CargarAlbumes(string fichero)
        {
            Log.Instance.ImprimirMensaje("Cargando álbumes almacenados en " + fichero, TipoMensaje.Info, "cargarAlbumes(string)");
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamReader lector = new StreamReader(fichero))
            {
                string LineaJson = "";
                while (!lector.EndOfStream)
                {
                    LineaJson = lector.ReadLine();
                    AlbumData a = JsonConvert.DeserializeObject<AlbumData>(LineaJson);
                    a.Genre = genres[FindGenero(a.Genre.Id)];
                    a.ConfigurarCanciones();
                    miColeccion.agregarAlbum(ref a);
                    a.CanBeRemoved = true;
                }
            }
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargados " + miColeccion.albumes.Count + " álbumes correctamente", TipoMensaje.Correcto, crono);
            RefrescarVista();
        }

        public static void CargarAlbumesCSV(string fichero)
        {
            Log.Instance.ImprimirMensaje("Cargando álbumes CSV almacenados en " + fichero, TipoMensaje.Info, "cargarAlbumesLegacy(string)");
            Stopwatch crono = Stopwatch.StartNew();
            //cargando CSV a lo bestia
            int lineaC = 1;
            using (StreamReader lector = new StreamReader(fichero))
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
                        Log.Instance.ImprimirMensaje("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, TipoMensaje.Error);
                        MessageBox.Show("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(-1);
                    }
                    short nC = 0;
                    int gen = FindGenero(datos[4]);
                    Genre g = Programa.genres[gen];
                    if (string.IsNullOrEmpty(datos[5])) datos[5] = string.Empty;
                    AlbumData a = null;
                    try
                    {
                        nC = Convert.ToInt16(datos[3]);
                        a = new AlbumData(g, datos[0], datos[1], Convert.ToInt16(datos[2]), datos[5]);
                    }
                    catch (FormatException e)
                    {
                        Log.Instance.ImprimirMensaje("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, TipoMensaje.Error);
                        MessageBox.Show("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(-1);
                    }
                    if (!string.IsNullOrEmpty(datos[6]))
                        a.IdSpotify = datos[6];
                    if (!string.IsNullOrEmpty(datos[7]))
                        a.SoundFilesPath = datos[7];
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
                                    byte bonus = Convert.ToByte(datosCancion[2]);
                                    Song c = new Song(datosCancion[0], TimeSpan.FromSeconds(Convert.ToInt32(datosCancion[1])), ref a, Convert.ToBoolean(bonus));
                                    a.AddSong(c, i);
                                }
                                else
                                {
                                    CancionLarga cl = new CancionLarga(datosCancion[0], ref a);
                                    int np = Convert.ToInt32(datosCancion[1]);
                                    for (int j = 0; j < np; j++)
                                    {
                                        linea = lector.ReadLine();
                                        lineaC++;
                                        datosCancion = linea.Split(';');
                                        Song c = new Song(datosCancion[0], TimeSpan.FromSeconds(Convert.ToInt32(datosCancion[1])), ref a);
                                        cl.addParte(c);
                                    }
                                    a.AddSong(cl, i);
                                }
                            }
                            catch (FormatException e)
                            {
                                Log.Instance.ImprimirMensaje("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, TipoMensaje.Error);
                                MessageBox.Show("Error cargando el álbum. Revise la línea " + lineaC + " del fichero " + fichero, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(-1);
                            }
                        }
                    }
                    if (miColeccion.estaEnColeccion(a))
                    {
                        exito = false; //pues ya está repetido.
                        Log.Instance.ImprimirMensaje("Álbum repetido -> " + a.Artist + " - " + a.Title, TipoMensaje.Advertencia);
                    }
                    if (exito)
                        miColeccion.agregarAlbum(ref a);

                    a.CanBeRemoved = true;
                    lineaC++;
                }
            }
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargados " + miColeccion.albumes.Count + " álbumes correctamente", TipoMensaje.Correcto, crono);
            RefrescarVista();
        }
        public static void CargarCDS(string fichero = "cd.json")
        {
            if (!File.Exists(fichero))
                return;
            using(StreamReader lector = new StreamReader(fichero))
            {
                string linea;
                while(!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    DiscoCompacto cd = JsonConvert.DeserializeObject<DiscoCompacto>(linea);
                    cd.InstallAlbum();
                    miColeccion.AgregarCD(ref cd);
                    cd.Album.CanBeRemoved = false;
                }
            }
        }
        private static void CargarPATHS()
        {
            Log.Instance.ImprimirMensaje("Cargando PATHS", TipoMensaje.Info);
            using(StreamReader entrada = new FileInfo("paths.txt").OpenText())
            {
                string linea = null;
                while(!entrada.EndOfStream)
                {
                    linea = entrada.ReadLine();
                    string[] datos = linea.Split(';');
                    List<AlbumData> listaAlbumes = miColeccion.buscarAlbum(datos[2]);
                    if (listaAlbumes.Count != 0)
                    {
                        foreach (AlbumData album in listaAlbumes)
                        {
                            if(album.Artist == datos[0] && album.Title == datos[2])
                            {
                                Song c = album.GetSong(datos[1]);
                                linea = entrada.ReadLine();
                                c.PATH = linea;
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
        public static void GuardarPATHS()
        {
            Log.Instance.ImprimirMensaje("Guardando PATHS", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using(StreamWriter salida = new FileInfo("paths.txt").CreateText())
            {
                foreach (AlbumData album in miColeccion.albumes)
                {
                    if (string.IsNullOrEmpty(album.SoundFilesPath))
                        continue;
                    foreach (Song cancion in album.Songs)
                    {
                        if (!string.IsNullOrEmpty(cancion.PATH))
                        {
                            salida.Write(cancion.GuardarPATH());
                        }
                    }
                }
            }
            crono.Stop();
            Log.Instance.ImprimirMensaje("Guardados los PATHS", TipoMensaje.Correcto, crono);
        }
        public static void GuardarDiscos(string path, TipoGuardado tipoGuardado, bool json = false)
        {

            Stopwatch crono = Stopwatch.StartNew();
            FileInfo fich = new FileInfo(path);
            if (json)
            {
                using (StreamWriter salida = fich.CreateText())
                {
                    switch (tipoGuardado)
                    {
                        case TipoGuardado.Digital:
                            Log.Instance.ImprimirMensaje(nameof(GuardarDiscos) + " - Guardando la base de datos... (" + Programa.miColeccion.albumes.Count + " discos)", TipoMensaje.Info);
                            Log.Instance.ImprimirMensaje("Nombre del fichero: " + path, TipoMensaje.Info);
                            foreach (AlbumData a in Programa.miColeccion.albumes)
                            {
                                JsonSerializer s = new JsonSerializer();
                                s.TypeNameHandling = TypeNameHandling.All;
                                salida.WriteLine(JsonConvert.SerializeObject(a));
                            }
                            break;
                        case TipoGuardado.CD:
                            Log.Instance.ImprimirMensaje(nameof(GuardarDiscos) + " - Guardando la base de datos... (" + Programa.miColeccion.cds.Count + " cds)", TipoMensaje.Info);
                            Log.Instance.ImprimirMensaje("Nombre del fichero: " + path, TipoMensaje.Info);
                            foreach (DiscoCompacto compacto in Programa.miColeccion.cds)
                            {
                                salida.WriteLine(JsonConvert.SerializeObject(compacto));
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            else
            {
                using (StreamWriter salida = fich.CreateText())
                {
                    switch (tipoGuardado)
                    {
                        case TipoGuardado.Digital:
                            Log.Instance.ImprimirMensaje(nameof(GuardarDiscos) + " - Guardando la base de datos... (" + Programa.miColeccion.albumes.Count + " discos)", TipoMensaje.Info);
                            Log.Instance.ImprimirMensaje("Nombre del fichero: " + path, TipoMensaje.Info);
                            foreach (AlbumData a in miColeccion.albumes)
                            {
                                if (!(a.Songs[0] == null)) //no puede ser un album con 0 canciones
                                {
                                    salida.WriteLine(a.Title + ";" + a.Artist + ";" + a.Year + ";" + a.NumberOfSongs + ";" + a.Genre.Id + ";" + a.Cover + ";"+a.IdSpotify + ";"+a.SoundFilesPath);
                                    for (int i = 0; i < a.NumberOfSongs; i++)
                                    {
                                        if (a.Songs[i] is CancionLarga cl)
                                        {
                                            salida.WriteLine(cl.titulo + ";" + cl.Partes.Count);//no tiene duracion y son 2 datos a guardar
                                            foreach (Song parte in cl.Partes)
                                            {
                                                salida.WriteLine(parte.titulo + ";" + parte.duracion.TotalSeconds);
                                            }

                                        }
                                        else //titulo;400;0
                                            salida.WriteLine(a.Songs[i].titulo + ";" + (int)a.Songs[i].duracion.TotalSeconds + ";"+Convert.ToInt32(a.Songs[i].IsBonus));
                                    }
                                }
                                salida.WriteLine();
                            }
                            break;
                        case TipoGuardado.CD:
                            break;
                        case TipoGuardado.Vinilo:
                            break;
                        default:
                            break;
                    }

                }
            }
            crono.Stop();
            fich.Refresh();
            Log.Instance.ImprimirMensaje(nameof(GuardarDiscos) + "- Guardado", TipoMensaje.Correcto, crono);
            crono.Stop();
        }
        private static void CargarLyrics()
        {
            Log.Instance.ImprimirMensaje("Cargando lyrics", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamReader entrada = new FileInfo("lyrics.txt").OpenText())
            {
                string linea = null;
                while (!entrada.EndOfStream)
                {
                    linea = entrada.ReadLine();
                    string[] datos = linea.Split(';');
                    AlbumData albumData = miColeccion.buscarAlbum(datos[2])[0];
                    Song song = albumData.GetSong(datos[1]);
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
            Log.Instance.ImprimirMensaje("Lyrics cargadas", TipoMensaje.Correcto, crono);
        }
        private static void GuardarLyrics()
        {
            Log.Instance.ImprimirMensaje("Guardando lyrics", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            using (StreamWriter salida = new FileInfo("lyrics.txt").CreateText())
            {
                foreach (AlbumData album in miColeccion.albumes)
                {
                    foreach (Song cancion in album.Songs)
                    {
                        if (cancion.Lyrics != null && cancion.Lyrics.Length != 0)
                        {
                            salida.WriteLine(album.Artist + ";" + cancion.titulo + ";" + album.Title);
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
            Log.Instance.ImprimirMensaje("Guardados las letras", TipoMensaje.Correcto, crono);
            FileInfo lyrics = new FileInfo("lyrics.txt");
            Log.Instance.ImprimirMensaje("Tamaño del fichero: " + lyrics.Length/1024 + " kb", TipoMensaje.Info);
        }
        static bool HayActualizacions(out string verNueva)
        {
            HttpWebRequest GithubRequest = WebRequest.CreateHttp("https://api.github.com/repos/orestescm76/aplicacion-gestormusica/releases");
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
                Log.Instance.ImprimirMensaje("Hubo un problema intentando localizar la nueva versión...", TipoMensaje.Error);
                Log.Instance.ImprimirMensaje("Respuesta del servidor: " + e.Response, TipoMensaje.Info);
                verNueva = string.Empty;
                return false;
            }
            int indexVersion = contenido.IndexOf("tag_name");
            verNueva = contenido.Substring(indexVersion, 40);
            verNueva = verNueva.Split('v')[1].Split('\"')[0];
            if (verNueva != version)
                return true;
            else
                return false;
        }
        [STAThread]
        static void Main(String[] args)
        {
            //Creación log.
            Log Log = Log.Instance;
            //prepara la aplicación para que ejecute formularios y demás.
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Config.CargarConfiguracion();
            textosLocal = new ResXResourceSet(@"./idiomas/" + "original." + Config.Idioma + ".resx");
            //Parseo de argumentos...
            foreach (string Arg in args)
            {
                switch (Arg)
                {
                    case "-consola":
                        Consola = true;
                        AllocConsole();
                        Console.Title = "Consola debug v" + version;
                        Console.WriteLine("Log creado " + DateTime.Now);
                        Log.ImprimirMensaje("Se ha iniciado la aplicación con el parámetro -consola", TipoMensaje.Info);
                        break;
                    case "-noSpotify":
                        Spotify = false;
                        break;
                    case "-modoStream":
                    case "-streamMode":
                        ModoStream = true;
                        break;
                    case "-noActualizar":
                    case "-noUpdates":
                        ComprobarActualizaciones = false;
                        break;
                    case "-reproductor":
                    case "-player":
                        InicioReproductor = true;
                        break;
                    default:
                        break;
                }
            }
            //Cargar idiomas...
            DirectoryInfo cod = new DirectoryInfo("./idiomas");
            Programa.idiomas = new String[cod.GetFiles().Length];
            int j = 0;
            foreach (var idioma in cod.GetFiles())
            {
                Programa.NumIdiomas++;
                string id = idioma.Name.Replace(".resx", "");
                id = id.Replace("original.", "");
                Programa.idiomas[j] = id;
                j++;
            }


            string versionNueva;
            if (HayActualizacions(out versionNueva) && ComprobarActualizaciones)
            {
                Log.ImprimirMensaje("Está disponible la actualización " + versionNueva, TipoMensaje.Info);
                DialogResult act = MessageBox.Show(textosLocal.GetString("actualizacion1") + Environment.NewLine + versionNueva + Environment.NewLine + textosLocal.GetString("actualizacion2"), "", MessageBoxButtons.YesNo);
                if (act == DialogResult.Yes)
                    Process.Start("https://github.com/orestescm76/aplicacion-gestormusica/releases");
            }
            miColeccion = new Coleccion();
            SpotifyActivado = false;
            principal = new principal();
            if(Spotify)
            {
                if (!Config.VinculadoConSpotify)
                    _spotify = new Spotify(false);
                else
                {
                    _spotify = new Spotify(true);
                    SpotifyActivado = true;
                    principal.DesactivarVinculacion();
                }
            }
            else
            {
                SpotifyActivado = false;
                Log.ImprimirMensaje("Se ha iniciado la aplicación con el parámetro -noSpotify, no habrá integración con Spotify", TipoMensaje.Info);
                _spotify = null;
                principal.HayInternet(false);
            }
            if(!ModoStream)
            {
                Log.ImprimirMensaje("Configurando géneros", TipoMensaje.Info);
                for (int i = 0; i < idGeneros.Length; i++)
                {
                    if (idGeneros[i] == "")
                    {
                        genres[i] = new Genre(idGeneros[i]);
                        genres[i].Name = "-";
                    }
                    else
                    {
                        genres[i] = new Genre(idGeneros[i]);
                        genres[i].Name = textosLocal.GetString("genero_" + genres[i].Id);
                    }
                }
                if (args.Contains("-json"))
                    CargarAlbumes("discos.json");
                else
                {
                    if (File.Exists("discos.csv"))
                    {
                        CargarAlbumesCSV("discos.csv");
                        CargarCDS();
                    }
                    else
                    {
                        Log.ImprimirMensaje("discos.csv no existe, se creará una base de datos vacía.", TipoMensaje.Advertencia);
                    }
                }
                if (File.Exists("paths.txt"))
                    CargarPATHS();
                if (File.Exists("lyrics.txt"))
                    CargarLyrics();
            }
            //creo el Reproductor
            src.Forms.Reproductor.Instancia = new src.Forms.Reproductor();
            src.Forms.Reproductor.Instancia.RefrescarTextos();
            if (ModoStream) //enchufa la app sin nada, solo el spotify y el texto
            {
                Application.Run();
            }
            else if (!InicioReproductor) //tirale con el principal
                Application.Run(principal);
            else
            {
                Application.Run(src.Forms.Reproductor.Instancia);
            }
            if(_spotify != null && tareaRefrescoToken != null)
                tareaRefrescoToken.Abort();
            GuardarPATHS();
            GuardarLyrics();
            Config.GuardarConfiguracion();

            if (File.Exists("./covers/np.jpg"))
                File.Delete("./covers/np.jpg");
            if (Consola)
            {
                Console.WriteLine("Programa finalizado, presione una tecla para continuar...");
                Console.ReadKey();
            }
            Log.Instance.CerrarLog();
        }

    }
}
