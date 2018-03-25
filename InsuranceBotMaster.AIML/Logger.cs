using System;

namespace InsuranceBotMaster.AIML
{
    public static class Logger
    {
        private static string fileToLog = Environment.CurrentDirectory + @"\log.txt";

        public static void Log(string messageToLog)
        {
            System.IO.File.WriteAllText(fileToLog, $"{DateTime.Now}: {messageToLog}");
        }
    }
}
