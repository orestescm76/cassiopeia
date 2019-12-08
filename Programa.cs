using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace aplicacion_ipo
{
    static class Programa
    {
        static String[] textos; //carga TODOS los textos
        public static String[] textosLocal;
        public static List<String> codigosIdiomas;
        public static List<int> idiomasIndices;
        public static int numIdiomas;
        public static String idioma;

        public static void cambiarIdioma(String idioma)
        {
            Programa.idioma = idioma;
            int numCadenas = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(1).First());
            int numImagenes = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(2 + numCadenas).First());
            textosLocal = textos.SkipWhile(linea => linea != idioma).Skip(2).Take(numCadenas).ToArray();
        }

        [STAThread]
        static void Main(String[] args)
        {
            textos = File.ReadAllLines("inter.txt");
            idioma = textos[1];
            codigosIdiomas = new List<string>();
            codigosIdiomas.Add(idioma);
            idiomasIndices = new List<int>();
            idiomasIndices.Add(1);
            int numCadenas = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(1).First());
            int numImagenes = Convert.ToInt32(textos.SkipWhile(linea => linea != idioma).Skip(2 + numCadenas).First());
            textosLocal = textos.SkipWhile(linea => linea != idioma).Skip(2).Take(numCadenas).ToArray();
            numIdiomas = Convert.ToInt32(textos[0]);
            //textosLocal = 

            int cadenas = textosLocal.Length + numImagenes + 3;
            for (int i = 1; i < numIdiomas; i++)
            {
                codigosIdiomas.Add(textos[(2 + i * cadenas)-1]);
                idiomasIndices.Add((2 + i * cadenas) - 1);
            }


            //prepara la aplicación para que ejecute formularios y demás.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new principal());
        }
    }
}
