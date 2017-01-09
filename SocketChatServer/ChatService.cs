using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChatServer
{
    class ChatService
    {
        private List<string> LogListe = new List<string>();
        private List<StreamWriter> broadcastWriters = new List<StreamWriter>();

        
        public void AddToBroadcastList(StreamWriter stream)
        {
            broadcastWriters.Add(stream);
        }
        public void RemoveFromBroadcastList(StreamWriter stream)
        {
            broadcastWriters.Remove(stream);
        }
        public void GetChatlog(StreamWriter thisWriter)
        {
            Monitor.Enter(LogListe);
            try
            {
                foreach (string item in LogListe)
                {
                    thisWriter.WriteLine(item);
                    thisWriter.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Monitor.Exit(LogListe);
            }
            
        }
        public void BroadCast(string message, StreamWriter thisWriter)
        {
            Monitor.Enter(LogListe);
            try
            {
                LogListe.Add(message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Monitor.Exit(LogListe);
            }
            
            foreach (StreamWriter writer in broadcastWriters)
            {
                if (thisWriter != writer)
                {

                    writer.WriteLine("From: " + message);
                    writer.Flush();
                }

            }
        }
    }
}
