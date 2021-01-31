using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();                // Host Name
            IPHostEntry ipHost = Dns.GetHostEntry(host);    // Host Entry
            IPAddress ipAddr = ipHost.AddressList[0];       // Host IP Address
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            while(true)
            {
                // Socket Ready
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Contact socket for entry
                    socket.Connect(endPoint);
                    Console.WriteLine($"Connected to {socket.RemoteEndPoint.ToString()}");

                    // Send
                    for (int i = 0; i < 5; ++i)
                    {
                        byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello World {i}");
                        int sendBytes = socket.Send(sendBuff);
                    }

                    // Recv
                    byte[] recvBuff = new byte[1024];
                    int recvByte = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvByte);
                    Console.WriteLine($"[From Server] {recvData}");

                    // Disconnect
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }
        }
    }
}
