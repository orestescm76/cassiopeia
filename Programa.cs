using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
/*VERSIÓN 1.1 - TODO
 * botón borrado -fet pero falta meter idioma
 * perfilado el visualizar (añadir el numero de cancion) -fet
 * quitar imágenes -fet
 * rediseño ??
 * más generos...
 * +arreglado el bug de canciones +60 min
 */
namespace aplicacion_ipo
{
    static class Programa
    {
        static String[] textos; //carga TODOS los textos
        public static String[] textosLocal;
        public static String[] imagenesLocal;
        public static List<String> codigosIdiomas;
        public static List<int> idiomasIndices;
        public static int numIdiomas;
        public static String idioma;
        public static String[] idGeneros = {"clasica", "hardrock", "rockprog", "progmetal", "rockpsicodelico", "heavymetal", "blackmetal", "electronica", "postrock", "indierock",
            "stoner", "pop", "jazz", "disco", "vaporwave", "chiptune", ""}; //lista hardcoded que tendrá su respectiva traducción en las últimas líneas del fichero !!
        public static Coleccion miColeccion;
        public static Genero[] generos = new Genero[idGeneros.Length];
        public static readonly string version = "1.1";
        public static string ErrorIdioma;
        private static readonly int ultimaCadena = 28;
        public static void cambiarIdioma(String idioma)
        {
            string idiomatemp = Programa.idioma;
            try
            {
                Programa.idioma = idioma;
                int numCadenas = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(1).First());
                int numImagenes = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(2 + numCadenas).First());
                String[] textosLocalNou = textos.SkipWhile(linea => linea != idioma).Skip(2).Take(numCadenas).ToArray();
                if (textosLocalNou.Length != textosLocal.Length)
                    throw new IndexOutOfRangeException();
                ErrorIdioma = textosLocalNou[15];
                String[] imagenesNuevas = textos.SkipWhile(linea => linea != idioma).Skip(3 + numCadenas).Take(numImagenes).ToArray();
                imagenesLocal = imagenesNuevas;
                textosLocal = textosLocalNou;
                refrescarGeneros();
            }
            catch (Exception)
            {
                MessageBox.Show(ErrorIdioma);
                Programa.idioma = idiomatemp;
            }
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
                generos[i].traducido = textosLocal[i + ultimaCadena];
            }
        }
        [STAThread]
        static void Main(String[] args)
        {
            miColeccion = new Coleccion();
            textos = File.ReadAllLines("inter.txt");
            idioma = File.ReadAllLines("idioma.cfg")[0];
            //idioma = textos[1];
            codigosIdiomas = new List<string>();
            //codigosIdiomas.Add(idioma);
            idiomasIndices = new List<int>();
            //idiomasIndices.Add(1);
            int numCadenas = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(1).First());
            int numImagenes = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(2 + numCadenas).First());
            textosLocal = textos.SkipWhile(linea => linea != idioma).Skip(2).Take(numCadenas).ToArray();
            imagenesLocal = textos.SkipWhile(linea => linea != idioma).Skip(3 + numCadenas).Take(numImagenes).ToArray();

            ErrorIdioma = textosLocal[15];
            numIdiomas = Convert.ToInt32(textos[0]);
            //textosLocal = 
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
                    generos[i].setTraduccion(textosLocal[i + ultimaCadena]);
                }
            }
            int cadenas = textosLocal.Length + numImagenes + 3;
            for (int i = 0; i < numIdiomas; i++)
            {
                codigosIdiomas.Add(textos[(2 + i * cadenas)-1]);
                idiomasIndices.Add((2 + i * cadenas) - 1);
            }
            if(args.Length != 0 && args[0] == "-preguntar")//cambiar parametro para cargar otro fichero
            {
                DialogResult resultado = MessageBox.Show(Programa.textosLocal[16], "", MessageBoxButtons.YesNo);
                if (resultado == DialogResult.Yes)
                    Programa.miColeccion.cargarAlbumes("discos.mdb");
            }
            else Programa.miColeccion.cargarAlbumes("discos.mdb");
            //prepara la aplicación para que ejecute formularios y demás.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new principal());
        }
    }
}
