using System;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Globalization;

namespace Cassiopeia
{
    //Clase para temas de configuración
    public static class Config
    {
        public static string Language;
        public static string LyricsFont;
        public static bool LinkedWithSpotify;
        public static string LastOpenedDirectory;
        public static string Clipboard; //String que almacena cómo se guarda un álbum al portapapeles.
        public static Color ColorBonus;
        public static Color ColorLongSong;
        private static ResXResourceSet cargador;
        private static ResXResourceWriter guardador;
       
        public static void CargarConfiguracion()
        {
            if(File.Exists("config.cfg"))
            {
                cargador = new ResXResourceSet("config.cfg");
                Language = cargador.GetString("Language");
                LyricsFont = cargador.GetString("LyricsFont");
                LastOpenedDirectory = cargador.GetString("LastOpenedDirectory");
                LinkedWithSpotify = Convert.ToBoolean(cargador.GetString("LinkedWithSpotify"));
                Clipboard = cargador.GetString("Clipboard");
                //Load the colors
                ColorLongSong = Color.FromArgb(int.Parse(cargador.GetString("ColorLongSong"), NumberStyles.HexNumber));
                ColorBonus = Color.FromArgb(int.Parse(cargador.GetString("ColorBonus"), NumberStyles.HexNumber));
            }
            else
            {
                Language = Properties.Resources.Idioma;
                LyricsFont = Properties.Resources.TipografiaLyrics;
                LinkedWithSpotify = Convert.ToBoolean(Properties.Resources.VinculadoConSpotify);
                LastOpenedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                Clipboard = "%artist% - %title% (%year%)";
                ColorLongSong = Color.Salmon;
                ColorBonus = Color.SkyBlue;
            }
            guardador = new ResXResourceWriter("config.cfg");
        }
        public static void GuardarConfiguracion()
        {
            guardador.AddResource("Language", Language);
            guardador.AddResource("LyricsFont", LyricsFont);
            guardador.AddResource("LinkedWithSpotify", LinkedWithSpotify.ToString());
            guardador.AddResource("LastOpenedDirectory", LastOpenedDirectory);
            guardador.AddResource("Clipboard", Clipboard);
            guardador.AddResource("ColorBonus", ColorBonus.ToArgb().ToString("X"));
            guardador.AddResource("ColorLongSong", ColorLongSong.ToArgb().ToString("X"));
            guardador.Close();
        }
        public static Image GetIconoBandera()
        {
            switch (Language)
            {
                case "es":
                    return Properties.Resources.es;
                case "ca":
                    return Properties.Resources.ca;
                case "en":
                    return Properties.Resources.en;
                case "el":
                    return Properties.Resources.el;
            }
            return null;
        }
    }
}
