using Server;
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

        Console.WriteLine($"{movePacket.posX} , {movePacket.posY} , {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Move(clientSession, movePacket));
    }
}

