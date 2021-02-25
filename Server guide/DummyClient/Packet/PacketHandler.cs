using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Handle function that will be called after receiving a packet is completed 
/// </summary>
class PacketHandler
{
    // Chat ver
    //public static void S_ChatHandler(PacketSession session, IPacket packet)
    //{
    //    S_Chat chatPacket = packet as S_Chat;
    //    ServerSession serverSession = session as ServerSession;


    //    // Chat Test
    //    //if (chatPacket.playerId == 1)
    //    //    Console.WriteLine(chatPacket.chat);
    //}

    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
    }
}
