using Assets.Scripts.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Ui.MainMenu
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] string sceneToLoad = "Level";

        void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneLoader.Instance.LoadScene(sceneToLoad);
            }
        }
    }
}
