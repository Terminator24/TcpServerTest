using System;
using System.IO;
using System.Net;
using System.Timers;

namespace TcpServerTest
{
    internal class IPChecker : IDisposable
    {
        private Timer timer;
        private string lastIP;

        public IPChecker()
        {
            timer = new Timer(10000);
            lastIP = File.ReadAllText((Environment.ExpandEnvironmentVariables("%OneDrive%") + @"\IP.txt"));

            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                string currentIP = webClient.DownloadString("https://api.ipify.org/");

                if (lastIP != currentIP)
                {
                    MainProgram.ConsoleLog("Diversi: " + currentIP);

                    File.WriteAllText((Environment.ExpandEnvironmentVariables("%OneDrive%") + @"\IP.txt"), currentIP);

                    lastIP = currentIP;
                }
                else
                {
                    MainProgram.ConsoleLog("Uguali: " + currentIP);
                }
            }
        }

        #region DISPOSE
        private bool disposedValue = false;

        public void Dispose()
        {
            if (!disposedValue)
            {
                disposedValue = true;

                timer.Stop();
                timer.Dispose();
            }
        }
        #endregion
    }
}
