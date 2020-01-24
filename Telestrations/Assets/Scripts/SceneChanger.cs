using UnityEngine;
using UnityEngine.SceneManagement;

namespace Telestrations
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static void StaticChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
