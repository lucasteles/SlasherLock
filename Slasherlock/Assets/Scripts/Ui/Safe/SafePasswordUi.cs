using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class SafePasswordUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI passwordInScreen;
        [SerializeField] float timeToBlinkPassword;
        Safe safe;
        string currentPassword;
        const float blinkTime = 0.05f;

        void Awake()
        {
            safe = GetComponentInParent<Safe>();
            passwordInScreen.text = string.Empty;
        }

        public void ResetPassword()
        {
            passwordInScreen.text = currentPassword = string.Empty;
        }

        public void EnterNumber(int number)
        {
            passwordInScreen.text += "*";
            currentPassword += number;

            if (currentPassword.Length == 4)
                StartCoroutine(BlinkPassord());
        }

        IEnumerator BlinkPassord()
        {
            var timeToBlinkPassword = this.timeToBlinkPassword;

            while (timeToBlinkPassword > 0)
            {
                passwordInScreen.gameObject.SetActive(false);
                yield return new WaitForSeconds(blinkTime);
                passwordInScreen.gameObject.SetActive(true);
                yield return new WaitForSeconds(blinkTime);
                timeToBlinkPassword -= blinkTime;
            }

            TryPassword();
        }

        void TryPassword()
        {
            safe.TestPassword(currentPassword);
            ResetPassword();
        }
    }
}
