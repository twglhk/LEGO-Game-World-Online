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
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();    // 이 컴퓨터의 호스트 이름
            IPHostEntry ipHost = Dns.GetHostEntry(host);    // 호스트 입장 포인트 정보
            IPAddress ipAddr = ipHost.AddressList[0];       // 호스트의 ip 주소 겟
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 문지기 교육
                listenSocket.Bind(endPoint);

                // 영업 시작
                // backlog : 최대 대기 수
                listenSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("손님 입장 대기 중");

                    // 손님 입장 (대리인 생성)
                    Socket clientSocket = listenSocket.Accept();

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
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
