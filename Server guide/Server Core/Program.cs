using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_Core
{
    class Program
    {
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                // 요청 사항 받기
                byte[] recvBuff = new byte[1024];
                int recvBytes = clientSocket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Client] {recvData}");

                // 보내기
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome!");
                clientSocket.Send(sendBuff);

                // 내보내기
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();    // 이 컴퓨터의 호스트 이름
            IPHostEntry ipHost = Dns.GetHostEntry(host);    // 호스트 입장 포인트 정보
            IPAddress ipAddr = ipHost.AddressList[0];       // 호스트의 ip 주소 겟
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
             
            _listener.Init(endPoint, OnAcceptHandler);

            while (true)
            {
                ;
            }
        }
    }
}
