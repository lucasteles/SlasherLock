using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Interactables.Physics;
using Assets.Scripts.Ui.Character;
using UnityEngine;

public class Safe : MonoBehaviour
{
    bool canInteract;

    [SerializeField] string message;
    [SerializeField] GameObject keyPefab;
    [SerializeField] KeyColors keyColor;
    [SerializeField] AudioClip beep;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            ThoughtBubble.Instance.ShowThought(message);
            canInteract = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            ThoughtBubble.Instance.ShowThought(message);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            canInteract = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.Q))
            TryOpenSafe();
    }

    void TryOpenSafe()
    {
        OpenSafe();
    }

    void OpenSafe()
    {
        source.Play();
        IEnumerator wait()
        {
            yield return new WaitForSeconds(1);
            Instantiate(keyPefab, transform.position, Quaternion.identity).GetComponent<KeyData>()
                .SetKeyColor(keyColor);
            GetComponentInChildren<Renderer>().enabled = false;
            yield return new WaitUntil(() => !source.isPlaying);
            Destroy(gameObject);
        }
        StartCoroutine(wait());
    }
}