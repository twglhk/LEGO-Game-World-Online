using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    interface ITask
    {
        void Execute();
    }

    [System.Obsolete("Manual jobqueue ver")]
    class BroadcastTask : ITask
    {
        GameRoom _room;
        ClientSession _session;
        string _chat;

        BroadcastTask(GameRoom room, ClientSession session, string chat)
        {
            _room = room;
            _session = session;
            _chat = chat;
        }
        public void Execute()
        {
            //_room.Broadcast(_session, _chat);
        }
    }

    class TaskQueue
    {
        Queue<ITask> _queue = new Queue<ITask>();

    }
}
