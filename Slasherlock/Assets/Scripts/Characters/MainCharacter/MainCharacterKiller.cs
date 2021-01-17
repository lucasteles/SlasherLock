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
        [SerializeField] float timeToLoadGameOverScene;
        [SerializeField] GameObject bloodSplash;
        [SerializeField] GameObject flashEffect;
        [SerializeField] Transform effectsCanvas;
        [SerializeField] GameObject gameOverCanvas;

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
            yield return new WaitForSeconds(timeToKill);
            audioSource.PlayOneShot(deathSound);
            Instantiate(flashEffect, effectsCanvas);

            yield return new WaitForSeconds(timeToLoadGameOverScene);
            Instantiate(gameOverCanvas);
            Destroy(Instantiate(bloodSplash, transform.position, Quaternion.identity),5);
            spriteRenderer.enabled = false;
        }
    }
}
