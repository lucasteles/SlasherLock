using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class DisableSpriteRenderer : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
