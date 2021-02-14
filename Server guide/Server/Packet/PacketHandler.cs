using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// Handle function that will be called after receiving a packet is completed 
    /// </summary>
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;
            Console.WriteLine($"PlayerInfoRequest : {p.playerId}, {p.name}");

            foreach (PlayerInfoReq.Skill skill in p.skills)
            {
                Console.WriteLine($"skill({skill.id}) ({skill.level}) ({skill.duration})");
            }
        }
    }
}
