using Assets.Scripts.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI winText;
        [SerializeField] float timeToShowText;
        [SerializeField] float timePerLetter;
        [SerializeField] float timeToGoToMainMenu;
        string textToShow;

        void Awake()
        {
            textToShow = winText.text;
            winText.text = string.Empty;
        }

        void Start() => StartCoroutine(ShowText());

        IEnumerator ShowText()
        {
            var i = 0;

            yield return new WaitForSeconds(timeToShowText);

            while (i < textToShow.Length)
            {
                winText.text += textToShow[i];
                i++;
                yield return new WaitForSeconds(timePerLetter);
            }

            yield return new WaitForSeconds(timeToGoToMainMenu);

            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
