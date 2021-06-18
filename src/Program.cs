using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;


/* VERSION 1.7.xx CODENAME STORM
 * Traspaso a Net 5.
*/

namespace Cassiopeia
{
    static class Program
    {
        
        
        
        public static bool ModoOscuro = false;
        
       
        

        [STAThread]
        static void Main(String[] args)
        {
            //preparar la aplicación para que ejecute formularios y demás.
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Creación log.
            Log Log = Log.Instance;

            /*LOADING PROCESS*/
            Log.Instance.PrintMessage("Starting...", MessageType.Info);
            //Load configuration
            Kernel.LoadConfig();
            //Checking arguments
            Kernel.ParseArgs(args);
            //Loading languages
            Kernel.LoadLanguages();
            //Check updates
            Kernel.CheckForUpdates();
            //Create program
            Kernel.CreateProgram();
            //Init Spotify
            Kernel.InitSpotify();

            if(!Kernel.MetadataStream)
            {
                Kernel.InitGenres();

                if (Kernel.JSON)
                    Kernel.LoadAlbums("discos.json");
                else
                {
                    if (File.Exists("discos.csv"))
                    {
                        Kernel.LoadCSVAlbums("discos.csv");
                        Kernel.LoadCD();
                    }
                    else
                    {
                        Log.PrintMessage("discos.csv does not exist, a new database will be created...", MessageType.Warning);
                    }
                }
                if (File.Exists("paths.txt"))
                    Kernel.LoadPATHS();
                if (File.Exists("lyrics.txt"))
                    Kernel.LoadLyrics();
            }
            //Create player Instance
            Kernel.InitPlayer();

            //ApplicationStart
            Kernel.StartApplication();
            //Program halts here untill Application.Exit is called.
            Kernel.Quit();
        }
    }
}
