using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using System.Net.Sockets;
using System.Threading;

namespace DummyClient
{
    class ServerSession : Session
    {
        [System.Obsolete("Unsafe version for serialization")]
        static unsafe void ToBytes(byte[] array, int offset, ulong value)
        {
            fixed(byte* ptr = &array[offset])
                *(ulong*)ptr = value;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            C_PlayerInfoReq packet = new C_PlayerInfoReq() { 
                playerId = 1001,
                name = "HUMBA"};
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 101, level = 1, duration = 3.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 201, level = 1, duration = 2.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 301, level = 1, duration = 1.0f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 401, level = 1, duration = 0.0f });

            // Send
            ArraySegment<byte> s = packet.Write();

            if (s != null)
                Send(s);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");

            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
