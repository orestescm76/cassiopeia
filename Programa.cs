using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
/*VERSION 1.4.X - FEATURE DISCO COMPACTO
 * 
 */
/*para portear a netcore https://stackoverflow.com/questions/43181904/how-to-get-the-resx-file-strings-in-asp-net-core*/
namespace aplicacion_musica
{
    static class Programa
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        public static ResXResourceSet textosLocal;
        public static String[] idGeneros = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", "punk", "postpunk", "folk", "blues" ,"funk", "new wave", "rocksinfonico", "ska", "flamenquito", "house", "jazz fusion", ""}; //lista hardcoded que tendrá su respectiva traducción en las últimas líneas del fichero !!
        public static Coleccion miColeccion;
        public static Genero[] generos = new Genero[idGeneros.Length];
        private static Version ver = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string version = ver.Major + "." + ver.Minor + "." + ver.Build + "." + ver.Revision + "  (build Disco Compacto)";
        public static string ErrorIdioma;
        public static Spotify _spotify;
        private static principal principal;
        public static string Idioma;
        public static bool ModoOscuro = false;
        public static void HayInternet(bool i)
        {
            principal.HayInternet(i);
        }
        //public static void cambiarIdioma(String idioma)
        //{
        //    string idiomatemp = Programa.idioma;
        //    try
        //    {
        //        Programa.idioma = idioma;
        //        int numCadenas = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(1).First());
        //        int numImagenes = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(2 + numCadenas).First());
        //        String[] textosLocalNou = textos.SkipWhile(linea => linea != idioma).Skip(2).Take(numCadenas).ToArray();
        //        if (textosLocalNou.Length != textosLocal.Length)
        //            throw new IndexOutOfRangeException();
        //        ErrorIdioma = textosLocalNou[15];
        //        String[] imagenesNuevas = textos.SkipWhile(linea => linea != idioma).Skip(3 + numCadenas).Take(numImagenes).ToArray();
        //        imagenesLocal = imagenesNuevas;
        //        textosLocal = textosLocalNou;
        //        refrescarGeneros();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show(ErrorIdioma);
        //        Programa.idioma = idiomatemp;
        //    }
        //}
        public static void refrescarVista()
        {
            principal.Refrescar();
        }
        public static int findGenero(string g)
        {
            for (int i = 0; i < idGeneros.Length; i++)
            {
                if (g == idGeneros[i])
                    return i;
            }
            return -1;
        }
        public static int findGeneroTraducido(string g)
        {
            for (int i = 0; i < generos.Length; i++)
            {
                if (g == generos[i].traducido)
                    return i;
            }
            return -1;
        }
        public static void refrescarGeneros()
        {
            for (int i = 0; i < generos.Length-1; i++)
            {
                generos[i].traducido = textosLocal.GetString("genero_" + generos[i].Id);
            }
        }
        public static void cargarAlbumes(string fichero)
        {
            Console.WriteLine(nameof(cargarAlbumes) + " - Cargando álbumes almacenados en " + fichero);
            Stopwatch crono = Stopwatch.StartNew();
            bool versionAntigua = true;
            string ver = File.ReadLines(fichero).First();
            if (ver == "1.4")
                versionAntigua = false;
            if (versionAntigua)
            {
                using (StreamReader lector = new StreamReader(fichero))
                {
                    string linea;
                    while (!lector.EndOfStream)
                    {
                        linea = lector.ReadLine();
                        while (linea == "") linea = lector.ReadLine();
                        if (linea == null) continue; //si no hay nada tu sigue, que hemos llegado al final del fichero, después del nulo porque siempre al terminar un disco pongo línea nueva.
                        string[] datos = linea.Split(';');
                        short nC = Convert.ToInt16(datos[3]);
                        int gen = Programa.findGenero(datos[4]);
                        Genero g = Programa.generos[gen];
                        if (datos[5] == null) datos[5] = "";
                        Album a = new Album(g, datos[0], datos[1], Convert.ToInt16(datos[2]), Convert.ToInt16(datos[3]), datos[5]);
                        bool exito = false;
                        for (int i = 0; i < nC; i++)
                        {
                            exito = false;
                            linea = lector.ReadLine();
                            if (linea == null || linea == "")
                            {
                                /*System.Windows.Forms.MessageBox.Show("mensajeError"+Environment.NewLine
                                    + a.nombre + " - " + a.nombre + Environment.NewLine
                                    + "saltarAlSiguiente", "error", System.Windows.Forms.MessageBoxButtons.OK);*/
                                break; //no sigue cargando el álbum
                            }
                            else
                            {
                                exito = true;
                                string[] datosCancion = linea.Split(';');
                                if (datosCancion.Length == 3)
                                {
                                    Cancion c = new Cancion(datosCancion[0], new TimeSpan(0, Convert.ToInt32(datosCancion[1]), Convert.ToInt32(datosCancion[2])), ref a);
                                    a.agregarCancion(c, i);
                                }
                                else
                                {
                                    CancionLarga cl = new CancionLarga(datosCancion[0], ref a);
                                    int np = Convert.ToInt32(datosCancion[1]);
                                    for (int j = 0; j < np; j++)
                                    {
                                        linea = lector.ReadLine();
                                        datosCancion = linea.Split(';');
                                        Cancion c = new Cancion(datosCancion[0], new TimeSpan(0, Convert.ToInt32(datosCancion[1]), Convert.ToInt32(datosCancion[2])), ref a);
                                        cl.addParte(ref c);
                                    }
                                    a.agregarCancion(cl, i);
                                }

                            }
                        }
                        if (miColeccion.estaEnColeccion(a))
                        {
                            exito = false; //pues ya está repetido.
                            Debug.WriteLine("Repetido");
                        }
                        if (exito)
                            miColeccion.agregarAlbum(ref a);
                    }
                }
            }
            else
            {
                using (StreamReader lector = new StreamReader(fichero))
                {
                    string linea;
                    lector.ReadLine();
                    while (!lector.EndOfStream)
                    {
                        linea = lector.ReadLine();

                        while (linea == "") linea = lector.ReadLine();
                        if (linea == null) continue; //si no hay nada tu sigue, que hemos llegado al final del fichero, después del nulo porque siempre al terminar un disco pongo línea nueva.
                        string[] datos = linea.Split(';');
                        short nC = Convert.ToInt16(datos[3]);
                        int gen = Programa.findGenero(datos[4]);
                        Genero g = Programa.generos[gen];
                        if (datos[5] == null) datos[5] = ""; //caratula
                        Album a = new Album(g, datos[0], datos[1], Convert.ToInt16(datos[2]), Convert.ToInt16(datos[3]), datos[5]);
                        bool exito = false;
                        for (int i = 0; i < nC; i++)
                        {
                            exito = false;
                            linea = lector.ReadLine();
                            if (linea == null || linea == "")
                            {
                                /*System.Windows.Forms.MessageBox.Show("mensajeError"+Environment.NewLine
                                    + a.nombre + " - " + a.nombre + Environment.NewLine
                                    + "saltarAlSiguiente", "error", System.Windows.Forms.MessageBoxButtons.OK);*/
                                break; //no sigue cargando el álbum
                            }
                            else
                            {
                                exito = true;
                                string[] datosCancion = linea.Split(';');
                                if (datosCancion.Length == 3) //cancion, titulo;423123;0
                                {
                                    bool b = false;
                                    if (datosCancion[2] == "1")
                                        b = true;
                                    Cancion c = new Cancion(datosCancion[0], new TimeSpan(0, 0, 0, 0, Convert.ToInt32(datosCancion[1])), ref a, b);
                                    a.agregarCancion(c, i);
                                    if (c.Bonus)
                                        a.duracion -= c.duracion;
                                }
                                else
                                {
                                    CancionLarga cl = new CancionLarga(datosCancion[0], ref a);
                                    int np = Convert.ToInt32(datosCancion[1]);
                                    for (int j = 0; j < np; j++)
                                    {
                                        linea = lector.ReadLine();
                                        datosCancion = linea.Split(';');
                                        Cancion c = new Cancion(datosCancion[0], new TimeSpan(0, 0, 0, 0, Convert.ToInt32(datosCancion[1])), ref a);
                                        cl.addParte(ref c);
                                    }
                                    a.agregarCancion(cl, i);
                                }

                            }
                        }
                        if (miColeccion.estaEnColeccion(a))
                        {
                            exito = false; //pues ya está repetido.
                            Debug.WriteLine("Repetido");
                        }
                        if (exito)
                            miColeccion.agregarAlbum(ref a);
                    }
                    crono.Stop();
                    Console.WriteLine(nameof(cargarAlbumes) + " - Cargados " + miColeccion.albumes.Count + " álbumes correctamente");
                    Console.WriteLine(nameof(cargarAlbumes) + " - Cargado en " + crono.ElapsedMilliseconds + " ms");
                    Programa.refrescarVista();
                }
            }
        }
        private static void cargarCDS()
        {
            if (!File.Exists("cd.mdb"))
                return;
            using(StreamReader lector = new StreamReader("cd.mdb"))
            {
                string linea;
                linea = lector.ReadLine();
                while(!lector.EndOfStream)
                {
                    linea = lector.ReadLine();
                    string[] datos = linea.Split(';');
                    Album a = miColeccion.devolverAlbum(datos[1] + "_" + datos[0]);
                    FormatoCD f = (FormatoCD)Enum.Parse(typeof(FormatoCD), datos[3]);
                    EstadoMedio ee = (EstadoMedio)Enum.Parse(typeof(EstadoMedio), datos[4]);
                    int nd = Convert.ToInt32(datos[2]);
                    DiscoCompacto cd = new DiscoCompacto(ref a, ee, f, nd);
                    cd.InstallID(datos[7]);
                    Disco d = null;
                    for (int i = 0; i < nd; i++)
                    {
                        linea = lector.ReadLine();
                        datos = linea.Split(';');
                        d = new Disco(Convert.ToInt16(datos[0]), (EstadoMedio)Enum.Parse(typeof(EstadoMedio), datos[1]));
                        cd.Discos[i] = d;
                    }

                    miColeccion.AgregarCD(ref cd);
                }
            }
        }
        [STAThread]
        static void Main(String[] args)
        {
            if(args.Contains("-consola"))
            {
                AllocConsole();
                Console.Title = "Consola debug v" + version;
                Console.WriteLine("Consola habilitada, se mostrarán detalles sobre la ejecución en español.\nSi la cierra se cerrará la aplicación.");
            }

            textosLocal = new ResXResourceSet(@"./idiomas/es.resx");
            Idioma = "es"; //provisional

            miColeccion = new Coleccion();
            //if (File.Exists("idioma.cfg"))
            //    idioma = File.ReadAllLines("idioma.cfg")[0];
            //else
            //    idioma = textos[1]; //tiene que haber minimo un idioma
            //idioma = textos[1];

            //prepara la aplicación para que ejecute formularios y demás.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            principal = new principal();

            _spotify = new Spotify();
            
            
            for (int i = 0; i < idGeneros.Length; i++)
            {
                if (idGeneros[i] == "")
                {
                    generos[i] = new Genero(idGeneros[i]);
                    generos[i].setTraduccion("-");
                }
                else
                {
                    generos[i] = new Genero(idGeneros[i]);
                    generos[i].setTraduccion(textosLocal.GetString("genero_"+generos[i].Id));
                }
            }
            if (File.Exists("discos.mdb"))
            {
                if (args.Length != 0 && args.Contains("-pregunta"))//cambiar parametro para cargar otro fichero
                {
                    ////DialogResult resultado = MessageBox.Show(Programa.textosLocal[16], "", MessageBoxButtons.YesNo);
                    //if (resultado == DialogResult.Yes)
                    //    Programa.miColeccion.cargarAlbumes("discos.mdb");
                }
                else
                {
                    cargarAlbumes("discos.mdb");
                    cargarCDS();
                }
            }
            else
            {
                Console.WriteLine("discos.mdb no existe, se creará una base de datos vacía.");
            }



            Application.Run(principal);
            if(args.Contains("-consola"))
            {
                Console.WriteLine("Programa finalizado, presione una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
