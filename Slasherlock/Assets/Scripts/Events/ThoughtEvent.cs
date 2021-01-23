using System;
using Assets.Scripts.Ui.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Events
{
    public class ThoughtEvent : MonoBehaviour
    {
        [SerializeField] string[] possibleThoughts;
        [SerializeField] bool playOnlyOnce = true;

        LayerMask playerLayerMask;

        void Start()
        {
            playerLayerMask = LayerMask.NameToLayer("Player");
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != playerLayerMask)
                return;

            ThoughtBubble.Instance.ShowThought(possibleThoughts[Random.Range(0, possibleThoughts.Length)]);

            if (playOnlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
}
