using Assets.Scripts.Ui.Character;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class ThoughtEvent : MonoBehaviour
    {
        [SerializeField] string[] possibleThoughts;
        [SerializeField] bool playOnlyOnce = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ThoughtBubble.Instance.ShowThought(possibleThoughts[Random.Range(0, possibleThoughts.Length)]);

            if (playOnlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}
