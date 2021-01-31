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
                // session creation and initialization
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
            string host = Dns.GetHostName();    
            IPHostEntry ipHost = Dns.GetHostEntry(host);    
            IPAddress ipAddr = ipHost.AddressList[0];       
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
             
            _listener.Init(endPoint, OnAcceptHandler);

            while (true)
            {
                ;
            }
        }
    }
}
