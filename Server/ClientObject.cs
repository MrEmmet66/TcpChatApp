using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class ClientObject
    {
        public string Name { get; set; }
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public TcpClient Client { get; set; }
        private NetworkStream stream;

        public ClientObject(TcpClient client)
        {
            Client = client;
            stream = client.GetStream();
            Reader  = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }
    }
}
