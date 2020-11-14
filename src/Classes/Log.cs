using aplicacion_musica.src.Forms;
using System;
using System.Diagnostics;
using System.IO;
namespace aplicacion_musica
{
    public enum TipoMensaje
    {
        Info, Correcto, Advertencia, Error
    }
    public class Log
    {
        private static readonly Log instance = new Log();

        private Stopwatch CronometroTotal;
        private StreamWriter Fichero;
        private VisorLog VisorLog;
        private Log()
        {
            CronometroTotal = Stopwatch.StartNew();
            Fichero = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", false);
            Fichero.AutoFlush = true;
            VisorLog = new VisorLog();
            Fichero.WriteLine("Gestor de música " + Programa.version);
            Fichero.WriteLine("Versión de NET: " + Environment.Version);
            Fichero.WriteLine("Log creado el " + DateTime.Now);
        }
        public static Log Instance {get {return instance;} }
        public void ImprimirMensaje(string m, TipoMensaje tm)
        {
            switch (tm)
            {
                case TipoMensaje.Info:
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m);
                    break;
                case TipoMensaje.Correcto:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m);
                    break;
                case TipoMensaje.Advertencia:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m);
                    break;
                case TipoMensaje.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ERROR> " + m);
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ImprimirMensaje(string m, TipoMensaje tm, Stopwatch crono)
        {
            switch (tm)
            {
                case TipoMensaje.Info:
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    break;
                case TipoMensaje.Correcto:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    break;
                case TipoMensaje.Advertencia:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    break;
                case TipoMensaje.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10000 + "ms");
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ImprimirMensajeTiempoCorto(string m, TipoMensaje tm, Stopwatch crono)
        {
            switch (tm)
            {
                case TipoMensaje.Info:
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    break;
                case TipoMensaje.Correcto:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    break;
                case TipoMensaje.Advertencia:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ADVERTENCIA> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    break;
                case TipoMensaje.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    VisorLog.AddText(CronometroTotal.Elapsed + " : <ERROR> " + m + " en " + crono.ElapsedTicks / 10 + "μs");
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ImprimirMensaje(string m, TipoMensaje tm, string funcion)
        {
            switch (tm)
            {
                case TipoMensaje.Info:
                    Console.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    break;
                case TipoMensaje.Correcto:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : En " + funcion + " " + m);
                    break;
                case TipoMensaje.Advertencia:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " <ADVERTENCIA> " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " <ADVERTENCIA> " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : En " + funcion + " <ADVERTENCIA> " + m);
                    break;
                case TipoMensaje.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " <ERROR> " + m);
                    Fichero.WriteLine(CronometroTotal.Elapsed + " : En " + funcion + " <ERROR> " + m);
                    VisorLog.AddText(CronometroTotal.Elapsed + " : En " + funcion + " <ERROR> " + m);
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void MostrarLog()
        {
            VisorLog.Show();
        }
        public void CerrarLog()
        {
            Fichero.Close();
            VisorLog.Apagar();
        }
    }
}
