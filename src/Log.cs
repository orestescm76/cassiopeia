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
        Milliseconds, Microseconds, Seconds
    }

    public class Log
    {
        public static Log Instance { get; private set; }

        private Stopwatch timeSinceStart;
        private StreamWriter file;
        private LogViewer logView;
        public static void InitLog()
        {
            Instance = new Log();
        }
        private Log()
        {
            timeSinceStart = Stopwatch.StartNew();
            logView = new LogViewer();
            file = new StreamWriter(Environment.CurrentDirectory + "\\log.txt", false);
            file.AutoFlush = true;
            logView = new LogViewer();
            if (file != null)
            {
                PrintInfo();
            }
        }
        private void PrintInfo()
        {
            PrintMessage("Cassiopeia - Music Manager " + Kernel.Version, MessageType.Info);
            PrintMessage(".NET Version: " + Environment.Version, MessageType.Info);
            PrintMessage("OS version: " + Environment.OSVersion.ToString(), MessageType.Info);
            PrintMessage("Log created on " + DateTime.Now, MessageType.Info);
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
                case TimeType.Seconds:
                    timeStamp = crono.ElapsedMilliseconds / 1000.0 + "s";
                    break;

                case TimeType.Milliseconds:
                    timeStamp = crono.ElapsedTicks / 10000.0 + "ms";
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
                    logMessage += "<OK> " + message;
                    break;

                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    logMessage += "<WARNING> " + message;
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