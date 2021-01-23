using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Interactables.Physics;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] AudioClip dropKeySound;
    [SerializeField] AudioClip unlockSound;
    [SerializeField] AudioClip lockedSound;
    [SerializeField] KeyColors lockColor;
    [SerializeField] KeyColors keyDropColor;
    [SerializeField] GameObject keyItemPrefab;


    [SerializeField] Sprite golden;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite green;
    [SerializeField] Sprite red;
    [SerializeField] Sprite cyan;
    [SerializeField] Sprite brown;
    [SerializeField] SpriteRenderer keyLockRenderer;

    AudioSource source;
    CharacterInventary inventary;
    bool canInteract;
    bool isOpen;

    void Start()
    {
        keyLockRenderer.sprite = lockColor switch
        {
            KeyColors.Golden => golden,
            KeyColors.Blue => blue,
            KeyColors.Green => green,
            KeyColors.Red => red,
            KeyColors.Cyan => cyan,
            KeyColors.Brown => brown,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;

        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
           inventary = other.gameObject.GetComponent<CharacterInventary>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        inventary = null;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.Space))
            return;

        if (!canInteract || isOpen) return;
        if (inventary == null || !inventary.HasKey(lockColor))
        {
            source.PlayOneShot(lockedSound);
            return;
        }


        IEnumerator wait()
        {
            source.PlayOneShot(unlockSound);
            yield return new WaitUntil(() => !source.isPlaying);
            source.PlayOneShot(dropKeySound);

            Instantiate(keyItemPrefab, transform.position, Quaternion.identity).GetComponent<KeyData>()
                .SetKeyColor(keyDropColor);
            foreach (var c in GetComponentsInChildren<Renderer>())
                c.enabled = false;
            yield return new WaitUntil(() => !source.isPlaying);
            Destroy(gameObject);
        }

        isOpen = true;
        StartCoroutine(wait());
    }
}