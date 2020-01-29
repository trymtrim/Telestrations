using Mirror;
using Telestrations.Server;
using UnityEngine;
using UnityEngine.UI;

namespace Telestrations.GameBrowser
{
    [RequireComponent(typeof(Button))]
    public class GameBrowserButtonController : MonoBehaviour
    {
        public string GameIp;

        public void Initialize(string gameName, string gameIp)
        {
            GameIp = gameIp;

            Button button = GetComponent<Button>();

            button.onClick.AddListener(JoinGame);
            button.GetComponentInChildren<Text>().text = gameName;
        }

        private void JoinGame()
        {
            NetworkManager networkManager = NetworkManager.singleton;

            networkManager.networkAddress = GameIp;
            networkManager.StartClient();
        }
    }
}
