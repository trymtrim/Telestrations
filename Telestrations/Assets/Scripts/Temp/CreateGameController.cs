using UnityEngine;
using Telestrations.Server;

public class CreateGameController : MonoBehaviour
{
    private Server _serverConnection;
    private ServerBroadcaster _serverBroadcaster;

    private void Start()
    {
        _serverConnection = new Server();
        _serverBroadcaster = new ServerBroadcaster();
    }

    private void OnDestroy()
    {
        _serverConnection.Dispose(); //Temp
        _serverBroadcaster.Dispose();
    }
}
