using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fleck;
using System;

namespace Telestrations.Server
{
    public class Server : IDisposable
    {
        public static Server ServerConnection;

        public string IpAndPort => $"{_ip}:{_port}";

        private string _ip = "192.168.1.116";
        private int _port = 9000;

        private WebSocketServer _server;

        public Server()
        {
            if (ServerConnection == null)
                ServerConnection = this;

            StartServer();
        }

        private void StartServer()
        {
            _server = new WebSocketServer($"ws://{IpAndPort}");

            //server.RestartAfterListenError = true;

            _server.Start(socket =>
            {
                socket.OnOpen = () => OnOpen(socket);
                socket.OnClose = () => OnClose(socket);
                socket.OnMessage = message => OnMessage(socket, message);
            });

            Debug.Log($"Server started [{_server.Location}]");
        }

        private void OnOpen(IWebSocketConnection socket)
        {
            string clientIpAddress = socket.ConnectionInfo.ClientIpAddress;
            Debug.Log($"Client connected [{clientIpAddress}]");            
        }

        private void OnClose(IWebSocketConnection socket)
        {
            string clientIpAddress = socket.ConnectionInfo.ClientIpAddress;
            Debug.Log($"Client discconnected [{clientIpAddress}]");
        }

        private void OnMessage(IWebSocketConnection socket, string message)
        {
            Debug.Log($"Server message received:\n{message}");
            //socket.Send(message);
        }

        public virtual void Dispose()
        {
            Debug.Log("Server stopped");

            _server.Dispose();
        }
    }
}
