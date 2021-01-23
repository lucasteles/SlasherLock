using Assets.Scripts.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.GameLoop
{
    public class GameOverOptions : MonoBehaviour
    {
        [SerializeField] string gameScene = "Mansion";
        [SerializeField] string mainMenuScene = "MainMenu";

        public void TryAgain() => SceneLoader.Instance.LoadScene(gameScene);
        public void GotToMainMenu() => SceneLoader.Instance.LoadScene(mainMenuScene);
        public void QuitGame() => Application.Quit();
    }
}
