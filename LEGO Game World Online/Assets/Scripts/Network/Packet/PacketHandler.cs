using DummyClient;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

    // Chat ver
    //public static void C_ChatHandler(PacketSession session, IPacket packet)
    //{
    //    C_Chat chatPacket = packet as C_Chat;
    //    ClientSession clientSession = session as ClientSession;

    //    if (clientSession.Room == null)
    //        return;

    //    GameRoom room = clientSession.Room;
    //    room.Push(()=> room.Broadcast(clientSession, chatPacket.chat));
    //    //clientSession.Room.Broadcast(clientSession, chatPacket.chat);
    //}

    public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
        PlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
        PlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
        PlayerManager.Instance.Add(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;

        // TO DO : handle in Unity client
        PlayerManager.Instance.Move(pkt);
    }

    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
    }

    public static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        //Console.WriteLine($"{movePacket.posX} , {movePacket.posY} , {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Move(clientSession, movePacket));
    }
}
