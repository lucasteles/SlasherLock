using System;
using System.Linq;
using Assets.Scripts.Ui.Character;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    [SerializeField] Safe safe;
    [SerializeField] string mask = "****";
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
            var maskedPassword = safe.Password
                .Zip(mask, (p, m) => m == '_' ? m.ToString() : p.ToString())
                .Aggregate(string.Concat);
            source.PlayIfNotPlaying();
            ThoughtBubble.Instance.ShowThoughtUntil($"{safe.name} password: {maskedPassword}", () => !isActive);
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
