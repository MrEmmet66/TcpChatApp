using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Server
{
    internal class Program
    {
        static List<ClientObject> clients = new List<ClientObject>();
        static List<Message> messageHistory = new List<Message>(); 
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 6666);
            try
            {
                server.Start();
                Console.WriteLine($"Server started on {server.LocalEndpoint}");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    string name = clientObject.Reader.ReadLine();
                    clientObject.Name = name;
                    clients.Add(clientObject);
                    Console.WriteLine($"{name} connected");
                    Task.Run(() => MessageListen(clientObject));
                    SendMessageToClients(JsonConvert.SerializeObject(new Message("SERVER", $"{clientObject.Name} вошел в чат")));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SendMessageToClients(string jsonMsg)
        {
            foreach(ClientObject client in clients)
            {
                client.Writer.WriteLine(jsonMsg);
                client.Writer.Flush();
            }
        }

        static void MessageListen(ClientObject client)
        {
            try
            {
                while (true)
                {
                    string message = client.Reader.ReadLine();
                    Message msg = new Message(client.Name, message);
                    Console.WriteLine($"[{msg.TimeStamp}] Got message from {msg.SenderName} | Content: {msg.Content}");
                    string response = JsonConvert.SerializeObject(msg);
                    SendMessageToClients(response);
                }
            }
            finally
            {
                clients.Remove(client);
                Console.WriteLine($"{client.Name} leaved");
                SendMessageToClients(JsonConvert.SerializeObject(new Message("SERVER", $"{client.Name} покинул чат")));
            }

        }
    }
}