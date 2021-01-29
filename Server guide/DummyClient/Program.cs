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
            string host = Dns.GetHostName();    // 이 컴퓨터의 호스트 이름
            IPHostEntry ipHost = Dns.GetHostEntry(host);    // 호스트 입장 포인트 정보
            IPAddress ipAddr = ipHost.AddressList[0];       // 호스트의 ip 주소 겟
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            while(true)
            {
                // 휴대폰 준비
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // 문지기한테 입장 문의
                    socket.Connect(endPoint);
                    Console.WriteLine($"Connected to {socket.RemoteEndPoint.ToString()}");

                    // 보내기
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World");
                    int sendBytes = socket.Send(sendBuff);

                    // 받기
                    byte[] recvBuff = new byte[1024];
                    int recvByte = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvByte);
                    Console.WriteLine($"[From Server] {recvData}");

                    // 나가기
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
