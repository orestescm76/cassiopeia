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
        public static bool LinkedWithSpotify;
        public static string LastOpenedDirectory;
        public static string Clipboard; //String que almacena cómo se guarda un álbum al portapapeles.
        public static string History;
        public static bool HistoryEnabled;
        public static string StreamString;
        public static bool StreamEnabled;
        public static Color ColorBonus;
        public static Color ColorLongSong;
        public static Font FontLyrics;
        public static Font FontView;
        public static Size MainFormSize;
        public static bool MainFormViewSidebar;
        private static ResXResourceSet cargador;
        private static ResXResourceWriter guardador;
       
        public static void CargarConfiguracion()
        {
            if(File.Exists("config.cfg"))
            {
                try
                {
                    cargador = new ResXResourceSet("config.cfg");
                    Language = cargador.GetString("Language");
                    LastOpenedDirectory = cargador.GetString("LastOpenedDirectory");
                    LinkedWithSpotify = (bool)cargador.GetObject("LinkedWithSpotify");
                    Clipboard = cargador.GetString("Clipboard");
                    History = cargador.GetString("History");
                    HistoryEnabled = (bool)cargador.GetObject("HistoryEnabled");
                    StreamString = cargador.GetString("StreamString");
                    StreamEnabled = (bool)cargador.GetObject("StreamEnabled");
                    //Load the colors
                    ColorLongSong = Color.FromArgb(int.Parse(cargador.GetString("ColorLongSong"), NumberStyles.HexNumber));
                    ColorBonus = Color.FromArgb(int.Parse(cargador.GetString("ColorBonus"), NumberStyles.HexNumber));
                    //Load fonts
                    string[] FontViewString = cargador.GetString("FontView").Split(',');
                    string[] FontLyricsString = cargador.GetString("FontLyrics").Split(',');
                    FontView = new Font(FontViewString[0], (float)Convert.ToInt32(FontViewString[1]));
                    FontLyrics = new Font(FontLyricsString[0], (float)Convert.ToInt32(FontLyricsString[1]));
                    MainFormSize = (Size)cargador.GetObject("MainFormSize");
                    MainFormViewSidebar = (bool)cargador.GetObject("MainFormViewSidebar");
                }
                catch (NullReferenceException)
                {
                    //Load defaults
                    MainFormViewSidebar = true;
                }

            }
            else
            {
                Language = "es";
                LinkedWithSpotify = false;
                LastOpenedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                Clipboard = "%artist% - %title% (%year%)";
                History = "#%track_num%. %artist% - %title%";
                StreamString = "#%track_num%. %artist% - %title%";
                StreamEnabled = false;
                HistoryEnabled = true;
                ColorLongSong = Color.Salmon;
                ColorBonus = Color.SkyBlue;
                FontLyrics = new Font("Segoe UI", 10);
                FontView = new Font("Segoe UI", 10);
                //MainFormSize -> designer
                MainFormViewSidebar = true;
            }
            guardador = new ResXResourceWriter("config.cfg");
        }
        public static void GuardarConfiguracion()
        {
            guardador.AddResource("Language", Language);
            guardador.AddResource("LinkedWithSpotify", LinkedWithSpotify);
            guardador.AddResource("LastOpenedDirectory", LastOpenedDirectory);
            guardador.AddResource("Clipboard", Clipboard);
            guardador.AddResource("History", History);
            guardador.AddResource("HistoryEnabled", HistoryEnabled);
            guardador.AddResource("ColorBonus", ColorBonus.ToArgb().ToString("X"));
            guardador.AddResource("ColorLongSong", ColorLongSong.ToArgb().ToString("X"));
            guardador.AddResource("FontLyrics", FontLyrics.FontFamily.Name+","+ (int)FontLyrics.Size);
            guardador.AddResource("FontView", FontView.FontFamily.Name+ "," + (int)FontView.Size);
            guardador.AddResource("MainFormSize", MainFormSize);
            guardador.AddResource("MainFormViewSidebar", MainFormViewSidebar);
            guardador.AddResource("StreamString", StreamString);
            guardador.AddResource("StreamEnabled", StreamEnabled);
            guardador.Close();
        }
        public static Image GetIconoBandera(string Lang)
        {
            switch (Lang)
            {
                case "es":
                    return Properties.Resources.es;
                case "ca":
                    return Properties.Resources.ca;
                case "en":
                    return Properties.Resources.en;
                case "el":
                    return Properties.Resources.el;
                case "it":
                    return Properties.Resources.it;
            }
            return null;
        }
    }
}
