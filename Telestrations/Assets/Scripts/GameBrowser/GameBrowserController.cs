using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telestrations.Server;
using System;

namespace Telestrations.GameBrowser
{
    //TODO: Might have to change to .net 3.5 for it to work on mobile

    public class GameBrowserController : MonoBehaviour
    {
        public Action<string, string> OnGameFound;
        public Action<string> OnGameRemoved;

        private ServerFinder _serverFinder;

        private List<string> _availableGameServers = new List<string>();

        private void Start()
        {
            _serverFinder = new ServerFinder();

            _serverFinder.OnGameServerFound += OnGameServerFound;
        }

        private void OnGameServerFound(string gameName, string gameServerIp)
        {
            if (!_availableGameServers.Contains(gameServerIp))
            {
                _availableGameServers.Add(gameServerIp);
                OnGameFound.Invoke(gameName, gameServerIp);
            }
        }

        private void OnGameServerRemoved(string gameServerIp)
        {
            if (_availableGameServers.Contains(gameServerIp))
            {
                _availableGameServers.Remove(gameServerIp);
                OnGameRemoved.Invoke(gameServerIp);
            }
        }

        public void LeaveGameBrowser()
        {
            Destroy(Client.ClientConnection.gameObject);

            SceneChanger.StaticChangeScene("StartScene");
        }

        private void OnDestroy()
        {
            _serverFinder.OnGameServerFound -= OnGameServerFound;
            _serverFinder.Dispose();
        }
    }
}
