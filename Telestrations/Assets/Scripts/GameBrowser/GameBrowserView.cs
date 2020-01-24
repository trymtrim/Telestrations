using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telestrations.GameBrowser;

public class GameBrowserView : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] private GameBrowserController _gameBrowserController;

    [Header("Prefabs")]
    [SerializeField] private GameObject _gameBrowserButtonPrefab;

    private void Awake()
    {
        _gameBrowserController.OnGameFound += OnGameFound;
        _gameBrowserController.OnGameRemoved += OnGameRemoved;
    }

    private void OnGameFound(string gameName, string gameIpAndPort)
    {
        GameBrowserButtonController buttonController = Instantiate(_gameBrowserButtonPrefab, transform).GetComponent<GameBrowserButtonController>();
        buttonController.Initialize(gameName, gameIpAndPort);
    }

    private void OnGameRemoved(string gameIpAndPort)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameBrowserButtonController buttonController = transform.GetChild(i).GetComponent<GameBrowserButtonController>();

            if (buttonController.gameIpAndPort == gameIpAndPort)
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
