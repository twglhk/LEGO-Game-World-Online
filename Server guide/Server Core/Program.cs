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
                // 세션 생성 후 초기화
                Session session = new Session();
                session.Start(clientSocket);

                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome!");
                session.Send(sendBuff);

                Thread.Sleep(1000);

                session.Disconnect();
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
