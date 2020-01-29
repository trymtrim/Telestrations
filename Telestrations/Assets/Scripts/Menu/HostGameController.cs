using UnityEngine;
using Telestrations.Server;

namespace Telestrations.Menu
{
    public class HostGameController : MonoBehaviour
    {
        [SerializeField] private Transform _hostUIContainer;

        private ServerBroadcaster _serverBroadcaster;

        private void Start()
        {
            if (!ServerHelper.IsHost)
            {
                Destroy(_hostUIContainer.gameObject);
                Destroy(gameObject);
                return;
            }

            _serverBroadcaster = new ServerBroadcaster();
        }

        private void OnDestroy()
        {
            if (_serverBroadcaster != null)
                _serverBroadcaster.Dispose();
        }
    }
}
