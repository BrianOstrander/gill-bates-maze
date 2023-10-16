using UnityEngine;
using UnityEngine.SceneManagement;

namespace GillBates.Controller
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField]
        string linkAddress;
        
        [SerializeField]
        int restartSceneIndex;

        [SerializeField]
        GameObject canvas;

        public void OnClickLink()
        {
            Application.OpenURL(linkAddress);
        }
        
        public void OnClickContinue()
        {
            canvas.SetActive(false);
            SceneManager.LoadScene(restartSceneIndex);
        }
    }
}