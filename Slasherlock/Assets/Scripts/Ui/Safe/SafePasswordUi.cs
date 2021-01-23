using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class SafePasswordUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI passwordInScreen;
        [SerializeField] float timeToBlinkPassword;
        [SerializeField] AudioClip beep;
        [SerializeField] AudioClip denied;
        Safe safe;
        AudioSource source;
        string currentPassword;
        const float blinkTime = 0.05f;

        void Awake()
        {
            safe = GetComponentInParent<Safe>();
            passwordInScreen.text = string.Empty;
            source = GetComponent<AudioSource>();
        }

        public void ResetPassword()
        {
            passwordInScreen.text = currentPassword = string.Empty;
        }

        public void EnterNumber(int number)
        {
            source.PlayOneShot(beep);
            passwordInScreen.text += "*";
            currentPassword += number;

            if (currentPassword.Length == 4)
                StartCoroutine(BlinkPassord());
        }

        IEnumerator BlinkPassord()
        {
            var timeToBlinkPassword = this.timeToBlinkPassword;

            if (!safe.TestPassword(currentPassword))
                source.PlayOneShot(denied);
            else
            {
                while (timeToBlinkPassword > 0)
                {
                    passwordInScreen.gameObject.SetActive(false);
                    yield return new WaitForSeconds(blinkTime);
                    passwordInScreen.gameObject.SetActive(true);
                    yield return new WaitForSeconds(blinkTime);
                    timeToBlinkPassword -= blinkTime;
                }
            }

            ResetPassword();
        }
    }
}