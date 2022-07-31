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
using System.Linq;
using System.Threading.Tasks;

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
            //Check if console is needed
            if (args.Contains("-consola") || args.Contains("-console"))
                Kernel.Console = true;
            int console;
            if (Kernel.Console)
            {
                console = Kernel.AllocConsole();
                Console.CancelKeyPress += (sender, args) => Kernel.Quit();
            }
            /*LOADING PROCESS*/
            Log.InitLog();
            Log.Instance.PrintMessage("Starting...", MessageType.Info);
            //Set exit event
            Application.ApplicationExit += (sender, args) => Kernel.Quit();
            Log.Instance.PrintMessage("Parsing arguments... (" + args.Length + " args)", MessageType.Info);
            //Checking arguments
            Kernel.ParseArgs(args);
#if DEBUG
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
#endif

            Stopwatch StartStopwatch = Stopwatch.StartNew();
            //Loading languages, else it will not load the textbox.
            Log.Instance.PrintMessage("Loading language file...", MessageType.Info);
            Kernel.LoadLanguages();
            
            //Load configuration
            Log.Instance.PrintMessage("Loading config file...", MessageType.Info);
            Kernel.LoadConfig();
            
            if (!Kernel.MetadataStream)
            {
                //Create genres
                Kernel.InitGenres();
                //Create program
                Kernel.InitProgram();
                //Load the files
                Kernel.LoadFiles();
                //Init Spotify
                //Main form does that for us
                //Create player Instance
                Kernel.InitPlayer();
            }
            //else
            //    Task.Run(() => Kernel.InitSpotify());
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
        }
    }
}