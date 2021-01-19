using Assets.Interactables.Physics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui.Character
{
    public class KeysInventoryUi : MonoBehaviour
    {
        [SerializeField] GameObject keysHolder;
        [SerializeField] GameObject keyOnScreenPrefab;
        [SerializeField] KeyUi[] keysUi;
        [SerializeField] float distanceBetweenKeys;
        [SerializeField] GameObject keyUiText;
        int keysInScreen;

        void Awake()
            => keyUiText.SetActive(false);


        public void AddKeyOfColor(KeyColors color)
        {
            var distanceFromOtherKeys = keysInScreen * distanceBetweenKeys;

            if (keysInScreen == 0) keyUiText.SetActive(true);

            var keyInUI = Instantiate(keyOnScreenPrefab, keysHolder.transform);
            keyInUI.GetComponent<RectTransform>().position += new Vector3(distanceFromOtherKeys, 0);
            keyInUI.GetComponent<Image>().sprite = keysUi.First(a => a.color == color).sprite;

            keysInScreen++; 
        }
    }

    [System.Serializable]
    public struct KeyUi
    {
        public KeyColors color;
        public Sprite sprite;
    }
}
