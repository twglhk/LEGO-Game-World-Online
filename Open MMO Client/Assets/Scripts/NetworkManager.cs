using DummyClient;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    // Start is called before the first frame update
    void Start()
    {
        // DNS (Domain Name System)
        string host = Dns.GetHostName();                // Host Name
        IPHostEntry ipHost = Dns.GetHostEntry(host);    // Host Entry
        IPAddress ipAddr = ipHost.AddressList[0];       // Host IP Address
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();
        connector.Connect(endPoint,
            () => { return _session; }, 1);
    }

    // Update is called once per frame
    void Update()
    {
        IPacket packet = PacketQueue.Instance.Pop();
        Debug.Log("Update");
        if (packet != null)
        {
            Debug.Log("Packet!");
            PacketManager.Instance.HandlePacket(_session, packet);
        }
    }
}
