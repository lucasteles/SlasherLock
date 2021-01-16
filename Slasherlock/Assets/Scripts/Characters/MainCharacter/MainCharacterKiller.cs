using Assets.Scripts.Physics;
using Assets.Scripts.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.MainCharacter
{
    public class MainCharacterKiller : MonoBehaviour
    {
        Mover mover;
        SpriteRenderer spriteRenderer;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip deathSound;
        [SerializeField] float timeToKill;
        [SerializeField] string gameOverScene = "GameOver";
        [SerializeField] GameObject bloodSplash;

        void Awake()
        {
            mover = GetComponent<Mover>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Kill()
        {
            mover.PreventMovement();
            StartCoroutine(DestroyAfterTime());
        }

        IEnumerator DestroyAfterTime()
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(Instantiate(bloodSplash, transform.position, Quaternion.identity),5);
            yield return new WaitForSeconds(timeToKill);
            SceneLoader.Instance.LoadScene(gameOverScene);
            spriteRenderer.enabled = false;
        }
    }
}
