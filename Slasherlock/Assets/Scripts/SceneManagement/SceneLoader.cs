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
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            var transitionInScreen = Instantiate(objectWithTransition);
            transition = transitionInScreen.GetComponentInChildren<Animator>();
        }

        public void LoadScene(string level)
            => StartCoroutine(LoadSceneWithTransition(level));

        IEnumerator LoadSceneWithTransition(string level)
        {
            transition.SetTrigger("start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(level);
        }
    }
}
