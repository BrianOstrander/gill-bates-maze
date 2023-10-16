using UnityEngine;
using UnityEngine.SceneManagement;

namespace GillBates.Controller
{
    public class RestartController : MonoBehaviour
    {
        [SerializeField]
        int mainSceneIndex;
        
        void Update()
        {
            if (1 != SceneManager.loadedSceneCount)
            {
                return;
            }

            enabled = false;
            SceneManager.LoadScene(mainSceneIndex);
        }
    }
}