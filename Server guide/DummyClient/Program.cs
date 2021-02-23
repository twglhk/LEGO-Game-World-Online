using ServerCore;
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

            Connector connector = new Connector();
            connector.Connect(endPoint, 
                () => { return SessionManager.Instance.Generate(); },
                10);

            while (true)
            {
                try
                {
                    SessionManager.Instance.SendForEach();  // Send Message to server
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(250);
            }
        }
    }
}
