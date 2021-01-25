using Assets.Scripts.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui.MainMenu
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] string sceneToLoad = "Level";
        [SerializeField] float timeToLoadScene;
        [SerializeField] GameObject bloodAnimation;


        [SerializeField] AudioSource enter;

        bool canStartGame = false;
        bool gameStarted = false;

        void Update()
        {
            if (Input.anyKeyDown && canStartGame && !gameStarted)
                StartCoroutine(ShowBloodAndLoadScene());
        }

        IEnumerator ShowBloodAndLoadScene()
        {
            gameStarted = true;
            enter.Play();
            Instantiate(bloodAnimation, transform.parent);

            yield return new WaitForSeconds(timeToLoadScene);

            SceneLoader.Instance.LoadScene(sceneToLoad);
        }

        public void EnableGameStart() => canStartGame = true;
    }
}
