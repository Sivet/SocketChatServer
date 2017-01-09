using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChatServer
{
    class ClientHandler
    {
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        ChatService chat;

        private Socket client;
        
        public ClientHandler(Socket client, ChatService chat)
        {
            //tager den socket den får fra serveren
            this.client = client;
            this.chat = chat;
        }

        public void Run()
        {
            stream = new NetworkStream(client);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            
            DoChat();


            writer.Close();
            reader.Close();
            stream.Close();
            client.Close();
        }
        public void DoChat()
        {
            try
            {
                chat.AddToBroadcastList(writer);

                writer.WriteLine("Connected");
                writer.Flush();

                chat.GetChatlog(writer);

                //kører metoden i et while loop så længe metoden returner true
                while (ChatCommands());

            }
            catch
            {
                //Stuff
            }
            finally
            {
                chat.RemoveFromBroadcastList(writer);
            }
        }
        private bool ChatCommands()
        {
            //sætter det input jeg får fra clienter til en string
            string input = recieveFromClient();

            //sender inputet til alle andre clients end den der sendte
            chat.BroadCast(input, writer);

            return true;
        }
        private string recieveFromClient()
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
    }
}
