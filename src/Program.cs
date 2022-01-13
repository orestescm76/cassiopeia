/*
 * CASSIOPEIA 2.0.225.30
 * NET 5 PORT - OK
 * SPOTIFYAPI 6 PORT - OK
 * CODENAME STORM
 * MADE BY ORESTESCM76
 */

using System;
using System.Diagnostics;
using System.Windows.Forms;

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
            Kernel.ParseArgs(args);
            int console;
            if (Kernel.Console)
            {
                console = Kernel.AllocConsole();
                Console.CancelKeyPress += (sender, args) => Kernel.Quit();
            }
#if DEBUG
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
#endif
            Log.InitLog();
            /*LOADING PROCESS*/
            Log.Instance.PrintMessage("Starting...", MessageType.Info);

            Stopwatch StartStopwatch = Stopwatch.StartNew();
            //Checking arguments
            Log.Instance.PrintMessage("Loading config file...", MessageType.Info);
            //Load configuration
            Kernel.LoadConfig();
            //Loading languages, else it will not load the textbox.
            Log.Instance.PrintMessage("Loading language file...", MessageType.Info);
            Kernel.LoadLanguages();

            if (!Kernel.MetadataStream)
            {
                //Create genres
                Kernel.InitGenres();
                //Create program
                Kernel.CreateProgram();
                //Load the files
                Kernel.LoadFiles();
                //Init Spotify
                Kernel.InitSpotify();
                //Create player Instance
                Kernel.InitPlayer();
            }
            else
                Kernel.InitSpotify();
            //We're done!
            StartStopwatch.Stop();
            Log.Instance.PrintMessage("Application loaded!", MessageType.Correct, StartStopwatch, TimeType.Seconds);

            //Before everything... check updates
#if !DEBUG
            if(Kernel.CheckUpdates)
                Kernel.CheckForUpdates();
#endif
            //ApplicationStart
            Kernel.StartApplication();
            //Program halts here until Application.Exit is called.
            Kernel.Quit();
        }
    }
}