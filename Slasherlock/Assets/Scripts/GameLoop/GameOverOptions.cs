using Assets.Scripts.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameLoop
{
    public class GameOverOptions : MonoBehaviour
    {
        [SerializeField] string gameScene = "Level";
        [SerializeField] string mainMenuScene = "MainMenu";
        [SerializeField] float timeToShowButtons;

        GameObject buttons;

        void Awake()
        {
            buttons = transform.GetChild(0).gameObject;
            buttons.SetActive(false);
        }

        public void TryAgain() => SceneLoader.Instance.LoadScene(gameScene);
        public void GotToMainMenu() => SceneLoader.Instance.LoadScene(mainMenuScene);
        public void QuitGame() => Application.Quit();

        void Start()
        {
            StartCoroutine(ShowButtonsAfterTime());
        }

        IEnumerator ShowButtonsAfterTime()
        {
            yield return new WaitForSeconds(timeToShowButtons);
            buttons.SetActive(true);
        }
    }
}
