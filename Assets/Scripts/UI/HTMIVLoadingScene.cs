using UnityEngine;
using UnityEngine.SceneManagement;

namespace HTMIV.UI
{
    public class HTMIVLoadingScene : MonoBehaviour
    {
        private void Start()
        {
            var htmivRandTime = Random.Range(1f, 2f);
            Invoke("HTMIVLoadScene", htmivRandTime);
        }
        
        private void HTMIVLoadScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}