using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Handle function that will be called after receiving a packet is completed 
/// </summary>
class PacketHandler
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;
        Console.WriteLine($"PlayerInfoRequest : {p.playerId}, {p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"skill({skill.id}) ({skill.level}) ({skill.duration})");
        }
    }
}

