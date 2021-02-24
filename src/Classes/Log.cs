using Cassiopeia.src.Forms;
using System;
using System.Diagnostics;
using System.IO;

namespace Cassiopeia
{
    public enum MessageType
    {
        Info, Correct, Warning, Error
    }

    public enum TimeType
    {
        Miliseconds, Microseconds
    }

    public class Log
    {
        private static readonly Log instance = new Log();
        public static Log Instance { get => instance; }

        private Stopwatch timeSinceStart;
        private StreamWriter file;
        private VisorLog logView;

        private Log()
        {
            timeSinceStart = Stopwatch.StartNew();
            logView = new VisorLog();

            file = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", false)
            {
                AutoFlush = true
            };

            if (file != null)
            {
                file.WriteLine("Cassiopeia - Music Manager " + Program.Version);
                file.WriteLine(".NET Version: " + Environment.Version);
                file.WriteLine("Log created on " + DateTime.Now);
            }
        }

        public void ShowLog()
        {
            logView.Show();
        }

        public void CloseLog()
        {
            file.Close();
            logView.Dispose();
        }

        public void PrintMessage(string message, MessageType messageType)
        {
            ProcessLogMessage(message, messageType);
        }

        public void PrintMessage(string message, MessageType messageType, Stopwatch crono, TimeType timeType)
        {
            string timeStamp = "";

            switch (timeType)
            {
                case TimeType.Miliseconds:
                    timeStamp = crono.ElapsedTicks / 10000 + "ms";
                    break;

                case TimeType.Microseconds:
                    timeStamp = crono.ElapsedTicks / 10 + "μs";
                    break;

                default:
                    break;
            }

            string messageToProcess = message + " (in " + timeStamp + ")";
            ProcessLogMessage(messageToProcess, messageType);
        }

        public void PrintMessage(string message, MessageType messageType, string functionName)
        {
            string messageToProcess = "In function: " + functionName + " - " + message;
            ProcessLogMessage(messageToProcess, messageType);            
        }

        private void ProcessLogMessage(string message, MessageType messageType)
        {
            string logMessage = timeSinceStart.Elapsed + " : ";

            switch (messageType)
            {
                case MessageType.Info:
                    logMessage += message;
                    break;

                case MessageType.Correct:
                    Console.ForegroundColor = ConsoleColor.Green;
                    logMessage += message;
                    break;

                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    logMessage += "<ADVERTENCIA> " + message;
                    break;

                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    logMessage += "<ERROR> " + message;
                    break;

                default:
                    break;
            }

            Console.WriteLine(logMessage);
            file.WriteLine(logMessage);
            logView.AddText(logMessage);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}