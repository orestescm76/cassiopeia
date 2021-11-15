using System;
using System.Windows.Forms;
using System.Diagnostics;

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

            //Load configuration
            Kernel.LoadConfig();
            //Loading languages, else it will not load the textbox.
            Kernel.LoadLanguages();
            //Create program
            Kernel.CreateProgram();
            //Init Spotify
            Kernel.InitSpotify();
            //Create genres
            Kernel.InitGenres();
            //Load the files
            Kernel.LoadFiles();
            //Create player Instance
            Kernel.InitPlayer();

            //We're done!
            StartStopwatch.Stop();
            Log.Instance.PrintMessage("Application loaded!", MessageType.Correct, StartStopwatch, TimeType.Milliseconds);

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