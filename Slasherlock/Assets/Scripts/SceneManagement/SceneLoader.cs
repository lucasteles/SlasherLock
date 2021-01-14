using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] GameObject objectWithTransition;
        [SerializeField] float transitionTime;
        Animator transition;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                var transitionInScreen = Instantiate(objectWithTransition);
                transition = transitionInScreen.GetComponentInChildren<Animator>();

                return;
            }

            Destroy(gameObject);
        }

        public void LoadLevel(string level)
            => StartCoroutine(LoadLevelAsync(level));

        IEnumerator LoadLevelAsync(string level)
        {
            transition.SetTrigger("start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(level);
        }
    }
}
