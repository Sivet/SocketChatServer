using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatServer
{
    
    class SocketChatServer
    {
        private IPAddress ip;
        private Socket client;

        private int port;
        private volatile bool stop = false;
        private ChatService chat = new ChatService();

        public SocketChatServer(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void Run()
        {
            //skaber og starter en listener
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            while (!stop)
            {
                //venter på at der kommer en client
                //når der gør acceptere den
                client = listener.AcceptSocket();

                //starter en thread til den nye client og sender acceptet fra listeneren med som en socket
                ClientHandler ch = new ClientHandler(client, chat);
                new Thread(ch.Run).Start();
            }
        }
    }
}
