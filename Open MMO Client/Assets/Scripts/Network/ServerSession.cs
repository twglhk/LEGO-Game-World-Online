using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using System.Net.Sockets;
using System.Threading;

namespace DummyClient
{
    class ServerSession : PacketSession
    {
        //[System.Obsolete("Unsafe version for serialization")]
        //static unsafe void ToBytes(byte[] array, int offset, ulong value)
        //{
        //    fixed(byte* ptr = &array[offset])
        //        *(ulong*)ptr = value;
        //}

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
