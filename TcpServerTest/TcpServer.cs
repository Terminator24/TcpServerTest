using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TcpServerTest
{
    public class TcpServer
    {
        private TcpListener tcpListener;

        public TcpServer(int port)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public async void StartAsync()
        {
            try
            {
                tcpListener.Start();

                while (true)
                {
                    ProcessClientAsync(await tcpListener.AcceptTcpClientAsync());
                }
            }
            catch (Exception exception)
            {
                if (!(exception is InvalidOperationException))
                {
                    MainProgram.ConsoleLog("Server: Error: " + exception.Message);
                }
            }
        }

        private async void ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                StreamReader streamReader = new StreamReader(tcpClient.GetStream());
                StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream());
                streamWriter.AutoFlush = true;

                if (Authenticate(await streamReader.ReadLineAsync()))
                {
                    await streamWriter.WriteLineAsync("Authenticated.");

                    while (true)
                    {
                        string request = await streamReader.ReadLineAsync();

                        if (request != null)
                        {
                            MainProgram.ConsoleLog("Server: Received service request: " + request);
                            string response = GetResponse(request);

                            MainProgram.ConsoleLog("Server: Computed response is: " + response);
                            await streamWriter.WriteLineAsync(response);
                        }
                        else
                        {
                            // Client closed connection
                            break;
                        }
                    }
                }

                tcpClient.Close();
            }
            catch (Exception exception)
            {
                MainProgram.ConsoleLog("Server: Error: " + exception.Message);

                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }
            finally
            {
                tcpClient.Dispose();
            }
        }

        private bool Authenticate(string request)
        {
            return true;
        }

        private string GetResponse(string request)
        {
            return ("Risposta.");
        }

        public void Stop()
        {
            tcpListener.Stop();
        }
    }
}

