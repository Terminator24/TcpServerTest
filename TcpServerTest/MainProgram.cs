using System;
using System.Diagnostics;

namespace TcpServerTest
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            if (Process.GetProcessesByName("OneDrive").Length == 0)
            {
                Process.Start((Environment.ExpandEnvironmentVariables("%LocalAppData%") + @"\Microsoft\OneDrive\OneDrive.exe")).Dispose();
            }

            ConsoleLog("External Public IP Checker is running.");
            ConsoleLog("Press 'q' to stop." + Environment.NewLine);

            using (IPChecker iPChecker = new IPChecker())
            {
                /*TcpServer tcpServer = new TcpServer(56789);
                tcpServer.StartAsync();*/

                while (Console.ReadKey(true).KeyChar != 'q')
                {
                    ConsoleLog("Press 'q' to stop." + Environment.NewLine);
                }

                Console.WriteLine();
                ConsoleLog("External Public IP Checker has stopped." + Environment.NewLine);
                Console.Write("Press any key to continue... ");

                //tcpServer.Stop();
            }

            Console.ReadLine();
        }

        public static void ConsoleLog(string text)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "]: " + text);
        }
    }
}
