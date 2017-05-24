using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unity聊天室
{
    /// <summary>
    /// 用来和客户端做通信
    /// </summary>
    class Client
    {
        private Socket clientSocket;
        private Thread t;
        private byte[] data = new byte[1024];//数据容器
        public Client(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            //启动一个线程，处理客户端的数据接收
             t = new Thread(ReceiveMessage);
            t.Start();
        }
        void ReceiveMessage()
        {
            while(true)
            {
                if(clientSocket.Poll(10,SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    break;//跳出循环，终止线程
                }
                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);
                //将收到的消息广播到各个客户端
                Program.BroadcastMessage(message);
                Console.WriteLine(message);
            }
        }
        public void SendMessage(string message)
        {
            byte[] data2 = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data2);
        }
        public bool Connected
        {
            get { return clientSocket.Connected; }
        }
       
    }
}
