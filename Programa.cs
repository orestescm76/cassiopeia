using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.Globalization;
/*VERSION 1.4.x
* rework idioma (por fin)
* 
*/
namespace aplicacion_musica
{
    static class Programa
    {
        public static ResXResourceSet textosLocal;
        public static String[] idGeneros = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", "punk", "postpunk", "folk", "blues" ,"funk", "new wave", "rocksinfonico", "ska", "flamenquito", "house", "jazz fusion", ""}; //lista hardcoded que tendrá su respectiva traducción en las últimas líneas del fichero !!
        public static Coleccion miColeccion;
        public static Genero[] generos = new Genero[idGeneros.Length];
        private static Version ver = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string version = ver.Major + "." + ver.Minor + "." +ver.MajorRevision+"."+ ver.Revision;
        public static string ErrorIdioma;
        public static Spotify _spotify;
        private static principal principal;
        public static string Idioma;
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
        [STAThread]
        static void Main(String[] args)
        {
            textosLocal = new ResXResourceSet(@"./idiomas/es.resx");
            Idioma = "es"; //provisional

            miColeccion = new Coleccion();
            //if (File.Exists("idioma.cfg"))
            //    idioma = File.ReadAllLines("idioma.cfg")[0];
            //else
            //    idioma = textos[1]; //tiene que haber minimo un idioma
            //idioma = textos[1];
            _spotify = new Spotify();
            
            
            for (int i = 0; i < idGeneros.Length; i++)
            {
                if (idGeneros[i] == "")
                {
                    generos[i] = new Genero(idGeneros[i]);
                    generos[i].setTraduccion("null");
                }
                else
                {
                    generos[i] = new Genero(idGeneros[i]);
                    generos[i].setTraduccion(textosLocal.GetString("genero_"+generos[i].Id));
                }
            }
            if (File.Exists("discos.mdb"))
            {
                if (args.Length != 0 && args.Contains("-preguntar"))//cambiar parametro para cargar otro fichero
                {
                    ////DialogResult resultado = MessageBox.Show(Programa.textosLocal[16], "", MessageBoxButtons.YesNo);
                    //if (resultado == DialogResult.Yes)
                    //    Programa.miColeccion.cargarAlbumes("discos.mdb");
                }
                else Programa.miColeccion.cargarAlbumes("discos.mdb");
            }
            else
                File.Create("discos.mdb");
            //prepara la aplicación para que ejecute formularios y demás.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            principal = new principal();
            Application.Run(principal);
        }
    }
}
