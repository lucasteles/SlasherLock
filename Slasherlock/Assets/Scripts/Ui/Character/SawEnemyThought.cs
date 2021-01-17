using UnityEngine;

namespace Assets.Scripts.Ui.Character
{
    public class SawEnemyThought : MonoBehaviour
    {
        public static SawEnemyThought Instance { get; private set; }

        ThoughtBubble thoughtBubble;
        [SerializeField] string[] possibleThoughts;
        [SerializeField] float chanceToPlay;
        int timesPlayed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                thoughtBubble = GetComponent<ThoughtBubble>();
                return;
            }

            Destroy(gameObject);
        }

        public void PlaySawEnemyThought()
        {
            if (timesPlayed == 0 || ShouldPlayThisTime())
            {
                thoughtBubble.ShowThought(possibleThoughts[Random.Range(0, possibleThoughts.Length)]);
                timesPlayed++;
            }
        }

        bool ShouldPlayThisTime() => Random.Range(0, 101) < chanceToPlay; 
    }
}
