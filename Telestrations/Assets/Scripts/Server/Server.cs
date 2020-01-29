using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fleck;
using System;
using System.Net;

namespace Telestrations.Server
{
    public class Server : IDisposable
    {
        public static Server ServerConnection;

        public string IpAndPort => $"{_ip}:{_port}";

        public Action<IWebSocketConnection, string> OnMessageFromClient = delegate { };
        public Action OnClientConnected = delegate { };
        public Action OnClientDisconnected = delegate { };

        private string _ip => GetLocalIPAddress(); //"192.168.1.116";
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
            OnClientConnected.Invoke();

            string clientIpAddress = socket.ConnectionInfo.ClientIpAddress;
            Debug.Log($"Client connected [{clientIpAddress}]");
        }

        private void OnClose(IWebSocketConnection socket)
        {
            OnClientDisconnected.Invoke();

            string clientIpAddress = socket.ConnectionInfo.ClientIpAddress;
            Debug.Log($"Client discconnected [{clientIpAddress}]");
        }

        private void OnMessage(IWebSocketConnection socket, string message)
        {
            OnMessageFromClient.Invoke(socket, message);

            Debug.Log($"Server message received:\n{message}");

            //socket.Send(message);
        }

        private static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }

            return "127.0.0.1";
        }

        public virtual void Dispose()
        {
            _server.Dispose();

            Debug.Log("Server stopped");
        }
    }
}
