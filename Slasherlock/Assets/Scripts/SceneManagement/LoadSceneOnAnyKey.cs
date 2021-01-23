using UnityEngine;

namespace Assets.Scripts.SceneManagement
{
    public class LoadSceneOnAnyKey : MonoBehaviour
    {
        [SerializeField] string sceneToLoad;

        private void Update()
        {
            if (Input.anyKeyDown)
                SceneLoader.Instance.LoadScene(sceneToLoad);
        }
    }
}