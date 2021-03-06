﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Telestrations.Server
{
    public class ServerBroadcaster : IDisposable
    {
        private float _broadcastInterval = 1.0f;

        private bool _isActive = true;

        public ServerBroadcaster()
        {
            StartBroadcasting();
        }

        private async void StartBroadcasting()
        {
            Debug.Log("Started broadcasting server");

            while (_isActive)
            {
                Broadcast();
                await Task.Delay((int)(_broadcastInterval * 1000.0f));
            }
        }

        private void Broadcast()
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
            byte[] bytes = Encoding.ASCII.GetBytes(ServerHelper.GetLocalIPAddress());
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }

        public virtual void Dispose()
        {
            _isActive = false;

            Debug.Log("Stopped broadcasting server");
        }
    }
}
