﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using UnityEngine;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            //Console.WriteLine($"OnConnected : {endPoint}");
            Debug.Log($"OnConnected : {endPoint}");

            ServerLog._instance.WriteLog(endPoint);
            ServerManager.Room.Push(() => ServerManager.Room.Enter(this));
            //Program.Room.Push(() => Program.Room.Enter(this));
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);

            if (Room != null)
            {
                GameRoom room = Room;

                room.Push(() => room.Leave(this));
                Room = null;
            }

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
