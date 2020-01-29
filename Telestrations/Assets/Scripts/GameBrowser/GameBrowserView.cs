using UnityEngine;

namespace Telestrations.GameBrowser
{
    public class GameBrowserView : MonoBehaviour
    {
        [Header("Scene Objects")]
        [SerializeField] private GameBrowserController _gameBrowserController;

        [Header("Prefabs")]
        [SerializeField] private GameObject _gameBrowserButtonPrefab;

        private void Start()
        {
            _gameBrowserController = GameBrowserController.Singleton;

            _gameBrowserController.OnGameFound += OnGameFound;
            _gameBrowserController.OnGameRemoved += OnGameRemoved;
        }

        private void OnGameFound(string gameName, string gameIp)
        {
            GameBrowserButtonController buttonController = Instantiate(_gameBrowserButtonPrefab, transform).GetComponent<GameBrowserButtonController>();
            buttonController.Initialize(gameName, gameIp);
        }

        private void OnGameRemoved(string gameIp)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameBrowserButtonController buttonController = transform.GetChild(i).GetComponent<GameBrowserButtonController>();

                if (buttonController.GameIp == gameIp)
                {
                    Destroy(buttonController.gameObject);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            _gameBrowserController.OnGameFound -= OnGameFound;
            _gameBrowserController.OnGameRemoved -= OnGameRemoved;
        }
    }
}
