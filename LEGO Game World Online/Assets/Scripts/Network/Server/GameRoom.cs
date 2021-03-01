using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
            {
                s.Send(_pendingList);
            }

            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            // Chat ver
            //S_Chat packet = new S_Chat();
            //packet.playerId = session.SessionId;
            //packet.chat = $"{chat}. I am {packet.playerId} ";
            //ArraySegment<byte> segment = packet.Write();

            _pendingList.Add(segment);
        }

        public void Enter(ClientSession session)
        {
            // Add the player
            _sessions.Add(session);
            session.Room = this;

            // Send player list to entered player
            S_PlayerList playerList = new S_PlayerList();
            foreach(var clientSession in _sessions)
            {
                playerList.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (clientSession == session),
                    playerId = clientSession.SessionId,
                    posX = clientSession.PosX,
                    posY = clientSession.PosY,
                    posZ = clientSession.PosZ
                });
            }
            session.Send(playerList.Write());

            // Send other players the new player entered 
            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;
            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            // Remove the player from list
            _sessions.Remove(session);

            // Send other players the player was leaved
            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerId = session.SessionId;
            Broadcast(leave.Write());
        }

        public void Move(ClientSession session, C_Move packet)
        {
            // Change position
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;

            // Send moved position to players
            S_BroadcastMove move = new S_BroadcastMove();
            move.playerId = session.SessionId;
            move.posX = session.PosX;
            move.posY = session.PosY;
            move.posZ = session.PosZ;
            Broadcast(move.Write());
        }
    }
}
