using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Telestrations.Lobby
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        public override void OnStartServer()
        {
            Debug.LogFormat("OnStartServer {0}", SceneManager.GetActiveScene().name);

            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            Transform playerContainer = GameObject.Find("Players").transform;
            transform.SetParent (playerContainer);
            transform.localScale = Vector3.one;

            Debug.LogFormat("OnStartClient {0}", SceneManager.GetActiveScene().name);

            base.OnStartClient();
        }

        public override void OnClientEnterRoom()
        {
            Debug.LogFormat("OnClientEnterRoom {0}", SceneManager.GetActiveScene().name);
        }

        public override void OnClientExitRoom()
        {
            Debug.LogFormat("OnClientExitRoom {0}", SceneManager.GetActiveScene().name);
        }
    }
}
