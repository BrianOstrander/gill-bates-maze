using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GillBates.Controller
{
    public class RestartController : MonoBehaviour
    {
        [SerializeField]
        int mainSceneIndex;

        [SerializeField]
        int restartSceneIndex;
        
        void Update()
        {
            if (1 != SceneManager.loadedSceneCount)
            {
                return;
            }

            enabled = false;
            SceneManager.LoadScene(mainSceneIndex);
            // SceneManager.UnloadSceneAsync(restartSceneIndex);
        }
    }
}