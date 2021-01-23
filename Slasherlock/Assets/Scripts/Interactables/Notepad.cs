using System;
using Assets.Scripts.Ui.Character;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    [SerializeField] Safe safe;
    AudioSource source;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Show(GameObject other)
    {
        if (LayerMask.LayerToName(other.layer) == "Player")
        {
            source.PlayIfNotPlaying();
            ThoughtBubble.Instance.ShowThought($"{safe.message} password: {safe.Password}");
        }

    }

    void OnTriggerEnter2D(Collider2D other) => Show(other.gameObject);
    void OnTriggerStay2D(Collider2D other) => Show(other.gameObject);

}
