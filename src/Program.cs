﻿using System;
using System.Windows.Forms;
using System.Diagnostics;


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

            /*LOADING PROCESS*/
            Log.Instance.PrintMessage("Starting...", MessageType.Info);

           

            Stopwatch stopwatch = Stopwatch.StartNew();
            //Checking arguments
            Kernel.ParseArgs(args);
            //First of all... check updates
            Kernel.CheckForUpdates();
            //Load configuration
            Kernel.LoadConfig();
            //Loading languages
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
            stopwatch.Stop();
            Log.Instance.PrintMessage("Application loaded!", MessageType.Correct, stopwatch, TimeType.Milliseconds);
            //ApplicationStart
            Kernel.StartApplication();
            //Program halts here untill Application.Exit is called.
            Kernel.Quit();
        }
    }
}