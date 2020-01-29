using Telestrations.Server;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameBrowserButtonController : MonoBehaviour
{
    public string gameIpAndPort;

    public void Initialize(string gameName, string gameIpAndPort)
    {
        this.gameIpAndPort = gameIpAndPort;

        Button button = GetComponent<Button>();

        button.onClick.AddListener(JoinGame);
        button.GetComponentInChildren<Text>().text = gameName;
    }

    private void JoinGame()
    {
        Client.ClientConnection.ConnectToServer(gameIpAndPort);
    }
}
