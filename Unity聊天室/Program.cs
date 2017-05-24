using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Unity聊天室
{
    class Program
    {
        static List<Client> clientList = new List<Client>();
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
       public  static void BroadcastMessage(string message)
        {
            List<Client> notConnectedList = new List<Client>();
            foreach(Client client in clientList)
            {
                if(client.Connected)
                {
                    client.SendMessage(message);
                }            
                else
                {
                    notConnectedList.Add(client);
                }
            }
            foreach (var client in notConnectedList)
            {
                clientList.Remove(client);
            }
        }

        static void Main(string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip =IPAddress.Parse("10.130.133.61");
            EndPoint point = new IPEndPoint(ip, 7788);
            tcpServer.Bind(point);
            tcpServer.Listen(100);
            Console.WriteLine("server is running");
            while(true)
            {
                Socket clientSocket = tcpServer.Accept();
                Client client = new Client(clientSocket);
                clientList.Add(client);
                Console.WriteLine("a client is connected!");
            }
            

        }
    }
}
