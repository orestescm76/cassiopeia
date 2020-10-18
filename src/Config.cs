using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicacion_musica
{
    //Clase para temas de configuración
    public static class Config
    {
        private static ExeConfigurationFileMap ConfigFileMap = new ExeConfigurationFileMap();
        public static Configuration Configuration;
        public static string Idioma;
        public static string TipografiaLyrics;
        public static bool VinculadoConSpotify;
        public static string UltimoDirectorioAbierto;
        public static void CargarConfiguracion()
        {
            ConfigFileMap.ExeConfigFilename = Environment.CurrentDirectory + "/aplicacion_musica.exe.config";
            Configuration = ConfigurationManager.OpenMappedExeConfiguration(ConfigFileMap, ConfigurationUserLevel.None);
            UltimoDirectorioAbierto = ConfigurationManager.AppSettings["UltimoDirectorioAbierto"];
            Idioma = ConfigurationManager.AppSettings["Idioma"];
            VinculadoConSpotify = Convert.ToBoolean(Configuration.AppSettings.Settings["VinculadoConSpotify"].Value);
            TipografiaLyrics = ConfigurationManager.AppSettings["TipografiaLyrics"];
        }
        public static void GuardarConfiguracion()
        {
            Configuration.AppSettings.Settings["Idioma"].Value = Idioma;
            Configuration.AppSettings.Settings["TipografiaLyrics"].Value = TipografiaLyrics;
            Configuration.AppSettings.Settings["UltimoDirectorioAbierto"].Value = UltimoDirectorioAbierto;
            Configuration.Save();
        }
    }

}
