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
    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;
        Console.WriteLine($"PlayerInfoRequest : {p.playerId}, {p.name}");

        if (clientSession.Room == null)
            return;

        clientSession.Room.Broadcast(clientSession, chatPacket.chat);
    }
}

