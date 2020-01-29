using System.Collections.Generic;
using Telestrations.Server;
using System;

namespace Telestrations.GameBrowser
{
    //TODO: Might have to change to .net 3.5 for it to work on mobile (server creation)

    public class GameBrowserController : IDisposable
    {
        public static GameBrowserController Singleton;

        public Action<string, string> OnGameFound;
        public Action<string> OnGameRemoved;

        private ServerFinder _serverFinder;

        private List<string> _availableGameServers = new List<string>();

        public GameBrowserController()
        {
            Singleton = this;

            _serverFinder = new ServerFinder();

            _serverFinder.OnGameServerFound += OnGameServerFound;
            _serverFinder.OnGameServerRemoved += OnGameServerRemoved;
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

        public void Dispose()
        {
            _serverFinder.OnGameServerFound -= OnGameServerFound;
            _serverFinder.Dispose();
        }
    }
}
