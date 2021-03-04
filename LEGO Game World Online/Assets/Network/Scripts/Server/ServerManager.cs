using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerCore;
using System.Net;
using Client;


namespace Server
{
    public class ServerManager : MonoBehaviour
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();   // Test room

        [SerializeField] private GameObject clientNetworkManagerPrefab;

        static void FlushRoom()
        {
            Room.Push(() => Room.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            JobTimer.Instance.Flush();
        }

        public void Hosting()
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // TO DO : Send host info to other clients

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });

            if (clientNetworkManagerPrefab == null)
                Debug.LogError($"{clientNetworkManagerPrefab} is not set");

            var async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            async.completed += (AsyncOperation asyncOperation) => { 
                // Create Host's server session
                PacketManager.Instance.IsHost = true;
                Instantiate(clientNetworkManagerPrefab);
            };

            JobTimer.Instance.Push(FlushRoom);
        }
    }
}