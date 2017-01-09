using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChatClient
{
    class ConnectionController
    {
        TcpClient server;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;

        private string ip;
        private int port;

        private volatile bool stop = false;

        public delegate void AddCompletedEventType(string msg);
        public event AddCompletedEventType AddCompletedEvent;

        public ConnectionController(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void Conect()
        {
            server = new TcpClient(ip, port);
            stream = server.GetStream();

            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            
        }
        public void sendToServer(string text)
        {
            writer.WriteLine(text);
            writer.Flush();
        }
        public void StartRecieveFromServerThread()
        {
            new System.Threading.Thread(() =>
            {
                while (!stop)
                {
                    if (AddCompletedEvent != null)
                        AddCompletedEvent(receiveFromServer());
                }
            }).Start();
        }
        
        public string receiveFromServer()
        {
            try
            {
                return reader.ReadLine();
            }
            catch
            {
                return null;
            }
        }
        public void Close()
        {
            stop = true;
            writer.Close();
            reader.Close();
            stream.Close();
            server.Close();
        }
    }
}
