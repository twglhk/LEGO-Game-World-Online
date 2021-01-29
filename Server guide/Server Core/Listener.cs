using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_Core
{
    class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHandler;

        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;

            // 문지기 교육
            _listenSocket.Bind(endPoint);

            // 영업 시작
            // backlog : 최대 대기 수
            _listenSocket.Listen(10);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 하나 만들어서 재활용 가능, 
                                                                    // SocketAsync의 Event와 관련된 정보를 저장
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }

        /// <summary>
        /// socket의 Accept를 async로 실행하는 함수
        /// </summary>
        /// <param name="args"></param>
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 기존 args의 데이터를 제거해야함.
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        /// <summary>
        /// Socket의 AcceptAsync가 완료되었을 때 callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // TO DO 
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }

            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);   // AcceptAsync 반복.
        }

        [System.Obsolete]
        public Socket Accept()
        {
            // 여기서 Socket의 Accpet는 blocking 계열의 함수. 게임에서는 사용 X
            return _listenSocket.Accept();
        }
    }
}
