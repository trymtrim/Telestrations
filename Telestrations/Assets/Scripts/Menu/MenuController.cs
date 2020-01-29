using Mirror;
using Telestrations.GameBrowser;
using Telestrations.Server;
using UnityEngine;

namespace Telestrations.Menu
{
    public class MenuController : MonoBehaviour
    {
        private GameBrowserController _gameBrowserController;

        private void Awake()
        {
            _gameBrowserController = new GameBrowserController();
        }

        private void Start()
        {
            ServerHelper.IsHost = false;
        }

        public void CreateGame()
        {
            ServerHelper.IsHost = true;

            NetworkManager networkManager = NetworkManager.singleton;

            networkManager.networkAddress = ServerHelper.GetLocalIPAddress();
            networkManager.StartHost();
        }

        private void OnDestroy()
        {
            _gameBrowserController.Dispose();
        }
    }
}
