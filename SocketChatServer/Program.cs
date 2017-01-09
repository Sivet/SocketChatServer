using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketChatServer server = new SocketChatServer(IPAddress.Any, 11000);
            server.Run();
        }
    }
}
