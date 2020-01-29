using Mirror;
using UnityEngine;

namespace Telestrations.Lobby
{
    public class LobbyController : MonoBehaviour
    {
        private NetworkRoomManager _networkManager;

        private void Start()
        {
            _networkManager = (NetworkRoomManager) NetworkManager.singleton;
        }

        public void StartGame()
        {
            _networkManager.ServerChangeScene(_networkManager.GameplayScene);
        }

        public void LeaveLobby()
        {
            _networkManager.StopHost();
        }
    }
}
