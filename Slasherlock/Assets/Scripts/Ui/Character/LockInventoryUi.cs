using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui.Character
{
    public class LockInventoryUi : MonoBehaviour
    {
        [SerializeField] GameObject toActivateWhenHasLocks;
        [SerializeField] TextMeshProUGUI locksCountInScreen;
        int numberOfLocks;

        void Awake()
        {
            toActivateWhenHasLocks.SetActive(false);
            locksCountInScreen.text = string.Empty;
        }

        public void AddLock()
        {
            if (numberOfLocks == 0)
                toActivateWhenHasLocks.SetActive(true);

            numberOfLocks++;
            locksCountInScreen.text = numberOfLocks.ToString();
        }

        public void RemoveLock()
        {
            numberOfLocks--;
            locksCountInScreen.text = numberOfLocks.ToString();

            if (numberOfLocks == 0)
            {
                toActivateWhenHasLocks.SetActive(false);
                locksCountInScreen.text = string.Empty;
            }
        }
    }
} 