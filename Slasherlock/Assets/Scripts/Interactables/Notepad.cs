using System;
using Assets.Scripts.Ui.Character;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    [SerializeField] Safe safe;
    AudioSource source;
    bool isActive;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Show(GameObject other)
    {
        if (LayerMask.LayerToName(other.layer) == "Player")
        {
            source.PlayIfNotPlaying();
            ThoughtBubble.Instance.ShowThoughtUntil($"{safe.name} password: {safe.Password}", () => !isActive);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isActive = true;
        Show(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isActive = false;
    }

}
