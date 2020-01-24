using UnityEngine;
using Telestrations.Server;

public class CreateGameController : MonoBehaviour
{
    private ServerBroadcaster _serverBroadcaster;

    private void Start()
    {
        new Server();

        _serverBroadcaster = new ServerBroadcaster();
    }

    private void OnDestroy()
    {
        Server.ServerConnection.Dispose(); //Temp
        _serverBroadcaster.Dispose();
    }
}
